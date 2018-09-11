using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA;
using FISCA.UDT;
using K12.Data.Configuration;
using FISCA.Presentation;
using FISCA.Permission;
using FISCA.Presentation.Controls;

namespace Ischool.Tidy_Competition
{
    public class Program
    {
        /// <summary>
        /// 整潔競賽模組專用腳色名稱
        /// </summary>
        public static string _roleName = "整潔競賽管理員";

        /// <summary>
        /// 整潔競賽模組專用角色的功能權限
        /// </summary>
        public static string _permission = @"
<Permissions>
<Feature Code=""7B6CC201-E823-4798-864C-A17DA6868229"" Permission=""Execute""/>
<Feature Code=""9B6004EC-7659-498E-A919-63356C61B36C"" Permission=""Execute""/>
<Feature Code=""719BDABE-98F7-4BE6-84FE-EB24E225A2B3"" Permission=""Execute""/>
<Feature Code=""1DF4E352-3987-437A-B60E-A419194CF976"" Permission=""Execute""/>
<Feature Code=""1F56F310-8EF9-4418-9613-9D246B538727"" Permission=""Execute""/>
<Feature Code=""51D738C4-F242-4DC6-A24D-4F484B9C808C"" Permission=""Execute""/>
<Feature Code=""F06A3980-A9D6-461D-93C5-15DAA1059E69"" Permission=""Execute""/>
<Feature Code=""22324346-A115-4EAD-90C7-261D02C83B5E"" Permission=""Execute""/>
<Feature Code=""14C345F3-A316-4F17-8A43-669FCA05C588"" Permission=""Execute""/>
<Feature Code=""610854DA-7789-4C75-B082-55F32F537E34"" Permission=""Execute""/>
<Feature Code=""22A139A4-C4D3-42D8-A239-62CB2E2691F9"" Permission=""Execute""/>
<Feature Code=""A0AC6D1A-3FE1-4000-B058-55100F1E5B83"" Permission=""Execute""/>
<Feature Code=""D060C5DD-21C1-414A-BA2F-02E4141BBC3D"" Permission=""Execute""/>
</Permissions>
";
        public static string _roleID;

