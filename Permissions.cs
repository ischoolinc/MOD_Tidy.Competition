using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ischool.Tidy_Competition
{
    class Permissions
    {
        public static string 設定區域類別 { get { return "51D738C4-F242-4DC6-A24D-4F484B9C808C"; } }
        public static bool 設定區域類別權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[設定區域類別].Executable;
            }
        }

        public static string 設定分數準則 { get { return "1F56F310-8EF9-4418-9613-9D246B538727"; } }
        public static bool 設定分數準則權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[設定分數準則].Executable;
            }
        }

        public static string 設定區域位置 { get { return "1DF4E352-3987-437A-B60E-A419194CF976"; } }
        public static bool 設定區域位置權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[設定區域位置].Executable;
            }
        }

        public static string 設定位置負責班級 { get { return "719BDABE-98F7-4BE6-84FE-EB24E225A2B3"; } }
        public static bool 設定位置負責班級權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[設定位置負責班級].Executable;
            }
        }

        public static string 設定扣分資料 { get { return "9B6004EC-7659-498E-A919-63356C61B36C"; } }
        public static bool 設定扣分資料權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[設定扣分資料].Executable;
            }
        }

        public static string 設定扣分物件 { get { return "A0AC6D1A-3FE1-4000-B058-55100F1E5B83"; } }
        public static bool 設定扣分物件權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[設定扣分資料].Executable;
            }
        }

        public static string 設定扣分項目 { get { return "D060C5DD-21C1-414A-BA2F-02E4141BBC3D"; } }
        public static bool 設定扣分項目權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[設定扣分資料].Executable;
            }
        }

        public static string 設定時段 { get { return "7B6CC201-E823-4798-864C-A17DA6868229"; } }
        public static bool 設定時段權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[設定時段].Executable;
            }
        }

        public static string 設定評分員 { get { return "F06A3980-A9D6-461D-93C5-15DAA1059E69"; } }
        public static bool 設定評分員權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[設定評分員].Executable;
            }
        }

        public static string 設定管理員 { get { return "22324346-A115-4EAD-90C7-261D02C83B5E"; } }
        public static bool 設定管理員權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[設定管理員].Executable;
            }
        }

        public static string 管理評分紀錄 { get { return "14C345F3-A316-4F17-8A43-669FCA05C588"; } }
        public static bool 管理評分紀錄權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[管理評分紀錄].Executable;
            }
        }

        public static string 計算週排名 { get { return "610854DA-7789-4C75-B082-55F32F537E34"; } }
        public static bool 計算週排名權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[計算週排名].Executable;
            }
        }

        public static string 週排名報表 { get { return "22A139A4-C4D3-42D8-A239-62CB2E2691F9"; } }
        public static bool 週排名報表權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[週排名報表].Executable;
            }
        }
    }
}
