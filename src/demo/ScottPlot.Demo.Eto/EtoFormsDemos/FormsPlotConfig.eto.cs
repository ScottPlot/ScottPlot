using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    partial class FormsPlotConfig : Form
    {
        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.Eto.PlotView();
            this.cbPannable = new CheckBox();
            this.cbZoomable = new CheckBox();
            this.cbLowQualWhileDragging = new CheckBox();
            this.cbRightClickMenu = new CheckBox();
            this.cbDoubleClickBenchmark = new CheckBox();
            this.cbLockVertical = new CheckBox();
            this.cbLockHorizontal = new CheckBox();
            this.cbEqualAxes = new CheckBox();
            this.cbCustomRightClick = new CheckBox();
            this.SuspendLayout();

            var layout = new DynamicLayout() { Padding = 5 };
            layout.AddRow(cbPannable, cbLockVertical, cbRightClickMenu, null);
            layout.AddRow(cbZoomable, cbLockHorizontal, cbCustomRightClick, null);
            layout.AddRow(cbLowQualWhileDragging, cbEqualAxes, cbDoubleClickBenchmark, null);


            this.Content = new DynamicLayout(layout, formsPlot1);

            // 
            // formsPlot1
            // 
            //this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            //| System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            //this.formsPlot1.Location = new System.Drawing.Point(14, 93);
            //this.formsPlot1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            //this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new Size(905, 412);
            this.formsPlot1.TabIndex = 0;
            // 
            // cbPannable
            // 
            //this.cbPannable.AutoSize = true;
            this.cbPannable.Checked = true;
            //this.cbPannable.CheckState = System.Windows.Forms.CheckState.Checked;
            //this.cbPannable.Location = new System.Drawing.Point(14, 14);
            //this.cbPannable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            //this.cbPannable.Name = "cbPannable";
            this.cbPannable.Size = new Size(75, 19);
            this.cbPannable.TabIndex = 1;
            this.cbPannable.Text = "Pannable";
            //this.cbPannable.UseVisualStyleBackColor = true;
            // 
            // cbZoomable
            // 
            // this.cbZoomable.AutoSize = true;
            this.cbZoomable.Checked = true;
            // this.cbZoomable.CheckState = System.Windows.Forms.CheckState.Checked;
            // this.cbZoomable.Location = new System.Drawing.Point(14, 40);
            // this.cbZoomable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // this.cbZoomable.Name = "cbZoomable";
            this.cbZoomable.Size = new Size(80, 19);
            this.cbZoomable.TabIndex = 2;
            this.cbZoomable.Text = "Zoomable";
            // this.cbZoomable.UseVisualStyleBackColor = true;
            // 
            // cbLowQualWhileDragging
            // 
            // this.cbLowQualWhileDragging.AutoSize = true;
            this.cbLowQualWhileDragging.Checked = true;
            // this.cbLowQualWhileDragging.CheckState = System.Windows.Forms.CheckState.Checked;
            // this.cbLowQualWhileDragging.Location = new System.Drawing.Point(14, 67);
            // this.cbLowQualWhileDragging.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // this.cbLowQualWhileDragging.Name = "cbLowQualWhileDragging";
            this.cbLowQualWhileDragging.Size = new Size(169, 19);
            this.cbLowQualWhileDragging.TabIndex = 3;
            this.cbLowQualWhileDragging.Text = "Low quality while dragging";
            // this.cbLowQualWhileDragging.UseVisualStyleBackColor = true;
            // 
            // cbRightClickMenu
            // 
            // this.cbRightClickMenu.AutoSize = true;
            this.cbRightClickMenu.Checked = true;
            // this.cbRightClickMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            // this.cbRightClickMenu.Location = new System.Drawing.Point(435, 14);
            // this.cbRightClickMenu.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // this.cbRightClickMenu.Name = "cbRightClickMenu";
            this.cbRightClickMenu.Size = new Size(117, 19);
            this.cbRightClickMenu.TabIndex = 4;
            this.cbRightClickMenu.Text = "Right-click menu";
            // this.cbRightClickMenu.UseVisualStyleBackColor = true;
            // 
            // cbDoubleClickBenchmark
            // 
            // this.cbDoubleClickBenchmark.AutoSize = true;
            this.cbDoubleClickBenchmark.Checked = true;
            // this.cbDoubleClickBenchmark.CheckState = System.Windows.Forms.CheckState.Checked;
            // this.cbDoubleClickBenchmark.Location = new System.Drawing.Point(435, 67);
            // this.cbDoubleClickBenchmark.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // this.cbDoubleClickBenchmark.Name = "cbDoubleClickBenchmark";
            this.cbDoubleClickBenchmark.Size = new Size(156, 19);
            this.cbDoubleClickBenchmark.TabIndex = 5;
            this.cbDoubleClickBenchmark.Text = "Double-click benchmark";
            // this.cbDoubleClickBenchmark.UseVisualStyleBackColor = true;
            // 
            // cbLockVertical
            // 
            // this.cbLockVertical.AutoSize = true;
            // this.cbLockVertical.Location = new System.Drawing.Point(224, 14);
            // this.cbLockVertical.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // this.cbLockVertical.Name = "cbLockVertical";
            this.cbLockVertical.Size = new Size(115, 19);
            this.cbLockVertical.TabIndex = 6;
            this.cbLockVertical.Text = "Lock vertical axis";
            // this.cbLockVertical.UseVisualStyleBackColor = true;
            // 
            // cbLockHorizontal
            // 
            // this.cbLockHorizontal.AutoSize = true;
            // this.cbLockHorizontal.Location = new System.Drawing.Point(224, 40);
            // this.cbLockHorizontal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // this.cbLockHorizontal.Name = "cbLockHorizontal";
            this.cbLockHorizontal.Size = new Size(130, 19);
            this.cbLockHorizontal.TabIndex = 7;
            this.cbLockHorizontal.Text = "Lock horizontal axis";
            // this.cbLockHorizontal.UseVisualStyleBackColor = true;
            // 
            // cbEqualAxes
            // 
            // this.cbEqualAxes.AutoSize = true;
            // this.cbEqualAxes.Location = new System.Drawing.Point(224, 67);
            // this.cbEqualAxes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // this.cbEqualAxes.Name = "cbEqualAxes";
            this.cbEqualAxes.Size = new Size(81, 19);
            this.cbEqualAxes.TabIndex = 8;
            this.cbEqualAxes.Text = "Equal axes";
            // this.cbEqualAxes.UseVisualStyleBackColor = true;
            // 
            // cbCustomRightClick
            // 
            // this.cbCustomRightClick.AutoSize = true;
            // this.cbCustomRightClick.Location = new System.Drawing.Point(435, 40);
            // this.cbCustomRightClick.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // this.cbCustomRightClick.Name = "cbCustomRightClick";
            this.cbCustomRightClick.Size = new Size(161, 19);
            this.cbCustomRightClick.TabIndex = 9;
            this.cbCustomRightClick.Text = "Custom right-click action";
            // this.cbCustomRightClick.UseVisualStyleBackColor = true;
            // 
            // FormsPlotConfig
            // 
            // this.AutoScaleDimensions = new SizeF(7F, 15F);
            // this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(933, 519);
            // this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            // this.Name = "FormsPlotConfig";
            // this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Title = "FormsPlotConfig";
            this.ResumeLayout();
        }

        private ScottPlot.Eto.PlotView formsPlot1;
        private CheckBox cbPannable;
        private CheckBox cbZoomable;
        private CheckBox cbLowQualWhileDragging;
        private CheckBox cbRightClickMenu;
        private CheckBox cbDoubleClickBenchmark;
        private CheckBox cbLockVertical;
        private CheckBox cbLockHorizontal;
        private CheckBox cbEqualAxes;
        private CheckBox cbCustomRightClick;
    }
}