        [MainMethod("整潔競賽模組")]
        static public void Main()
        {
            #region Init UDT
            {
                ConfigData cd = K12.Data.School.Configuration["整潔競賽模組載入設定1.0.0.7"];

                bool checkUDT = false;
                string name = "整潔競賽UDT是否已載入";

                //如果尚無設定值,預設為
                if (string.IsNullOrEmpty(cd[name]))
                {
                    cd[name] = "false";
                }

                //檢查是否為布林
                bool.TryParse(cd[name], out checkUDT);

                if (!checkUDT) //
                {
                    AccessHelper access = new AccessHelper();
                    access.Select<UDT.Admin>("UID = '00000'");
                    access.Select<UDT.Scorer>("UID = '00000'");
                    access.Select<UDT.Period>("UID = '00000'");
                    access.Select<UDT.Area>("UID = '00000'");
                    access.Select<UDT.ScoreSheet>("UID = '00000'");
                    // access.Select<UDT.Zone>("UID = '00000'"); 廢除區塊UDT
                    access.Select<UDT.Place>("UID = '00000'");
                    access.Select<UDT.DeDuctionItem>("UID = '00000'");
                    access.Select<UDT.DeDuctionStandard>("UID = '00000'");
                    access.Select<UDT.ScoreRule>("UID = '00000'");
                    access.Select<UDT.PcBelong>("UID = '00000'");
                    access.Select<UDT.WorkShift>("UID = '00000'");
                    access.Select<UDT.ScoreSheet>("UID = '00000'");
                    access.Select<UDT.SnapshotScoreSheet>("UID = '00000'");
                    access.Select<UDT.WeeklyStats>("UID = '00000'");
                    access.Select<UDT.WeeklyRank>("UID = '00000'");

                    cd[name] = "true";
                    cd.Save();
                }
            }
            #endregion

            #region Init Role
            {
                // 檢查腳色是否存在
                if (!DAO.Role.CheckRoleExist())
                {
                    // 建立腳色
                    _roleID = DAO.Role.CreatRole();
                }
                else
                {
                    DAO.Role.UpdateRole(); // 更新角色權限
                    _roleID = DAO.Role.GetRoleID();
                }
            }
            #endregion

            #region 整潔競賽
            {
                // 整潔競賽模組頁面
                MotherForm.AddPanel(TidyCompetitionPanel.Instance);

                MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["人員設定"].Image = Properties.Resources.foreign_language_config_64;
                MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["人員設定"].Size = RibbonBarButton.MenuButtonSize.Large;
                MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["分數準則設定"].Image = Properties.Resources.virtualcurse_config_64;
                MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["分數準則設定"].Size = RibbonBarButton.MenuButtonSize.Large;
                MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["區域設定"].Image = Properties.Resources.bank_config_128;
                MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["區域設定"].Size = RibbonBarButton.MenuButtonSize.Large;
                MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["評分設定"].Image = Properties.Resources.presentation_a_config_64;
                MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["評分設定"].Size = RibbonBarButton.MenuButtonSize.Large;
                MotherForm.RibbonBarItems["整潔競賽", "評分管理/統計報表"]["報表"].Image = Properties.Resources.Report;
                MotherForm.RibbonBarItems["整潔競賽", "評分管理/統計報表"]["報表"].Size = RibbonBarButton.MenuButtonSize.Large;
                MotherForm.RibbonBarItems["整潔競賽", "評分管理/統計報表"]["管理評分紀錄"].Image = Properties.Resources.blacklist_zoom_128;
                MotherForm.RibbonBarItems["整潔競賽", "評分管理/統計報表"]["管理評分紀錄"].Size = RibbonBarButton.MenuButtonSize.Large;
                MotherForm.RibbonBarItems["整潔競賽", "排名作業"]["計算排名"].Image = Properties.Resources.calc_fav_64;
                MotherForm.RibbonBarItems["整潔競賽", "排名作業"]["計算排名"].Size = RibbonBarButton.MenuButtonSize.Large;


                #region 設定
                #region 設定分數準則
                {
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["分數準則設定"]["設定分數準則"].Enable = Permissions.設定分數準則權限;
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["分數準則設定"]["設定分數準則"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmScoreRule()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }
                    };
                }
                #endregion

                #region 設定區域類別
                {
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["區域設定"]["設定區域類別"].Enable = Permissions.設定區域類別權限;
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["區域設定"]["設定區域類別"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmEditArea()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }
                        
                    };
                }
                #endregion

                #region 設定區域位置
                {
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["區域設定"]["設定區域位置"].Enable = Permissions.設定區域位置權限;
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["區域設定"]["設定區域位置"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmPlace()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }
                        
                    };
                }
                #endregion

