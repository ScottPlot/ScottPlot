using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class FormsPlotConfig : Form
    {
        public FormsPlotConfig()
        {
            InitializeComponent();
        }

        private void FormsPlotConfig_Load(object sender, EventArgs e)
        {
            int pointCount = 51;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            formsPlot1.Plot.AddScatter(dataXs, dataSin);
            formsPlot1.Plot.AddScatter(dataXs, dataCos);

            formsPlot1.Refresh();
        }

        private void cbPannable_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.LeftClickDragPan = cbPannable.Checked;
        }

        private void cbZoomable_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.RightClickDragZoom = cbZoomable.Checked;
            formsPlot1.Configuration.ScrollWheelZoom = cbZoomable.Checked;
        }

        private void cbLowQualWhileDragging_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.Quality =
                cbLowQualWhileDragging.Checked ?
                Control.QualityMode.LowWhileDragging :
                Control.QualityMode.High;
        }

        private void cbLockVertical_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.LockVerticalAxis = cbLockVertical.Checked;
        }

        private void cbLockHorizontal_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.LockHorizontalAxis = cbLockHorizontal.Checked;
        }

        private void cbEqualAxes_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Plot.AxisScaleLock(cbEqualAxes.Checked);
            formsPlot1.Refresh();
        }

        private void cbDoubleClickBenchmark_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.DoubleClickBenchmark = cbDoubleClickBenchmark.Checked;
        }

        private void cbRightClickMenu_CheckedChanged(object sender, EventArgs e) => InitializeRightClickMenu();

        private void cbCustomRightClick_CheckedChanged(object sender, EventArgs e) => InitializeRightClickMenu();

        private void InitializeRightClickMenu()
        {
            // remove both possible right-click actions
            formsPlot1.RightClicked -= formsPlot1.DefaultRightClickEvent;
            formsPlot1.RightClicked -= CustomRightClickEvent;

            if (cbRightClickMenu.Enabled == false)
                return;

            if (cbCustomRightClick.Checked)
                formsPlot1.RightClicked += CustomRightClickEvent;
            else
                formsPlot1.RightClicked += formsPlot1.DefaultRightClickEvent;
        }

        private void CustomRightClickEvent(object sender, EventArgs e) =>
            MessageBox.Show("This is a custom right-click action");
    }
}
