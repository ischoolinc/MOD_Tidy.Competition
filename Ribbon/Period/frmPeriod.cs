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
    public partial class frmPeriod : BaseForm
    {
        private AccessHelper _access = new AccessHelper();
        private string _userAccount = DAO.Actor.Instance().GetUserAccount();

        public frmPeriod()
        {
            InitializeComponent();
        }

        private void frmPeriod_Load(object sender, EventArgs e)
        {
            ReloadDataGridView();
        }

        private void ReloadDataGridView()
        {
            this.SuspendLayout();

            dataGridViewX1.Rows.Clear();
            // 取得時段資料
            List<UDT.Period> listPeriod = this._access.Select<UDT.Period>();

            foreach (UDT.Period data in listPeriod)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;
                dgvrow.Cells[col++].Value = data.Enabled;
                dgvrow.Cells[col++].Value = data.Name;
                dgvrow.Tag = data;

                dataGridViewX1.Rows.Add(dgvrow);
            }

            this.ResumeLayout();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 資料驗證
            {
                List<string> listPeriodName = new List<string>();
                int row = 0;
                foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
                {
                    if (row == dataGridViewX1.Rows.Count - 1)
                    {
                        break; // 最後一行資料不驗證
                    }
                    row++;
                    if (string.IsNullOrEmpty("" + dgvrow.Cells[1].Value))
                    {
                        dgvrow.Cells[1].ErrorText = "名稱欄位不可空白!";
                        return;
                    }
                    else
                    {
                        if (listPeriodName.Contains("" + dgvrow.Cells[1].Value))
                        {
                            dgvrow.Cells[1].ErrorText = "名稱欄位不可重複!";
                            return;
                        }
                        else
                        {
                            dgvrow.Cells[1].ErrorText = null;
                        }
                    }
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
                    break; // 最後一行部儲存
                }
                rowIndex++;
                string uid = ("" + dgvrow.Tag) == "" ? "null" : ((UDT.Period)dgvrow.Tag).UID;
                string name = "" + dgvrow.Cells[1].Value;
                string enabled = ("" + dgvrow.Cells[0].Value) == "True" ? "true" : "false";
                string createTime = DateTime.Now.ToString("yyyy/MM/dd");
                string userAccount = this._userAccount;
                string data = string.Format(@"
SELECT
{0}::BIGINT AS uid
, '{1}'::TEXT AS name
, {2}::BOOLEAN AS enabled
, '{3}'::TIMESTAMP AS create_time
, '{4}'::TEXT AS created_by
                ", uid, name, enabled, createTime, userAccount);

                listDataRow.Add(data);
            }
            #endregion

            // 資料儲存
            try
            {
                DAO.Period.saveData(string.Join("UNION ALL",listDataRow));
                MsgBox.Show("資料儲存成功!");
                ReloadDataGridView();
            }
            catch(Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
        }

        private void bntLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
