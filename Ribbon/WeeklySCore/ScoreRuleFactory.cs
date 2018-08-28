using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.Tidy_Competition
{
    class ScoreRuleFactory
    {
        public static IScoreRule Get(string scoreRuleName)
        {
            IScoreRule result = null;

            if (scoreRuleName == "一般週統計計算公式")
            {
                result =  new GeneralScoreRule();
            }
            else // 資源回收週統計計算公式 
            {
                result =  new RecycleScoreRule();
            }

            return result;
        }
    }
}
