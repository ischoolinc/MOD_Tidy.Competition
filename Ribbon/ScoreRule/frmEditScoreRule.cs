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

namespace Ischool.Tidy_Competition
{
    public partial class frmEditScoreRule : BaseForm
    {
        private FormMode _mode = FormMode.Add ;
        private UDT.ScoreRule _data;
        private AccessHelper _access = new AccessHelper();

        public frmEditScoreRule()
        {
            InitializeComponent();
        }

        public frmEditScoreRule(FormMode mode) : this()
        {
            this._mode = mode;
        }

        /// <summary>
        /// 設定畫面資料
        /// </summary>
        /// <param name="data"></param>
        public void setData(UDT.ScoreRule data)
        {
            this._data = data;
            tbxName.Text = data.Name;
            tbxWeekTotal.Text = "" + data.WeeklyTotal;
            tbxScoreLimit.Text = "" + data.MacDailyDeduction;

        }

        private void frmAddScoreRule_Load(object sender, EventArgs e)
        {
            lbAccount.Text = DAO.Actor.Instance().GetUserAccount();
            if (this._mode == FormMode.Add)
            {
                lbCreatTime.Text = DateTime.Now.ToString("yyyy/MM/dd");
            }
            else
            {
                lbCreatTime.Text = this._data.CreateTime.ToString("yyyy/MM/dd");
            }

            cbxFormula.SelectedIndex = 0;
        }

        private bool tbxName_Validate()
        {
            if (string.IsNullOrEmpty(tbxName.Text.Trim()))
            {
                errorProvider1.SetError(tbxName,"欄位不可空白!");
                return false;
            }
            errorProvider1.SetError(tbxName, null);
            return true;
        }

        private bool tbxWeekTotal_Validate()
        {
            if (string.IsNullOrEmpty(tbxWeekTotal.Text.Trim()))
            {
                errorProvider1.SetError(tbxWeekTotal, "欄位不可空白!");
                return false;
            }
            else
            {
                int n = 0;
                if (!int.TryParse(tbxWeekTotal.Text,out n))
                {
                    errorProvider1.SetError(tbxWeekTotal, "只允許填入數值!");
                    return false;
                }
                else
                {
                    errorProvider1.SetError(tbxWeekTotal, null);
                    return true;
                }
            }
        }

        private bool tbxScoreLimit_Validate()
        {
            if (string.IsNullOrEmpty(tbxScoreLimit.Text.Trim()))
            {
                errorProvider1.SetError(tbxScoreLimit, "欄位不可空白!");
                return false;
            }
            else
            {
                int n = 0;
                if (!int.TryParse(tbxScoreLimit.Text, out n))
                {
                    errorProvider1.SetError(tbxScoreLimit, "只允許填入數值!");
                    return false;
                }
                else
                {
                    if (int.Parse(tbxScoreLimit.Text) < 0)
                    {
                        errorProvider1.SetError(tbxScoreLimit, "只允許填入正數!");
                        return false;
                    }
                    errorProvider1.SetError(tbxScoreLimit, null);
                    return true;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbxName_Validate() && tbxWeekTotal_Validate() && tbxScoreLimit_Validate())
            {
                List<UDT.ScoreRule> listData = new List<UDT.ScoreRule>();

                if (this._mode == FormMode.Add)
                {
                    UDT.ScoreRule sr = new UDT.ScoreRule();
                    fillData(sr);
                    listData.Add(sr);

                    try
                    {
                        this._access.InsertValues(listData);
                        MsgBox.Show("資料儲存成功!");

                        this.DialogResult = DialogResult.Yes;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Show(ex.Message);
                    }
                }
                else
                {
                    UDT.ScoreRule sr = this._data;
                    fillData(sr);
                    listData.Add(sr);

                    try
                    {
                        this._access.UpdateValues(listData);
                        MsgBox.Show("資料更新成功!");

                        this.DialogResult = DialogResult.Yes;
                        this.Close();
                    }
                    catch(Exception ex)
                    {
                        MsgBox.Show(ex.Message);
                    }
                }
            }
        }

        private void fillData(UDT.ScoreRule sr)
        {
            sr.Name = tbxName.Text;
            sr.WeeklyTotal = int.Parse(tbxWeekTotal.Text);
            sr.MacDailyDeduction = int.Parse(tbxScoreLimit.Text);
            sr.Formula = cbxFormula.SelectedItem.ToString();
            sr.CreateTime = DateTime.Parse(lbCreatTime.Text);
            sr.CreatedBy = lbAccount.Text;
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbxName_TextChanged(object sender, EventArgs e)
        {
            tbxName_Validate();
        }

        private void tbxWeekTotal_TextChanged(object sender, EventArgs e)
        {
            tbxWeekTotal_Validate();
        }

        private void tbxScoreLimit_TextChanged(object sender, EventArgs e)
        {
            tbxScoreLimit_Validate();
        }
    }
}
