using System;
using Eto.Forms;

namespace ScottPlot.Demo.Eto
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            versionLabel.Text = Plot.Version;

            this.cookbookButton.Click += this.CookbookButton_Click;
            this.MultiAxisLockButton.Click += this.MultiAxisLockButton_Click;
            this.richTextBox1.MouseUp += this.WebsiteLink_Click;
            this.btnColormapViewer.Click += this.ColormapViewerButton_Click;
            this.AxisLimitsButton.Click += this.AxisLimitsButton_Click;
            this.ScrollViewerButton.Click += this.ScrollViewerButton_Click;
            this.RightClickMenuButton.Click += this.RightClickMenuButton_Click;
            this.PlotViewerButton.Click += this.PlotViewerButton_Click;
            this.btnShowOnHover.Click += this.ShowOnHover_Click;
            this.GrowingData.Click += this.GrowingData_Click;
            this.LiveData.Click += this.LiveData_Click;
            this.LinkedAxesButton.Click += this.LinkedAxesButton_Click;
            this.ConfigButton.Click += this.ConfigButton_Click;
            this.ToggleVisibilityButton.Click += this.ToggleVisibilityButton_Click;
            this.MouseTrackerButton.Click += this.MouseTrackerButton_Click;
            this.TransparentBackgroundButton.Click += this.TransparentBackgroundButton_Click;
            this.StyleBrowserButton.Click += this.StyleBrowserButton_Click;
        }

        private void WebsiteLink_Click(object sender, EventArgs e) => Tools.LaunchBrowser("https://ScottPlot.NET/demo");
        private void CookbookButton_Click(object sender, EventArgs e) => new FormCookbook().ShowDialog();
        private void MouseTrackerButton_Click(object sender, EventArgs e) => new EtoFormsDemos.MouseTracker().ShowDialog();
        private void ToggleVisibilityButton_Click(object sender, EventArgs e) => new EtoFormsDemos.ToggleVisibility().ShowDialog();
        private void ConfigButton_Click(object sender, EventArgs e) => new EtoFormsDemos.FormsPlotConfig().ShowDialog();
        private void TransparentBackgroundButton_Click(object sender, EventArgs e) => new EtoFormsDemos.TransparentBackground().ShowDialog();
        private void LinkedAxesButton_Click(object sender, EventArgs e) => new EtoFormsDemos.LinkedPlots().ShowDialog();
        private void LiveData_Click(object sender, EventArgs e) => new EtoFormsDemos.LiveDataUpdate().ShowDialog();
        private void GrowingData_Click(object sender, EventArgs e) => new EtoFormsDemos.LiveDataIncoming().ShowDialog();
        private void ShowOnHover_Click(object sender, EventArgs e) => new EtoFormsDemos.ShowValueOnHover2().ShowDialog();
        private void PlotViewerButton_Click(object sender, EventArgs e) => new EtoFormsDemos.PlotViewerDemo().ShowDialog();
        private void RightClickMenuButton_Click(object sender, EventArgs e) => new EtoFormsDemos.RightClickMenu().ShowDialog();
        private void ScrollViewerButton_Click(object sender, EventArgs e) => new EtoFormsDemos.PlotsInScrollViewer().ShowDialog();
        private void AxisLimitsButton_Click(object sender, EventArgs e) => new EtoFormsDemos.AxisLimits().ShowDialog();
        private void ColormapViewerButton_Click(object sender, EventArgs e) => new EtoFormsDemos.ColormapViewer().ShowDialog();
        private void MultiAxisLockButton_Click(object sender, EventArgs e) => new EtoFormsDemos.MultiAxisLock().ShowDialog();
        private void StyleBrowserButton_Click(object sender, EventArgs e) => new EtoFormsDemos.Styles().ShowDialog();
    }
}
