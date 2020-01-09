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
            realLineLabel.Text = $"Y = {slope}x + {offset} (n={pointCount})";
            double[] ys = ScottPlot.DataGen.NoisyLinear(
                rand: rand,
                pointCount: pointCount,
                slope: slope,
                offset: offset,
                noise: rand.Next(10, 500)
            );

            // create matching X-axis ticks
            double[] xs = ScottPlot.DataGen.Consecutive(ys.Length, spacing: 0.01);

            // plot data
            formsPlot1.plt.Clear();
            formsPlot1.plt.PlotScatter(xs, ys, lineWidth: 0);
            formsPlot1.plt.AxisAuto();

            // perform the linear regression
            var linreg = new ScottPlot.Statistics.LinearRegressionLine(xs, ys);
            double[] coeffecients = linreg.GetCoefficients();
            double fittedOffset = coeffecients[0];
            double pointSpacing = xs[1] - xs[0];
            double fittedSlope = coeffecients[1] * pointSpacing;
            fittedLineLabel.Text = string.Format("Y = {0:0.00}x + {1:0.00}", fittedSlope, fittedOffset);

            // plot the linear regression line (as a scatter plot with just two points)
            double[] fittedXs = new double[] { xs.First(), xs.Last() };

            double[] fittedYs = new double[] {
                fittedSlope * xs.First() / pointSpacing + fittedOffset,
                fittedSlope * xs.Last() / pointSpacing + fittedOffset
            };

            // plot the linear regression
            formsPlot1.plt.PlotScatter(fittedXs, fittedYs, markerSize: 0, lineWidth: 3, color: Color.Black);

            // force a redraw of the user control
            formsPlot1.Render();

            // create a standalone plot the way Benny did
            var plt2 = new ScottPlot.Plot(600, 400);
            plt2 = linreg.Draw(plt2);
            plt2 = linreg.DrawResidual(plt2);
            string filePath = System.IO.Path.GetFullPath("benny.png");
            plt2.SaveFig(filePath);
            Debug.WriteLine($"Saved: {filePath}");
        }
    }
}
