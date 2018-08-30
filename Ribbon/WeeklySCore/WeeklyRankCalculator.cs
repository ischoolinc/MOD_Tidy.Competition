using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;
using FISCA.Data;
using System.Data;
using System.Collections;

namespace Ischool.Tidy_Competition
{
    class WeeklyRankCalculator
    {
        private string _schoolYear;
        private string _semester;
        private int _weekNumber;
        private string _starDatet;
        private string _endDate;
        private string _userAccount = DAO.Actor.Instance().GetUserAccount();
        private AccessHelper _access = new AccessHelper();
        private Dictionary<int, List<string>> _dicAllClassByGrade = new Dictionary<int, List<string>>();
        private Dictionary<string, UDT.WeeklyStats> _dicWeeklyStatsByClassID = new Dictionary<string, UDT.WeeklyStats>();
        private Dictionary<string, UDT.WeeklyRank> _dicLastWeeklyRankByClassID = new Dictionary<string, UDT.WeeklyRank>();

        public WeeklyRankCalculator(string schoolYear, string semester, int weekNumber, string startDate, string endDate)
        {
            this._schoolYear = schoolYear;
            this._semester = semester;
            this._weekNumber = weekNumber;
            this._starDatet = startDate;
            this._endDate = endDate;

            getAllClassData();

            GetWeeklyStats();

            GetLastWeeklyRank();
        }

        // 1. 取得週統計資料
        private void GetWeeklyStats()
        {
            string condition = string.Format(@"school_year = {0} AND semester = {1} AND week_number = {2} AND start_date = '{3}' AND end_date = '{4}'"
                                ,this._schoolYear,this._semester,this._weekNumber,this._starDatet,this._endDate);
            List<UDT.WeeklyStats> listWeeklyStats = this._access.Select<UDT.WeeklyStats>(condition);
            foreach (UDT.WeeklyStats weeklyStats in listWeeklyStats)
            {
                if (!this._dicWeeklyStatsByClassID.ContainsKey("" + weeklyStats.RefClassID))
                {
                    this._dicWeeklyStatsByClassID.Add("" + weeklyStats.RefClassID, weeklyStats);
                }
            }
        }

        // 2. 取得上週週排名資料
        private void GetLastWeeklyRank()
        {
            int lastWeekNo = this._weekNumber - 1;
            string condition = string.Format(@"school_year = {0} AND semester = {1} AND week_number = {2}",this._schoolYear,this._semester, lastWeekNo);
            List<UDT.WeeklyRank>listLastWeeklyRank = this._access.Select<UDT.WeeklyRank>(condition);

            foreach (UDT.WeeklyRank rank in listLastWeeklyRank)
            {
                if (!_dicLastWeeklyRankByClassID.ContainsKey("" + rank.RefClassID))
                {
                    _dicLastWeeklyRankByClassID.Add("" + rank.RefClassID, rank);
                }
            }
        }

        // 取得所有班級資料
        private void getAllClassData()
        {
            string sql = @"
SELECT
    *
FROM
    class
WHERE
    class.grade_year IN (1,2,3)
";
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                int gradeYear = int.Parse("" + row["grade_year"]);
                if (!this._dicAllClassByGrade.ContainsKey(gradeYear))
                {
                    this._dicAllClassByGrade.Add(gradeYear,new List<string>());
                }
                this._dicAllClassByGrade[gradeYear].Add("" + row["id"]);
            }
        }

