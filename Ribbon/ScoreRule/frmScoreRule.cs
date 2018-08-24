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
    public partial class frmScoreRule : BaseForm
    {
        private AccessHelper _access = new AccessHelper();

        public frmScoreRule()
        {
            InitializeComponent();
        }

        private void frmEditScoreRule_Load(object sender, EventArgs e)
        {
            ReloadDataGridView();
        }

        private void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();
            // 取得分數準則資料
            List<UDT.ScoreRule> listScoreRule = this._access.Select<UDT.ScoreRule>();

            foreach (UDT.ScoreRule sr in listScoreRule)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;
                dgvrow.Cells[col++].Value = sr.Name;
                dgvrow.Cells[col++].Value = sr.WeeklyTotal;
                dgvrow.Cells[col++].Value = sr.MacDailyDeduction;
                dgvrow.Cells[col++].Value = sr.Formula;
                dgvrow.Cells[col++].Value = sr.CreatedBy;
                dgvrow.Tag = sr;

                dataGridViewX1.Rows.Add(dgvrow);
            }
            
        }

        private void dataGridViewX1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                frmEditScoreRule form = new frmEditScoreRule(FormMode.Update);
                form.Text = "修改分數準則";
                form.setData((UDT.ScoreRule)dataGridViewX1.Rows[e.RowIndex].Tag);
                form.FormClosed += delegate
                {
                    if (form.DialogResult == DialogResult.Yes)
                    {
                        ReloadDataGridView();
                    }
                };
                form.ShowDialog();
            }
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmEditScoreRule form = new frmEditScoreRule();
            form.FormClosed += delegate 
            {
                if (form.DialogResult == DialogResult.Yes)
                {
                    ReloadDataGridView();
                }
            };
            form.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.SelectedRows[0].Index > -1)
            {
                string name = "" + dataGridViewX1.Rows[dataGridViewX1.SelectedRows[0].Index].Cells[0].Value;
                UDT.ScoreRule data = (UDT.ScoreRule)dataGridViewX1.Rows[dataGridViewX1.SelectedRows[0].Index].Tag;
                DialogResult result = MsgBox.Show(string.Format("確定刪除「{0}」此分數準則?",name), "提醒", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    List<UDT.ScoreRule> listDeleteData = new List<UDT.ScoreRule>();
                    listDeleteData.Add(data);

                    try
                    {
                        this._access.DeletedValues(listDeleteData);
                        MsgBox.Show("資料刪除成功");
                        ReloadDataGridView();
                    }
                    catch(Exception ex)
                    {
                        MsgBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
