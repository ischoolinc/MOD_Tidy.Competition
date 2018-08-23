using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FISCA.Data;

namespace Ischool.Tidy_Competition.DAO
{
    class PcBelong
    {

        public static DataTable GetPcBelong(string schoolYear,string areaID)
        {
            string sql = string.Format(@"
SELECT 
     place.uid AS ref_place_id
     , place.name AS place_name
     , pc_belong.uid
     , class.id AS ref_class_id
     , class.class_name
     , pc_belong.created_by
FROM 
    $ischool.tidy_competition.place AS place
    LEFT OUTER JOIN $ischool.tidy_competition.pc_belong AS pc_belong
        ON place.uid = pc_belong.ref_place_id
        AND pc_belong.school_year = {0}
    LEFT OUTER JOIN class
        ON class.id = pc_belong.ref_class_id
WHERE
    place.ref_area_id = {1}
    AND place.enabled = 'true'
ORDER BY
    place.display_order
                ",schoolYear,areaID);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            return dt;
        }
    }
}
