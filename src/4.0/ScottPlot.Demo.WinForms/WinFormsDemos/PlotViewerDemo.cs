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
    public partial class PlotViewerDemo : Form
    {
        Random rand = new Random();

        public PlotViewerDemo()
        {
            InitializeComponent();
        }

        private void btnLaunchRandomWalk_Click(object sender, EventArgs e)
        {
            int pointCount = (int)nudWalkPoints.Value;
            double[] randomWalkData = DataGen.RandomWalk(rand, pointCount);

            var plt = new ScottPlot.Plot();
            plt.PlotSignal(randomWalkData);
            plt.Title($"{pointCount} Random Walk Points");

            var plotViewer = new ScottPlot.FormsPlotViewer(plt, 500, 300, "Random Walk Data");
            plotViewer.Owner = this; // so it closes if this window closes
            plotViewer.formsPlot1.Configure(lowQualityWhileDragging: false); // customize as desired
            plotViewer.Show(); // or ShowDialog() for a blocking window
        }

        private void btnLaunchRandomSine_Click(object sender, EventArgs e)
        {
            int sinCount = (int)nudSineCount.Value;
            var plt = new ScottPlot.Plot();
            for (int i = 0; i < sinCount; i++)
            {
                double[] randomSinValues = DataGen.Sin(50, rand.NextDouble() * 10, rand.NextDouble(), rand.NextDouble(), rand.NextDouble() * 100);
                plt.PlotSignal(randomSinValues);
            }
            plt.Title($"{sinCount} Random Sine Waves");

            var plotViewer = new ScottPlot.FormsPlotViewer(plt, 500, 300, "Random Walk Data");
            plotViewer.Owner = this; // so it closes if this window closes
            plotViewer.formsPlot1.Configure(lowQualityWhileDragging: false); // customize as desired
            plotViewer.Show(); // or ShowDialog() for a blocking window
        }
    }
}
