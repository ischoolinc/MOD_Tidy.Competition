using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;
using K12.Data;

namespace Ischool.Tidy_Competition
{
    class SnapshotData
    {
        // 複製評分紀錄
        // 1. 已學年度、學期、開始時間、結束時間 做為判斷評分紀錄的key值
        // 2. 先刪除快照資料後再新增新的快照資料
        public static void SnapshotScoreSheet(List<string>listScoreSheetID, string schoolYear, string semester, string startTime, string endTime)
        {
            AccessHelper access = new AccessHelper();
            UpdateHelper up = new UpdateHelper();
            // 取得原快照資料並刪除
            if (listScoreSheetID.Count > 0)
            {
                //access.DeletedValues(access.Select<UDT.SnapshotScoreSheet>(string.Format("ref_score_sheet_id IN({0})", string.Join(",", listScoreSheetID))));
                string sql = string.Format(@"
DELETE
FROM
    $ischool.tidy_competition.snapshot_score_sheet
WHERE
    school_year = {0}
    AND semester = {1}
    AND DATE_TRUNC('day',create_time) >= '{2}'
    AND DATE_TRUNC('day',create_time) <= '{3}'
                    ",schoolYear,semester,startTime,endTime);

                up.Execute(sql);

                // 取得計算週排名的評分紀錄
                List<UDT.ScoreSheet> listData = access.Select<UDT.ScoreSheet>(string.Format("uid IN({0})", string.Join(",", listScoreSheetID)));
                List<UDT.SnapshotScoreSheet> listInsertData = new List<UDT.SnapshotScoreSheet>();
                // Data Sync
                foreach (UDT.ScoreSheet data in listData)
                {
                    UDT.SnapshotScoreSheet ss = new UDT.SnapshotScoreSheet();
                    ss.RefScoreSheetID = int.Parse(data.UID);
                    ss.SchoolYear = data.SchoolYear;
                    ss.Semester = data.Semester;
                    ss.RefPeriodID = data.RefPeriodID;
                    ss.RefPlaceID = data.RefPlaceID;
                    ss.RefDeductionItemID = data.RefDeductionItemID;
                    ss.RefDeductionStandardID = data.RefDeductionStandardID;
                    ss.Remark = data.Remark;
                    ss.Picture1 = data.Picture1;
                    ss.Pic1Comment = data.Pic1Comment;
                    ss.Pic1Size = data.Pic1Size;
                    ss.Pic1LocalUrl = data.Pic1LocalUrl;
                    ss.Picture2 = data.Picture2;
                    ss.Pic2Comment = data.Pic2Comment;
                    ss.Pic2Size = data.Pic2Size;
                    ss.Pic2LocalUrl = data.Pic2LocalUrl;
                    ss.Acount = data.Acount;
                    ss.CreateTime = data.CreateTime;
                    ss.LastUpdateName = data.LastUpdateName;
                    ss.LastUpdateBy = data.LastUpdateBy;
                    ss.CheckedTime = data.CheckedTime;
                    ss.CheckedName = data.CheckedName;
                    ss.CanceledBy = data.CanceledBy;
                    ss.CanceledReason = data.CanceledReason;


                    listInsertData.Add(ss);
                }
                // 新增快照資料
                access.InsertValues(listInsertData);

            }
        }
    }
}
