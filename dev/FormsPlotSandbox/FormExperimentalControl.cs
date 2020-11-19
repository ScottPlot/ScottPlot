using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsPlotSandbox
{
    public partial class FormExperimentalControl : Form
    {
        public FormExperimentalControl()
        {
            InitializeComponent();
        }

        private void FormExperimentalControl_Load(object sender, EventArgs e)
        {
            formsPlotExperimental1.MainPlot.Benchmark(true);
            FastButton_Click(null, null);
        }

        private void FastButton_Click(object sender, EventArgs e)
        {
            double[] ys = ScottPlot.DataGen.RandomWalk(null, 10_000);
            formsPlotExperimental1.MainPlot.PlotSignal(ys);
            formsPlotExperimental1.Render();
        }

        private void SlowButton_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            double[] ys = ScottPlot.DataGen.Random(rand, 1_000, 100);
            double[] xs = ScottPlot.DataGen.Random(rand, 1_000, 10_000);
            formsPlotExperimental1.MainPlot.PlotScatter(xs, ys, markerSize: 0);
            formsPlotExperimental1.Render();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            formsPlotExperimental1.MainPlot.Clear();
            formsPlotExperimental1.Render();
        }
    }
}
