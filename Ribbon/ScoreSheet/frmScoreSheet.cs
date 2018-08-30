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
    public partial class frmScoreSheet : BaseForm
    {
        private AccessHelper _access = new AccessHelper();
        private Dictionary<string, UDT.Area> _dicAreaByName = new Dictionary<string, UDT.Area>();
        private Dictionary<string, UDT.Period> _dicPeriodByName = new Dictionary<string, UDT.Period>();
        private bool _initFinsh = false;

        public frmScoreSheet()
        {
            InitializeComponent();
        }

        private void frmScoreSheet_Load(object sender, EventArgs e)
        {
            // Init SchoolYear 
            lbSchoolYear.Text = School.DefaultSchoolYear;
            // Init Semester
            lbSemester.Text = School.DefaultSemester;

            #region Init Area
            {
                // 取得區域資料
                List<UDT.Area> listArea = this._access.Select<UDT.Area>("enabled = true");
                foreach (UDT.Area data in listArea)
                {
                    cbxArea.Items.Add(data.Name);
                    this._dicAreaByName.Add(data.Name, data);
                }
                if (cbxArea.Items.Count > 0)
                {
                    cbxArea.SelectedIndex = 0;
                }
                else
                {
                    MsgBox.Show("請先設定區域資料!");
                    this.Close();
                }
            }
            #endregion

            #region Init Period
            {
                // 取得時段資料
                List<UDT.Period> listPeriod = this._access.Select<UDT.Period>();
                foreach (UDT.Period data in listPeriod)
                {
                    cbxPeriod.Items.Add(data.Name);
                    this._dicPeriodByName.Add(data.Name,data);
                }
                if (cbxPeriod.Items.Count > 0)
                {
                    cbxPeriod.SelectedIndex = 0;
                }
                else
                {
                    MsgBox.Show("請先設定時段資料!");
                    return;
                }
            }
            #endregion

            // Init DateTimeInput
            dateTimeInput1.Value = DateTime.Now;

            ReloadDataGridView();

            this._initFinsh = true;
        }

        private void cbxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._initFinsh)
            {
                ReloadDataGridView();
            }
        }

        private void cbxPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._initFinsh)
            {
                ReloadDataGridView();
            }
        }

        private void dateTimeInput1_ValueChanged(object sender, EventArgs e)
        {
            if (this._initFinsh)
            {
                ReloadDataGridView();
            }
        }

        private void ReloadDataGridView()
        {
            this.SuspendLayout();

            string schoolYear = lbSchoolYear.Text;
            string semester = lbSemester.Text;
            string periodID = this._dicPeriodByName[cbxPeriod.SelectedItem.ToString()].UID;
            string areaID = this._dicAreaByName[cbxArea.SelectedItem.ToString()].UID;
            string date = dateTimeInput1.Value.ToString("yyyy/MM/dd");

            DataTable dt = DAO.ScoreSheet.GetScoreSheet(schoolYear, semester, periodID, areaID, date);

            dataGridViewX1.Rows.Clear();
            foreach (DataRow row in dt.Rows)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int col = 0;
                dgvrow.Cells[col++].Value = "" + row["place_name"];
                dgvrow.Cells[col++].Value = "" + row["class_name"];
                dgvrow.Cells[col++].Value = "" + row["item_name"];
                dgvrow.Cells[col++].Value = "" + row["standard_name"];
                switch ("" + row["身分"])
                {
                    case "管理員":
                        dgvrow.Cells[col++].Value = "" + row["teacher_name"];
                        break;
                    case "評分員":
                        dgvrow.Cells[col++].Value = "" + row["student_name"];
                        break;
                    default:
                        dgvrow.Cells[col++].Value = "";
                        break;
                }
                dgvrow.Cells[col++].Value = "" + row["身分"];
                dgvrow.Cells[col++].Value = "" + row["checked_time"] == "0001/01/01 00:00:00" ? "" : "" + row["checked_time"];
                dgvrow.Cells[col++].Value = "" + row["checked_name"];
                dgvrow.Tag = row; //"" + row["uid"]; // 評分紀錄編號

                dataGridViewX1.Rows.Add(dgvrow);
            }

            this.ResumeLayout();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddScoreSheet form = new frmAddScoreSheet();
            form.FormClosed += delegate
            {
                if (form.DialogResult == DialogResult.Yes)
                {
                    ReloadDataGridView();
                }
            };
            form.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.SelectedRows.Count > 0)
            {
                frmEditScoreSheet form = new frmEditScoreSheet((DataRow)dataGridViewX1.SelectedRows[0].Tag);
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

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
