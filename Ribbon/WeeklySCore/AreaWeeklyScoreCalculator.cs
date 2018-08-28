using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.Tidy_Competition
{
    /// <summary>
    /// 計算指定區域的週成績
    /// </summary>
    class AreaWeeklyScoreCalculator
    {
        public string AreaID { get; set; }
        public string AreaName { get; set; }
        public decimal WeeklyBaseScore { get; set; }     // 本區域的成績計算基準分，如一般區域是 45分，資源回收為10分等。
        public string ScoreRuleName { get; set; }        // 本區採用的成績計算規則名稱
        public decimal DailyMaxScore { get; set; }       // 本區域每天最多扣幾分? 

        private Dictionary<string, AreaDailyScoreCalculator> dicDailyScores;

        public AreaWeeklyScoreCalculator(string areaID, string areaName, decimal weeklyBaseScore, string scoreRuleName, decimal dailyMaxScore)
        {
            this.AreaID = areaID;
            this.AreaName = areaName;
            this.WeeklyBaseScore = weeklyBaseScore;
            this.ScoreRuleName = scoreRuleName;

            this.dicDailyScores = new Dictionary<string, AreaDailyScoreCalculator>();
        }

        public void Add(ScoreItem si)
        {
            if (!this.dicDailyScores.ContainsKey(si.OccurDate))
            {
                //-------
                AreaDailyScoreCalculator adsc = new AreaDailyScoreCalculator(si.OccurDate);
                this.dicDailyScores.Add(si.OccurDate, adsc);
            }

            this.dicDailyScores[si.OccurDate].Add(si);
        }

        /// <summary>
        /// 計算本區之週成績。
        /// 1. 本週每天的總扣分數(已套用每日扣分上限)之加總  2. 再套用本區域之週統計計算公式
        /// </summary>
        /// <returns></returns>
        public decimal CalculateWeeklyScore()
        {
            IScoreRule scoreRule = ScoreRuleFactory.Get(this.ScoreRuleName);

            // 計算本 週總扣分數
            decimal totalDeduction = 0;
            decimal totalScore = 0;

            foreach (string occurDate in this.dicDailyScores.Keys)
            {
                totalDeduction += this.dicDailyScores[occurDate].CalculateScore(scoreRule.DailyMaxScore);
            }

            // 套用計算公式
            totalScore = scoreRule.Calculate(this.DailyMaxScore,this.WeeklyBaseScore, totalDeduction);

            return totalScore;
        }
    }
}