        private class ScoreSort : IComparer<decimal>
        {
            int IComparer<decimal>.Compare(decimal x, decimal y)
            {
                if (x > y)
                {
                    return -1;
                }
                if (x < y)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public void Execute()
        {
            List<UDT.WeeklyRank> listInsertData = new List<UDT.WeeklyRank>();
            // 1.對於每個年級
            foreach (int grade in _dicAllClassByGrade.Keys)
            {
                List<UDT.WeeklyStats> weeklyStats = new List<UDT.WeeklyStats>();

                // 建立分數名次清單
                List<decimal> listScoreRank = new List<decimal>();

                // 2.找出該年級所有班級
                foreach (string classID in _dicAllClassByGrade[grade])
                {
                    // 3.找出班級週統計成績
                    weeklyStats.Add(this._dicWeeklyStatsByClassID[classID]); // 該年級所有統計資
                    listScoreRank.Add(this._dicWeeklyStatsByClassID[classID].WeekTotal); // 記錄該年級所有分數
                }

                // 4.計算排名
                // 4.0 排序分數名次清單
                listScoreRank.Sort(new ScoreSort());

                foreach (UDT.WeeklyStats ws in weeklyStats)
                {
                    // 找出分數所對應的排名
                    int rank = listScoreRank.IndexOf(ws.WeekTotal) + 1;
                    // 4.1 建立排名物件
                    UDT.WeeklyRank wr = this.createRankObject(ws, rank, grade);

                    // 5. 判斷是否前5週連2
                    checkIfTop2In5Weeks(wr);

                    // 6. 判斷是否連8週前三
                    checkIfTop3In8Weeks(wr);

                    listInsertData.Add(wr);

                }
            }

            // 0.刪除學年度、學期、週次的週排行資料
            List<UDT.WeeklyRank> listRank = this._access.Select<UDT.WeeklyRank>(string.Format("school_year = {0} AND semester = {1} AND week_number = {2}", this._schoolYear, this._semester, this._weekNumber));
            this._access.DeletedValues(listRank);
            // 5.寫入資料庫
            this._access.InsertValues(listInsertData);
        }

        /// <summary>
        /// 判斷是否連五周前2。
        /// 如果本州沒有前2，則次數歸零。
        /// 如果本週有前二：
        ///    a. 如果上週沒有資料，則上週次數視為 0
        ///    b. 如果上週的 needReset 欄位為 true，表示上周已經完成，所以上週次數視為 0
        ///    c. 否則：
        ///         1. 如果上週已經連四週二，則本週會連五週達成，所以次數為5，且 needReset 欄位為 true。
        ///         2.否則，就上週的次數 + 1
        /// </summary>
        /// <param name="wr"></param>
        private void checkIfTop2In5Weeks(UDT.WeeklyRank wr)
        {
            // 如果本週沒有前2
            if (wr.Rank > 2)
            {
                wr.Top2InARow = 0;
            }
            else
            {
                // 找出此班級上週的排名
                UDT.WeeklyRank wrLast = null;

                if (_dicLastWeeklyRankByClassID.ContainsKey("" + wr.RefClassID))
                {
                    wrLast = this._dicLastWeeklyRankByClassID["" + wr.RefClassID];
                }

                // 如果上週沒有資料，或上週的 needReset 欄位為 true，則上週次數視為 0
                if (wrLast == null || wrLast.NeedReset)
                {
                    wr.Top2InARow = 1;
                    return;
                }

                wr.Top2InARow = wrLast.Top2InARow + 1;

                // 如果上週已經連四週二，則本週會連五週達成，所以次數為5，且 needReset 欄位為 true。
                if (wrLast.Top2InARow >= 4)
                {
                    wr.NeedReset = true;
                }
            }
        }

        /// <summary>
        /// 判斷是否連續八周前3。
        /// 如果本週沒有前3，則次數歸零。
        /// 如果本週有前三：
        ///    a. 如果上週沒有資料，則上週次數視為 0
        ///    b. 如果上週的 needReset 欄位為 true，表示上周已經完成，所以上週次數視為 0
        ///    c. 否則：
        ///         1. 如果上週已經連七週三，則本週會連八週達成，所以次數為8，且 needReset 欄位為 true。
        ///         2.否則，就上週的次數 + 1
        /// </summary>
        /// <param name="wr"></param>
        private void checkIfTop3In8Weeks(UDT.WeeklyRank wr)
        {
            // 如果本週沒有前3
            if (wr.Rank > 3)
            {
                wr.Top3InARow = 0;
            }
            else
            {
                // 找出此班級上週的排名
                UDT.WeeklyRank wrLast = null;

                if (_dicLastWeeklyRankByClassID.ContainsKey("" + wr.RefClassID))
                {
                    wrLast = this._dicLastWeeklyRankByClassID["" + wr.RefClassID];
                }

                // 如果上週沒有資料，或上週的 needReset 欄位為 true，則上週次數視為 0
                if (wrLast == null || wrLast.NeedReset)
                {
                    wr.Top3InARow = 1;
                    return;
                }

                wr.Top3InARow = wrLast.Top3InARow + 1;

                // 如果上週已經連四週二，則本週會連五週達成，所以次數為5，且 needReset 欄位為 true。
                if (wrLast.Top3InARow >= 7)
                {
                    wr.NeedReset = true;
                }
            }
        }

        private UDT.WeeklyRank createRankObject(UDT.WeeklyStats ws, int rank, int grade)
        {
            UDT.WeeklyRank wr = new UDT.WeeklyRank();
            wr.RefWeeklyStatsID = int.Parse(ws.UID);
            wr.SchoolYear = int.Parse(this._schoolYear);
            wr.Semester = int.Parse(this._semester);
            wr.RefClassID = ws.RefClassID;
            wr.GradeYear = grade;
            wr.WeekTotal = ws.WeekTotal;
            wr.WeekNumber = this._weekNumber;
            wr.CreateTime = DateTime.Now;
            wr.CreatedBy = this._userAccount;
            wr.PublicTime = DateTime.Now;
            wr.PublicBy = this._userAccount;
            wr.Rank = rank;

            return wr;
        }
    }
}
