using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.Tidy_Competition.Ribbon.WeeklySCore
{
    class AreaDailyScoreCalculator
    {
        public string OccurDate { get; set; }

        private List<ScoreItem> scoreItems ;

        public AreaDailyScoreCalculator(string occurDate)
        {
            this.OccurDate = occurDate;
            this.scoreItems = new List<ScoreItem>();
        }

        public void Add(ScoreItem si)
        {
            this.scoreItems.Add(si);
        }

        public decimal CalculateScore(decimal dailyMaxScore)
        {
            decimal total = 0;

            foreach(ScoreItem si in this.scoreItems)
            {
                total += si.Score;
            }
            if (total > dailyMaxScore)
            {
                total = dailyMaxScore;
            }

            return total;
        }
    }
}
