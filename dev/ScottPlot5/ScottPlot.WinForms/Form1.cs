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
        PlotInfo? ViewNow = null;

        public Form1()
        {
            InitializeComponent();
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

            if (ViewNow is null)
                Plot.Draw(canvas);
            else
                Plot.Draw(canvas, ViewNow);
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
            ViewNow = MouseDownView?.WithPan(MouseDownPixel.Value, MouseNowPixel);
            skglControl1.Invalidate();
        }

        private void skglControl1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownPixel = new Pixel(e.X, e.Y);
            MouseDownView = Plot.Info;
        }

        private void skglControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (ViewNow is not null)
                Plot.Info = ViewNow;

            MouseDownPixel = null;
            ViewNow = null;
        }
    }
}