using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.Data;
using System.Data;

namespace Ischool.Tidy_Competition
{
    /// <summary>
    /// 計算某班級週成績的類別
    /// </summary>
    class ClassWeeklyScoreCalculator
    {
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public int GradeYear { get; set; }
        public decimal TotalScore { get; set; }

        /// <summary>
        /// 班級各區成績 : jason
        /// </summary>
        public List<string> ListAreaScore = new List<string>();
        private Dictionary<string, AreaWeeklyScoreCalculator> dicAreas;

        public ClassWeeklyScoreCalculator(string classID, string className,int gradeYear)
        {
            this.ClassID = classID;
            this.ClassName = className;
            this.GradeYear = gradeYear;

            getAreas();
        }

        private void getAreas()
        {
            this.dicAreas = new Dictionary<string, AreaWeeklyScoreCalculator>();
            //--- Get Areas of this class from DB, then add to dictionary dicArea.
            string sql = string.Format(@"
SELECT DISTINCT
    area.uid
    , area.name
    , score_rule.formula 
    , score_rule.weekly_total
    , score_rule.max_daily_deduction
FROM
    $ischool.tidy_competition.area AS area
    LEFT OUTER JOIN $ischool.tidy_competition.score_rule AS score_rule
        ON score_rule.uid = area.ref_rule_id
    LEFT OUTER JOIN $ischool.tidy_competition.place AS place
        ON place.ref_area_id = area.uid
    LEFT OUTER JOIN $ischool.tidy_competition.pc_belong AS pc_belong
        ON pc_belong.ref_place_id = place.uid
WHERE
    pc_belong.ref_class_id = {0}
            ",this.ClassID);
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                string areaID = "" + row["uid"];
                string areaName = "" + row["name"];
                decimal weeklyBaseScore = decimal.Parse("" + row["weekly_total"]);
                string formula = "" + row["formula"];
                decimal dailyMaxScore = decimal.Parse("" + row["max_daily_deduction"]);

                if (!this.dicAreas.ContainsKey(areaID))
                {
                    this.dicAreas.Add(areaID, new AreaWeeklyScoreCalculator(areaID, areaName, weeklyBaseScore, formula, dailyMaxScore));
                }
            }
        }

        /// <summary>
        /// 加入一筆扣分紀錄
        /// </summary>
        /// <param name="si"></param>
        public void Add(ScoreItem si)
        {
            if (!this.dicAreas.ContainsKey(si.AreaID))
            {
                return;
            }

            this.dicAreas[si.AreaID].Add(si);
        }

        /// <summary>
        /// 計算班級週總分。
        /// 班級週總分的規則為班級負責的各區的週總分之總和
        /// </summary>
        /// <returns></returns>
        public decimal CalculateWeeklyScore()
        {
            decimal totalScore = 0;
            foreach(string areaID in this.dicAreas.Keys)
            {
                string areaNmae = this.dicAreas[areaID].AreaName;
                decimal score = this.dicAreas[areaID].CalculateWeeklyScore();
                totalScore += score; // 加總各區週成績

                string data = string.Format("{{\"區域名稱\": {0} ,\"Score\": {1}}}", areaNmae, score);
                this.ListAreaScore.Add(data);
            }
            this.TotalScore = totalScore;
            
            // 記錄班級各區週得分
            return totalScore;
        }

    }
}
