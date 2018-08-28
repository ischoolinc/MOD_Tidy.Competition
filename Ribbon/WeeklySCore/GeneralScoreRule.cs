using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.Tidy_Competition
{
    /// <summary>
    /// 一般週統計計算公式
    /// </summary>
    class GeneralScoreRule : IScoreRule
    {
        public decimal DailyMaxScore { get; set; }

        /// <summary>
        /// 計算區域週分數
        /// </summary>
        /// <param name="dailyMaxScore"></param>
        /// <param name="baseScore"></param>
        /// <param name="totalDeduction"></param>
        /// <returns></returns>
        public decimal Calculate(decimal dailyMaxScore, decimal baseScore ,decimal totalDeduction)
        {
            return baseScore * (1 - (totalDeduction / 100));
        }
    }
}
