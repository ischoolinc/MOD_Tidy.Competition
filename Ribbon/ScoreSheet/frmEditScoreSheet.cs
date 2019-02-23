using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using System.Reflection;

namespace Ischool.Tidy_Competition
{
    public partial class frmEditScoreSheet : BaseForm
    {
        private string _userName = DAO.Actor.Instance().GetUserName();
        private string _userAccount = DAO.Actor.Instance().GetUserAccount();
        private DataRow _row;
        private AccessHelper _access = new AccessHelper();
        // 名稱取的資料
        private Dictionary<string, UDT.Period> _dicPeriodByName = new Dictionary<string, UDT.Period>();
        private Dictionary<string, UDT.Area> _dicAreaByName = new Dictionary<string, UDT.Area>();
        private Dictionary<string, UDT.Place> _dicPlaceByName = new Dictionary<string, UDT.Place>();
        private Dictionary<string, UDT.DeDuctionItem> _dicItemByName = new Dictionary<string, UDT.DeDuctionItem>();
        private Dictionary<string, UDT.DeDuctionStandard> _dicStandardByName = new Dictionary<string, UDT.DeDuctionStandard>();
        // 編號取得資料
        private Dictionary<string, UDT.Period> _dicPeriodByID = new Dictionary<string, UDT.Period>();
        private Dictionary<string, UDT.Area> _dicAreaByID = new Dictionary<string, UDT.Area>();
        private Dictionary<string, UDT.Place> _dicPlaceByID = new Dictionary<string, UDT.Place>();
        private Dictionary<string, UDT.DeDuctionItem> _dicItemByID = new Dictionary<string, UDT.DeDuctionItem>();
        private Dictionary<string, UDT.DeDuctionStandard> _dicStandardByID = new Dictionary<string, UDT.DeDuctionStandard>();


        public frmEditScoreSheet(DataRow row)
        {
            InitializeComponent();
            this._row = row;
        }

        private void frmEditScoreSheet_Load(object sender, EventArgs e)
        {
            #region 評分紀錄
            {
                lbSchoolYear.Text = "" + this._row["school_year"];
                lbSemester.Text = "" + this._row["semester"];
                switch ("" + this._row["身分"])
                {
                    case "管理員":
                        lbScorer.Text = "" + this._row["teacher_name"];
                        lbIdentity.Text = "管理員";
                        break;
                    case "評分員":
                        lbScorer.Text = "" + this._row["student_name"];
                        lbIdentity.Text = "評分員";
                        break;
                    default:
                        lbScorer.Text = "";
                        break;
                }
                lbCreateTime.Text = (DateTime.Parse("" + this._row["create_time"])).ToString("yyyy/MM/dd");

                #region 時段
                {
                    // 取得時段資料
                    List<UDT.Period> listPeriod = this._access.Select<UDT.Period>();
                    foreach(UDT.Period data in listPeriod)
                    {
                        cbxPeriod.Items.Add(data.Name);
                        this._dicPeriodByName.Add(data.Name,data);
                        this._dicPeriodByID.Add(data.UID,data);
                    }
                    if (cbxPeriod.Items.Count > 0)
                    {
                        if (cbxPeriod.Items.IndexOf("" + this._row["period_name"]) == -1)
                        {
                            errorProvider1.SetError(cbxPeriod, "原評分時段已被刪除!");
                        }
                        cbxPeriod.SelectedIndex = cbxPeriod.Items.IndexOf("" + this._row["period_name"]);
                    }
                    else
                    {
                        errorProvider1.SetError(cbxPeriod,"請先設定評分時段!");
                    }
                }
                #endregion

                #region 區域
                {
                    // 取得區域資料
                    List<UDT.Area> listArea = this._access.Select<UDT.Area>();
                    foreach (UDT.Area data in listArea)
                    {
                        cbxArea.Items.Add(data.Name);
                        this._dicAreaByName.Add(data.Name,data);
                        this._dicAreaByID.Add(data.UID,data);

                    }
                    if (cbxArea.Items.Count > 0)
                    {
                        if (cbxArea.Items.IndexOf("" + this._row["area_name"]) == -1)
                        {
                            errorProvider1.SetError(cbxArea, "原區域已被刪除!");
                        }
                        cbxArea.SelectedIndex = cbxArea.Items.IndexOf("" + this._row["area_name"]);
                    }
                    else
                    {
                        errorProvider1.SetError(cbxArea, "請先設定區域!");
                    }
                }
                #endregion

                tbxRemark.Text = "" + this._row["remark"];
                picbx1.ImageLocation = "" + this._row["picture1"];
                tbxUrl1.Text = "" + this._row["picture1"];
                tbxPicComment1.Text = "" + this._row["pic1_comment"];
                picbx2.ImageLocation = "" + this._row["picture2"];
                tbxUrl2.Text = "" + this._row["picture2"];
                tbxPicComment2.Text = "" + this._row["pic2_comment"];
            }
            #endregion

            #region 查核
            {
                ckbxCheck.Checked = "" + this._row["checked_time"] == "0001/01/01 00:00:00" ? false : true;
                if (ckbxCheck.Checked)
                {
                    tbxCheckName.Text = "" + this._row["checked_name"];
                    tbxCheckAccount.Text = "" + this._row["checked_by"];
                    lbCheckTime.Text = "" + this._row["checked_time"] == "" ? "" : (DateTime.Parse("" + this._row["checked_time"])).ToString("yyyy/MM/dd HH:mm");
                }
            }
            #endregion

            #region 取消
            {
                ckbxCancel.Checked = "" + this._row["is_canceled"] == "true" ? true : false;
                if (ckbxCancel.Checked)
                {
                    tbxCancelName.Text = "" + this._row["canceled_name"];
                    tbxCancelAccount.Text = "" + this._row["canceled_by"];
                    tbxCancelReason.Text = "" + this._row["cancel_reason"];
                    lbCancelTime.Text = "" + this._row["canceled_time"] == "" ? "" : DateTime.Parse("" + this._row["canceled_time"]).ToString("yyyy/MM/dd hh:mm");
                }
            }
            #endregion

            #region 修改者資料
            {
                tbxUpdateName.Text = this._userName;//"" + this._row["last_update_name"];
                tbxUpdateBy.Text = this._userAccount;//"" + this._row["last_update_by"];
                lbUpdateTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm");//"" + this._row["last_update_time"];
            }
            #endregion
        }

