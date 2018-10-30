using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FISCA.Presentation.Controls;
using K12.Data;
using FISCA.Data;
using Aspose.Cells;

namespace Ischool.Tidy_Competition
{
    public partial class frmWeeklyScoreSheetReport : BaseForm
    {
        private bool _initFinish = false;
        private Dictionary<string, DataRow> _dicAreaDataByName = new Dictionary<string, DataRow>();

        private QueryHelper _qh = new QueryHelper();

        private class WeeklyDate
        {
            public string WeekNo { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        private class ClassRecord
        {
            public string ClassName { get; set; }
            public string Score { get; set; }
            public string Rank { get; set; }
        }

        private Dictionary<string, WeeklyDate> _dicWeekDataByWeekNo = new Dictionary<string, WeeklyDate>();

        public frmWeeklyScoreSheetReport()
        {
            InitializeComponent();
        }

        private void frmWeeklyScoreSheetReport_Load(object sender, EventArgs e)
        {
            int schoolYear = int.Parse(School.DefaultSchoolYear);
            int semester = int.Parse(School.DefaultSemester);
            // Init SchoolYear
            cbxSchoolYear.Items.Add(schoolYear - 1);
            cbxSchoolYear.Items.Add(schoolYear);
            cbxSchoolYear.Items.Add(schoolYear + 1);
            cbxSchoolYear.SelectedIndex = 1;
            // Init Semester
            cbxSemester.Items.Add(1);
            cbxSemester.Items.Add(2);
            cbxSemester.SelectedIndex = semester - 1;
            // Init WeekNo
            ReloadWeekNo();

            GetAreaData();

            this._initFinish = true;
        }

        private void GetAreaData()
        {
            this._dicAreaDataByName.Clear();

            string sql = @"
SELECT
    area.name
    , rule.weekly_total
    , max_daily_deduction
    , formula
FROM
    $ischool.tidy_competition.area AS area
    LEFT OUTER JOIN $ischool.tidy_competition.score_rule AS rule
        ON area.ref_rule_id = rule.uid
";

            DataTable dt = this._qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                if (!this._dicAreaDataByName.ContainsKey("" + row["name"]))
                {
                    this._dicAreaDataByName.Add("" + row["name"],row);
                }
            }
        }

        private void ReloadWeekNo()
        {
            cbxWeekNo.Items.Clear();
            this._dicWeekDataByWeekNo.Clear();

            #region SQL
            string sql = string.Format(@"
SELECT DISTINCT
    week_number
	, start_date
	, end_date
FROM
    $ischool.tidy_competition.weekly_stats
WHERE
    school_year = {0}
    AND semester = {1}
ORDER BY
    week_number
            ", cbxSchoolYear.SelectedItem.ToString(), cbxSemester.SelectedItem.ToString()); 
            #endregion

            DataTable dt = this._qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                string weekNo = "" + row["weeK_number"];
                cbxWeekNo.Items.Add(weekNo);
                if (!this._dicWeekDataByWeekNo.ContainsKey(weekNo))
                {
                    this._dicWeekDataByWeekNo.Add(weekNo,new WeeklyDate());
                }
                this._dicWeekDataByWeekNo[weekNo].WeekNo = weekNo;
                this._dicWeekDataByWeekNo[weekNo].StartDate = DateTime.Parse("" + row["start_date"]).ToString("yyyy/MM/dd");
                this._dicWeekDataByWeekNo[weekNo].EndDate = DateTime.Parse("" + row["start_date"]).AddDays(4).ToString("yyyy/MM/dd");
            }
            if (cbxWeekNo.Items.Count > 0)
            {
                cbxWeekNo.SelectedIndex = 0;
            }
        }

        private void cbxSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._initFinish)
            {
                ReloadWeekNo();
            }
        }

