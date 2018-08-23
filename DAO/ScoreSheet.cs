using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FISCA.Data;

namespace Ischool.Tidy_Competition.DAO
{
    class ScoreSheet
    {
        public static DataTable GetScoreSheet(string schoolYear, string semester, string periodID, string areaID, string dateTime)
        {
            string sql = string.Format(@"
SELECT
    place.name AS place_name
    , period.name AS period_name
    , area.uid AS ref_area_id
    , area.name AS area_name
    , class.class_name
    , deduction_item.name AS item_name
    , deduction_standard.name AS standard_name
    , student.name AS student_name
    , teacher.teacher_name
    , CASE
        WHEN student.name IS NOT NULL THEN '評分員'
        WHEN teacher.teacher_name IS NOT NULL THEN '管理員'
        ELSE ''
        END AS 身分
    , score_sheet.*
FROM
    $ischool.tidy_competition.score_sheet AS score_sheet
    LEFT OUTER JOIN $ischool.tidy_competition.period AS period
        ON period.uid = score_sheet.ref_period_id
    LEFT OUTER JOIN $ischool.tidy_competition.place AS place
        ON score_sheet.ref_place_id = place.uid
    LEFT OUTER JOIN $ischool.tidy_competition.area AS area
        ON area.uid = place.ref_area_id
    LEFT OUTER JOIN $ischool.tidy_competition.pc_belong AS pc_belong
        ON pc_belong.ref_place_id = place.uid
    LEFT OUTER JOIN class
        ON class.id = pc_belong.ref_class_id
    LEFT OUTER JOIN $ischool.tidy_competition.deduction_item AS deduction_item
        ON deduction_item.uid = score_sheet.ref_deduction_item_id
    LEFT OUTER JOIN $ischool.tidy_competition.deduction_standard AS deduction_standard
        ON deduction_standard.uid = score_sheet.ref_deduction_standard_id
    LEFT OUTER JOIN $ischool.tidy_competition.scorer AS scorer
        ON score_sheet.account = scorer.account
    LEFT OUTER JOIN student
        ON scorer.ref_student_id = student.id
    LEFT OUTER JOIN $ischool.tidy_competition.admin AS admin
        ON admin.account = score_sheet.account
    LEFT OUTER JOIN teacher
        ON teacher.id = admin.ref_teacher_id
        
WHERE
    score_sheet.school_year = {0}
    AND score_sheet.semester = {1}
    AND DATE_TRUNC('day',score_sheet.create_time) = '{2}'::TIMESTAMP
            ", schoolYear, semester, dateTime);

            if (periodID != null)
            {
                sql += string.Format(@"
AND score_sheet.ref_period_id = {0}
                ", periodID);
            }
            if (areaID != null)
            {
                sql += string.Format(@"
AND place.ref_area_id = {0}
AND place.uid IS NOT NULL
                ", areaID);
            }



//            sql = string.Format(@"
//SELECT
//FROM
//    $ischool.tidy_competition.score_sheet AS score_sheet
//    LEFT OUTER JOIN $ischool.tidy_competition.place AS place
//        ON score_sheet.ref_place_id = place.uid
//WHERE
//    score_sheet.school_year = {0}
//    AND score_sheet.semester = {1}
//    AND score_sheet.ref_period_id = {2}
//    AND place.ref_area_id = {3}
//    AND place.uid IS NOT NULL
//    AND DATE_TRUNC('day',score_sheet.create_time) = '{4}'::TIMESTAMP
//            ", schoolYear, semester, periodID, areaID, dateTime);

            QueryHelper qh = new QueryHelper();

            return qh.Select(sql);
        }
    }
}
