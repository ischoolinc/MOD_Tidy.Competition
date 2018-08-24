using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using K12.Data;

namespace Ischool.Tidy_Competition.DAO
{
    class DeductionStandard
    {
        public static void SaveData(string dataRow,string areaID)
        {
            string sql = string.Format(@"
WITH data_row AS(
    {0}
) , insert_data AS(
    INSERT INTO $ischool.tidy_competition.deduction_standard(
        name
        , ref_area_id
        , enabled
        , points
        , display_order
        , create_time
        , created_by
    )
    SELECT
        name
        , ref_area_id
        , enabled
        , points
        , display_order
        , create_time
        , created_by
    FROM
        data_row
    WHERE
        data_row.uid IS NULL
) , update_data AS(
    UPDATE $ischool.tidy_competition.deduction_standard SET
        name = data_row.name
        , ref_area_id = data_row.ref_area_id
        , enabled = data_row.enabled
        , points = data_row.points
        , display_order = data_row.display_order
        --, create_time = data_row.create_time
        --, created_by = data_row.created_by
    FROM
        data_row
    WHERE
        $ischool.tidy_competition.deduction_standard.uid = data_row.uid
)
DELETE
FROM
    $ischool.tidy_competition.deduction_standard
WHERE
    uid IN(
        SELECT
            standard.uid
        FROM
            $ischool.tidy_competition.deduction_standard AS standard
            LEFT OUTER JOIN data_row
                ON data_row.uid = standard.uid
        WHERE
            data_row.uid IS NULL
            AND standard.ref_area_id = {1}
    )
            ", dataRow,areaID);

            UpdateHelper up = new UpdateHelper();
            up.Execute(sql);
        }
    }
}
