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

namespace FormsPlotSandbox
{
    public partial class FormColormapGenerator : Form
    {
        public FormColormapGenerator()
        {
            InitializeComponent();
            btnRandomize_Click(null, null);
        }

        private void tb1_Scroll(object sender, EventArgs e) => Replot();
        private void tb2_Scroll(object sender, EventArgs e) => Replot();
        private void tb3_Scroll(object sender, EventArgs e) => Replot();
        private void tb4_Scroll(object sender, EventArgs e) => Replot();
        private void tb5_Scroll(object sender, EventArgs e) => Replot();
        private void tb6_Scroll(object sender, EventArgs e) => Replot();

        private void tbGreen1_Scroll(object sender, EventArgs e) => Replot();
        private void tbGreen2_Scroll(object sender, EventArgs e) => Replot();
        private void tbGreen3_Scroll(object sender, EventArgs e) => Replot();
        private void tbGreen4_Scroll(object sender, EventArgs e) => Replot();
        private void tbGreen5_Scroll(object sender, EventArgs e) => Replot();
        private void tbGreen6_Scroll(object sender, EventArgs e) => Replot();

        private void tbBlue1_Scroll(object sender, EventArgs e) => Replot();
        private void tbBlue2_Scroll(object sender, EventArgs e) => Replot();
        private void tbBlue3_Scroll(object sender, EventArgs e) => Replot();
        private void tbBlue4_Scroll(object sender, EventArgs e) => Replot();
        private void tbBlue5_Scroll(object sender, EventArgs e) => Replot();
        private void tbBlue6_Scroll(object sender, EventArgs e) => Replot();

        private void InterpolateCos(double[] fullCurve, int x1, int x2, double y1, double y2)
        {
            int distance = x2 - x1;
            for (int i = 0; i < distance; i++)
            {
                double muLinear = (double)i / distance;
                double muCos = (1 - Math.Cos(muLinear * Math.PI)) / 2;
                fullCurve[x1 + i] = (y1 * (1 - muCos) + y2 * muCos);
            }
        }

        private void Replot()
        {
            formsPlot1.plt.Clear();

            int[] xs = { 0, 51, 102, 153, 204, 255 };
            int[] redYs = { tbRed1.Value, tbRed2.Value, tbRed3.Value, tbRed4.Value, tbRed5.Value, tbRed6.Value };
            int[] greenYs = { tbGreen1.Value, tbGreen2.Value, tbGreen3.Value, tbGreen4.Value, tbGreen5.Value, tbGreen6.Value };
            int[] blueYs = { tbBlue1.Value, tbBlue2.Value, tbBlue3.Value, tbBlue4.Value, tbBlue5.Value, tbBlue6.Value };

            double[] xsCurve = ScottPlot.DataGen.Consecutive(255);
            double[] redCurve = new double[255];
            double[] greenCurve = new double[255];
            double[] blueCurve = new double[255];

            for (int splineIndex = 0; splineIndex < xs.Length - 1; splineIndex++)
            {
                int x1 = xs[splineIndex];
                int x2 = xs[splineIndex + 1];
                InterpolateCos(redCurve, x1, x2, redYs[splineIndex], redYs[splineIndex + 1]);
                InterpolateCos(greenCurve, x1, x2, greenYs[splineIndex], greenYs[splineIndex + 1]);
                InterpolateCos(blueCurve, x1, x2, blueYs[splineIndex], blueYs[splineIndex + 1]);
            }

            double[] meanCurve = new double[255];
            for (int i = 0; i < meanCurve.Length; i++)
                meanCurve[i] = (redCurve[i] + greenCurve[i] + blueCurve[i]) / 3;

            formsPlot1.plt.PlotScatter(
                ScottPlot.Tools.DoubleArray(xs),
                ScottPlot.Tools.DoubleArray(redYs),
                Color.Red, 0);
            formsPlot1.plt.PlotScatter(
                ScottPlot.Tools.DoubleArray(xs),
                ScottPlot.Tools.DoubleArray(greenYs),
                Color.Green, 0);
            formsPlot1.plt.PlotScatter(
                ScottPlot.Tools.DoubleArray(xs),
                ScottPlot.Tools.DoubleArray(blueYs),
                Color.Blue, 0);

            formsPlot1.plt.PlotScatter(xsCurve, redCurve, color: Color.Red, markerSize: 0);
            formsPlot1.plt.PlotScatter(xsCurve, greenCurve, color: Color.Green, markerSize: 0);
            formsPlot1.plt.PlotScatter(xsCurve, blueCurve, color: Color.Blue, markerSize: 0);
            formsPlot1.plt.PlotScatter(xsCurve, meanCurve, color: Color.Black, markerSize: 0, lineStyle: ScottPlot.LineStyle.Dash);

            formsPlot1.plt.Axis(0, 255, 0, 255);
            formsPlot1.Render();

            Size cmapSize = pbColorbar.Size;
            Bitmap bmp = new Bitmap(cmapSize.Width, cmapSize.Height);
            using (var gfx = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.White))
            {
                for (int x = 0; x < cmapSize.Width; x++)
                {
                    double intensityFrac = (double)x / cmapSize.Width;
                    int intensityValue = (int)(255.0 * intensityFrac);
                    int r = (int)redCurve[intensityValue];
                    int g = (int)greenCurve[intensityValue];
                    int b = (int)blueCurve[intensityValue];
                    pen.Color = Color.FromArgb(255, r, g, b);
                    gfx.DrawLine(pen, x, 0, x, cmapSize.Height);
                }
            }

            pbColorbar.Image?.Dispose();
            pbColorbar.Image = bmp;
        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            List<TrackBar> trackbars = Controls.OfType<TrackBar>().Cast<TrackBar>().ToList();

            Random rand = new Random();
            foreach (var tb in trackbars)
                tb.Value = rand.Next(255);
            Replot();
        }
    }
}
