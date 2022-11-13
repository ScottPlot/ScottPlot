namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class SpectrogramDemo
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
            this.comboBoxMoveMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxMoveDirection = new System.Windows.Forms.ComboBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDemoDataFilePath = new System.Windows.Forms.TextBox();
            this.btnChioceDemoDataFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxMoveMode
            // 
            this.comboBoxMoveMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMoveMode.FormattingEnabled = true;
            this.comboBoxMoveMode.Location = new System.Drawing.Point(528, 44);
            this.comboBoxMoveMode.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxMoveMode.Name = "comboBoxMoveMode";
            this.comboBoxMoveMode.Size = new System.Drawing.Size(140, 25);
            this.comboBoxMoveMode.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(388, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "移动模式(MoveMode)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "移动方向(MoveOrientation)";
            // 
            // comboBoxMoveDirection
            // 
            this.comboBoxMoveDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMoveDirection.FormattingEnabled = true;
            this.comboBoxMoveDirection.Location = new System.Drawing.Point(196, 48);
            this.comboBoxMoveDirection.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxMoveDirection.Name = "comboBoxMoveDirection";
            this.comboBoxMoveDirection.Size = new System.Drawing.Size(140, 25);
            this.comboBoxMoveDirection.TabIndex = 8;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(900, 43);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 26);
            this.btnTest.TabIndex = 7;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.BackColor = System.Drawing.Color.Transparent;
            this.formsPlot1.Location = new System.Drawing.Point(16, 97);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(991, 525);
            this.formsPlot1.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "(示例数据文件)DemoDataFile";
            // 
            // txtDemoDataFilePath
            // 
            this.txtDemoDataFilePath.Location = new System.Drawing.Point(195, 14);
            this.txtDemoDataFilePath.Name = "txtDemoDataFilePath";
            this.txtDemoDataFilePath.Size = new System.Drawing.Size(699, 23);
            this.txtDemoDataFilePath.TabIndex = 14;
            // 
            // btnChioceDemoDataFile
            // 
            this.btnChioceDemoDataFile.Location = new System.Drawing.Point(900, 14);
            this.btnChioceDemoDataFile.Name = "btnChioceDemoDataFile";
            this.btnChioceDemoDataFile.Size = new System.Drawing.Size(75, 23);
            this.btnChioceDemoDataFile.TabIndex = 15;
            this.btnChioceDemoDataFile.Text = "...";
            this.btnChioceDemoDataFile.UseVisualStyleBackColor = true;
            this.btnChioceDemoDataFile.Click += new System.EventHandler(this.btnChioceDemoDataFile_Click);
            // 
            // SpectrogramDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 638);
            this.Controls.Add(this.btnChioceDemoDataFile);
            this.Controls.Add(this.txtDemoDataFilePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxMoveMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxMoveDirection);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.btnTest);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SpectrogramDemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SpectrogramDemo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpectrogramDemo_FormClosing);
            this.Load += new System.EventHandler(this.SpectrogramDemo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxMoveMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxMoveDirection;
        private System.Windows.Forms.Button btnTest;
        private FormsPlot formsPlot1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDemoDataFilePath;
        private System.Windows.Forms.Button btnChioceDemoDataFile;
    }
}