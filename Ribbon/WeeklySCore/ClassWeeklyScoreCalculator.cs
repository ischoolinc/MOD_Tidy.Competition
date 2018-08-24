using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.Tidy_Competition.Ribbon.WeeklySCore
{
    /// <summary>
    /// 計算某班級週成績的類別
    /// </summary>
    class ClassWeeklyScoreCalculator
    {
        public string ClassID { get; set; }
        public string ClassName { get; set; }

        private Dictionary<string, AreaWeeklyScoreCalculator> dicAreas;

        public ClassWeeklyScoreCalculator(string classID, string className)
        {
            this.ClassID = classID;
            this.ClassName = className;

            getAreas();
        }

        private void getAreas()
        {
            this.dicAreas = new Dictionary<string, AreaWeeklyScoreCalculator>();
            //--- Get Areas of this class from DB, then add to dictionary dicArea.

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
                totalScore += this.dicAreas[areaID].CalculateWeeklyScore();
            }
            return totalScore;
        }

    }
}
