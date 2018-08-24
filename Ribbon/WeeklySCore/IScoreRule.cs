using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.Tidy_Competition.Ribbon.WeeklySCore
{
    interface IScoreRule
    {
        decimal Calculate(decimal totalScore);
        decimal DailyMaxScore { get; set; }
    }
}