                #region 設定位置負責班級
                {
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["區域設定"]["設定位置負責班級"].Enable = Permissions.設定位置負責班級權限;
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["區域設定"]["設定位置負責班級"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmPcBelong()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }
                        
                    };
                }
                #endregion

                #region 設定時段
                {
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["評分設定"]["設定時段"].Enable = Permissions.設定時段權限;
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["評分設定"]["設定時段"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmPeriod()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }

                    };
                }
                #endregion

                #region 設定扣分資料: 扣分物件&扣分項目
                {
                    //MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["評分設定"]["設定扣分資料"].Enable = Permissions.設定扣分資料權限;
                    //MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["評分設定"]["設定扣分資料"].Click += delegate
                    //{
                    //    if (DAO.Actor.Instance().CheckAdmin())
                    //    {
                    //        (new frmDeduction()).ShowDialog();
                    //    }
                    //    else
                    //    {
                    //        MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                    //    }

                    //};
                }
                #endregion

                #region 設定扣分物件
                {
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["評分設定"]["設定扣分物件"].Enable = Permissions.設定扣分物件權限;
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["評分設定"]["設定扣分物件"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmDeductionItem()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }

                    };
                }
                #endregion

                #region 設定扣分項目
                {
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["評分設定"]["設定扣分項目"].Enable = Permissions.設定扣分項目權限;
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["評分設定"]["設定扣分項目"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmDeductionStandard()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }

                    };
                }
                #endregion

                #region 設定評分員
                {
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["人員設定"]["設定評分員"].Enable = Permissions.設定評分員權限;
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["人員設定"]["設定評分員"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmScorer()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }
                        
                    };
                }
                #endregion

                #region 設定管理員
                {
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["人員設定"]["設定管理員"].Enable = Permissions.設定管理員權限;
                    MotherForm.RibbonBarItems["整潔競賽", "基本設定"]["人員設定"]["設定管理員"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmAdmin()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }
                        
                    };
                }
                #endregion
                #endregion

                #region 管理評分紀錄
                {
                    MotherForm.RibbonBarItems["整潔競賽", "評分管理/統計報表"]["管理評分紀錄"].Enable = Permissions.管理評分紀錄權限;
                    MotherForm.RibbonBarItems["整潔競賽", "評分管理/統計報表"]["管理評分紀錄"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmScoreSheet()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }
                        
                    };
                }
                #endregion

                #region 計算排名
                {
                    MotherForm.RibbonBarItems["整潔競賽", "排名作業"]["計算排名"]["計算週排名"].Enable = Permissions.計算週排名權限;
                    MotherForm.RibbonBarItems["整潔競賽", "排名作業"]["計算排名"]["計算週排名"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmWeeklyScore()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }
                    };
                }
                #endregion

                #region 週排名報表
                {
                    MotherForm.RibbonBarItems["整潔競賽", "評分管理/統計報表"]["報表"]["週排名報表"].Enable = Permissions.週排名報表權限;
                    MotherForm.RibbonBarItems["整潔競賽", "評分管理/統計報表"]["報表"]["週排名報表"].Click += delegate
                    {
                        if (DAO.Actor.Instance().CheckAdmin())
                        {
                            (new frmWeeklyRankReport()).ShowDialog();
                        }
                        else
                        {
                            MsgBox.Show("此帳號沒有整潔競賽管理權限!");
                        }
                    };
                }
                #endregion

                #region 權限管理

                Catalog detail = new Catalog();
                detail = RoleAclSource.Instance["整潔競賽"]["功能按鈕"];
                detail.Add(new RibbonFeature(Permissions.設定區域類別, "設定區域類別"));
                detail.Add(new RibbonFeature(Permissions.設定分數準則, "設定分數準則"));
                detail.Add(new RibbonFeature(Permissions.設定區域位置, "設定區域位置"));
                detail.Add(new RibbonFeature(Permissions.設定位置負責班級, "設定位置負責班級"));
                // detail.Add(new RibbonFeature(Permissions.設定扣分資料, "設定扣分資料"));
                detail.Add(new RibbonFeature(Permissions.設定扣分物件, "設定扣分物件"));
                detail.Add(new RibbonFeature(Permissions.設定扣分項目, "設定扣分項目"));
                detail.Add(new RibbonFeature(Permissions.設定時段, "設定時段"));
                detail.Add(new RibbonFeature(Permissions.設定評分員, "設定評分員"));
                detail.Add(new RibbonFeature(Permissions.設定管理員, "設定管理員"));
                detail.Add(new RibbonFeature(Permissions.管理評分紀錄, "管理評分紀錄"));
                detail.Add(new RibbonFeature(Permissions.計算週排名, "計算週排名"));
                detail.Add(new RibbonFeature(Permissions.週排名報表, "週排名報表"));
                #endregion
            }
            #endregion
        }
    }
}
