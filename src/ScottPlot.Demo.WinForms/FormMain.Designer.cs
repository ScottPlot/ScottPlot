namespace ScottPlot.Demo.WinForms
{
    partial class FormMain
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
            this.label14 = new System.Windows.Forms.Label();
            this.AxisLimitsButton = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.ScrollViewerButton = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.RightClickMenuButton = new System.Windows.Forms.Button();
            this.PlotViewerButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnShowOnHover = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.GrowingData = new System.Windows.Forms.Button();
            this.LiveData = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.LinkedAxesButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ConfigButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.ToggleVisibilityButton = new System.Windows.Forms.Button();
            this.MouseTrackerButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TransparentBackgroundButton = new System.Windows.Forms.Button();
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
            this.label1.Size = new System.Drawing.Size(160, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "ScottPlot Demo";
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
            this.label3.Size = new System.Drawing.Size(232, 72);
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
            this.groupBox1.Size = new System.Drawing.Size(330, 98);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ScottPlot Cookbook";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.AxisLimitsButton);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.ScrollViewerButton);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.RightClickMenuButton);
            this.groupBox2.Controls.Add(this.PlotViewerButton);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.btnShowOnHover);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.GrowingData);
            this.groupBox2.Controls.Add(this.LiveData);
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
            this.groupBox2.Size = new System.Drawing.Size(330, 657);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "WinForms-Specific Examples";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.Location = new System.Drawing.Point(87, 602);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(237, 47);
            this.label14.TabIndex = 27;
            this.label14.Text = "Demonstrate how to apply axis limits to interactive plots";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AxisLimitsButton
            // 
            this.AxisLimitsButton.Location = new System.Drawing.Point(6, 602);
            this.AxisLimitsButton.Name = "AxisLimitsButton";
            this.AxisLimitsButton.Size = new System.Drawing.Size(75, 47);
            this.AxisLimitsButton.TabIndex = 26;
            this.AxisLimitsButton.Text = "Axis Limits";
            this.AxisLimitsButton.UseVisualStyleBackColor = true;
            this.AxisLimitsButton.Click += new System.EventHandler(this.AxisLimitsButton_Click);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.Location = new System.Drawing.Point(87, 549);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(237, 47);
            this.label13.TabIndex = 25;
            this.label13.Text = "Show a plot inside a scrolling window";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScrollViewerButton
            // 
            this.ScrollViewerButton.Location = new System.Drawing.Point(6, 549);
            this.ScrollViewerButton.Name = "ScrollViewerButton";
            this.ScrollViewerButton.Size = new System.Drawing.Size(75, 47);
            this.ScrollViewerButton.TabIndex = 24;
            this.ScrollViewerButton.Text = "Plot in a Scroll Viewer";
            this.ScrollViewerButton.UseVisualStyleBackColor = true;
            this.ScrollViewerButton.Click += new System.EventHandler(this.ScrollViewerButton_Click);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.Location = new System.Drawing.Point(87, 496);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(237, 47);
            this.label12.TabIndex = 23;
            this.label12.Text = "Shows how to create a custom right-click context menu";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RightClickMenuButton
            // 
            this.RightClickMenuButton.Location = new System.Drawing.Point(6, 496);
            this.RightClickMenuButton.Name = "RightClickMenuButton";
            this.RightClickMenuButton.Size = new System.Drawing.Size(75, 47);
            this.RightClickMenuButton.TabIndex = 22;
            this.RightClickMenuButton.Text = "Custom Right-Click Menu";
            this.RightClickMenuButton.UseVisualStyleBackColor = true;
            this.RightClickMenuButton.Click += new System.EventHandler(this.RightClickMenuButton_Click);
            // 
            // PlotViewerButton
            // 
            this.PlotViewerButton.Location = new System.Drawing.Point(6, 19);
            this.PlotViewerButton.Name = "PlotViewerButton";
            this.PlotViewerButton.Size = new System.Drawing.Size(75, 47);
            this.PlotViewerButton.TabIndex = 20;
            this.PlotViewerButton.Text = "Plot Viewer";
            this.PlotViewerButton.UseVisualStyleBackColor = true;
            this.PlotViewerButton.Click += new System.EventHandler(this.PlotViewerButton_Click);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.Location = new System.Drawing.Point(87, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(237, 47);
            this.label11.TabIndex = 21;
            this.label11.Text = "Launch a plot in a pop-up interactive window";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Location = new System.Drawing.Point(87, 390);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(237, 47);
            this.label10.TabIndex = 19;
            this.label10.Text = "Demonstrates how to show the value of the point under the cursor";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnShowOnHover
            // 
            this.btnShowOnHover.Location = new System.Drawing.Point(6, 390);
            this.btnShowOnHover.Name = "btnShowOnHover";
            this.btnShowOnHover.Size = new System.Drawing.Size(75, 47);
            this.btnShowOnHover.TabIndex = 18;
            this.btnShowOnHover.Text = "Show Value on Hover";
            this.btnShowOnHover.UseVisualStyleBackColor = true;
            this.btnShowOnHover.Click += new System.EventHandler(this.btnShowOnHover_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Location = new System.Drawing.Point(87, 337);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(237, 47);
            this.label9.TabIndex = 17;
            this.label9.Text = "Shows how to plot data which grows with time";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Location = new System.Drawing.Point(87, 284);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(237, 47);
            this.label8.TabIndex = 16;
            this.label8.Text = "Shows how to plot live data from a fixed-size array";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GrowingData
            // 
            this.GrowingData.Location = new System.Drawing.Point(6, 337);
            this.GrowingData.Name = "GrowingData";
            this.GrowingData.Size = new System.Drawing.Size(75, 47);
            this.GrowingData.TabIndex = 15;
            this.GrowingData.Text = "Growing Data";
            this.GrowingData.UseVisualStyleBackColor = true;
            this.GrowingData.Click += new System.EventHandler(this.GrowingData_Click);
            // 
            // LiveData
            // 
            this.LiveData.Location = new System.Drawing.Point(5, 284);
            this.LiveData.Name = "LiveData";
            this.LiveData.Size = new System.Drawing.Size(75, 47);
            this.LiveData.TabIndex = 14;
            this.LiveData.Text = "Live Data";
            this.LiveData.UseVisualStyleBackColor = true;
            this.LiveData.Click += new System.EventHandler(this.LiveData_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(87, 231);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(237, 47);
            this.label7.TabIndex = 13;
            this.label7.Text = "Link axes from two FormsPlot controls together";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LinkedAxesButton
            // 
            this.LinkedAxesButton.Location = new System.Drawing.Point(6, 231);
            this.LinkedAxesButton.Name = "LinkedAxesButton";
            this.LinkedAxesButton.Size = new System.Drawing.Size(75, 47);
            this.LinkedAxesButton.TabIndex = 12;
            this.LinkedAxesButton.Text = "Linked Axes";
            this.LinkedAxesButton.UseVisualStyleBackColor = true;
            this.LinkedAxesButton.Click += new System.EventHandler(this.LinkedAxesButton_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(87, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(237, 47);
            this.label6.TabIndex = 11;
            this.label6.Text = "Demonstrates how to customize the configuration of the FormsPlot user control";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConfigButton
            // 
            this.ConfigButton.Location = new System.Drawing.Point(6, 178);
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
            this.label5.Location = new System.Drawing.Point(87, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(237, 47);
            this.label5.TabIndex = 9;
            this.label5.Text = "Checkboxes control visibility of individual plot objects";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ToggleVisibilityButton
            // 
            this.ToggleVisibilityButton.Location = new System.Drawing.Point(6, 125);
            this.ToggleVisibilityButton.Name = "ToggleVisibilityButton";
            this.ToggleVisibilityButton.Size = new System.Drawing.Size(75, 47);
            this.ToggleVisibilityButton.TabIndex = 8;
            this.ToggleVisibilityButton.Text = "Toggle Visibility";
            this.ToggleVisibilityButton.UseVisualStyleBackColor = true;
            this.ToggleVisibilityButton.Click += new System.EventHandler(this.ToggleVisibilityButton_Click);
            // 
            // MouseTrackerButton
            // 
            this.MouseTrackerButton.Location = new System.Drawing.Point(6, 72);
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
            this.label2.Location = new System.Drawing.Point(87, 443);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(237, 47);
            this.label2.TabIndex = 7;
            this.label2.Text = "Shows how to creat a transparent FormsPlot that lets you see through to the backg" +
    "round of the form";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(87, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(237, 47);
            this.label4.TabIndex = 5;
            this.label4.Text = "Display mouse position in pixel coordinates and graph coordinates";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TransparentBackgroundButton
            // 
            this.TransparentBackgroundButton.Location = new System.Drawing.Point(6, 443);
            this.TransparentBackgroundButton.Name = "TransparentBackgroundButton";
            this.TransparentBackgroundButton.Size = new System.Drawing.Size(75, 47);
            this.TransparentBackgroundButton.TabIndex = 6;
            this.TransparentBackgroundButton.Text = "Transparent Background";
            this.TransparentBackgroundButton.UseVisualStyleBackColor = true;
            this.TransparentBackgroundButton.Click += new System.EventHandler(this.TransparentBackgroundButton_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(405, 370);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.label1);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScottPlot Demo";
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
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button GrowingData;
        private System.Windows.Forms.Button LiveData;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnShowOnHover;
        private System.Windows.Forms.Button PlotViewerButton;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button RightClickMenuButton;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button ScrollViewerButton;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button AxisLimitsButton;
    }
}