        private void cbxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 位置
            {
                cbxPlace.Items.Clear();
                this._dicPlaceByName.Clear();
                this._dicPlaceByID.Clear();
                // 取得位置資料
                List<UDT.Place> listPlace = this._access.Select<UDT.Place>(string.Format("ref_area_id = {0}","" + this._row["ref_area_id"]));
                foreach (UDT.Place data in listPlace)
                {
                    cbxPlace.Items.Add(data.Name);
                    this._dicPlaceByName.Add(data.Name,data);
                    this._dicPlaceByID.Add(data.UID,data);
                }
                if (cbxPlace.Items.Count > 0)
                {
                    if (cbxPlace.Items.IndexOf("" + this._row["place_name"]) == -1)
                    {
                        errorProvider1.SetError(cbxPlace, "原區域位置已被刪除!");
                    }
                    cbxPlace.SelectedIndex = cbxPlace.Items.IndexOf("" + this._row["place_name"]);
                }
                else
                {
                    errorProvider1.SetError(cbxPlace,"請先設定區域位置!");
                }
            }
            #endregion

            #region 扣分物件
            {
                cbxItem.Items.Clear();
                this._dicItemByName.Clear();
                this._dicItemByID.Clear();
                // 取得扣分物件資料
                List<UDT.DeDuctionItem> listItem = this._access.Select<UDT.DeDuctionItem>(string.Format("ref_area_Id = {0}",this._row["ref_area_id"]));
                foreach (UDT.DeDuctionItem data in listItem)
                {
                    cbxItem.Items.Add(data.Name);
                    this._dicItemByName.Add(data.Name,data);
                    this._dicItemByID.Add(data.UID,data);
                }
                if (cbxItem.Items.Count > 0)
                {
                    if (cbxItem.Items.IndexOf("" + this._row["item_name"]) == -1)
                    {
                        errorProvider1.SetError(cbxItem, "原扣分物件已被刪除!");
                    }
                    cbxItem.SelectedIndex = cbxItem.Items.IndexOf("" + this._row["item_name"]);
                }
                else
                {
                    errorProvider1.SetError(cbxItem,"請先設定扣分物件!");
                }
            }
            #endregion

