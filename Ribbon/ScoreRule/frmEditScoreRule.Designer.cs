namespace Ischool.Tidy_Competition
{
    partial class frmEditScoreRule
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.tbxName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tbxWeekTotal = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tbxScoreLimit = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cbxFormula = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.lbAccount = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.lbCreatTime = new DevComponents.DotNetBar.LabelX();
            this.btnLeave = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 71);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(90, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "準則名稱";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(12, 159);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(90, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "區域每週總分";
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(12, 203);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(90, 23);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "每日扣分上限";
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(12, 115);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(90, 23);
            this.labelX4.TabIndex = 3;
            this.labelX4.Text = "準則公式";
            // 
            // tbxName
            // 
            // 
            // 
            // 
            this.tbxName.Border.Class = "TextBoxBorder";
            this.tbxName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbxName.Location = new System.Drawing.Point(108, 70);
            this.tbxName.Name = "tbxName";
            this.tbxName.Size = new System.Drawing.Size(130, 25);
            this.tbxName.TabIndex = 4;
            this.tbxName.TextChanged += new System.EventHandler(this.tbxName_TextChanged);
            // 
            // tbxWeekTotal
            // 
            // 
            // 
            // 
            this.tbxWeekTotal.Border.Class = "TextBoxBorder";
            this.tbxWeekTotal.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbxWeekTotal.Location = new System.Drawing.Point(108, 158);
            this.tbxWeekTotal.Name = "tbxWeekTotal";
            this.tbxWeekTotal.Size = new System.Drawing.Size(60, 25);
            this.tbxWeekTotal.TabIndex = 5;
            this.tbxWeekTotal.TextChanged += new System.EventHandler(this.tbxWeekTotal_TextChanged);
            // 
            // tbxScoreLimit
            // 
            // 
            // 
            // 
            this.tbxScoreLimit.Border.Class = "TextBoxBorder";
            this.tbxScoreLimit.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbxScoreLimit.Location = new System.Drawing.Point(108, 202);
            this.tbxScoreLimit.Name = "tbxScoreLimit";
            this.tbxScoreLimit.Size = new System.Drawing.Size(60, 25);
            this.tbxScoreLimit.TabIndex = 6;
            this.tbxScoreLimit.TextChanged += new System.EventHandler(this.tbxScoreLimit_TextChanged);
            // 
            // cbxFormula
            // 
            this.cbxFormula.DisplayMember = "Text";
            this.cbxFormula.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxFormula.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFormula.FormattingEnabled = true;
            this.cbxFormula.ItemHeight = 19;
            this.cbxFormula.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2});
            this.cbxFormula.Location = new System.Drawing.Point(108, 114);
            this.cbxFormula.Name = "cbxFormula";
            this.cbxFormula.Size = new System.Drawing.Size(226, 25);
            this.cbxFormula.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxFormula.TabIndex = 7;
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = "";
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(12, 17);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(75, 23);
            this.labelX5.TabIndex = 8;
            this.labelX5.Text = "建立者帳號";
            // 
            // lbAccount
            // 
            this.lbAccount.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbAccount.BackgroundStyle.Class = "";
            this.lbAccount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbAccount.Location = new System.Drawing.Point(91, 17);
            this.lbAccount.Name = "lbAccount";
            this.lbAccount.Size = new System.Drawing.Size(116, 23);
            this.lbAccount.TabIndex = 9;
            this.lbAccount.Text = "labelX6";
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = "";
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(269, 17);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(65, 23);
            this.labelX7.TabIndex = 10;
            this.labelX7.Text = "建立日期";
            // 
            // lbCreatTime
            // 
            this.lbCreatTime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbCreatTime.BackgroundStyle.Class = "";
            this.lbCreatTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbCreatTime.Location = new System.Drawing.Point(340, 17);
            this.lbCreatTime.Name = "lbCreatTime";
            this.lbCreatTime.Size = new System.Drawing.Size(115, 23);
            this.lbCreatTime.TabIndex = 11;
            this.lbCreatTime.Text = "labelX8";
            // 
            // btnLeave
            // 
            this.btnLeave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLeave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLeave.BackColor = System.Drawing.Color.Transparent;
            this.btnLeave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLeave.Location = new System.Drawing.Point(380, 274);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(75, 23);
            this.btnLeave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLeave.TabIndex = 12;
            this.btnLeave.Text = "離開";
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(299, 274);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "一般週統計計算公式";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "資源回收週統計計算公式";
            // 
            // frmAddScoreRule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 309);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.lbCreatTime);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.lbAccount);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.cbxFormula);
            this.Controls.Add(this.tbxScoreLimit);
            this.Controls.Add(this.tbxWeekTotal);
            this.Controls.Add(this.tbxName);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.Name = "frmAddScoreRule";
            this.Text = "新增分數準則";
            this.Load += new System.EventHandler(this.frmAddScoreRule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX tbxName;
        private DevComponents.DotNetBar.Controls.TextBoxX tbxWeekTotal;
        private DevComponents.DotNetBar.Controls.TextBoxX tbxScoreLimit;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxFormula;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX lbAccount;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX lbCreatTime;
        private DevComponents.DotNetBar.ButtonX btnLeave;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
    }
}