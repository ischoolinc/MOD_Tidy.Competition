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
using FISCA.Data;
using K12.Data;
using System.Xml;

namespace Ischool.Tidy_Competition
{
    public partial class ResponsibleArea : BaseForm
    {
        private QueryHelper _qh = new QueryHelper();
        private UpdateHelper _up = new UpdateHelper();
        private string _configName = "整潔競賽負責區域相簿連結";
        private string _configID = ""; // list ID

        public ResponsibleArea()
        {
            InitializeComponent();
        }

        private void ResponsibleArea_Load(object sender, EventArgs e)
        {
            string sql = string.Format(@"
SELECT
    *
FROM
    list
WHERE
    name = '{0}'
            ",this._configName);


            DataTable dt = this._qh.Select(sql);
            
            if (dt.Rows.Count > 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("" + dt.Rows[0]["content"]);
                XmlElement element = (XmlElement)doc.SelectNodes("Configurations/Configuration")[0];
                string link = element.InnerText;
                tbxLink.Text = link;

                this._configID = "" + dt.Rows[0]["list_id"];
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sql = "";
            string content = string.Format("<Configurations><Configuration Name=\"url\">{0}</Configuration></Configurations>", tbxLink.Text.Trim());

            if (this._configID == "")
            {
                sql = string.Format(@"
INSERT INTO list(
    name
    , content
) VALUES(
    '{0}'
    , '{1}'
)
                ",this._configName, content);
            }
            else
            {
                sql = string.Format(@"
UPDATE list SET
    content = '{0}'
WHERE
    list_id = {1}
                ", content, this._configID);
            }

            try
            {
                _up.Execute(sql);
                MsgBox.Show("儲存成功!");
            }
            catch(Exception ex)
            {
                MsgBox.Show(ex.Message);
            }

        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
