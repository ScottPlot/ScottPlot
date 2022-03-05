using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using ScottPlot.Eto;
using ScottPlot.Drawing;
#pragma warning disable CS0618 // Type or member is obsolete

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    public partial class ColormapViewer : Form
    {
        Drawing.Colormap[] colormaps = Colormap.GetColormaps();
        public ColormapViewer()
        {
            InitializeComponent();

            this.lbColormapNames.SelectedIndexChanged += this.lbColormapNames_SelectedIndexChanged;
            this.rbImage.CheckedChanged += this.rbImage_CheckedChanged;
            this.rbData.CheckedChanged += this.rbData_CheckedChanged;
            this.SizeChanged += this.ColormapViewer_SizeChanged;
            this.Load += this.ColormapViewer_Load;

            foreach (Drawing.Colormap cmap in colormaps)
                lbColormapNames.Items.Add(cmap.Name);
            //lbColormapNames.SelectedIndex = lbColormapNames.Items.IndexOf("Turbo");
            var turbo = lbColormapNames.Items.Where(x => x.Text == "Turbo").FirstOrDefault();
            lbColormapNames.SelectedIndex = lbColormapNames.Items.IndexOf(turbo);
        }

        private void ColormapViewer_Load(object sender, EventArgs e) => Redraw();
        private void lbColormapNames_SelectedIndexChanged(object sender, EventArgs e) => Redraw();
        private void ColormapViewer_SizeChanged(object sender, EventArgs e) => Redraw();
        private void rbData_CheckedChanged(object sender, EventArgs e) => Redraw();
        private void rbImage_CheckedChanged(object sender, EventArgs e) => Redraw();

        private void Redraw()
        {
            Drawing.Colormap cmap = colormaps[lbColormapNames.SelectedIndex >= 0 ? lbColormapNames.SelectedIndex : 0];
            lblColormap.Text = cmap.Name;

            pbColormap.Image?.Dispose();
            pbColormap.Image = Colormap.Colorbar(cmap, pbColormap.Width, pbColormap.Height).ToEto();

            PlotColormapCurves(formsPlot1.Plot, cmap);
            formsPlot1.Refresh();

            if (rbImage.Checked)
                PlotHeatmapImage(formsPlot2.Plot, cmap);
            else
                PlotHeatmapGaussianNoise(formsPlot2.Plot, cmap);
            formsPlot2.Refresh();

            PlotLineSeries(formsPlot3.Plot, cmap);
            formsPlot3.Refresh();
        }

        public static void PlotColormapCurves(Plot plt, Colormap cmap)
        {
            byte[] pixelValueRange = Enumerable.Range(0, 256).Select(x => (byte)x).ToArray();
            double[] xs = pixelValueRange.Select(x => (double)x / 255).ToArray();
            double[] rs = pixelValueRange.Select(x => (double)cmap.GetRGB(x).r).ToArray();
            double[] gs = pixelValueRange.Select(x => (double)cmap.GetRGB(x).g).ToArray();
            double[] bs = pixelValueRange.Select(x => (double)cmap.GetRGB(x).b).ToArray();
            double[] ms = new double[pixelValueRange.Length];
            for (int i = 0; i < ms.Length; i++)
                ms[i] = (rs[i] + gs[i] + bs[i]) / 3.0;

            plt.Clear();
            plt.PlotScatter(xs, rs, Color.Red, markerSize: 0);
            plt.PlotScatter(xs, gs, Color.Green, markerSize: 0);
            plt.PlotScatter(xs, bs, Color.Blue, markerSize: 0);
            plt.PlotScatter(xs, ms, Color.Black, markerSize: 0, lineStyle: LineStyle.Dash);
            plt.AxisAuto();
            plt.YLabel("Pixel Intensity");
            plt.XLabel("Fractional Data Value");
        }

        public static void PlotHeatmapGaussianNoise(Plot plt, Colormap cmap)
        {
            Random rand = new Random(0);

            int[] xs = DataGen.RandomNormal(rand, 10000, 25, 10).Select(x => (int)x).ToArray();
            int[] ys = DataGen.RandomNormal(rand, 10000, 25, 10).Select(y => (int)y).ToArray();

            double[,] intensities = Tools.XYToIntensities(IntensityMode.Gaussian, xs, ys, 50, 50, 4);

            plt.Clear();
            plt.AddHeatmap(intensities, cmap);
            plt.AxisAuto();
        }

        public static void PlotHeatmapImage(Plot plt, Colormap cmap)
        {
            double[,] intensities = DataGen.SampleImageData();
            plt.Clear();
            plt.AddHeatmap(intensities, cmap);
        }

        public static void PlotLineSeries(Plot plt, Colormap cmap)
        {
            int lineCount = 7;

            plt.Clear();
            for (int i = 0; i < lineCount; i++)
            {
                double fraction = (double)i / lineCount;
                double[] ys = DataGen.Sin(100, 2, mult: 1 + fraction * 2);
                Color c = cmap.GetColor(fraction);
                plt.PlotSignal(ys, color: c);
            }
            plt.AxisAuto();
        }
    }
}
