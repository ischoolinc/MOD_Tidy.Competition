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
    public partial class frmScorer : BaseForm
    {
        private AccessHelper _access = new AccessHelper();
        private UpdateHelper _up = new UpdateHelper();

        public frmScorer()
        {
            InitializeComponent();
        }

        private void frmScorer_Load(object sender, EventArgs e)
        {
            // Init SchoolYear
            int schoolYear = int.Parse(School.DefaultSchoolYear);
            cbxSchoolYear.Items.Add(schoolYear - 1);
            cbxSchoolYear.Items.Add(schoolYear);
            cbxSchoolYear.Items.Add(schoolYear + 1);

            cbxSchoolYear.SelectedIndex = 1;

            // Init dgv Identity Column
            dgvIdentity.Items.Add("評分員");
            dgvIdentity.Items.Add("評分員幹部");
        }

        private void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();
            // 取得評分員資料
            DataTable dt = DAO.Scorer.GetScorerDataBySchoolYear(cbxSchoolYear.SelectedItem.ToString());
            lbScorerCount.Text = string.Format("評分員人數: {0}位",dt.Rows.Count);
            foreach (DataRow row in dt.Rows)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;
                dgvrow.Cells[col++].Value = "" + row["grade_year"];
                dgvrow.Cells[col++].Value = "" + row["class_name"];
                dgvrow.Cells[col++].Value = "" + row["name"];
                dgvrow.Cells[col++].Value = "" + row["student_number"];
                dgvrow.Cells[col++].Value = ("" + row["is_leader"]) == "true" ? "評分員幹部" : "評分員";
                dgvrow.Cells[col++].Value = "" + row["account"];
                dgvrow.Cells[col++].Value = "" + row["code"];
                dgvrow.Cells[col++].Value = "刪除";
                dgvrow.Tag = "" + row["uid"]; // 評分員編號

                dataGridViewX1.Rows.Add(dgvrow);
            }

        }

        private void cbxSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> listDataRow = new List<string>();
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                string data = string.Format(@"
SELECT
    {0}::BIGINT AS uid
    , {1}::BOOLEAN AS is_leader
    , '{2}'::TEXT AS code
                ","" + dgvrow.Tag,"" + dgvrow.Cells[4].Value == "評分員幹部" ? "true" : "false", "" + dgvrow.Cells[6].Value);
                listDataRow.Add(data);
            }

            if (listDataRow.Count > 0)
            {
                string sql = string.Format(@"
WITH data_row AS(
    {0}
)
UPDATE $ischool.tidy_competition.scorer SET
    is_leader = data_row.is_leader
    , code = data_row.code
FROM
    data_row
WHERE
    $ischool.tidy_competition.scorer.uid = data_row.uid
            ", string.Join("UNION ALL", listDataRow));

                try
                {
                    this._up.Execute(sql);
                    MsgBox.Show("資料更新成功!");
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }
            }
            else
            {
                MsgBox.Show("沒有可更新資料!");
            }
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddScorer form = new frmAddScorer(cbxSchoolYear.SelectedItem.ToString());
            form.FormClosed += delegate
            {
                if (form.DialogResult == DialogResult.Yes)
                {
                    ReloadDataGridView();
                }
            };
            form.ShowDialog();
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 7)
            {
                string name = "" + dataGridViewX1.Rows[e.RowIndex].Cells[2].Value;
                DialogResult result = MsgBox.Show(string.Format("確定刪除學生{0}評分員身分?", name),"提醒",MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    List<UDT.Scorer> listTargetScorer = this._access.Select<UDT.Scorer>(string.Format("uid = {0}", "" + dataGridViewX1.Rows[e.RowIndex].Tag));

                    try
                    {
                        this._access.DeletedValues(listTargetScorer);
                        MsgBox.Show("資料刪除成功!");
                        ReloadDataGridView();
                    }
                    catch(Exception ex)
                    {
                        MsgBox.Show(ex.Message);
                    }
                }
            }
        }

        
    }
}
