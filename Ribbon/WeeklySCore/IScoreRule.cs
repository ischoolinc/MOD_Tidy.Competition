using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.Tidy_Competition
{
    interface IScoreRule
    {
        /// <summary>
        /// 美日扣分上限、區域總分、區域總扣分
        /// </summary>
        /// <param name="dailyMaxScore"></param>
        /// <param name="baseScore"></param>
        /// <param name="totalScore"></param>
        /// <returns></returns>
        decimal Calculate(decimal dailyMaxScore, decimal baseScore ,decimal totalScore);
        decimal DailyMaxScore { get; set; }
    }
}
