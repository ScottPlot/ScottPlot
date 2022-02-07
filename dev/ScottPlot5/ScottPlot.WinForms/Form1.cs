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
            skglControl1.MouseWheel += SkglControl1_MouseWheel;
            Plot.Info = Plot.Info.WithSize(skglControl1.Width, skglControl1.Height);

            double[] xs = ScottPlot.Generate.Consecutive(51);
            double[] ys1 = ScottPlot.Generate.Sin(51);
            double[] ys2 = ScottPlot.Generate.Cos(51);
            Plot.AddScatter(xs, ys1);
            Plot.AddScatter(xs, ys2);
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
    }
}