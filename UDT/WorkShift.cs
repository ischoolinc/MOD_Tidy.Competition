using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.Tidy_Competition.UDT
{
    /// <summary>
    /// 輪值表
    /// </summary>
    [TableName("ischool.tidy_competition.work_shift")]
    class WorkShift : ActiveRecord
    {
        /// <summary>
        /// 日期
        /// </summary>
        [Field(Field ="date",Indexed =false)]
        public DateTime Date { get; set; }

        /// <summary>
        /// 區塊系統編號
        /// </summary>
        [Field(Field = "ref_zone_id", Indexed = false)]
        public int RefZoneID { get; set; }

        /// <summary>
        /// 時段系統編號
        /// </summary>
        [Field(Field = "ref_period_id", Indexed = false)]
        public int RefPeriodID { get; set; }

        /// <summary>
        /// 評分員登入帳號
        /// </summary>
        [Field(Field ="account",Indexed =false)]
        public string Account { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Field(Field ="name",Indexed =false)]
        public string Name { get; set; }

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
