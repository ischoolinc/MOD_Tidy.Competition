using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.Tidy_Competition.UDT
{
    /// <summary>
    /// 分數準則
    /// </summary>
    [TableName("ischool.tidy_competition.score_rule")]
    public class ScoreRule : ActiveRecord
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [Field(Field ="name",Indexed =false)]
        public string Name { get; set; }

        /// <summary>
        /// 指定區域每周總分
        /// </summary>
        [Field(Field ="weekly_total",Indexed =false)]
        public int WeeklyTotal { get; set; }

        /// <summary>
        /// 每日扣分上限
        /// </summary>
        [Field(Field ="max_daily_deduction")]
        public int MacDailyDeduction { get; set; }

        /// <summary>
        /// 公式
        /// </summary>
        [Field(Field ="formula",Indexed =false)]
        public string Formula { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [Field(Field = "create_time", Indexed = false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 建立者帳號
        /// </summary>
        [Field(Field = "created_by", Indexed = false)]
        public string CreatedBy { get; set; }
    }
}
