using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FISCA.Data;
using K12.Data;
using FISCA.UDT;
using FISCA.Presentation.Controls;

namespace Ischool.Tidy_Competition
{
    class WeeklyStatsCalculator
    {
        private string _schoolYear;
        private string _semester;
        private int _weekNumber;
        private string _startDate;
        private string _endDate;
        private string _userName = DAO.Actor.Instance().GetUserAccount();
        private AccessHelper _access = new AccessHelper();
        private Dictionary<string, ClassWeeklyScoreCalculator> dicClassCalculatorByID = new Dictionary<string, ClassWeeklyScoreCalculator>();

        public WeeklyStatsCalculator(string schoolYear, string semester, int weekNumber, string startDate, string endDate)
        {
            this._schoolYear = schoolYear;
            this._semester = semester;
            this._weekNumber = weekNumber;
            this._startDate = startDate;
            this._endDate = endDate;

            // 取得全校所有班級
            getClassData();

        }

        private void getClassData()
        {
            string sql = @"
SELECT
    id
    , class_name
    , grade_year
FROM
    class
WHERE
    class.grade_year IN (1,2,3)
";
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                if (!dicClassCalculatorByID.ContainsKey("" + row["id"]))
                {
                    ClassWeeklyScoreCalculator calculator = new ClassWeeklyScoreCalculator("" + row["id"], "" + row["class_name"], int.Parse("" + row["grade_year"]));
                    dicClassCalculatorByID.Add("" + row["id"], calculator);
                }
            }
        }

        // 1. 取得學年度、學期、日期區間、沒有被取消的【評分紀錄】
        private void GetScoreSheet()
        {
            #region SQL
            string sql = string.Format(@"
SELECT
    place.ref_area_id
    , pc_belong.ref_class_id
    , standard.points
    , score_sheet.create_time
FROM
    $ischool.tidy_competition.score_sheet AS score_sheet
    LEFT OUTER JOIN $ischool.tidy_competition.place AS place
        ON place.uid = score_sheet.ref_place_id
    LEFT OUTER JOIN $ischool.tidy_competition.pc_belong AS pc_belong
        ON pc_belong.ref_place_id = score_sheet.ref_place_id
    LEFT OUTER JOIN $ischool.tidy_competition.deduction_standard AS standard
        ON standard.uid = score_sheet.ref_deduction_standard_id
    WHERE
        score_sheet.school_year = {0}
        AND score_sheet.semester = {1}
        AND date_trunc('day', score_sheet.create_time) <= '{2}'::TIMESTAMP
        AND date_trunc('day', score_sheet.create_time) >= '{3}'::TIMESTAMP
        AND score_sheet.is_canceled = false
            ", this._schoolYear, this._semester, this._endDate, this._startDate); 
            #endregion

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                if (this.dicClassCalculatorByID.ContainsKey("" + row["ref_class_id"]))
                {
                    ScoreItem item = new ScoreItem();
                    item.AreaID = "" + row["ref_area_id"];
                    item.ClassID = "" + row["ref_class_id"];
                    item.Score = int.Parse("" + row["points"]);
                    item.OccurDate = "" + row["create_time"];

                    this.dicClassCalculatorByID[item.ClassID].Add(item);
                }
            }
        }

        public void Execute()
        {
            GetScoreSheet();

            List<UDT.WeeklyStats> listInsertData = new List<UDT.WeeklyStats>();

            // 1.計算各班週成績
            foreach (string classID in this.dicClassCalculatorByID.Keys)
            {
                ClassWeeklyScoreCalculator calculator = this.dicClassCalculatorByID[classID];
                // 取得各班週成績
                decimal weeklyScore = calculator.CalculateWeeklyScore();
                // 取得班級各區週成績
                List<string> listAreaScore = calculator.ListAreaScore;
                // 資料整理
                UDT.WeeklyStats data = new UDT.WeeklyStats();
                data.SchoolYear = int.Parse(this._schoolYear);
                data.Semester = int.Parse(this._semester);
                data.StartDate = DateTime.Parse(this._startDate);
                data.EndDate = DateTime.Parse(this._endDate);
                data.RefClassID = int.Parse(classID);
                data.GradeYear = calculator.GradeYear;
                data.WeekTotal = weeklyScore;
                data.AreaWeeklyTotal = string.Join(",", listAreaScore);
                data.WeekNumber = this._weekNumber;
                data.CreateTime = DateTime.Now;
                data.CreatedBy = this._userName;

                listInsertData.Add(data);
            }

            // 0. 刪除日期區間週統計
            List<UDT.WeeklyStats> listWeeklyStats = this._access.Select<UDT.WeeklyStats>(string.Format("school_year = {0} AND semester = {1} AND week_number = {2}", this._schoolYear, this._semester, this._weekNumber));
            this._access.DeletedValues(listWeeklyStats);

            // 2.更新資料庫
            this._access.InsertValues(listInsertData);
        }
    }
}
