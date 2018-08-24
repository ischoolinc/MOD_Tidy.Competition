using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.Tidy_Competition.Ribbon.WeeklySCore
{
    /// <summary>
    /// 一筆供計算成績用的扣分項目
    /// </summary>
    class ScoreItem
    {
        public string AreaID { get; set; }
        public string ClassID { get; set; }
        public string OccurDate { get; set; }
        public int Score { get; set; }
    }
}
