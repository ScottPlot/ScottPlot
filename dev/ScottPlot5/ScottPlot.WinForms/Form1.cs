using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using System.Text;
using System.Diagnostics;

namespace ScottPlot.WinForms
{
    public partial class Form1 : Form
    {
        readonly Plot Plot = new();
        Pixel? MouseDownPixel = null;
        PlotInfo? MouseDownView = null;
        PlotInfo? InfoNow = null;

        public Form1()
        {
            InitializeComponent();
            Plot.Info.Style.FigureBackgroundColor = Microsoft.Maui.Graphics.Color.FromInt(SystemColors.Control.ToArgb());
            skglControl1.MouseWheel += SkglControl1_MouseWheel;
            Plot.Info = Plot.Info.WithSize(skglControl1.Width, skglControl1.Height);
        }

        private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            ICanvas canvas = new SkiaCanvas() { Canvas = e.Surface.Canvas };

            if (InfoNow is null)
                Plot.Draw(canvas);
            else
                Plot.Draw(canvas, InfoNow);
        }

        private void skglControl1_SizeChanged(object sender, EventArgs e)
        {
            Plot.Info = Plot.Info.WithSize(skglControl1.Width, skglControl1.Height);
            skglControl1.Invalidate();
        }

        private void skglControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseDownPixel is null)
                return;

            Pixel MouseNowPixel = new(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
                InfoNow = MouseDownView?.WithPan(MouseDownPixel.Value, MouseNowPixel);
            else if (e.Button == MouseButtons.Right)
                InfoNow = MouseDownView?.WithZoom(MouseDownPixel.Value, MouseNowPixel);

            skglControl1.Invalidate();
        }

        private void skglControl1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownPixel = new Pixel(e.X, e.Y);
            MouseDownView = Plot.Info;
        }

        private void skglControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (InfoNow is not null)
                Plot.Info = InfoNow;

            if (e.Button == MouseButtons.Middle)
            {
                Plot.Autoscale();
                skglControl1.Invalidate();
            }

            MouseDownPixel = null;
            InfoNow = null;
        }

        private void SkglControl1_MouseWheel(object? sender, MouseEventArgs e)
        {
            double fraction = e.Delta > 0 ? 1.15 : 0.85;
            Pixel MouseNowPixel = new(e.X, e.Y);
            Plot.Info = Plot.Info.WithZoom(MouseNowPixel, fraction);

            skglControl1.Refresh();
        }

        private void skglControl1_DoubleClick(object sender, EventArgs e)
        {
            Plot.BenchmarkToggle();
            skglControl1.Invalidate();
        }

        private void SinePlots(int pointCount)
        {
            Plot.Clear();
            double[] xs = ScottPlot.Generate.Consecutive(pointCount);
            double[] ys1 = ScottPlot.Generate.Sin(pointCount);
            double[] ys2 = ScottPlot.Generate.Cos(pointCount);
            Plot.AddScatter(xs, ys1);
            Plot.AddScatter(xs, ys2);
            Plot.Autoscale();
            skglControl1.Invalidate();
        }

        private void btnScatterBasic_Click(object sender, EventArgs e) => SinePlots(51);

        private void btnScatter100k_Click(object sender, EventArgs e) => SinePlots(10_000);
    }
}