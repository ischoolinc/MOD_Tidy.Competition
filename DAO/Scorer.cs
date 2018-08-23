using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.Data;
using System.Data;

namespace Ischool.Tidy_Competition.DAO
{
    class Scorer
    {
        public static DataTable GetScorerDataBySchoolYear(string schoolYear)
        {
            string sql = string.Format(@"
SELECT
    class.grade_year
    , class.class_name
    , student.name
    , student.student_number
    , scorer.*
FROM
    $ischool.tidy_competition.scorer AS scorer
    LEFT OUTER JOIN student
        ON student.id = scorer.ref_student_id
    LEFT OUTER JOIN class
        ON student.ref_class_id = class.id
WHERE
    scorer.school_year = {0}
            ",schoolYear);

            QueryHelper qh = new QueryHelper();

            return qh.Select(sql);
        }
    }
}
