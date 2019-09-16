using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using K12.Data;
using FISCA.UDT;
using FISCA.Data;

namespace Ischool.Tidy_Competition
{
    public partial class frmPcBelong : BaseForm
    {
        private AccessHelper _access = new AccessHelper();
        private Dictionary<string, UDT.Area> _dicAreaByName = new Dictionary<string, UDT.Area>();
        private Dictionary<string, UDT.Place> _dicPlaceByName = new Dictionary<string, UDT.Place>();
        private Dictionary<string, DataRow> _dicClassRecordByName = new Dictionary<string, DataRow>();
        private bool _isValueChange = false;
        private bool _initFinish;

        public frmPcBelong()
        {
            InitializeComponent();
        }

        private void frmPcBelong_Load(object sender, EventArgs e)
        {
            _initFinish = false;
            // 取得全校所有班級資料
            DataTable dt = getAllClassData();
            //List<ClassRecord> listClassRecord = Class.SelectAll();

            dgvClass.Width = 140;
            foreach (DataRow row in dt.Rows)
            {
                if (!this._dicClassRecordByName.ContainsKey("" + row["class_name"]))
                {
                    this._dicClassRecordByName.Add("" + row["class_name"], row);

                    dgvClass.Items.Add("" + row["class_name"]);
                }
            }

            #region Init SchoolYear
            int schoolYear = int.Parse(School.DefaultSchoolYear);
            cbxSchoolYear.Items.Add(schoolYear - 1);
            cbxSchoolYear.Items.Add(schoolYear);
            cbxSchoolYear.Items.Add(schoolYear + 1);

            cbxSchoolYear.SelectedIndex = 1; 
            #endregion

            #region Init Area
            List<UDT.Area> listArea = this._access.Select<UDT.Area>();
            if (listArea.Count > 0)
            {
                foreach (UDT.Area data in listArea)
                {
                    cbxArea.Items.Add(data.Name);
                    this._dicAreaByName.Add(data.Name, data);
                }

                cbxArea.SelectedIndex = 0;
            }
            else
            {
                MsgBox.Show("請先設定區域類別資料!");
            }
            #endregion

            ReloadDataGridView();

            _initFinish = true;
        }

        private DataTable getAllClassData()
        {
            string sql = @"
SELECT
    *
FROM
    class
ORDER BY
    display_order
    , class_name
";
            QueryHelper qh = new QueryHelper();
            return qh.Select(sql);
        }

        private void ReloadDataGridView()
        {
            this.SuspendLayout();

            dataGridViewX1.Rows.Clear();
            _dicPlaceByName.Clear();
            _isValueChange = false;
            string areaID = this._dicAreaByName[cbxArea.SelectedItem.ToString()].UID;

            // 取得區域位置資料
            {
                List<UDT.Place> listPlace = this._access.Select<UDT.Place>(string.Format("ref_area_id = {0}", areaID));
                foreach (UDT.Place place in listPlace)
                {
                    this._dicPlaceByName.Add(place.Name, place);
                }
            }

            // 取得位置負責班級資料
            DataTable dt = DAO.PcBelong.GetPcBelong(cbxSchoolYear.SelectedItem.ToString(),areaID);

            foreach (DataRow row in dt.Rows)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;
                string pcBelongID = "" + row["uid"];
                dgvrow.Cells[col++].Value = "" + row["place_name"];
                dgvrow.Cells[col++].Value = "" + row["class_name"];
                if (!string.IsNullOrEmpty(pcBelongID))
                {
                    dgvrow.Cells[1].Style.BackColor = Color.LightGreen;
                }
                dgvrow.Tag = pcBelongID;

                dataGridViewX1.Rows.Add(dgvrow);
            }

            this.ResumeLayout();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 資料驗證
            if (dataGridViewX1.Rows.Count == 0)
            {
                MsgBox.Show("沒有可儲存資料!");
                return;
            }
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (!string.IsNullOrEmpty(dgvrow.ErrorText))
                {
                    MsgBox.Show("資料驗證錯誤!");
                    return;
                }
            }
            // 資料整理
            List<UDT.PcBelong> listInsertData = new List<UDT.PcBelong>();
            List<UDT.PcBelong> listUpdateData = new List<UDT.PcBelong>();

            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                string placeID = this._dicPlaceByName["" + dgvrow.Cells[0].Value].UID;
                string pcBelongID = "" + dgvrow.Tag;
                if (string.IsNullOrEmpty(pcBelongID))// 新增資料
                {
                    if (!string.IsNullOrEmpty("" + dgvrow.Cells[1].Value)) // 打掃區域有指定負責班級
                    {
                        string classID = "" + this._dicClassRecordByName["" + dgvrow.Cells[1].Value]["id"];
                        UDT.PcBelong data = new UDT.PcBelong();
                        data.SchoolYear = int.Parse(cbxSchoolYear.SelectedItem.ToString());
                        data.RefPlaceID = int.Parse(placeID);
                        data.RefClassID = int.Parse(classID);
                        data.CreateTime = DateTime.Now;
                        data.CreatedBy = DAO.Actor.Instance().GetUserAccount();

                        listInsertData.Add(data);
                    }
                }
                else // 更新資料
                {
                    UDT.PcBelong data = this._access.Select<UDT.PcBelong>(string.Format("uid = {0}", pcBelongID))[0];
                    if (data != null)
                    {
                        string classID = ("" + dgvrow.Cells[1].Value) == "" ? null : "" + this._dicClassRecordByName["" + dgvrow.Cells[1].Value]["id"];

                        data.RefClassID = int.Parse(classID);

                        listUpdateData.Add(data);
                    }
                }
            }

            // 資料儲存
            try
            {
                if (listInsertData.Count > 0)
                {
                    this._access.InsertValues(listInsertData);
                }
                if (listUpdateData.Count > 0)
                {
                    this._access.UpdateValues(listUpdateData);
                }
                MsgBox.Show("資料儲存成功!");
                ReloadDataGridView();
            }
            catch(Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
        }

        private bool dgvClass_Validate(DataGridViewRow dgvrow)
        {
            if (!string.IsNullOrEmpty(("" + dgvrow.Cells[1].Value).Trim()))
            {
                if (!this._dicClassRecordByName.ContainsKey("" + dgvrow.Cells[1].Value))
                {
                    dgvrow.ErrorText = "負責班級不存在!";
                    return false;
                }
                else
                {
                    dgvrow.Cells[1].Style.BackColor = Color.LightPink;
                    this._isValueChange = true;
                    dgvrow.ErrorText = null;
                    return true; 
                }
            }
            return true;
        }

        private void dataGridViewX1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                dgvClass_Validate(dataGridViewX1.Rows[e.RowIndex]);
            }
        }

        private void cbxSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView();
            }
        }

        private void cbxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView();
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            if (this._isValueChange)
            {
                DialogResult result = MsgBox.Show("已修改資料尚未儲存，確定離開?","提醒",MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

    }
}
