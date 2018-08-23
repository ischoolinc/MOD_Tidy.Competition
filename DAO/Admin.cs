using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FISCA.Data;
using K12.Data;

namespace Ischool.Tidy_Competition.DAO
{
    class Admin
    {
        /// <summary>
        /// 取得管理員資料
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdminData()
        {
            string sql = string.Format(@"
SELECT
    teacher.teacher_name
    , admin.*
FROM
    $ischool.tidy_competition.admin AS admin
    LEFT OUTER JOIN _login
        ON _login.login_name = admin.account
    LEFT OUTER JOIN _lr_belong
        ON _lr_belong._login_id = _login.id
    LEFT OUTER JOIN _role
        ON _role.id = _lr_belong._role_id
    LEFT OUTER JOIN teacher
        ON teacher.id = admin.ref_teacher_id
WHERE
    _role.role_name = '{0}'
    AND _role.id IS NOT NULL
            ",Program._roleName);

            QueryHelper qh = new QueryHelper();

            return qh.Select(sql);
        }

        public static void InsertAdminData(string account, string teacherID, string createTime, string createdBy, string roleID, string loginID)
        {
            string sql = "";
            if (string.IsNullOrEmpty(loginID))
            {
                #region SQL
                sql = string.Format(@"
WITH data_row AS(
    SELECT
        {0}::TEXT AS account
        , {1}::TEXT AS ref_teacher_id
        , {2}::TIMESTAMP AS create_time
        , {3}::TEXT AS created_by
) , insert_admin AS(
    INSERT INTO $ischool.tidy_competition.admin(
        account
        , ref_teacher_id
        , create_time
        , created_by
    )
    SELECT
        account
        , ref_teacher_id
        , create_time
        , created_by
    FROM
        data_row
) , insert_login AS(
    INSERT INTO _login(
        login_name
        , sys_admin
        , account_type
    )
    SELECT
        account
        , '0'
        , 'greening'
    FROM
        data_row
    RETURNING *
) , insert_lr_belong AS(
    INSERT INTO _lr_belong(
        _login_id
        , _role_id
    )
    SELECT
        insert_login.id
        , {4}
    FROM
        insert_login
)
                ", account, teacherID, createTime, createdBy, roleID);
                #endregion
            }
            else
            {
                #region SQL
                sql = string.Format(@"
WITH insert_unit_admin AS(
    INSERT INTO $ischool.tidy_competition.admin(
        account
        , ref_teacher_id
        , create_time
        , created_by
    )
    VALUES(
        '{0}'
        , {1}
        , '{2}'
        , '{3}'
    )
) 
INSERT INTO _lr_belong(
    _login_id
    , _role_id
)
SELECT
    {4}
    , {5}
                    ", account, teacherID, createTime, createdBy, loginID, roleID);

                #endregion
            }
            UpdateHelper up = new UpdateHelper();
            up.Execute(sql);
        }

        public static void DeleteAdminData(string roleID, string adminID)
        {
            string sql = string.Format(@"
WITH data_row AS(
    SELECT
        _lr_belong.*
    FROM
        $ischool.tidy_competition.admin AS admin
        LEFT OUTER JOIN _login
            ON _login.login_name = admin.account
        LEFT OUTER JOIN _lr_belong
            ON _lr_belong._login_id = _login.id
            AND _lr_belong._role_id = {0}
    WHERE
        admin.uid = {1}
) , delete_admin AS(
    DELETE
    FROM
        $ischool.tidy_competition.admin
    WHERE
        uid = {1}
) 
DELETE
FROM
    _lr_belong
WHERE
    id IN (
        SELECT
            id
        FROM
            data_row
    ) 
                ", roleID, adminID);
            UpdateHelper up = new UpdateHelper();
            up.Execute(sql);

        }
    }
}
