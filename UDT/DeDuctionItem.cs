using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.Tidy_Competition.UDT
{
    /// <summary>
    /// 扣分物件
    /// </summary>
    [TableName("ischool.tidy_competition.deduction_item")]
    class DeDuctionItem : ActiveRecord
    {
        /// <summary>
        /// 名稱
        /// </summary>
        [Field(Field ="name",Indexed =false)]
        public string Name { get; set; }

        /// <summary>
        /// 區域類別系統編號
        /// </summary>
        [Field(Field ="ref_area_id",Indexed =false)]
        public int RefAreaID { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Field(Field ="enabled",Indexed =false)]
        public bool Enabled { get; set; }

        /// <summary>
        /// 顯示順序
        /// </summary>
        [Field(Field ="display_order",Indexed =false)]
        public int DisplayOrder { get; set; }

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
