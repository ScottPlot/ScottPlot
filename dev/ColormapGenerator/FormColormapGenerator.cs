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

namespace ColormapGenerator
{
    public partial class FormColormapGenerator : Form
    {
        public FormColormapGenerator()
        {
            InitializeComponent();
            btnRandomize_Click(null, null);
            richTextBox1.ForeColor = ColorTranslator.FromHtml("#d1cfce");
            richTextBox1.BackColor = ColorTranslator.FromHtml("#1e1e1e");
        }

        private void FormColormapGenerator_Load(object sender, EventArgs e) => Replot();

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

        int[] colormap = new int[256];
        private void Replot()
        {
            formsPlot1.plt.Clear();

            double[] xs = { 0, 51, 102, 153, 204, 255 };
            double[] redYs = { tbRed1.Value, tbRed2.Value, tbRed3.Value, tbRed4.Value, tbRed5.Value, tbRed6.Value };
            double[] greenYs = { tbGreen1.Value, tbGreen2.Value, tbGreen3.Value, tbGreen4.Value, tbGreen5.Value, tbGreen6.Value };
            double[] blueYs = { tbBlue1.Value, tbBlue2.Value, tbBlue3.Value, tbBlue4.Value, tbBlue5.Value, tbBlue6.Value };

            int resolution = 51;
            var redInterp = new ScottPlot.Statistics.Interpolation.NaturalSpline(xs, redYs, resolution);
            var greenInterp = new ScottPlot.Statistics.Interpolation.NaturalSpline(xs, greenYs, resolution);
            var blueInterp = new ScottPlot.Statistics.Interpolation.NaturalSpline(xs, blueYs, resolution);

            double[] xsCurve = redInterp.interpolatedXs;
            double[] redCurve = redInterp.interpolatedYs;
            double[] greenCurve = greenInterp.interpolatedYs;
            double[] blueCurve = blueInterp.interpolatedYs;

            double[] meanCurve = new double[redCurve.Length];
            for (int i = 0; i < 256; i++)
            {
                byte redByte = (byte)Math.Max(Math.Min(redCurve[i], 255), 0);
                byte greenByte = (byte)Math.Max(Math.Min(greenCurve[i], 255), 0);
                byte blueByte = (byte)Math.Max(Math.Min(blueCurve[i], 255), 0);
                byte alphaByte = 255;
                byte[] bytes = { blueByte, greenByte, redByte, alphaByte };
                colormap[i] = BitConverter.ToInt32(bytes, 0);
                meanCurve[i] = (redByte + greenByte + blueByte) / 3.0;
            }

            formsPlot1.plt.PlotScatter(xs, redYs, Color.Red, 0);
            formsPlot1.plt.PlotScatter(xs, greenYs, Color.Green, 0);
            formsPlot1.plt.PlotScatter(xs, blueYs, Color.Blue, 0);

            formsPlot1.plt.PlotScatter(xsCurve, redCurve, color: Color.Red, markerSize: 0);
            formsPlot1.plt.PlotScatter(xsCurve, greenCurve, color: Color.Green, markerSize: 0);
            formsPlot1.plt.PlotScatter(xsCurve, blueCurve, color: Color.Blue, markerSize: 0);
            formsPlot1.plt.PlotScatter(xsCurve, meanCurve, color: Color.Black, markerSize: 0, lineStyle: ScottPlot.LineStyle.Dash);

            //formsPlot1.plt.Frame(false);
            //formsPlot1.plt.Ticks(false, false);
            //formsPlot1.plt.Axis(0, 255, 0, 255);
            formsPlot1.plt.PlotVLine(0, Color.Black, lineStyle: ScottPlot.LineStyle.Dot);
            formsPlot1.plt.PlotVLine(255, Color.Black, lineStyle: ScottPlot.LineStyle.Dot);
            formsPlot1.plt.PlotHLine(0, Color.Black, lineStyle: ScottPlot.LineStyle.Dot);
            formsPlot1.plt.PlotHLine(255, Color.Black, lineStyle: ScottPlot.LineStyle.Dot);
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
                    pen.Color = Color.FromArgb(colormap[intensityValue]);
                    gfx.DrawLine(pen, x, 0, x, cmapSize.Height);
                }
            }

            pbColorbar.Image?.Dispose();
            pbColorbar.Image = bmp;
            newValues = true;
        }

        private void btnRandomize_Click(object sender, EventArgs e)
        {
            List<TrackBar> trackbars = Controls.OfType<TrackBar>().Cast<TrackBar>().ToList();
            Random rand = new Random();
            foreach (var tb in trackbars)
                tb.Value = rand.Next(255);
            Replot();
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            List<TrackBar> trackbars = Controls.OfType<TrackBar>().Cast<TrackBar>().ToList();
            foreach (var tb in trackbars)
                tb.Value = 0;
            Replot();
        }

        private bool newValues = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (newValues == false)
                return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("private readonly int[] argb =");
            sb.AppendLine("{");
            sb.Append("    ");
            for (int i = 0; i < colormap.Length; i++)
            {
                sb.Append($"{colormap[i]:D8}, ");
                if (i % 8 == 7)
                    sb.Append(Environment.NewLine + "    ");
            }
            richTextBox1.Text = sb.ToString().Trim() + "\n};";
            newValues = false;
        }

    }
}
