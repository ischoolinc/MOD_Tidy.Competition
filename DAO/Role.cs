using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FISCA.Data;

namespace Ischool.Tidy_Competition.DAO
{
    class Role
    {
        /// <summary>
        /// 檢查整潔競賽模組專用腳色是否存在
        /// </summary>
        /// <returns></returns>
        public static bool CheckRoleExist()
        {
            string sql = string.Format(@"
SELECT
    *
FROM
    _role
WHERE
    role_name = '{0}'
            ", Program._roleName);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);


            return dt.Rows.Count > 0 ? true : false;
        }

        /// <summary>
        /// 建立整潔競賽模組專用角色:並回傳角色編號
        /// </summary>
        /// <returns></returns>
        public static string CreatRole()
        {
            string sql = string.Format(@"
WITH insert_role AS(
    INSERT INTO _role(
        role_name
        , permission
    )
    VALUES(
            '{0}'
            ,'{1}'
    )
    RETURNING *
)
SELECT * FROM insert_role
            ", Program._roleName,Program._permission);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            return "" + dt.Rows[0]["id"];
        }
        
        /// <summary>
        /// 取得角色系統編號
        /// </summary>
        /// <returns></returns>
        public static string GetRoleID()
        {
            string sql = string.Format(@"
SELECT
    *
FROM
    _role
WHERE
    role_name = '{0}'
            ", Program._roleName);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            return "" + dt.Rows[0]["id"]; // 腳色系統編號

        }
    }
}
