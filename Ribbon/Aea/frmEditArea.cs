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
    public partial class frmEditArea : BaseForm
    {
        private AccessHelper _access = new AccessHelper();

        private string _userAccount = DAO.Actor.Instance().GetUserAccount();

        private List<UDT.Area> listDeleteData;
        private Dictionary<string, UDT.ScoreRule> _dicScoreRuleByName = new Dictionary<string, UDT.ScoreRule>();
        private Dictionary<string, UDT.ScoreRule> _dicScoreRuleByID = new Dictionary<string, UDT.ScoreRule>();

        public frmEditArea()
        {
            InitializeComponent();
        }

        private void frmEditArea_Load(object sender, EventArgs e)
        {
            // 取得分數準則資料
            List<UDT.ScoreRule> listScoreRule = this._access.Select<UDT.ScoreRule>();
            foreach (UDT.ScoreRule sr in listScoreRule)
            {
                _dicScoreRuleByName.Add(sr.Name,sr);
                _dicScoreRuleByID.Add(sr.UID,sr);
                dgvCbxScoreRule.Items.Add(sr.Name);
            }

            ReloadDataGridView();

            this.listDeleteData = new List<UDT.Area>();
        }

        private void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();
            // 取得區域類別資料
            List<UDT.Area> listArea = this._access.Select<UDT.Area>();

            foreach (UDT.Area area in listArea)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;
                dgvrow.Cells[col++].Value = area.Enabled;
                dgvrow.Cells[col++].Value = area.Name;
                dgvrow.Cells[col++].Value = this._dicScoreRuleByID["" + area.RefRuleID].Name;
                dgvrow.Cells[col++].Value = area.CreatedBy;
                dgvrow.Tag = area;

                dataGridViewX1.Rows.Add(dgvrow);
            }
        }

        private void dataGridViewX1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && string.IsNullOrEmpty("" + dataGridViewX1.Rows[e.RowIndex].Cells[3].Value))
            {
                dataGridViewX1.Rows[e.RowIndex].Cells[3].Value = this._userAccount;
            }
        }

        private bool dgv_Validate()
        {
            int row = 0;

            List<string> listAreaName = new List<string>();

            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (row == dataGridViewX1.Rows.Count - 1)
                {
                    break;
                }
                row++;

                if (string.IsNullOrEmpty(("" + dgvrow.Cells[1].Value).Trim()))
                {
                    MsgBox.Show("名稱欄位不可空白!");
                    return false;
                }
                else
                {
                    if (listAreaName.Contains("" + dgvrow.Cells[1].Value))
                    {
                        MsgBox.Show("名稱欄位不可重複!");
                        return false;
                    }
                    else
                    {
                        listAreaName.Add("" + dgvrow.Cells[1].Value);
                    }
                }

                if (string.IsNullOrEmpty("" + dgvrow.Cells[2].Value))
                {
                    MsgBox.Show("請先設定分數準則!");
                    return false;
                }
                else
                {
                    if (!this._dicScoreRuleByName.ContainsKey("" + dgvrow.Cells[2].Value))
                    {
                        MsgBox.Show(string.Format("{0}分數準則不存在!",dgvrow.Cells[2].Value));
                        return false;
                    }
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgv_Validate()) // 資料驗證
            {
                // 資料整理
                List<UDT.Area> listInsertArea = new List<UDT.Area>();
                List<UDT.Area> listUpdateArea = new List<UDT.Area>();

                int row = 0;
                foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
                {
                    if (row == dataGridViewX1.Rows.Count - 1)
                    {
                        break;
                    }
                    row++;
                    if (dgvrow.Tag != null)
                    {
                        UDT.Area data = (UDT.Area)dgvrow.Tag;
                        data.Enabled = bool.Parse("" + dgvrow.Cells[0].Value);
                        data.Name = "" + dgvrow.Cells[1].Value;
                        data.RefRuleID = int.Parse(this._dicScoreRuleByName["" + dgvrow.Cells[2].Value].UID);
                        data.CreatedBy = "" + dgvrow.Cells[3].Value;

                        listUpdateArea.Add(data);
                    }
                    else
                    {
                        UDT.Area data = new UDT.Area();
                        data.Enabled = bool.Parse("" + dgvrow.Cells[0].Value == "true" ? "true" : "false");
                        data.Name = "" + dgvrow.Cells[1].Value;
                        data.RefRuleID = int.Parse(this._dicScoreRuleByName["" + dgvrow.Cells[2].Value].UID);
                        data.CreatedBy = "" + dgvrow.Cells[3].Value;

                        listInsertArea.Add(data);
                    }
                }

                // 資料儲存
                try
                {
                    if (listInsertArea.Count > 0)
                    {
                        this._access.InsertValues(listInsertArea);
                    }
                    if (listUpdateArea.Count > 0)
                    {
                        this._access.UpdateValues(listUpdateArea);
                    }
                    if (this.listDeleteData.Count > 0)
                    {
                        this._access.DeletedValues(this.listDeleteData);
                    }
                    MsgBox.Show("資料儲存成功!");
                    ReloadDataGridView();
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

        private void dataGridViewX1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (e.Row.Tag != null)
            {
                this.listDeleteData.Add((UDT.Area)e.Row.Tag); // 記錄使用者刪除的區域資料
            }
        }
    }
}
