namespace ScottPlot.Demo.WinForms
{
    partial class FormStartup
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
            this.cookbookButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ConfigButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.ToggleVisibilityButton = new System.Windows.Forms.Button();
            this.MouseTrackerButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TransparentBackgroundButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.LinkedAxesButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cookbookButton
            // 
            this.cookbookButton.Location = new System.Drawing.Point(6, 19);
            this.cookbookButton.Name = "cookbookButton";
            this.cookbookButton.Size = new System.Drawing.Size(75, 72);
            this.cookbookButton.TabIndex = 0;
            this.cookbookButton.Text = "Launch ScottPlot Cookbook";
            this.cookbookButton.UseVisualStyleBackColor = true;
            this.cookbookButton.Click += new System.EventHandler(this.cookbookButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "ScottPlot WinForms Demos";
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(14, 34);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(96, 25);
            this.versionLabel.TabIndex = 3;
            this.versionLabel.Text = "version 9.9.99";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(92, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(226, 72);
            this.label3.TabIndex = 5;
            this.label3.Text = "A collection of simple examples which demonstrate most features of ScottPlot";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cookbookButton);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 98);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ScottPlot Cookbook";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.LinkedAxesButton);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.ConfigButton);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.ToggleVisibilityButton);
            this.groupBox2.Controls.Add(this.MouseTrackerButton);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.TransparentBackgroundButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 166);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(324, 284);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "WinForms-Specific Examples";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(87, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(231, 47);
            this.label6.TabIndex = 11;
            this.label6.Text = "Demonstrates how to customize the configuration of the FormsPlot user control";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConfigButton
            // 
            this.ConfigButton.Location = new System.Drawing.Point(6, 125);
            this.ConfigButton.Name = "ConfigButton";
            this.ConfigButton.Size = new System.Drawing.Size(75, 47);
            this.ConfigButton.TabIndex = 10;
            this.ConfigButton.Text = "FormsPlot Config";
            this.ConfigButton.UseVisualStyleBackColor = true;
            this.ConfigButton.Click += new System.EventHandler(this.ConfigButton_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(87, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(231, 47);
            this.label5.TabIndex = 9;
            this.label5.Text = "Checkboxes control visibility of individual plot objects";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ToggleVisibilityButton
            // 
            this.ToggleVisibilityButton.Location = new System.Drawing.Point(6, 72);
            this.ToggleVisibilityButton.Name = "ToggleVisibilityButton";
            this.ToggleVisibilityButton.Size = new System.Drawing.Size(75, 47);
            this.ToggleVisibilityButton.TabIndex = 8;
            this.ToggleVisibilityButton.Text = "Toggle Visibility";
            this.ToggleVisibilityButton.UseVisualStyleBackColor = true;
            this.ToggleVisibilityButton.Click += new System.EventHandler(this.ToggleVisibilityButton_Click);
            // 
            // MouseTrackerButton
            // 
            this.MouseTrackerButton.Location = new System.Drawing.Point(6, 19);
            this.MouseTrackerButton.Name = "MouseTrackerButton";
            this.MouseTrackerButton.Size = new System.Drawing.Size(75, 47);
            this.MouseTrackerButton.TabIndex = 4;
            this.MouseTrackerButton.Text = "Mouse Tracker";
            this.MouseTrackerButton.UseVisualStyleBackColor = true;
            this.MouseTrackerButton.Click += new System.EventHandler(this.MouseTrackerButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(87, 231);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(231, 47);
            this.label2.TabIndex = 7;
            this.label2.Text = "Shows how to creat a transparent FormsPlot that lets you see through to the backg" +
    "round of the form";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(87, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(231, 47);
            this.label4.TabIndex = 5;
            this.label4.Text = "Display mouse position in pixel coordinates and graph coordinates.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TransparentBackgroundButton
            // 
            this.TransparentBackgroundButton.Location = new System.Drawing.Point(6, 231);
            this.TransparentBackgroundButton.Name = "TransparentBackgroundButton";
            this.TransparentBackgroundButton.Size = new System.Drawing.Size(75, 47);
            this.TransparentBackgroundButton.TabIndex = 6;
            this.TransparentBackgroundButton.Text = "Transparent Background";
            this.TransparentBackgroundButton.UseVisualStyleBackColor = true;
            this.TransparentBackgroundButton.Click += new System.EventHandler(this.TransparentBackgroundButton_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(87, 178);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(231, 47);
            this.label7.TabIndex = 13;
            this.label7.Text = "Link axes from two FormsPlot controls together";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LinkedAxesButton
            // 
            this.LinkedAxesButton.Location = new System.Drawing.Point(6, 178);
            this.LinkedAxesButton.Name = "LinkedAxesButton";
            this.LinkedAxesButton.Size = new System.Drawing.Size(75, 47);
            this.LinkedAxesButton.TabIndex = 12;
            this.LinkedAxesButton.Text = "Linked Axes";
            this.LinkedAxesButton.UseVisualStyleBackColor = true;
            this.LinkedAxesButton.Click += new System.EventHandler(this.LinkedAxesButton_Click);
            // 
            // FormStartup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(365, 460);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.label1);
            this.Name = "FormStartup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScottPlot Demos (WinForms)";
            this.Load += new System.EventHandler(this.FormStartup_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cookbookButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button MouseTrackerButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button TransparentBackgroundButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ConfigButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ToggleVisibilityButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button LinkedAxesButton;
    }
}