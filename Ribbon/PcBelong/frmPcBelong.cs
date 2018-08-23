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

namespace Ischool.Tidy_Competition
{
    public partial class frmPcBelong : BaseForm
    {
        private AccessHelper _access = new AccessHelper();
        private Dictionary<string, UDT.Area> _dicAreaByName = new Dictionary<string, UDT.Area>();
        private Dictionary<string, UDT.Place> _dicPlaceByName = new Dictionary<string, UDT.Place>();
        private Dictionary<string, ClassRecord> _dicClassRecordByName = new Dictionary<string, ClassRecord>();
        
        public frmPcBelong()
        {
            InitializeComponent();
        }

        private void frmPcBelong_Load(object sender, EventArgs e)
        {
            // 取得全校所有班級資料
            List<ClassRecord> listClassRecord = Class.SelectAll();

            dgvClass.Width = 140;
            foreach (ClassRecord cr in listClassRecord)
            {
                if (!this._dicClassRecordByName.ContainsKey(cr.Name))
                {
                    this._dicClassRecordByName.Add(cr.Name, cr);

                    dgvClass.Items.Add(cr.Name);
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
        }

        private void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();
            string areaID = this._dicAreaByName[cbxArea.SelectedItem.ToString()].UID;

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
                    dgvrow.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                dgvrow.Tag = pcBelongID;

                dataGridViewX1.Rows.Add(dgvrow);
            }
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
                        string classID = this._dicClassRecordByName["" + dgvrow.Cells[1].Value].ID;
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
                        string classID = ("" + dgvrow.Cells[1].Value) == "" ? null : this._dicClassRecordByName["" + dgvrow.Cells[1].Value].ID;

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

        private void cbxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._dicPlaceByName.Clear();
            string areaID = this._dicAreaByName[cbxArea.SelectedItem.ToString()].UID;
            List<UDT.Place> listPlace = this._access.Select<UDT.Place>(string.Format("ref_area_id = {0}",areaID));
            foreach (UDT.Place place in listPlace)
            {
                this._dicPlaceByName.Add(place.Name,place);
            }

            ReloadDataGridView();
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
