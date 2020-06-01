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

            formsPlot1.plt.PlotScatter(dataXs, dataSin);
            formsPlot1.plt.PlotScatter(dataXs, dataCos);

            formsPlot1.Render();
        }

        private void cbPannable_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configure(enablePanning: cbPannable.Checked);
        }

        private void cbZoomable_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configure(enableZooming: cbZoomable.Checked, enableScrollWheelZoom: cbZoomable.Checked);
        }

        private void cbLowQualWhileDragging_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configure(lowQualityWhileDragging: cbLowQualWhileDragging.Checked);
        }

        private void cbLockVertical_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configure(lockVerticalAxis: cbLockVertical.Checked);
        }

        private void cbLockHorizontal_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configure(lockHorizontalAxis: cbLockHorizontal.Checked);
        }

        private void cbEqualAxes_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configure(equalAxes: cbEqualAxes.Checked);
            formsPlot1.Render();
        }

        private void cbRightClickMenu_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configure(enableRightClickMenu: cbRightClickMenu.Checked);
        }

        private void cbDoubleClickBenchmark_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDoubleClickBenchmark.Checked is false)
            {
                formsPlot1.plt.Benchmark(show: false);
                formsPlot1.Render();
            }

            formsPlot1.Configure(enableDoubleClickBenchmark: cbDoubleClickBenchmark.Checked);
        }

        private void cbCustomRightClick_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCustomRightClick.Checked)
            {
                cbRightClickMenu.Checked = false;
                formsPlot1.Configure(enableRightClickMenu: false);
            }
        }

        private void formsPlot1_MouseClicked(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && cbCustomRightClick.Checked)
            {
                MessageBox.Show("This is a custom right-click action");
            }
        }

        private void cbTooltip_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configure(showCoordinatesTooltip: cbTooltip.Checked);
        }
    }
}
