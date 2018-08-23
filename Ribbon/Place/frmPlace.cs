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
using FISCA.UDT;
using K12.Data;
using System.Collections;

namespace Ischool.Tidy_Competition
{
    public partial class frmPlace : BaseForm
    {
        private AccessHelper _access = new AccessHelper();
        private Dictionary<string, UDT.Area> _dicAreaByName = new Dictionary<string, UDT.Area>();
        private string _userAccount = DAO.Actor.Instance().GetUserAccount();
        private bool _initFinsh = false;

        public frmPlace()
        {
            InitializeComponent();
        }

        private void frmPlace_Load(object sender, EventArgs e)
        {
            // 取得區域類別資料
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
                MsgBox.Show("請先設定區域類別，才能進一步設定區域位置!");
                this.Close();
            }

            this._initFinsh = true;
        }

        private void ReloadDataGridView()
        {
            string areaID = this._dicAreaByName[cbxArea.SelectedItem.ToString()].UID;
            // 取得區域位置資料
            List<UDT.Place> listPlace = this._access.Select<UDT.Place>(string.Format("ref_area_id = {0}", areaID));

            dataGridViewX1.Rows.Clear();

            foreach (UDT.Place data in listPlace)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;
                dgvrow.Cells[col++].Value = data.Enabled;
                dgvrow.Cells[col++].Value = data.Name;
                dgvrow.Cells[col++].Value = "" + data.DisplayOrder == "0" ? "" : "" + data.DisplayOrder;
                dgvrow.Cells[col++].Value = "" + data.Zone == "0" ? "" : "" + data.Zone;
                dgvrow.Cells[col++].Value = data.CreatedBy;
                dgvrow.Tag = data;

                dataGridViewX1.Rows.Add(dgvrow);
            }
        }

        private void cbxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadDataGridView();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 快速複製全部班級名稱
            DialogResult result = MsgBox.Show("此功能將會複製全校班級名稱為區域名稱，確定執行此功能?","提醒",MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // 取得所有班級資料
                List<ClassRecord> listclass = Class.SelectAll();
                listclass.Sort(new sortClassData());

                foreach (ClassRecord cr in listclass)
                {
                    DataGridViewRow dgvrow = new DataGridViewRow();
                    dgvrow.CreateCells(dataGridViewX1);

                    dgvrow.Cells[1].Value = cr.Name;
                    dgvrow.Cells[2].Value = cr.DisplayOrder;
                    dgvrow.Cells[3].Value = this._userAccount;

                    dataGridViewX1.Rows.Add(dgvrow);
                }
                
            }
        }

        /// <summary>
        /// 班級資料排序規則
        /// </summary>
        private class sortClassData : IComparer<ClassRecord>
        {
            int IComparer<ClassRecord>.Compare(ClassRecord x, ClassRecord y)
            {
                if (int.Parse(x.DisplayOrder == "" ? "0" : x.DisplayOrder) > int.Parse(y.DisplayOrder == "" ? "0" : y.DisplayOrder))
                {
                    return 1;
                }
                if (int.Parse(x.DisplayOrder == "" ? "0" : x.DisplayOrder) < int.Parse(y.DisplayOrder == "" ? "0" : y.DisplayOrder))
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private class dgvDisplayOrder_Sort : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                DataGridViewRow row1 = (DataGridViewRow)x;
                DataGridViewRow row2 = (DataGridViewRow)y;

                if (int.Parse("" + row1.Cells[2].Value == "" ? "0" : "" + row1.Cells[2].Value) > int.Parse("" + row2.Cells[2].Value == "" ? "0" : "" + row2.Cells[2].Value))
                {
                    return 1;
                }
                if (int.Parse("" + row1.Cells[2].Value == "" ? "0" : "" + row1.Cells[2].Value) < int.Parse("" + row2.Cells[2].Value == "" ? "0" : "" + row2.Cells[2].Value))
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void dataGridViewX1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && string.IsNullOrEmpty("" + dataGridViewX1.Rows[e.RowIndex].Cells[4].Value))
            {
                dataGridViewX1.Rows[e.RowIndex].Cells[4].Value = this._userAccount;
            }
        }

        private void dataGridViewX1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this._initFinsh)
            {
                if (e.ColumnIndex == 1)
                {
                    dgvName_Validate();
                }

                if (e.ColumnIndex == 2)
                {
                    if (dgvDisplayOrder_Validate())
                    {
                        //dataGridViewX1.Sort(dataGridViewX1.Columns[2], ListSortDirection.Ascending);
                        dataGridViewX1.Sort(new dgvDisplayOrder_Sort());
                    }
                }

                if (e.ColumnIndex == 3)
                {
                    dgvZone_Validate();
                }
            }
        }

        private bool dgvDisplayOrder_Validate()
        {
            int row = 0;
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (row == dataGridViewX1.Rows.Count -1)
                {
                    break;
                }
                row++;
                if (!string.IsNullOrEmpty("" + dgvrow.Cells[2].Value))
                {
                    int n = 0;
                    if (!int.TryParse("" + dgvrow.Cells[2].Value,out n))
                    {
                        dgvrow.Cells[2].ErrorText = "請填入數值!";
                        return false;
                    }
                    else
                    {
                        dgvrow.Cells[2].ErrorText = null;
                    }
                }
            }
            return true;
        }

        private bool dgvName_Validate()
        {
            List<string> listPlaceName = new List<string>();
            int row = 0;
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (row == dataGridViewX1.Rows.Count - 1)
                {
                    break;
                }
                row++;
                if (string.IsNullOrEmpty("" + dgvrow.Cells[1].Value))
                {
                    dgvrow.Cells[1].ErrorText = "此欄位不可空白!";
                    return false;
                }
                else
                {
                    if (listPlaceName.Contains("" + dgvrow.Cells[1].Value))
                    {
                        dgvrow.Cells[1].ErrorText = "位置名稱重複!";
                        return false;
                    }
                    else
                    {
                        listPlaceName.Add("" + dgvrow.Cells[1].Value);
                        dgvrow.Cells[1].ErrorText = null;
                    }
                }
            }
            return true;
        }

        private bool dgvZone_Validate()
        {
            int row = 0;
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (row == dataGridViewX1.Rows.Count - 1)
                {
                    break;
                }
                row++;
                if (!string.IsNullOrEmpty("" + dgvrow.Cells[3].Value))
                {
                    int n = 0;
                    if (!int.TryParse("" + dgvrow.Cells[3].Value,out n))
                    {
                        dgvrow.Cells[3].ErrorText = "請填入數值!";
                        return false;
                    }
                    else
                    {
                        dgvrow.Cells[3].ErrorText = null;
                    }
                }
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvDisplayOrder_Validate() && dgvName_Validate() && dgvZone_Validate()) // 資料驗證.
            {
                // 資料整理
                List<UDT.Place> listInsertData = new List<UDT.Place>();
                List<UDT.Place> listUpdateData = new List<UDT.Place>();

                int row = 0;
                foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
                {
                    if (row == dataGridViewX1.Rows.Count - 1)
                    {
                        break;
                    }
                    row++;
                    if (dgvrow.Tag == null)
                    {
                        UDT.Place data = new UDT.Place();
                        fillData(data,dgvrow);

                        listInsertData.Add(data);
                    }
                    else
                    {
                        UDT.Place data = (UDT.Place)dgvrow.Tag;
                        fillData(data, dgvrow);

                        listUpdateData.Add(data);
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
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }
            }

        }

        private void fillData(UDT.Place data,DataGridViewRow dgvrow)
        {
            data.Enabled = bool.Parse(("" + dgvrow.Cells[0].Value) == "" ? "false" : "" + dgvrow.Cells[0].Value);
            data.Name = "" + dgvrow.Cells[1].Value;
            if (!string.IsNullOrEmpty("" + dgvrow.Cells[2].Value))
            {
                data.DisplayOrder = int.Parse("" + dgvrow.Cells[2].Value);
            }
            data.RefAreaID = int.Parse(this._dicAreaByName[cbxArea.SelectedItem.ToString()].UID);
            if (!string.IsNullOrEmpty("" + dgvrow.Cells[3].Value))
            {
                data.Zone = int.Parse("" + dgvrow.Cells[3].Value);
            }
            data.CreatedBy = "" + dgvrow.Cells[4].Value;
            data.CreateTime = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
        }

        private void rightBtnEnable_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvrow in dataGridViewX1.SelectedRows)
            {
                dgvrow.Cells[0].Value = true;
            }
        }

        private void rightBtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MsgBox.Show("確定刪除選取資料行?","提醒",MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                List<UDT.Place> listDeleteData = new List<UDT.Place>();

                foreach (DataGridViewRow dgvrow in dataGridViewX1.SelectedRows)
                {
                    if (dgvrow.Tag == null)
                    {
                        dataGridViewX1.Rows.Remove(dgvrow);
                    }
                    else
                    {
                        UDT.Place data = (UDT.Place)dgvrow.Tag;
                        listDeleteData.Add(data);
                        dataGridViewX1.Rows.Remove(dgvrow);
                    }
                }

                try
                {
                    this._access.DeletedValues(listDeleteData);
                    MsgBox.Show("資料刪除成功!");
                }
                catch(Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewX1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(dataGridViewX1,new Point(e.X,e.Y));
            }
        }
    }
}
