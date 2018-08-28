using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.Data;
using K12.Data;

namespace Ischool.Tidy_Competition.DAO
{
    class DeductionItem
    {
        public static void SaveData(string dataRow,string areaID)
        {
            string sql = "";

            if (string.IsNullOrEmpty(dataRow))
            {
                sql = string.Format(@"
DELETE FROM $ischool.tidy_competition.deduction_item WHERE ref_area_id = {0}
                ", areaID);
            }
            else
            {
                #region SQL
                sql = string.Format(@"
WITH data_row AS(
    {0}
) , insert_data AS(
    INSERT INTO $ischool.tidy_competition.deduction_item(
        name
        , ref_area_id
        , enabled
        , display_order
        , create_time
        , created_by
    )
    SELECT
        name
        , ref_area_id
        , enabled
        , display_order
        , create_time
        , created_by
    FROM
        data_row
    WHERE
        data_row.uid IS NULL
    
) , update_data AS(
    UPDATE $ischool.tidy_competition.deduction_item SET
        name = data_row.name
        , ref_area_id = data_row.ref_area_id
        , enabled = data_row.enabled
        , display_order = data_row.display_order
        --, create_time = data_row.create_time
        --, created_by = data_row.created_by
    FROM
        data_row
    WHERE
        $ischool.tidy_competition.deduction_item.uid = data_row.uid
)
DELETE
FROM
    $ischool.tidy_competition.deduction_item
WHERE
    uid IN(
        SELECT
            item.uid
        FROM
            $ischool.tidy_competition.deduction_item AS item
            LEFT OUTER JOIN data_row
                ON data_row.uid = item.uid
        WHERE
            data_row.uid IS NULL
            AND item.ref_area_id = {1}
    )
            ", dataRow, areaID);
                #endregion
            }


            UpdateHelper up = new UpdateHelper();
            up.Execute(sql);
        }
    }
}
