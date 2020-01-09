using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormRegression : Form
    {
        public FormRegression()
        {
            InitializeComponent();
        }

        private void randomizeButton_Click(object sender, EventArgs e)
        {
            CreateScatterAndFitRegression();
        }

        private void FormRegression_Load(object sender, EventArgs e)
        {
            CreateScatterAndFitRegression();
        }

        private void CreateScatterAndFitRegression()
        {
            Random rand = new Random();

            // generate linear data with noise
            double slope = rand.Next(-1000, 1000) / 100.0;
            double offset = rand.Next(-1000, 1000);
            int pointCount = rand.Next(50, 1000);
            double[] ys = ScottPlot.DataGen.NoisyLinear(
                rand: rand,
                pointCount: pointCount,
                slope: slope,
                offset: offset,
                noise: rand.Next(10, 500)
            );

            // create matching X-axis ticks
            double pointSpacing = rand.Next(1, 1000) / 1000.0;
            double[] xs = ScottPlot.DataGen.Consecutive(ys.Length, spacing: pointSpacing);
            realLineLabel.Text = string.Format("Y = {0:0.00}x + {1:0.00} (n={2})", slope / pointSpacing, offset, pointCount);

            // plot data
            formsPlot1.plt.Clear();
            formsPlot1.plt.PlotScatter(xs, ys, lineWidth: 0);
            formsPlot1.plt.AxisAuto();

            // perform the linear regression
            var linreg = new ScottPlot.Statistics.LinearRegressionLine(xs, ys);
            double fittedSlope = linreg.slope;
            fittedLineLabel.Text = string.Format("Y = {0:0.00}x + {1:0.00}", fittedSlope, linreg.offset);

            // plot the linear regression line (as a scatter plot with just two points)
            double x1 = xs.First();
            double x2 = xs.Last();
            double y1 = linreg.GetValueAt(xs.First());
            double y2 = linreg.GetValueAt(xs.Last());
            formsPlot1.plt.PlotLine(x1, y1, x2, y2, lineWidth: 3, color: Color.Black);

            // force a redraw of the user control
            formsPlot1.Render();
        }
    }
}
