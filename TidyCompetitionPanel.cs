using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation;
using CefSharp.WinForms;

namespace Ischool.Tidy_Competition
{
    public partial class TidyCompetitionPanel : BlankPanel 
    {
        private CefSharp.WinForms.ChromiumWebBrowser _browser;

        private TidyCompetitionPanel()
        {
            InitializeComponent();

            Group = "整潔競賽";

            this._browser = new ChromiumWebBrowser("https://sites.google.com/ischool.com.tw/neat-competition/%E9%A6%96%E9%A0%81");
            this._browser.Dock = DockStyle.Fill;
            ContentPanePanel.Controls.Add(this._browser);
        }

        private static TidyCompetitionPanel _TidyCompetitionPanel;

        public static TidyCompetitionPanel Instance
        {
            get
            {
                if (_TidyCompetitionPanel == null)
                {
                    _TidyCompetitionPanel = new TidyCompetitionPanel();
                }
                return _TidyCompetitionPanel;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ContentPanePanel
            // 
            this.ContentPanePanel.Location = new System.Drawing.Point(0, 163);
            this.ContentPanePanel.Size = new System.Drawing.Size(870, 421);
            // 
            // TidyCompetitionPanel
            // 
            this.Name = "TidyCompetitionPanel";
            this.ResumeLayout(false);

        }
    }
}
