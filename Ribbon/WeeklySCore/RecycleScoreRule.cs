using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.Tidy_Competition.Ribbon.WeeklySCore
{
    class RecycleScoreRule : IScoreRule
    {
        public decimal DailyMaxScore { get ; set ; }

        public decimal Calculate(decimal totalScore)
        {
            throw new NotImplementedException();
        }
    }
}
