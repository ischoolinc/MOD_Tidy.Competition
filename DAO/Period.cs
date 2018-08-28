using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using K12.Data;

namespace Ischool.Tidy_Competition.DAO
{
    class Period
    {

        public static void saveData(string dataRow)
        {
            string sql = "";
            if (string.IsNullOrEmpty(dataRow))
            {
                sql = "DELETE FROM $ischool.tidy_competition.period";
            }
            else
            {
                #region SQL
                sql = string.Format(@"
WITH data_row AS(
    {0}
) , insert_data AS(
    INSERT INTO $ischool.tidy_competition.period(
        name
        , enabled
        , create_time
        , created_by
    )
    SELECT
        name
        , enabled
        , create_time
        , created_by
    FROM
        data_row
    WHERE
        uid IS NULL
), update_data AS(
    UPDATE $ischool.tidy_competition.period SET
        name = data_row.name
        , enabled = data_row.enabled
        --, create_time = data_row.create_time
        --, created_by = data_row.created_by
    FROM
        data_row
    WHERE
        $ischool.tidy_competition.period.uid = data_row.uid
)
DELETE
FROM
    $ischool.tidy_competition.period
WHERE
    uid IN(
        SELECT
            period.uid
        FROM
            $ischool.tidy_competition.period AS period
            LEFT OUTER JOIN data_row
                ON data_row.uid = period.uid
        WHERE
            data_row.uid IS NULL
    )
            ", dataRow);
                #endregion
            }


            UpdateHelper up = new UpdateHelper();
            up.Execute(sql);

        }
    }
}
