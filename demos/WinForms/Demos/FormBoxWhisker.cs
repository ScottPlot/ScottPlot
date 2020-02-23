using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormBoxWhisker : Form
    {

        public FormBoxWhisker()
        {
            InitializeComponent();
        }

        private void FormBoxWhisker_Load(object sender, EventArgs e)
        {
            Plot_ScottVersion();
            Plot_BennyVersion();
        }

        private void Plot_BennyVersion()
        {
            double[] xPositions = { 1, 2, 3 };
            double[][] dataArrays = new double[][] { ScottPlot.Demo.Data.LineLengths.plot, ScottPlot.Demo.Data.LineLengths.formsPlot, ScottPlot.Demo.Data.LineLengths.wpfPlot };
            formsPlot2.plt.PlotBoxAndWhiskerV2(xPositions, dataArrays);

            formsPlot2.plt.Title("Source Code Line Length (Benny's Version)");
            formsPlot2.plt.YLabel("Number of Characters");

            // set X axis tick labels manually
            string[] labels = { "Plot.cs", "FormsPlot.cs", "WpfPlot.cs" };
            formsPlot2.plt.XTicks(xPositions, labels);

            formsPlot2.plt.AxisAuto(.3, .2);
            formsPlot2.Render();
        }

        ScottPlot.Statistics.BoxAndWhisker GetBoxAndWhisker(double[] data, double xPosition)
        {
            var baw = new ScottPlot.Statistics.BoxAndWhisker(xPosition);

            var stats = new ScottPlot.Statistics.PopulationStats(data);
            baw.midline.position = stats.median;
            baw.whisker.max = stats.max;
            baw.whisker.min = stats.min;
            baw.box.max = stats.mean + stats.stDev;
            baw.box.min = stats.mean - stats.stDev;

            return baw;
        }

        private void Plot_ScottVersion()
        {
            var boxAndWiskers = new ScottPlot.Statistics.BoxAndWhisker[3];
            boxAndWiskers[0] = GetBoxAndWhisker(ScottPlot.Demo.Data.LineLengths.plot, 1);
            boxAndWiskers[1] = GetBoxAndWhisker(ScottPlot.Demo.Data.LineLengths.formsPlot, 2);
            boxAndWiskers[2] = GetBoxAndWhisker(ScottPlot.Demo.Data.LineLengths.wpfPlot, 3);

            formsPlot1.plt.Title("Source Code Line Length (Scott's Version)");
            formsPlot1.plt.PlotBoxAndWhisker(boxAndWiskers);
            formsPlot1.plt.YLabel("Number of Characters");

            // set X axis tick labels manually
            double[] xPositions = { 1, 2, 3 };
            string[] labels = { "Plot.cs", "FormsPlot.cs", "WpfPlot.cs" };
            formsPlot1.plt.XTicks(xPositions, labels);

            formsPlot1.plt.AxisAuto(.3, .2);
            formsPlot1.Render();
        }
    }

}
