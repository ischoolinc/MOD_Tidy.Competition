using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using FISCA.Data;

namespace Ischool.Tidy_Competition.DAO
{
    class Actor
    {
        private string _userAccount;
        private string _userName;
        private bool _isAdmin;
        private static Actor _actor;

        private Actor()
        {
            this._userAccount = FISCA.Authentication.DSAServices.UserAccount.Replace("'", "''");
            QueryHelper qh = new QueryHelper();

            #region 使用者姓名
            {
                string sql = string.Format(@"
SELECT
    *
FROM
    teacher
WHERE
    st_login_name = '{0}'
            ", this._userAccount);

                DataTable dt = qh.Select(sql);

                if (dt.Rows.Count > 0)
                {
                    this._userName = "" + dt.Rows[0]["teacher_name"];
                }
            }
            #endregion

            #region 檢查使用者是否為管理員角色
            {
                string sql = string.Format(@"
SELECT 
    teacher.*
FROM
    teacher
    LEFT OUTER JOIN _login
        ON teacher.st_login_name = _login.login_name
    LEFT OUTER JOIN _lr_belong
        ON _login.id = _lr_belong._login_id
WHERE
    teacher.st_login_name = '{0}'
    AND _lr_belong._role_id = {1}
                ", this._userAccount, Program._roleID);

                DataTable dt = qh.Select(sql);

                this._isAdmin = dt.Rows.Count > 0;
            }
            #endregion
        }

        public static Actor Instance()
        {
            if (_actor == null)
            {
                _actor = new Actor();
            }
            return _actor;
        }

        public bool CheckAdmin()
        {
            return this._isAdmin;
        }

        /// <summary>
        /// 取得登錄者帳號
        /// </summary>
        /// <returns></returns>
        public string GetUserAccount()
        {
            return this._userAccount;
        }

        /// <summary>
        /// 取得登錄者姓名
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return this._userName;
        }

        /// <summary>
        /// 取得_LoginID
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public string GetLoginIDByAccount(string Account)
        {
            string loginID;
            string sql = string.Format("SELECT * FROM _login WHERE login_name = '{0}'", Account);
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            if (dt.Rows.Count > 0)
            {
                loginID = "" + dt.Rows[0]["id"];
            }
            else
            {
                loginID = "";
            }

            return loginID;
        }
    }
}
