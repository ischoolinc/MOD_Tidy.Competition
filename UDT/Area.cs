using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.Tidy_Competition.UDT
{
    /// <summary>
    /// 區域類別
    /// </summary>
    [TableName("ischool.tidy_competition.area")]
    class Area : ActiveRecord
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [Field(Field ="name",Indexed =false)]
        public string Name { get; set; }

        /// <summary>
        /// 分數準則編號
        /// </summary>
        [Field(Field = "ref_rule_id", Indexed = false)]
        public int RefRuleID { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Field(Field ="enabled",Indexed =false)]
        public bool Enabled { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [Field(Field ="create_time",Indexed =false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 建立者帳號
        /// </summary>
        [Field(Field ="created_by",Indexed =false)]
        public string CreatedBy { get; set; }
    }
}
