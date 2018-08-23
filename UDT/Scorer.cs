using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.Tidy_Competition.UDT
{
    /// <summary>
    /// 評分員
    /// </summary>
    [TableName("ischool.tidy_competition.scorer")]
    class Scorer : ActiveRecord
    {
        /// <summary>
        /// 登入帳號
        /// </summary>
        [Field(Field ="account",Indexed =false)]
        public string Account { get; set; }

        /// <summary>
        /// 學生編號
        /// </summary>
        [Field(Field ="ref_student_id",Indexed =false)]
        public int RefStudentID { get; set; }

        /// <summary>
        /// 學年度
        /// </summary>
        [Field(Field ="school_year",Indexed =false)]
        public int SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        [Field(Field ="semester",Indexed =false)]
        public int semester { get; set; }
        
        /// <summary>
        /// 是否為幹部
        /// </summary>
        [Field(Field ="is_leader",Indexed =false)]
        public bool IsLeader { get; set; }

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
