﻿using System;
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
using Aspose.Cells;
using System.IO;
using FISCA.Data;
using FISCA.Presentation;

namespace Ischool.Tidy_Competition
{
    public partial class frmWeeklyScore : BaseForm
    {
        /// <summary>
        /// 記錄週次的日期區間
        /// </summary>
        private class WeekNoDateRange
        {
            public string WeekNumber { get; set; }
            public string StarTime { get; set; }
            public string EndTime { get; set; }
            public string schoolYear { get; set; }
            public string semester { get; set; }
        }

        private Dictionary<string, WeekNoDateRange> dicWeekNoData = new Dictionary<string, WeekNoDateRange>();

        private BackgroundWorker bgw = new BackgroundWorker();

        public frmWeeklyScore()
        {
            InitializeComponent();

            this.bgw.WorkerReportsProgress = true;
            this.bgw.DoWork += new DoWorkEventHandler(this.bgw_DoWork);
            this.bgw.ProgressChanged += new ProgressChangedEventHandler(this.bgw_ProgressChanged);
            this.bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bgw_RunWorkerCompleted);
        }

        private void frmWeeklyScore_Load(object sender, EventArgs e)
        {
            #region InitSchoolYear

            int schoolYear = int.Parse(School.DefaultSchoolYear);
            cbxSchoolYear.Items.Add(schoolYear + 1);
            cbxSchoolYear.Items.Add(schoolYear);
            cbxSchoolYear.Items.Add(schoolYear - 1);

            cbxSchoolYear.SelectedIndex = 1;
            #endregion

            #region InitSemester

            cbxSemester.Items.Add(1);
            cbxSemester.Items.Add(2);

            cbxSemester.SelectedIndex = int.Parse(School.DefaultSemester) - 1;
            #endregion

            dtStartTime.Value = DateTime.Now.AddDays(-4);
            dtEndTime.Value = DateTime.Now;

            getWeekNoData();
        }

        private void getWeekNoData()
        {
            string sql = @"
SELECT DISTINCT
    week_number
    , start_date
    , end_date
    , school_year
    , semester
FROM
    $ischool.tidy_competition.weekly_stats AS stats
";
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                string key = string.Format("{0}_{1}_{2}","" + row["school_year"],"" + row["semester"],"" + row["week_number"]);
                if (!this.dicWeekNoData.ContainsKey(key))
                {
                    this.dicWeekNoData.Add(key,new WeekNoDateRange());
                }
                this.dicWeekNoData[key].schoolYear = "" + row["school_year"];
                this.dicWeekNoData[key].semester = "" + row["semester"];
                this.dicWeekNoData[key].WeekNumber = "" + row["week_number"];
                this.dicWeekNoData[key].StarTime = DateTime.Parse("" + row["start_date"]).ToString("yyyy/MM/dd");
                this.dicWeekNoData[key].EndTime = DateTime.Parse("" + row["end_date"]).ToString("yyyy/MM/dd");

            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (checkWeekNo() && !this.bgw.IsBusy)
            {
                DataObject data = new DataObject();
                data.SchoolYear = cbxSchoolYear.SelectedItem.ToString();
                data.Semester = cbxSemester.SelectedItem.ToString();
                data.WeekNo = int.Parse(tbxWeekNo.Text);
                data.StartTime = dtStartTime.Value.ToString("yyyy/MM/dd");
                data.EndTime = dtEndTime.Value.ToString("yyyy/MM/dd");

                this.btnCalculate.Enabled = false;

                this.bgw.RunWorkerAsync(data);
            }
        }

