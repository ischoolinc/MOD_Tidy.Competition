using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace Ischool.Tidy_Competition.UDT
{
    /// <summary>
    /// 評分登記總表
    /// </summary>
    [TableName("ischool.tidy_competition.score_sheet")]
    class ScoreSheet : ActiveRecord
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
        /// 時段系統編號
        /// </summary>
        [Field(Field = "ref_period_id", Indexed = false)]
        public int RefPeriodID { get; set; }

        /// <summary>
        /// 位置系統編號
        /// </summary>
        [Field(Field = "ref_place_id", Indexed = false)]
        public int RefPlaceID { get; set; }

        /// <summary>
        /// 扣分物件系統編號
        /// </summary>
        [Field(Field = "ref_deduction_item_id", Indexed = false)]
        public int RefDeductionItemID { get; set; }

        /// <summary>
        /// 扣分系統編號
        /// </summary>
        [Field(Field = "ref_deduction_standard_id", Indexed = false)]
        public int RefDeductionStandardID { get; set; }

        /// <summary>
        /// 補充說明
        /// </summary>
        [Field(Field = "remark", Indexed = false)]
        public string Remark { get; set; }

        /// <summary>
        /// 照片1
        /// </summary>
        [Field(Field = "picture1", Indexed = false)]
        public string Picture1 { get; set; }

        /// <summary>
        /// 照片1的說明
        /// </summary>
        [Field(Field = "pic1_comment", Indexed = false)]
        public string Pic1Comment { get; set; }
        
        /// <summary>
        /// 照片1檔案大小
        /// </summary>
        [Field(Field = "pic1_size", Indexed = false)]
        public int Pic1Size { get; set; }

        /// <summary>
        /// 照片1原始路徑
        /// </summary>
        [Field(Field = "pic1_local_url", Indexed = false)]
        public string Pic1LocalUrl { get; set; }

        /// <summary>
        /// 照片2
        /// </summary>
        [Field(Field = "picture2", Indexed = false)]
        public string Picture2 { get; set; }

        /// <summary>
        /// 照片2的說明
        /// </summary>
        [Field(Field = "pic2_comment", Indexed = false)]
        public string Pic2Comment { get; set; }

        /// <summary>
        /// 照片2檔案大小
        /// </summary>
        [Field(Field = "pic2_size", Indexed = false)]
        public int Pic2Size { get; set; }

        /// <summary>
        /// 照片2原始路徑
        /// </summary>
        [Field(Field = "pic2_local_url", Indexed = false)]
        public string Pic2LocalUrl { get; set; }

        /// <summary>
        /// 評分員登入帳號
        /// </summary>
        [Field(Field = "account", Indexed = false)]
        public string Acount { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [Field(Field = "create_time", Indexed = false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最後修改日期日期
        /// </summary>
        [Field(Field = "last_update_time", Indexed = false)]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 最後修改者姓名
        /// </summary>
        [Field(Field = "last_update_name", Indexed = false)]
        public string LastUpdateName { get; set; }

        /// <summary>
        /// 最號修改者帳號
        /// </summary>
        [Field(Field = "last_update_by", Indexed = false)]
        public string LastUpdateBy { get; set; }

        /// <summary>
        /// 查核時間
        /// </summary>
        [Field(Field = "checked_time", Indexed = false)]
        public DateTime CheckedTime { get; set; }

        /// <summary>
        /// 查核者姓名
        /// </summary>
        [Field(Field = "checked_name", Indexed = false)]
        public string CheckedName { get; set; }

        /// <summary>
        /// 查核者帳號
        /// </summary>
        [Field(Field = "checked_by", Indexed = false)]
        public string CheckedBy { get; set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        [Field(Field = "is_canceled", Indexed = false)]
        public bool IsCanceled { get; set; }

        /// <summary>
        /// 取消時間
        /// </summary>
        [Field(Field = "canceled_time", Indexed = false)]
        public DateTime CanceledTime { get; set; }

        /// <summary>
        /// 取消者姓名
        /// </summary>
        [Field(Field = "canceled_name", Indexed = false)]
        public string CanceledName { get; set; }

        /// <summary>
        /// 取消者帳號
        /// </summary>
        [Field(Field = "canceled_by", Indexed = false)]
        public string CanceledBy { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        [Field(Field = "cancel_reason", Indexed = false)]
        public string CanceledReason { get; set; }

    }


}
