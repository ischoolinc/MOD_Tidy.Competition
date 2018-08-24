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
using K12.Data;

namespace Ischool.Tidy_Competition
{
    public partial class frmAddScoreSheet : BaseForm
    {
        private string _userAccount = DAO.Actor.Instance().GetUserAccount();
        private string _userName = DAO.Actor.Instance().GetUserName();
        private AccessHelper _access = new AccessHelper();
        private Dictionary<string, UDT.Area> _dicAreaByName = new Dictionary<string, UDT.Area>();
        private Dictionary<string, UDT.Period> _dicPeriodByName = new Dictionary<string, UDT.Period>();
        private Dictionary<string, UDT.Place> _dicPlaceByName = new Dictionary<string, UDT.Place>();
        private Dictionary<string, UDT.DeDuctionItem> _dicItemByName = new Dictionary<string, UDT.DeDuctionItem>();
        private Dictionary<string, UDT.DeDuctionStandard> _dicStandardByName = new Dictionary<string, UDT.DeDuctionStandard>();

        public frmAddScoreSheet()
        {
            InitializeComponent();
        }

        private void frmAddScoreSheet_Load(object sender, EventArgs e)
        {
            lbAccount.Text = this._userAccount;

            // Init SchoolYear Semester
            lbSchoolYear.Text = School.DefaultSchoolYear;
            lbSemester.Text = School.DefaultSemester;

            // Init DateTime
            dateTimeInput1.Value = DateTime.Now;

            #region Init Area
            {
                // 取得區域資料
                List<UDT.Area> listArea = this._access.Select<UDT.Area>("enabled = true");
                foreach (UDT.Area data in listArea)
                {
                    cbxArea.Items.Add(data.Name);
                    this._dicAreaByName.Add(data.Name, data);
                }
                if (cbxArea.Items.Count > 0)
                {
                    cbxArea.SelectedIndex = 0;
                }
                else
                {
                    MsgBox.Show("請先設定區域資料!");
                    this.Close();
                }
            }
            #endregion

            #region Init Period
            {
                // 取得時段資料
                List<UDT.Period> listPeriod = this._access.Select<UDT.Period>("enabled = true");
                foreach (UDT.Period data in listPeriod)
                {
                    cbxPeriod.Items.Add(data.Name);
                    this._dicPeriodByName.Add(data.Name, data);
                }
                if (cbxPeriod.Items.Count > 0)
                {
                    cbxPeriod.SelectedIndex = 0;
                }
                else
                {
                    MsgBox.Show("請先設定時段資料!");
                    return;
                }
            }
            #endregion

        }

        private void cbxArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            string areaID = this._dicAreaByName[cbxArea.SelectedItem.ToString()].UID;
            ReloadPlace(areaID);
            ReloadItem(areaID);
            ReloadStandard(areaID);
        }

        private void ReloadPlace(string areaID)
        {
            cbxPlace.Items.Clear();
            this._dicPlaceByName.Clear();
            List<UDT.Place> listPlace = this._access.Select<UDT.Place>(string.Format("ref_area_id = {0} AND enabled = true",areaID));
            foreach (UDT.Place data in listPlace)
            {
                cbxPlace.Items.Add(data.Name);
                this._dicPlaceByName.Add(data.Name,data);
            }
            if (cbxPlace.Items.Count > 0)
            {
                cbxPlace.SelectedIndex = 0;
                errorProvider1.SetError(cbxPlace, null);
                btnSave.Enabled = true;
            }
            else
            {
                errorProvider1.SetError(cbxPlace, string.Format("請先設定{0}區域位置資料!", cbxArea.SelectedItem.ToString()));
                btnSave.Enabled = false;
                //MsgBox.Show(string.Format("請先設定{0}區域位置資料!",cbxArea.SelectedItem.ToString()));
                //this.Close();
            }
        }

