namespace ScottPlot.UserControls
{
    partial class FormSettings
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
            this.btnFitData = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbY2 = new System.Windows.Forms.TextBox();
            this.tbY1 = new System.Windows.Forms.TextBox();
            this.tbX2 = new System.Windows.Forms.TextBox();
            this.tbX1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbAliasDynamic = new System.Windows.Forms.RadioButton();
            this.rbAliasNever = new System.Windows.Forms.RadioButton();
            this.rbAliasAlways = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbPlotObjects = new System.Windows.Forms.ListBox();
            this.btnExportCSV = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnApplyAxes = new System.Windows.Forms.Button();
            this.lblGitHub = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFitData
            // 
            this.btnFitData.Location = new System.Drawing.Point(71, 85);
            this.btnFitData.Name = "btnFitData";
            this.btnFitData.Size = new System.Drawing.Size(100, 23);
            this.btnFitData.TabIndex = 0;
            this.btnFitData.Text = "Fit Data";
            this.btnFitData.UseVisualStyleBackColor = true;
            this.btnFitData.Click += new System.EventHandler(this.BtnFitData_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnApplyAxes);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnFitData);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbY2);
            this.groupBox1.Controls.Add(this.tbY1);
            this.groupBox1.Controls.Add(this.tbX2);
            this.groupBox1.Controls.Add(this.tbX1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 119);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Axis Limits";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(210, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "upper";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(103, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "lower";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "vertical:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "horizontal:";
            // 
            // tbY2
            // 
            this.tbY2.Location = new System.Drawing.Point(177, 59);
            this.tbY2.Name = "tbY2";
            this.tbY2.Size = new System.Drawing.Size(100, 20);
            this.tbY2.TabIndex = 4;
            // 
            // tbY1
            // 
            this.tbY1.Location = new System.Drawing.Point(71, 59);
            this.tbY1.Name = "tbY1";
            this.tbY1.Size = new System.Drawing.Size(100, 20);
            this.tbY1.TabIndex = 5;
            // 
            // tbX2
            // 
            this.tbX2.Location = new System.Drawing.Point(177, 33);
            this.tbX2.Name = "tbX2";
            this.tbX2.Size = new System.Drawing.Size(100, 20);
            this.tbX2.TabIndex = 3;
            // 
            // tbX1
            // 
            this.tbX1.Location = new System.Drawing.Point(71, 33);
            this.tbX1.Name = "tbX1";
            this.tbX1.Size = new System.Drawing.Size(100, 20);
            this.tbX1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblGitHub);
            this.groupBox2.Controls.Add(this.lblVersion);
            this.groupBox2.Location = new System.Drawing.Point(12, 190);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(297, 61);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ScottPlot";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(6, 18);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(45, 13);
            this.lblVersion.TabIndex = 6;
            this.lblVersion.Text = "Version:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.rbAliasDynamic);
            this.groupBox3.Controls.Add(this.rbAliasNever);
            this.groupBox3.Controls.Add(this.rbAliasAlways);
            this.groupBox3.Location = new System.Drawing.Point(12, 137);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(297, 47);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Anti-Aliasing";
            // 
            // rbAliasDynamic
            // 
            this.rbAliasDynamic.AutoSize = true;
            this.rbAliasDynamic.Location = new System.Drawing.Point(162, 19);
            this.rbAliasDynamic.Name = "rbAliasDynamic";
            this.rbAliasDynamic.Size = new System.Drawing.Size(108, 17);
            this.rbAliasDynamic.TabIndex = 2;
            this.rbAliasDynamic.TabStop = true;
            this.rbAliasDynamic.Text = "off while dragging";
            this.rbAliasDynamic.UseVisualStyleBackColor = true;
            this.rbAliasDynamic.CheckedChanged += new System.EventHandler(this.RbAliasDynamic_CheckedChanged);
            // 
            // rbAliasNever
            // 
            this.rbAliasNever.AutoSize = true;
            this.rbAliasNever.Location = new System.Drawing.Point(84, 19);
            this.rbAliasNever.Name = "rbAliasNever";
            this.rbAliasNever.Size = new System.Drawing.Size(52, 17);
            this.rbAliasNever.TabIndex = 1;
            this.rbAliasNever.TabStop = true;
            this.rbAliasNever.Text = "never";
            this.rbAliasNever.UseVisualStyleBackColor = true;
            this.rbAliasNever.CheckedChanged += new System.EventHandler(this.RbAliasNever_CheckedChanged);
            // 
            // rbAliasAlways
            // 
            this.rbAliasAlways.AutoSize = true;
            this.rbAliasAlways.Location = new System.Drawing.Point(6, 19);
            this.rbAliasAlways.Name = "rbAliasAlways";
            this.rbAliasAlways.Size = new System.Drawing.Size(57, 17);
            this.rbAliasAlways.TabIndex = 0;
            this.rbAliasAlways.TabStop = true;
            this.rbAliasAlways.Text = "always";
            this.rbAliasAlways.UseVisualStyleBackColor = true;
            this.rbAliasAlways.CheckedChanged += new System.EventHandler(this.RbAliasAlways_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.lbPlotObjects);
            this.groupBox4.Controls.Add(this.btnExportCSV);
            this.groupBox4.Controls.Add(this.btnDelete);
            this.groupBox4.Controls.Add(this.btnDown);
            this.groupBox4.Controls.Add(this.btnUp);
            this.groupBox4.Location = new System.Drawing.Point(12, 257);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(297, 279);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Plot Objects";
            // 
            // lbPlotObjects
            // 
            this.lbPlotObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPlotObjects.FormattingEnabled = true;
            this.lbPlotObjects.Location = new System.Drawing.Point(6, 19);
            this.lbPlotObjects.Name = "lbPlotObjects";
            this.lbPlotObjects.Size = new System.Drawing.Size(285, 225);
            this.lbPlotObjects.TabIndex = 4;
            // 
            // btnExportCSV
            // 
            this.btnExportCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportCSV.Location = new System.Drawing.Point(177, 250);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(93, 23);
            this.btnExportCSV.TabIndex = 3;
            this.btnExportCSV.Text = "export CSV";
            this.btnExportCSV.UseVisualStyleBackColor = true;
            this.btnExportCSV.Click += new System.EventHandler(this.BtnExportCSV_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(112, 250);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(59, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDown.Enabled = false;
            this.btnDown.Location = new System.Drawing.Point(57, 250);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(49, 23);
            this.btnDown.TabIndex = 1;
            this.btnDown.Text = "down";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUp.Enabled = false;
            this.btnUp.Location = new System.Drawing.Point(6, 250);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(45, 23);
            this.btnUp.TabIndex = 0;
            this.btnUp.Text = "up";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnApplyAxes
            // 
            this.btnApplyAxes.Location = new System.Drawing.Point(177, 85);
            this.btnApplyAxes.Name = "btnApplyAxes";
            this.btnApplyAxes.Size = new System.Drawing.Size(100, 23);
            this.btnApplyAxes.TabIndex = 10;
            this.btnApplyAxes.Text = "Apply";
            this.btnApplyAxes.UseVisualStyleBackColor = true;
            this.btnApplyAxes.Click += new System.EventHandler(this.BtnApplyAxes_Click);
            // 
            // lblGitHub
            // 
            this.lblGitHub.AutoSize = true;
            this.lblGitHub.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblGitHub.ForeColor = System.Drawing.Color.Blue;
            this.lblGitHub.Location = new System.Drawing.Point(6, 38);
            this.lblGitHub.Name = "lblGitHub";
            this.lblGitHub.Size = new System.Drawing.Size(194, 13);
            this.lblGitHub.TabIndex = 7;
            this.lblGitHub.Text = "https://github.com/swharden/ScottPlot";
            this.lblGitHub.Click += new System.EventHandler(this.LblGitHub_Click);
            this.lblGitHub.MouseEnter += new System.EventHandler(this.LblGitHub_MouseEnter);
            this.lblGitHub.MouseLeave += new System.EventHandler(this.LblGitHub_MouseLeave);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 548);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormSettings";
            this.Text = "ScottPlot Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFitData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbY1;
        private System.Windows.Forms.TextBox tbY2;
        private System.Windows.Forms.TextBox tbX2;
        private System.Windows.Forms.TextBox tbX1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbAliasDynamic;
        private System.Windows.Forms.RadioButton rbAliasNever;
        private System.Windows.Forms.RadioButton rbAliasAlways;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnExportCSV;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.ListBox lbPlotObjects;
        private System.Windows.Forms.Button btnApplyAxes;
        private System.Windows.Forms.Label lblGitHub;
    }
}