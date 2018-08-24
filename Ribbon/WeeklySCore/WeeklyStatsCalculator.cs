using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FISCA.Data;

namespace Ischool.Tidy_Competition
{
    class WeeklyStatsCalculator
    {
        private string _schoolYear;
        private string _semester;
        private int _weekNumber;
        private string _startDate;
        private string _endDate;

        public WeeklyStatsCalculator(string schoolYear, string semester, int weekNumber, string startDate, string endDate)
        {
            this._schoolYear = schoolYear;
            this._semester = semester;
            this._weekNumber = weekNumber;
            this._startDate = startDate;
            this._endDate = endDate;

            // 取得全校所有班級
        }
        
        // 0. 公式
        /// <summary>
        /// 一般週統計計算公式
        /// x: 區域類別總分，y: 總扣分數
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int CalculatorA(int x,int y)
        {
            return x * (1 - (y / 100));
        }
        /// <summary>
        /// 資源回收統計計算公式
        ///  x: 區域類別總分，y: 總扣分數
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int CalculatorB(int x,int y)
        {
            return x * (1 - (y / 10));
        }
        // 1. 取得學年度、學期、日期區間、沒有被取消的【評分紀錄】
        private void GetScoreSheet()
        {
            string sql = string.Format(@"
SELECT
FROM
    $ischool.tidy_competition.score_sheet AS score_sheet
    LEFT OUTER JOIN $ischool.tidy_competition.deduction_standard AS standard
        ON standard.uid = score_sheet.ref_deduction_standard_id
    WHERE
        score_sheet.
");
        }
        // 2. 資料整理【班級】【區域】【天】
        // 2.1 根據分數準則統計週總扣分
        // 2.2 取得區域每週總分
        // 3. 計算分數
        // 3.1 計算類別週分數
        // 3.2 類別週分數加總
    }
}
