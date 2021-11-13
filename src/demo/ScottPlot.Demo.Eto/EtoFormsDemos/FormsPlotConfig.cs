using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    public partial class FormsPlotConfig : Form
    {
        public FormsPlotConfig()
        {
            InitializeComponent();
            this.cbPannable.CheckedChanged += this.cbPannable_CheckedChanged;
            this.cbZoomable.CheckedChanged += this.cbZoomable_CheckedChanged;
            this.cbLowQualWhileDragging.CheckedChanged += this.cbLowQualWhileDragging_CheckedChanged;
            this.cbRightClickMenu.CheckedChanged += this.cbRightClickMenu_CheckedChanged;
            this.cbDoubleClickBenchmark.CheckedChanged += this.cbDoubleClickBenchmark_CheckedChanged;
            this.cbLockVertical.CheckedChanged += this.cbLockVertical_CheckedChanged;
            this.cbLockHorizontal.CheckedChanged += this.cbLockHorizontal_CheckedChanged;
            this.cbEqualAxes.CheckedChanged += this.cbEqualAxes_CheckedChanged;
            this.cbCustomRightClick.CheckedChanged += this.cbCustomRightClick_CheckedChanged;
            this.Load += this.FormsPlotConfig_Load;
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
            formsPlot1.Configuration.LeftClickDragPan = cbPannable.Checked ?? false;
        }

        private void cbZoomable_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.RightClickDragZoom = cbZoomable.Checked ?? false;
            formsPlot1.Configuration.ScrollWheelZoom = cbZoomable.Checked ?? false;
        }

        private void cbLowQualWhileDragging_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.Quality =
                cbLowQualWhileDragging.Checked ?? false ?
                Control.QualityMode.LowWhileDragging :
                Control.QualityMode.High;
        }

        private void cbLockVertical_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.LockVerticalAxis = cbLockVertical.Checked ?? false;
        }

        private void cbLockHorizontal_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.LockHorizontalAxis = cbLockHorizontal.Checked ?? false;
        }

        private void cbEqualAxes_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Plot.AxisScaleLock(cbEqualAxes.Checked ?? false);
            formsPlot1.Refresh();
        }

        private void cbDoubleClickBenchmark_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configuration.DoubleClickBenchmark = cbDoubleClickBenchmark.Checked ?? false;
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

            if (cbCustomRightClick.Checked ?? false)
                formsPlot1.RightClicked += CustomRightClickEvent;
            else
                formsPlot1.RightClicked += formsPlot1.DefaultRightClickEvent;
        }

        private void CustomRightClickEvent(object sender, EventArgs e) =>
            MessageBox.Show("This is a custom right-click action");
    }
}
