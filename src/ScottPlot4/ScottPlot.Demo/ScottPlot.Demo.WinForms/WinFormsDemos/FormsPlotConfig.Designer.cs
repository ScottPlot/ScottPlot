namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    partial class FormsPlotConfig
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
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.cbPannable = new System.Windows.Forms.CheckBox();
            this.cbZoomable = new System.Windows.Forms.CheckBox();
            this.cbLowQualWhileDragging = new System.Windows.Forms.CheckBox();
            this.cbRightClickMenu = new System.Windows.Forms.CheckBox();
            this.cbDoubleClickBenchmark = new System.Windows.Forms.CheckBox();
            this.cbLockVertical = new System.Windows.Forms.CheckBox();
            this.cbLockHorizontal = new System.Windows.Forms.CheckBox();
            this.cbEqualAxes = new System.Windows.Forms.CheckBox();
            this.cbCustomRightClick = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.Location = new System.Drawing.Point(14, 93);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(905, 412);
            this.formsPlot1.TabIndex = 0;
            // 
            // cbPannable
            // 
            this.cbPannable.AutoSize = true;
            this.cbPannable.Checked = true;
            this.cbPannable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPannable.Location = new System.Drawing.Point(14, 14);
            this.cbPannable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbPannable.Name = "cbPannable";
            this.cbPannable.Size = new System.Drawing.Size(75, 19);
            this.cbPannable.TabIndex = 1;
            this.cbPannable.Text = "Pannable";
            this.cbPannable.UseVisualStyleBackColor = true;
            this.cbPannable.CheckedChanged += new System.EventHandler(this.cbPannable_CheckedChanged);
            // 
            // cbZoomable
            // 
            this.cbZoomable.AutoSize = true;
            this.cbZoomable.Checked = true;
            this.cbZoomable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbZoomable.Location = new System.Drawing.Point(14, 40);
            this.cbZoomable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbZoomable.Name = "cbZoomable";
            this.cbZoomable.Size = new System.Drawing.Size(80, 19);
            this.cbZoomable.TabIndex = 2;
            this.cbZoomable.Text = "Zoomable";
            this.cbZoomable.UseVisualStyleBackColor = true;
            this.cbZoomable.CheckedChanged += new System.EventHandler(this.cbZoomable_CheckedChanged);
            // 
            // cbLowQualWhileDragging
            // 
            this.cbLowQualWhileDragging.AutoSize = true;
            this.cbLowQualWhileDragging.Checked = true;
            this.cbLowQualWhileDragging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLowQualWhileDragging.Location = new System.Drawing.Point(14, 67);
            this.cbLowQualWhileDragging.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbLowQualWhileDragging.Name = "cbLowQualWhileDragging";
            this.cbLowQualWhileDragging.Size = new System.Drawing.Size(169, 19);
            this.cbLowQualWhileDragging.TabIndex = 3;
            this.cbLowQualWhileDragging.Text = "Low quality while dragging";
            this.cbLowQualWhileDragging.UseVisualStyleBackColor = true;
            this.cbLowQualWhileDragging.CheckedChanged += new System.EventHandler(this.cbLowQualWhileDragging_CheckedChanged);
            // 
            // cbRightClickMenu
            // 
            this.cbRightClickMenu.AutoSize = true;
            this.cbRightClickMenu.Checked = true;
            this.cbRightClickMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRightClickMenu.Location = new System.Drawing.Point(435, 14);
            this.cbRightClickMenu.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbRightClickMenu.Name = "cbRightClickMenu";
            this.cbRightClickMenu.Size = new System.Drawing.Size(117, 19);
            this.cbRightClickMenu.TabIndex = 4;
            this.cbRightClickMenu.Text = "Right-click menu";
            this.cbRightClickMenu.UseVisualStyleBackColor = true;
            this.cbRightClickMenu.CheckedChanged += new System.EventHandler(this.cbRightClickMenu_CheckedChanged);
            // 
            // cbDoubleClickBenchmark
            // 
            this.cbDoubleClickBenchmark.AutoSize = true;
            this.cbDoubleClickBenchmark.Checked = true;
            this.cbDoubleClickBenchmark.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDoubleClickBenchmark.Location = new System.Drawing.Point(435, 67);
            this.cbDoubleClickBenchmark.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbDoubleClickBenchmark.Name = "cbDoubleClickBenchmark";
            this.cbDoubleClickBenchmark.Size = new System.Drawing.Size(156, 19);
            this.cbDoubleClickBenchmark.TabIndex = 5;
            this.cbDoubleClickBenchmark.Text = "Double-click benchmark";
            this.cbDoubleClickBenchmark.UseVisualStyleBackColor = true;
            this.cbDoubleClickBenchmark.CheckedChanged += new System.EventHandler(this.cbDoubleClickBenchmark_CheckedChanged);
            // 
            // cbLockVertical
            // 
            this.cbLockVertical.AutoSize = true;
            this.cbLockVertical.Location = new System.Drawing.Point(224, 14);
            this.cbLockVertical.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbLockVertical.Name = "cbLockVertical";
            this.cbLockVertical.Size = new System.Drawing.Size(115, 19);
            this.cbLockVertical.TabIndex = 6;
            this.cbLockVertical.Text = "Lock vertical axis";
            this.cbLockVertical.UseVisualStyleBackColor = true;
            this.cbLockVertical.CheckedChanged += new System.EventHandler(this.cbLockVertical_CheckedChanged);
            // 
            // cbLockHorizontal
            // 
            this.cbLockHorizontal.AutoSize = true;
            this.cbLockHorizontal.Location = new System.Drawing.Point(224, 40);
            this.cbLockHorizontal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbLockHorizontal.Name = "cbLockHorizontal";
            this.cbLockHorizontal.Size = new System.Drawing.Size(130, 19);
            this.cbLockHorizontal.TabIndex = 7;
            this.cbLockHorizontal.Text = "Lock horizontal axis";
            this.cbLockHorizontal.UseVisualStyleBackColor = true;
            this.cbLockHorizontal.CheckedChanged += new System.EventHandler(this.cbLockHorizontal_CheckedChanged);
            // 
            // cbEqualAxes
            // 
            this.cbEqualAxes.AutoSize = true;
            this.cbEqualAxes.Location = new System.Drawing.Point(224, 67);
            this.cbEqualAxes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbEqualAxes.Name = "cbEqualAxes";
            this.cbEqualAxes.Size = new System.Drawing.Size(81, 19);
            this.cbEqualAxes.TabIndex = 8;
            this.cbEqualAxes.Text = "Equal axes";
            this.cbEqualAxes.UseVisualStyleBackColor = true;
            this.cbEqualAxes.CheckedChanged += new System.EventHandler(this.cbEqualAxes_CheckedChanged);
            // 
            // cbCustomRightClick
            // 
            this.cbCustomRightClick.AutoSize = true;
            this.cbCustomRightClick.Location = new System.Drawing.Point(435, 40);
            this.cbCustomRightClick.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbCustomRightClick.Name = "cbCustomRightClick";
            this.cbCustomRightClick.Size = new System.Drawing.Size(161, 19);
            this.cbCustomRightClick.TabIndex = 9;
            this.cbCustomRightClick.Text = "Custom right-click action";
            this.cbCustomRightClick.UseVisualStyleBackColor = true;
            this.cbCustomRightClick.CheckedChanged += new System.EventHandler(this.cbCustomRightClick_CheckedChanged);
            // 
            // FormsPlotConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.cbCustomRightClick);
            this.Controls.Add(this.cbEqualAxes);
            this.Controls.Add(this.cbLockHorizontal);
            this.Controls.Add(this.cbLockVertical);
            this.Controls.Add(this.cbDoubleClickBenchmark);
            this.Controls.Add(this.cbRightClickMenu);
            this.Controls.Add(this.cbLowQualWhileDragging);
            this.Controls.Add(this.cbZoomable);
            this.Controls.Add(this.cbPannable);
            this.Controls.Add(this.formsPlot1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FormsPlotConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormsPlotConfig";
            this.Load += new System.EventHandler(this.FormsPlotConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormsPlot formsPlot1;
        private System.Windows.Forms.CheckBox cbPannable;
        private System.Windows.Forms.CheckBox cbZoomable;
        private System.Windows.Forms.CheckBox cbLowQualWhileDragging;
        private System.Windows.Forms.CheckBox cbRightClickMenu;
        private System.Windows.Forms.CheckBox cbDoubleClickBenchmark;
        private System.Windows.Forms.CheckBox cbLockVertical;
        private System.Windows.Forms.CheckBox cbLockHorizontal;
        private System.Windows.Forms.CheckBox cbEqualAxes;
        private System.Windows.Forms.CheckBox cbCustomRightClick;
    }
}