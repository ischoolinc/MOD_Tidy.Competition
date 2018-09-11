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

namespace Ischool.Tidy_Competition
{
    public partial class frmDeductionItem : BaseForm
    {
        private AccessHelper _access = new AccessHelper();
        private Dictionary<string, UDT.Area> _dicAreaByName = new Dictionary<string, UDT.Area>();
        private string _userAccount = DAO.Actor.Instance().GetUserAccount();

        public frmDeductionItem()
        {
            InitializeComponent();
        }

        private void frmDeductionItem_Load(object sender, EventArgs e)
        {
            // Init Area
            List<UDT.Area> listArea = this._access.Select<UDT.Area>();
            foreach (UDT.Area area in listArea)
            {
                cbxArea.Items.Add(area.Name);
                _dicAreaByName.Add(area.Name, area);
            }
            if (cbxArea.Items.Count > 0)
            {
                cbxArea.SelectedIndex = 0;
            }
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadDataGridView();
        }

        private void ReloadDataGridView()
        {
            this.SuspendLayout();

            dataGridViewX1.Rows.Clear();
            // 取得扣分物件資料
            string areaID = this._dicAreaByName[cbxArea.SelectedItem.ToString()].UID;
            List<UDT.DeDuctionItem> listItem = this._access.Select<UDT.DeDuctionItem>(string.Format("ref_area_id = {0}", areaID));
            listItem.Sort(new sortDeDuctionItem());
            foreach (UDT.DeDuctionItem data in listItem)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);
                int col = 0;
                dgvrow.Cells[col++].Value = data.Enabled;
                dgvrow.Cells[col++].Value = data.Name;
                dgvrow.Cells[col++].Value = "" + data.DisplayOrder == "0" ? null : "" + data.DisplayOrder;
                dgvrow.Tag = data;

                dataGridViewX1.Rows.Add(dgvrow);
            }

            this.ResumeLayout();
        }

        private void dataGridViewX1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 1)
                {
                    dgvName_Validate(dataGridViewX1.Rows[e.RowIndex], 1);
                }
                if (e.ColumnIndex == 2)
                {
                    dgvDisplayOrder_Validate(dataGridViewX1.Rows[e.RowIndex], 2);
                }
            }
        }

        private bool dgvName_Validate(DataGridViewRow dgvrow, int col)
        {
            if (string.IsNullOrEmpty(("" + dgvrow.Cells[col].Value).Trim()))
            {
                dgvrow.Cells[col].ErrorText = "名稱欄位不可空白!";
                return false;
            }
            else
            {
                dgvrow.Cells[col].ErrorText = null;
                return true;
            }
        }

        private bool dgvDisplayOrder_Validate(DataGridViewRow dgvrow, int col)
        {
            if (string.IsNullOrEmpty("" + dgvrow.Cells[col].Value))
            {
                dgvrow.Cells[col].ErrorText = null;
                return true;
            }
            else
            {
                int n = 0;
                if (int.TryParse("" + dgvrow.Cells[col].Value, out n))
                {
                    dgvrow.Cells[col].ErrorText = null;
                    return true;
                }
                else
                {
                    dgvrow.Cells[col].ErrorText = "只允許填入數值!";
                    return false;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 資料驗證
            int row = 0;
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (row == dataGridViewX1.Rows.Count - 1)
                {
                    break; // 最後一行不驗證
                }
                row++;
                if (!dgvName_Validate(dgvrow, 1) || !dgvDisplayOrder_Validate(dgvrow, 2))
                {
                    MsgBox.Show("資料驗證錯誤!");
                    return;
                }
            }
            #endregion

            #region 資料整理
            List<string> listDataRow = new List<string>();
            int rowIndex = 0;
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (rowIndex == dataGridViewX1.Rows.Count - 1)
                {
                    break; // 最後一行資料不儲存
                }
                rowIndex++;
                string uid = dgvrow.Tag == null ? "null" : ((UDT.DeDuctionItem)dgvrow.Tag).UID;
                string name = "" + dgvrow.Cells[1].Value;
                string areaID = this._dicAreaByName[cbxArea.SelectedItem.ToString()].UID;
                string enabled = ("" + dgvrow.Cells[0].Value) == "True" ? "true" : "false";
                string displayOrder = ("" + dgvrow.Cells[2].Value) == "" ? "null" : "" + dgvrow.Cells[2].Value;
                string createTime = DateTime.Now.ToString("yyyy/MM/dd");
                string usertAccount = this._userAccount;
                string data = string.Format(@"
SELECT
    {0}::BIGINT AS uid
    , '{1}'::TEXT AS name
    , {2}::BIGINT AS ref_area_id
    , {3}::BOOLEAN AS enabled
    , {4}::BIGINT AS display_order
    , '{5}'::TIMESTAMP AS create_time
    , '{6}'::TEXT AS created_by
                ", uid, name, areaID, enabled, displayOrder, createTime, usertAccount);
                listDataRow.Add(data);
            }
            #endregion

            #region 資料儲存
            try
            {
                DAO.DeductionItem.SaveData(string.Join("UNION ALL", listDataRow), this._dicAreaByName[cbxArea.SelectedItem.ToString()].UID);
                MsgBox.Show("資料儲存成功!");
                string areaID = this._dicAreaByName[cbxArea.SelectedItem.ToString()].UID;
                ReloadDataGridView();
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
            #endregion
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    /// <summary>
    /// 扣分物件.顯示順序.排序規則
    /// </summary>
    class sortDeDuctionItem : IComparer<UDT.DeDuctionItem>
    {
        int IComparer<UDT.DeDuctionItem>.Compare(UDT.DeDuctionItem x, UDT.DeDuctionItem y)
        {
            if (x.DisplayOrder > y.DisplayOrder)
            {
                return 1;
            }
            if (x.DisplayOrder < y.DisplayOrder)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
