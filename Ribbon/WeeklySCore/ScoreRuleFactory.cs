using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.Tidy_Competition.Ribbon.WeeklySCore
{
    class ScoreRuleFactory
    {
        public static IScoreRule Get(string scoreRuleName)
        {
            if (scoreRuleName == "")
            {
                return new GeneralScoreRule();
            }
            else
            {
                return new RecycleScoreRule();
            }
        }
    }
}
