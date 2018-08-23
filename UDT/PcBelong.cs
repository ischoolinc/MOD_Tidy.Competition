using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.Tidy_Competition.UDT
{
    /// <summary>
    /// 位置(p)負責班級(c)
    /// </summary>
    [TableName("ischool.tidy_competition.pc_belong")]
    class PcBelong : ActiveRecord
    {
        /// <summary>
        /// 學年度
        /// </summary>
        [Field(Field ="school_year",Indexed =false)]
        public int SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        [Field(Field ="semester",Indexed =false)]
        public int Semester { get; set; }

        /// <summary>
        /// 位置系統編號
        /// </summary>
        [Field(Field ="ref_place_id",Indexed =false)]
        public int RefPlaceID { get; set; }

        /// <summary>
        /// 班級系統編號
        /// </summary>
        [Field(Field ="ref_class_id",Indexed =false)]
        public int RefClassID { get; set; }

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
