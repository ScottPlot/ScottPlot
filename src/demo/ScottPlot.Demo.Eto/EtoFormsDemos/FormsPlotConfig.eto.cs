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
            this.formsPlot1.Size = new Size(905, 412);
            // 
            // cbPannable
            // 
            this.cbPannable.Checked = true;
            this.cbPannable.Size = new Size(75, 19);
            this.cbPannable.Text = "Pannable";
            // 
            // cbZoomable
            // 
            this.cbZoomable.Checked = true;
            this.cbZoomable.Size = new Size(80, 19);
            this.cbZoomable.Text = "Zoomable";
            // 
            // cbLowQualWhileDragging
            // 
            this.cbLowQualWhileDragging.Checked = true;
            this.cbLowQualWhileDragging.Size = new Size(169, 19);
            this.cbLowQualWhileDragging.Text = "Low quality while dragging";
            // 
            // cbRightClickMenu
            // 
            this.cbRightClickMenu.Checked = true;
            this.cbRightClickMenu.Size = new Size(117, 19);
            this.cbRightClickMenu.Text = "Right-click menu";
            // 
            // cbDoubleClickBenchmark
            // 
            this.cbDoubleClickBenchmark.Checked = true;
            this.cbDoubleClickBenchmark.Size = new Size(156, 19);
            this.cbDoubleClickBenchmark.Text = "Double-click benchmark";
            // 
            // cbLockVertical
            // 
            this.cbLockVertical.Size = new Size(115, 19);
            this.cbLockVertical.Text = "Lock vertical axis";
            // 
            // cbLockHorizontal
            // 
            this.cbLockHorizontal.Size = new Size(130, 19);
            this.cbLockHorizontal.Text = "Lock horizontal axis";
            // 
            // cbEqualAxes
            // 
            this.cbEqualAxes.Size = new Size(81, 19);
            this.cbEqualAxes.Text = "Equal axes";
            // 
            // cbCustomRightClick
            // 
            this.cbCustomRightClick.Size = new Size(161, 19);
            this.cbCustomRightClick.Text = "Custom right-click action";
            // 
            // FormsPlotConfig
            // 
            this.ClientSize = new Size(933, 519);
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