        private void cbxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._initFinish)
            {
                ReloadWeekNo();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string schoolYear = cbxSchoolYear.SelectedItem.ToString();
            string semester = cbxSemester.SelectedItem.ToString();
            string weekNo = cbxWeekNo.SelectedItem.ToString();
            string startDate = this._dicWeekDataByWeekNo[weekNo].StartDate;
            string endDate = this._dicWeekDataByWeekNo[weekNo].EndDate;

            DateTime d = DateTime.Parse(startDate);
            List<DateTime> _listDate = new List<DateTime>();
            _listDate.Add(d);
            _listDate.Add(d.AddDays(1));
            _listDate.Add(d.AddDays(2));
            _listDate.Add(d.AddDays(3));
            _listDate.Add(d.AddDays(4));

            #region SQL
            string sql = string.Format(@"
WITH data_row AS(
	SELECT
		score_sheet.create_time
		, class.id AS ref_class_id
		, area.name
		, rule.max_daily_deduction
		, count(deduction.points)
	FROM
		$ischool.tidy_competition.snapshot_score_sheet AS score_sheet
		LEFT OUTER JOIN $ischool.tidy_competition.place AS place
			ON place.uid = score_sheet.ref_place_id
		LEFT OUTER JOIN $ischool.tidy_competition.pc_belong AS pc_belong
			ON pc_belong.ref_place_id = place.uid
		LEFT OUTER JOIN class
			ON class.id = pc_belong.ref_class_id
		LEFT OUTER JOIN $ischool.tidy_competition.area AS area
			ON area.uid = place.ref_area_id
		LEFT OUTER JOIN $ischool.tidy_competition.score_rule AS rule
			ON rule.uid = area.ref_rule_id
		LEFT OUTER JOIN $ischool.tidy_competition.deduction_standard AS deduction
			ON deduction.uid = score_sheet.ref_deduction_standard_id
	WHERE
		score_sheet.school_year = {0}
		AND score_sheet.semester = {1}
		AND date_trunc('day', score_sheet.create_time) >= '{2}' 
		AND date_trunc('day', score_sheet.create_time) <= '{3}'
	GROUP BY 
		score_sheet.create_time
		, class.id
		, area.name
		, rule.max_daily_deduction
) 
SELECT
	class.class_name
	, rank.grade_year
	, data_row.create_time
	, data_row.name AS area_name
	--, data_row.max_daily_deduction
	--, data_row.count
	, CASE 
		WHEN data_row.count > data_row.max_daily_deduction 
		THEN data_row.max_daily_deduction 
		ELSE data_row.count 
		END AS points
	, rank.week_total
	, rank.rank
FROM
    $ischool.tidy_competition.weekly_rank AS rank
    LEFT OUTER JOIN class
        ON class.id = rank.ref_class_id
    LEFT OUTER JOIN data_row
    	ON data_row.ref_class_id = class.id
WHERE
    school_year = {0}
    AND semester = {1}
    AND week_number = {4}
ORDER BY
    rank.grade_year
    , rank.rank
    , class.display_order
    , class.class_name
                ", schoolYear,semester,startDate,endDate,weekNo);
            #endregion

            DataTable dt = this._qh.Select(sql);
            Dictionary<string, Dictionary<string, Dictionary<string, DataRow>>> dicClassDailyAreaScore = new Dictionary<string, Dictionary<string, Dictionary<string, DataRow>>>();
            Dictionary<string, ClassRecord> dicClassRecordByName = new Dictionary<string, ClassRecord>();

            #region 資料整理
            foreach (DataRow row in dt.Rows)
            {
                string className = "" + row["class_name"];

                #region 資料整理1
                if (!dicClassDailyAreaScore.ContainsKey(className)) // 班級
                {
                    dicClassDailyAreaScore.Add(className, new Dictionary<string, Dictionary<string, DataRow>>());
                }
                if (!string.IsNullOrEmpty("" + row["create_time"]))  // 有日期資料
                {
                    string createTime = DateTime.Parse("" + row["create_time"]).ToString("yyyy/MM/dd");

                    if (!dicClassDailyAreaScore[className].ContainsKey(createTime)) // 日期
                    {
                        dicClassDailyAreaScore[className].Add(createTime, new Dictionary<string, DataRow>());
                    }

                    if (!string.IsNullOrEmpty("" + row["area_name"])) // 有區域類別資料
                    {
                        string areaName = "" + row["area_name"];

                        if (!dicClassDailyAreaScore[className][createTime].ContainsKey(areaName))
                        {
                            dicClassDailyAreaScore[className][createTime].Add(areaName, row);
                        }
                    }
                }
                #endregion

                #region 資料整理2
                if (!dicClassRecordByName.ContainsKey(className))
                {
                    dicClassRecordByName.Add(className,new ClassRecord());
                }
                dicClassRecordByName[className].ClassName = className;
                dicClassRecordByName[className].Score = "" + row["week_total"];
                dicClassRecordByName[className].Rank = "" + row["rank"];
                #endregion
            } 
            #endregion

            Workbook template = new Workbook(new MemoryStream(Properties.Resources.整潔競賽評分表樣板));
            Workbook wb = new Workbook();

            int areaCount = this._dicAreaDataByName.Keys.Count;
            int totalCol = 0;

            #region Copy樣板
            {
                int col = 0;
                wb.Worksheets[0].Cells.CopyColumn(template.Worksheets[0].Cells, 0, col++);

                for (int day = 1; day <= 6; day++) // 5天+分區小計
                {
                    for (int i = 1; i <= areaCount; i++)
                    {
                        if (i == 1)
                        {
                            wb.Worksheets[0].Cells.CopyColumn(template.Worksheets[0].Cells, 1, col++);
                        }
                        else if (i == areaCount)
                        {
                            wb.Worksheets[0].Cells.CopyColumn(template.Worksheets[0].Cells, 3, col++);
                        }
                        else
                        {
                            wb.Worksheets[0].Cells.CopyColumn(template.Worksheets[0].Cells, 2, col++);
                        }
                    }
                }

                wb.Worksheets[0].Cells.CopyColumn(template.Worksheets[0].Cells, 4, col++);
                wb.Worksheets[0].Cells.CopyColumn(template.Worksheets[0].Cells, 5, col);

                totalCol = col;
            } 
            #endregion

            // FillData
            wb.Worksheets[0].Name = "整潔競賽評分表";
            wb.Worksheets[0].Cells[0, 0].Value = string.Format("國立彰化師大附工{0}學年度第{1}學期整潔競賽評分表",schoolYear,semester);
            wb.Worksheets[0].Cells.Merge(0, 0, 1, totalCol);
            wb.Worksheets[0].Cells[2, 4].Value = string.Format("第{0}週", weekNo);

            #region 填打掃區域類別
            {
                int col = 1;
                for (int day = 1; day <= 6; day++) // 5天 + 分區小計
                {
                    if (day == 6) // 分區小計
                    {
                        wb.Worksheets[0].Cells[3, col].Value = "分區小計";
                        wb.Worksheets[0].Cells.Merge(3, col, 1, areaCount);

                        foreach (string areaName in this._dicAreaDataByName.Keys)
                        {
                            DataRow row = this._dicAreaDataByName[areaName];
                            wb.Worksheets[0].Cells[4, col].Value = "" + row["weekly_total"];
                            wb.Worksheets[0].Cells[5, col++].Value = areaName;
                        }
                    }
                    else
                    {
                        foreach (string areaName in this._dicAreaDataByName.Keys)
                        {
                            wb.Worksheets[0].Cells[5, col++].Value = areaName;
                        }
                    }
                }
            } 
            #endregion

            #region 填日期、填星期
            {
                int col = 1;

                foreach (DateTime date in _listDate)
                {
                    // 填日期
                    wb.Worksheets[0].Cells[3, col].Value = date.ToString("MM月dd日");
                    wb.Worksheets[0].Cells.Merge(3, col, 1, areaCount);

                    // 填星期
                    wb.Worksheets[0].Cells[4, col].Value = ParseDayOfWeek(date.DayOfWeek.ToString("d"));
                    wb.Worksheets[0].Cells.Merge(4 , col, 1, areaCount);

                    col += areaCount;
                }
            }
            #endregion

            #region 填班級每日區域扣分數
            {
                int rowIndex = 6;
                foreach (string className in dicClassDailyAreaScore.Keys)
                {
                    #region CopyRow
                    
                    if (rowIndex == 6) // 第一行
                    {
                        wb.Worksheets[0].Cells.CopyRow(wb.Worksheets[0].Cells, 6, rowIndex);
                    }
                    else if (rowIndex == 6 + dicClassDailyAreaScore.Keys.Count) // 最後一行
                    {
                        wb.Worksheets[0].Cells.CopyRow(wb.Worksheets[0].Cells, 8, rowIndex);
                    }
                    else
                    {
                        wb.Worksheets[0].Cells.CopyRow(wb.Worksheets[0].Cells, 7, rowIndex);
                    }
                    // 清空複製的資料
                    for (int colIndex = 1; colIndex <= totalCol;colIndex++)
                    {
                        wb.Worksheets[0].Cells.GetRow(rowIndex)[colIndex].Value = null;
                    }
                    #endregion

                    int col = 0;

                    wb.Worksheets[0].Cells[rowIndex, col++].Value = className; // 班級

                    Dictionary<string, int> dicTotalScoreByArea = new Dictionary<string, int>();

                    // 每天評分紀錄
                    foreach (DateTime _date in _listDate) // 每天
                    {
                        string date = _date.ToString("yyyy/MM/dd");

                        if (dicClassDailyAreaScore[className].ContainsKey(date)) // 有日期資料
                        {
                            foreach (string _areaName in this._dicAreaDataByName.Keys)  // 每個區域類別
                            {
                                if (dicClassDailyAreaScore[className][date].ContainsKey(_areaName))// 有區域資料
                                {
                                    int points = int.Parse("" + dicClassDailyAreaScore[className][date][_areaName]["points"]);
                                    wb.Worksheets[0].Cells[rowIndex, col++].Value = string.Format("-{0}",points);

                                    if (!dicTotalScoreByArea.ContainsKey(_areaName))
                                    {
                                        dicTotalScoreByArea.Add(_areaName, 0);
                                    }
                                    dicTotalScoreByArea[_areaName] += points; // 紀錄區域總分
                                }
                                else
                                {
                                    col++;
                                }
                            }
                        }
                        else
                        {
                            col+= areaCount;
                        }
                        
                    }

                    // 分區小計
                    foreach (string areaName in this._dicAreaDataByName.Keys)
                    {
                        if (dicTotalScoreByArea.ContainsKey(areaName))
                        {
                            wb.Worksheets[0].Cells[rowIndex, col++].Value = string.Format("-{0}%", dicTotalScoreByArea[areaName]);
                        }
                        else
                        {
                            col++;
                        }
                    }

                    if (dicClassRecordByName.ContainsKey(className))
                    {
                        // 總分
                        wb.Worksheets[0].Cells[rowIndex, col++].Value = dicClassRecordByName[className].Score;
                        // 排名
                        wb.Worksheets[0].Cells[rowIndex, col++].Value = dicClassRecordByName[className].Rank;
                    }
                    else
                    {
                        col++;
                    }

                    rowIndex++;
                }
            }
            #endregion

            #region 儲存資料
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string fileName = string.Format("國立彰化師大附工{0}學年度第{1}學期整潔競賽評分表", schoolYear, semester);
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

        private string ParseDayOfWeek(string d)
        {
            switch (d)
            {
                case ("1"):
                    return "星期一";
                case ("2"):
                    return "星期二";
                case ("3"):
                    return "星期三";
                case ("4"):
                    return "星期四";
                case ("5"):
                    return "星期五";
                case ("6"):
                    return "星期六";
                case ("0"):
                    return "星期日";
                default:
                    return "";
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