        private void bgw_DoWork(object sender,DoWorkEventArgs e)
        {
            DataObject data = (DataObject)e.Argument;
            string schoolYear = data.SchoolYear;
            string semester = data.Semester;
            int weekNo = data.WeekNo;
            string startTime = data.StartTime;
            string endTime = data.EndTime;

            string key = string.Format("{0}_{1}_{2}", schoolYear, semester, weekNo);

            if (this.dicWeekNoData.ContainsKey(key))
            {
                DialogResult dResult = MsgBox.Show(string.Format("「{0}」學年度、「{1}」學期、第「{2}」週排名作業以計算過! \n 計算日期區間為{3} ~ {4} \n 確定重新計算?"
                    , schoolYear, semester, weekNo, this.dicWeekNoData[key].StarTime, this.dicWeekNoData[key].EndTime), "提醒", MessageBoxButtons.YesNo);
                if (dResult == DialogResult.Yes)
                {
                    execute(schoolYear, semester, weekNo, startTime, endTime, e);
                }
            }
            else
            {
                execute(schoolYear, semester, weekNo, startTime, endTime, e);
            }
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // 顯示進度條
            MotherForm.SetStatusBarMessage("計算週排名中...", e.ProgressPercentage);
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MsgBox.Show(e.Error.Message);
            }
            else
            {
                this.btnCalculate.Enabled = true;
                MotherForm.SetStatusBarMessage("計算週排名完成。");

                DialogResult result = MsgBox.Show("週排名已計算完成，確定產出排名報表?", "提醒", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    DataTable dt = (DataTable)e.Result;
                    print(dt);
                }
            }
            
        }

        private void execute(string schoolYear,string semester,int weekNo,string startTime,string endTime, DoWorkEventArgs e)
        {
            this.bgw.ReportProgress(15);
            // 1. 統計當週各班成績
            WeeklyStatsCalculator calOne = new WeeklyStatsCalculator(schoolYear, semester, weekNo, startTime, endTime);
            calOne.Execute();
            this.bgw.ReportProgress(30);

            // 2. 計算各年級班排名
            WeeklyRankCalculator calTwo = new WeeklyRankCalculator(schoolYear, semester, weekNo, startTime, endTime);
            calTwo.Execute();
            this.bgw.ReportProgress(30);

            // 3. 找出當週排名
            DataTable dt = getWeeklyRank(schoolYear, semester, weekNo);
            this.bgw.ReportProgress(30);

            e.Result = dt;
        }

        // 驗證週次欄位
        private bool checkWeekNo()
        {
            if (string.IsNullOrEmpty(tbxWeekNo.Text))
            {
                MsgBox.Show("週次欄位不可空白!");
                return false;
            }
            int n;
            if (!int.TryParse(tbxWeekNo.Text, out n))
            {
                MsgBox.Show("週次欄位只能填數值!");
                return false;
            }
            else
            {
                return true;
            }
        }

        private DataTable getWeeklyRank(string schoolYear,string semester,int weekNo)
        {
            string sql = string.Format(@"
SELECT
    rank.*
    , class.class_name
FROM
    $ischool.tidy_competition.weekly_rank AS rank
    LEFT OUTER JOIN class
        ON class.id = rank.ref_class_id
WHERE
    school_year = {0}
    AND semester = {1}
    AND week_number = {2}
ORDER BY
    rank.grade_year
    , rank.rank
    , class.display_order
    , class.class_name
            ",schoolYear,semester,weekNo);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            return dt;
        }

        private void print(DataTable dt)
        {
            Workbook template = new Workbook(new MemoryStream(Properties.Resources.週統計樣板));
            Workbook wb = new Workbook(new MemoryStream(Properties.Resources.週統計樣板));

            int s1rowIndex = 1;
            int s2rowIndex = 1;
            int s3rowIndex = 1;

            foreach (DataRow row in dt.Rows)
            {
                int i = int.Parse("" + row["grade_year"]) - 1;
                if (i == 0)
                {
                    fillSheetData(wb, template, i, s1rowIndex, row);
                    s1rowIndex++;
                }
                if (i == 1)
                {
                    fillSheetData(wb, template, i, s2rowIndex, row);
                    s2rowIndex++;
                }
                if (i == 2)
                {
                    fillSheetData(wb, template, i, s3rowIndex, row);
                    s3rowIndex++;
                }

            }

            #region 儲存資料
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string fileName = string.Format("{0}_{1}_整潔競賽第{2}週_排名結果", cbxSchoolYear.SelectedItem.ToString(), cbxSemester.SelectedItem.ToString(), tbxWeekNo.Text);
            saveFileDialog.Title = fileName;
            saveFileDialog.FileName = string.Format("{0}.xlsx", fileName);
            saveFileDialog.Filter = "Excel (*.xlsx)|*.xlsx|所有檔案 (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = new DialogResult();
                try
                {
                    wb.Save(saveFileDialog.FileName);
                    result = MsgBox.Show("檔案儲存完成，是否開啟檔案?", "是否開啟", MessageBoxButtons.YesNo);
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Show("開啟檔案發生失敗:" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                this.Close();
            }

            #endregion
        }

        // 填工作表資料
        private void fillSheetData(Workbook wb, Workbook template, int sheetNo, int rowIndex, DataRow row)
        {
            // 複製樣板格式
            wb.Worksheets[sheetNo].Cells.CopyRow(template.Worksheets[0].Cells, 1, rowIndex);

            wb.Worksheets[sheetNo].Cells[rowIndex, 0].PutValue("" + row["grade_year"]); // 年級
            wb.Worksheets[sheetNo].Cells[rowIndex, 1].PutValue("" + row["class_name"]); // 班級名稱
            wb.Worksheets[sheetNo].Cells[rowIndex, 2].PutValue("" + row["week_total"]); // 週總分
            wb.Worksheets[sheetNo].Cells[rowIndex, 3].PutValue("" + row["rank"]); // 週排名
            wb.Worksheets[sheetNo].Cells[rowIndex, 4].PutValue("" + row["top2_in_a_row"]); // 前2名已連續幾週
            wb.Worksheets[sheetNo].Cells[rowIndex, 5].PutValue("" + row["top3_in_a_row"]); // 前3名已連續幾週
            wb.Worksheets[sheetNo].Cells[rowIndex, 6].PutValue(("" + row["need_reset"]) == "true" ? "是" : "否"); // 本次已達重新計算條件
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private class DataObject
        {
            public string SchoolYear;
            public string Semester;
            public int WeekNo;
            public string StartTime;
            public string EndTime;
        }
    }
}