        private void ReloadItem(string areaID)
        {
            cbxItem.Items.Clear();
            this._dicItemByName.Clear();
            List<UDT.DeDuctionItem> listItem = this._access.Select<UDT.DeDuctionItem>(string.Format("ref_area_id = {0} AND enabled = true", areaID));
            foreach (UDT.DeDuctionItem data in listItem)
            {
                cbxItem.Items.Add(data.Name);
                this._dicItemByName.Add(data.Name,data);
            }
            if (cbxItem.Items.Count > 0)
            {
                cbxItem.SelectedIndex = 0;
                errorProvider1.SetError(cbxItem, null);
                btnSave.Enabled = true;
            }
            else
            {
                errorProvider1.SetError(cbxItem, string.Format("請先設定{0}區域扣分物件資料!", cbxArea.SelectedItem.ToString()));
                btnSave.Enabled = false;
                //MsgBox.Show(string.Format("請先設定{0}區域扣分物件資料!",cbxArea.SelectedItem.ToString()));
                //this.Close();
            }
        }

        private void ReloadStandard(string areaID)
        {
            cbxStandard.Items.Clear();
            this._dicStandardByName.Clear();
            List<UDT.DeDuctionStandard> listStandard = this._access.Select<UDT.DeDuctionStandard>(string.Format("ref_area_id = {0} AND enabled = true", areaID));
            foreach (UDT.DeDuctionStandard data in listStandard)
            {
                cbxStandard.Items.Add(data.Name);
                this._dicStandardByName.Add(data.Name,data);
            }
            if (cbxStandard.Items.Count > 0)
            {
                cbxStandard.SelectedIndex = 0;
                errorProvider1.SetError(cbxStandard, null);
                btnSave.Enabled = true;
            }
            else
            {
                errorProvider1.SetError(cbxStandard, string.Format("請先設定{0}區域扣分項目資料!", cbxArea.SelectedItem.ToString()));
                btnSave.Enabled = false;
                //MsgBox.Show(string.Format("請先設定{0}區域扣分項目資料!",cbxArea.SelectedItem.ToString()));
                //this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<UDT.ScoreSheet> listInsertData = new List<UDT.ScoreSheet>();
            UDT.ScoreSheet data = new UDT.ScoreSheet();
            data.Acount = lbAccount.Text;
            data.SchoolYear = int.Parse(lbSchoolYear.Text);
            data.Semester = int.Parse(lbSemester.Text);
            data.RefPeriodID = int.Parse(this._dicPeriodByName[cbxPeriod.SelectedItem.ToString()].UID);
            data.RefPlaceID = int.Parse(this._dicPlaceByName[cbxPlace.SelectedItem.ToString()].UID);
            data.RefDeductionItemID = int.Parse(this._dicItemByName[cbxItem.SelectedItem.ToString()].UID);
            data.RefDeductionStandardID = int.Parse(this._dicStandardByName[cbxStandard.SelectedItem.ToString()].UID);
            data.Remark = tbxRemark.Text.Trim();
            data.CreateTime = DateTime.Parse(dateTimeInput1.Value.ToString("yyyy/MM/dd"));
            data.LastUpdateName = this._userName;
            data.LastUpdateBy = this._userAccount;

            listInsertData.Add(data);
            try
            {
                // 新增資料
                this._access.InsertValues(listInsertData);
                // 新增LOG
                string log = GetLogString();
                FISCA.LogAgent.ApplicationLog.Log("整潔競賽", "新增評分紀錄", log);
                MsgBox.Show("資料儲存成功!");
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            catch(Exception ex)
            {
                MsgBox.Show(ex.Message);
            }
        }

        private string GetLogString()
        {
            StringBuilder logs = new StringBuilder();
            string userName = this._userName;
            string schoolYear = lbSchoolYear.Text;
            string semester = lbSemester.Text;
            string periodName = cbxPeriod.SelectedItem.ToString();
            string placeName = cbxPlace.SelectedItem.ToString();
            string itemName = cbxItem.SelectedItem.ToString();
            string standardName = cbxStandard.SelectedItem.ToString();
            string remark = tbxRemark.Text.Trim();
            string time = DateTime.Now.ToString("yyyy/MM/dd");

            logs.AppendLine(string.Format("管理員「{0}」新增評分紀錄:\n 學年度「{1}」\n 學期「{2}」 時段「{3}」\n 位置「{4}」 扣分物件「{5}」\n 扣分項目「{6}」 \n 補充說明「{7}」\n 建立日期「{8}」"
                , userName, schoolYear, semester, periodName, placeName, itemName, standardName, remark, time));

            return logs.ToString();
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