            #region 扣分項目
            {
                cbxStandard.Items.Clear();
                this._dicStandardByName.Clear();
                this._dicStandardByID.Clear();
                // 取得扣分項目資料
                List<UDT.DeDuctionStandard> listStandard = this._access.Select<UDT.DeDuctionStandard>(string.Format("ref_area_id = {0}",this._row["ref_area_id"]));
                foreach (UDT.DeDuctionStandard data in listStandard)
                {
                    cbxStandard.Items.Add(data.Name);
                    this._dicStandardByName.Add(data.Name,data);
                    this._dicStandardByID.Add(data.UID,data);
                }
                if (cbxStandard.Items.Count > 0)
                {
                    if (cbxStandard.Items.IndexOf("" + this._row["standard_name"]) == -1)
                    {
                        errorProvider1.SetError(cbxStandard, "原扣分項目已被刪除!");
                    }
                    cbxStandard.SelectedIndex = cbxStandard.Items.IndexOf("" + this._row["standard_name"]);
                }
                else
                {
                    errorProvider1.SetError(cbxStandard,"請先設定扣分項目!");
                }
            }
            #endregion
        }

        private void ckbxCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbxCheck.Checked)
            {
                tbxCheckName.Text = this._userName;
                tbxCheckAccount.Text = this._userAccount;
                lbCheckTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm");
            }
            else
            {
                tbxCheckName.Text = "";
                tbxCheckAccount.Text = "";
                lbCheckTime.Text = "";
            }
        }

        private void ckbxCancel_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbxCancel.Checked)
            {
                tbxCancelName.Text = this._userName;
                tbxCancelAccount.Text = this._userAccount;
                lbCancelTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm");
            }
            else
            {
                tbxCancelName.Text = "";
                tbxCancelAccount.Text = "";
                lbCancelTime.Text = "";
            }
        }

        private void tbxUrl1_TextChanged(object sender, EventArgs e)
        {
            picbx1.ImageLocation = tbxUrl1.Text.Trim();
        }

        private void tbxUrl2_TextChanged(object sender, EventArgs e)
        {
            picbx2.ImageLocation = tbxUrl2.Text.Trim();
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool Validate()
        {
            #region 驗證時段
            if (cbxPeriod.SelectedItem == null)
            {
                if (cbxPeriod.Items.Count > 0)
                {
                    errorProvider1.SetError(cbxPeriod, "請選擇評分時段!");
                }
                else
                {
                    errorProvider1.SetError(cbxPeriod, "請先設定評分時段!");
                }
                return false;
            }
            else
            {
                errorProvider1.SetError(cbxPeriod, null);
            }
            #endregion

            #region 驗證區域
            if (cbxArea.SelectedItem == null)
            {
                if (cbxArea.Items.Count > 0)
                {
                    errorProvider1.SetError(cbxArea, "請選擇區域!");
                }
                else
                {
                    errorProvider1.SetError(cbxArea, "請先設定區域!");
                }
                return false;
            }
            else
            {
                errorProvider1.SetError(cbxArea, null);
            }
            #endregion

            #region 驗證區域位置
            if (cbxPlace.SelectedItem == null)
            {
                if (cbxPlace.Items.Count > 0)
                {
                    errorProvider1.SetError(cbxPlace, "請選擇區域位置!");
                }
                else
                {
                    errorProvider1.SetError(cbxPlace, "請先設定區域位置!");
                }
                return false;
            }
            else
            {
                errorProvider1.SetError(cbxPlace, null);
            }
            #endregion

            #region 驗證區域扣分物件
            if (cbxItem.SelectedItem == null)
            {
                if (cbxItem.Items.Count > 0)
                {
                    errorProvider1.SetError(cbxItem, "請選擇區域扣分物件!");
                }
                else
                {
                    errorProvider1.SetError(cbxItem, "請先設定區域扣分物件!");
                }
                return false;
            }
            else
            {
                errorProvider1.SetError(cbxItem, null);
            }
            #endregion

            #region 驗證區域扣分項目
            if (cbxStandard.SelectedItem == null)
            {
                if (cbxStandard.Items.Count > 0)
                {
                    errorProvider1.SetError(cbxStandard, "請選擇區域扣分項目!");
                }
                else
                {
                    errorProvider1.SetError(cbxStandard, "請先設定區域扣分項目!");
                }
                return false;
            }
            else
            {
                errorProvider1.SetError(cbxStandard, null);
            }
            #endregion

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate()) // 資料驗證
            {
                #region 資料整理
                List<UDT.ScoreSheet> listUpdateData = this._access.Select<UDT.ScoreSheet>(string.Format("uid = {0}", this._row["uid"]));
                UDT.ScoreSheet data = listUpdateData[0];

                string orgPeriodName = this._dicPeriodByID.ContainsKey("" + data.RefPeriodID) ? this._dicPeriodByID["" + data.RefPeriodID].Name : "";
                string orgPlaceName = this._dicPlaceByID.ContainsKey("" + data.RefPlaceID) ? this._dicPlaceByID["" + data.RefPlaceID].Name : "";
                string orgItemName = this._dicItemByID.ContainsKey("" + data.RefDeductionItemID) ? this._dicItemByID["" + data.RefDeductionItemID].Name : "";
                string orgStandardName = this._dicStandardByID.ContainsKey("" + data.RefDeductionStandardID) ? this._dicStandardByID["" + data.RefDeductionStandardID].Name : "";
                // LOG
                StringBuilder logs = new StringBuilder(string.Format(
                    "管理員「{0}」修改「'{1}'時段，'{2}'位置，'{3}'扣分物件，'{4}'扣分項目，'{5}'建立日期」的評分紀錄。內容如下:"
                    , this._userName, orgPeriodName, orgPlaceName, orgItemName, orgStandardName, data.CreateTime.ToString("yyyy/MM/dd")));

                #region 評分紀錄
                // 時段
                if (data.RefPeriodID != int.Parse(this._dicPeriodByName[cbxPeriod.SelectedItem.ToString()].UID))
                {
                    logs.AppendLine(string.Format("\n 原時段「{0}」修改為「{1}」", orgPeriodName, cbxPeriod.SelectedItem.ToString()));

                    data.RefPeriodID = int.Parse(this._dicPeriodByName[cbxPeriod.SelectedItem.ToString()].UID);
                }
                // 位置
                if (data.RefPlaceID != int.Parse(this._dicPlaceByName[cbxPlace.SelectedItem.ToString()].UID))
                {
                    logs.AppendLine(string.Format("\n 原位置「{0}」修改為「{1}」", orgPlaceName, cbxPlace.SelectedItem.ToString()));

                    data.RefPlaceID = int.Parse(this._dicPlaceByName[cbxPlace.SelectedItem.ToString()].UID);
                }
                // 扣分物件
                if (data.RefDeductionItemID != int.Parse(this._dicItemByName[cbxItem.SelectedItem.ToString()].UID))
                {
                    logs.AppendLine(string.Format("原扣分物件「{0}」修改為「{1}」", orgItemName, cbxItem.SelectedItem.ToString()));

                    data.RefDeductionItemID = int.Parse(this._dicItemByName[cbxItem.SelectedItem.ToString()].UID);
                }
                // 扣分項目
                if (data.RefDeductionStandardID != int.Parse(this._dicStandardByName[cbxStandard.SelectedItem.ToString()].UID))
                {
                    logs.AppendLine(string.Format("原扣分項目「{0}」修改為「{1}」", orgStandardName, cbxStandard.SelectedItem.ToString()));

                    data.RefDeductionStandardID = int.Parse(this._dicStandardByName[cbxStandard.SelectedItem.ToString()].UID);
                }
                // 備註
                if (data.Remark != tbxRemark.Text.Trim())
                {
                    logs.AppendLine(string.Format("原備註「{0}」修改為「{1}」", data.Remark, tbxRemark.Text.Trim()));

                    data.Remark = tbxRemark.Text.Trim();
                }
                // 照片1位置
                if (data.Picture1 != tbxUrl1.Text.Trim())
                {
                    logs.AppendLine(string.Format("原照片1位置「{0}」修改為「{1}」", data.Picture1, tbxUrl1.Text.Trim()));

                    data.Picture1 = tbxUrl1.Text.Trim();
                }
                // 照片1評論
                if (data.Pic1Comment != tbxPicComment1.Text.Trim())
                {
                    logs.AppendLine(string.Format("原照片1評論「{0}」修改為「{1}」", data.Pic1Comment, tbxPicComment1.Text.Trim()));

                    data.Pic1Comment = tbxPicComment1.Text.Trim();
                }
                // 照片2位置
                if (data.Picture2 != tbxUrl2.Text.Trim())
                {
                    logs.AppendLine(string.Format("原照片2評論「{0}」修改為「{1}」", data.Picture2, tbxUrl2.Text.Trim()));

                    data.Picture2 = tbxUrl2.Text.Trim();
                }
                // 照片2評論
                if (data.Pic2Comment != tbxPicComment2.Text.Trim())
                {
                    logs.AppendLine(string.Format("原照片2評論「{0}」修改為「{1}」", data.Pic2Comment, tbxPicComment2.Text.Trim()));

                    data.Pic2Comment = tbxPicComment2.Text.Trim();
                }
                #endregion

                #region 查核
                if (data.CheckedName != tbxCheckName.Text.Trim())
                {
                    logs.AppendLine(string.Format("原查核者「{0}」變更為「{1}」", data.CheckedName, tbxCheckName.Text.Trim()));

                    data.CheckedName = tbxCheckName.Text.Trim();
                }
                if (data.CheckedBy != tbxCheckAccount.Text.Trim())
                {
                    logs.AppendLine(string.Format("原查核者帳號「{0}」變更為「{1}」", data.CheckedBy, tbxCheckAccount.Text.Trim()));

                    data.CheckedBy = tbxCheckAccount.Text.Trim();
                }
                if (data.CheckedTime.ToString("yyyy/MM/dd hh:mm") != lbCheckTime.Text.Trim() && !string.IsNullOrEmpty(lbCheckTime.Text))
                {
                    logs.AppendLine(string.Format("原查核時間「{0}」變更為「{1}」", data.CheckedTime.ToString("yyyy/MM/dd hh:mm"), lbCheckTime.Text.Trim()));

                    data.CheckedTime = DateTime.Parse(lbCheckTime.Text.Trim());
                }

                #endregion

                #region 取消
                if (data.IsCanceled != ckbxCancel.Checked)
                {
                    logs.AppendLine(string.Format("取消此評分紀錄「{0}」變更為「{1}」", data.IsCanceled == true ? "是" : "否", ckbxCancel.Checked == true ? "是" : "否"));

                    data.IsCanceled = ckbxCancel.Checked;
                }
                if (data.CanceledName != tbxCancelName.Text.Trim())
                {
                    logs.AppendLine(string.Format("原取消者「{0}」變更為「{1}」", data.CanceledName, tbxCancelName.Text.Trim()));

                    data.CanceledName = tbxCancelName.Text.Trim();
                }
                if (data.CanceledBy != tbxCancelAccount.Text.Trim())
                {
                    logs.AppendLine(string.Format("原取消者帳號「{0}」變更為「{1}」", data.CanceledBy, tbxCancelAccount.Text.Trim()));

                    data.CanceledBy = tbxCancelAccount.Text.Trim();
                }
                if (data.CanceledReason != tbxCancelReason.Text.Trim())
                {
                    logs.AppendLine(string.Format("原取消原因「{0}」變更為「{1}」", data.CanceledReason, tbxCancelReason.Text.Trim()));

                    data.CanceledReason = tbxCancelReason.Text.Trim();
                }
                if (data.CanceledTime.ToString("yyyy/MM/dd hh:mm") != lbCancelTime.Text.Trim() /*&& data.CanceledTime.ToString("yyyy/MM/dd") != "0001/01/01"*/)
                {
                    logs.AppendLine(string.Format("原取消時間「{0}」變更為「{1}」", data.CanceledTime.ToString("yyyy/MM/dd hh:mm"), lbCancelTime.Text.Trim()));

                    data.CanceledTime = DateTime.Parse(lbCancelTime.Text.Trim());
                }

                #endregion

                #region 修改者資料
                if (data.LastUpdateName != tbxUpdateName.Text.Trim())
                {
                    logs.AppendLine(string.Format("原修改者「{0}」變更為「{1}」", data.LastUpdateName, tbxUpdateName.Text.Trim()));

                    data.LastUpdateName = tbxUpdateName.Text.Trim();
                }
                if (data.LastUpdateBy != tbxUpdateBy.Text.Trim())
                {
                    logs.AppendLine(string.Format("原修改者帳號「{0}」變更為「{1}」", data.LastUpdateBy, tbxUpdateBy.Text.Trim()));

                    data.LastUpdateBy = tbxUpdateBy.Text.Trim();
                }
                if (data.LastUpdateTime != DateTime.Parse(lbUpdateTime.Text.Trim()))
                {
                    logs.AppendLine(string.Format("原修改者時間「{0}」變更為「{1}」", data.LastUpdateTime.ToString("yyyy/MM/dd hh:mm"), lbUpdateTime.Text.Trim()));

                    data.LastUpdateTime = DateTime.Parse(lbUpdateTime.Text.Trim());
                }

                #endregion
                #endregion

                // 資料儲存
                try
                {
                    this._access.UpdateValues(listUpdateData);
                    FISCA.LogAgent.ApplicationLog.Log("整潔競賽", "修改評分紀錄", logs.ToString());
                    MsgBox.Show("資料更新成功!");
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MsgBox.Show(ex.Message);
                }
            }
        }
    }
}
