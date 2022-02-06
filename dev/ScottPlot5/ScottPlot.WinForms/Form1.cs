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
            Plot.LastRenderInfo = Plot.LastRenderInfo.WithSize(skglControl1.Width, skglControl1.Height);

            double[] xs = ScottPlot.Generate.Consecutive(51);
            double[] ys1 = ScottPlot.Generate.Sin(51);
            double[] ys2 = ScottPlot.Generate.Cos(51);
            Plot.AddScatter(xs, ys1);
            Plot.AddScatter(xs, ys2);
        }

        private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            ICanvas canvas = new SkiaCanvas() { Canvas = e.Surface.Canvas };

            if (ViewNow is not null)
            {
                Plot.Draw(canvas, ViewNow);
            }
            else
            {
                Plot.Draw(canvas);
            }
        }

        private void skglControl1_SizeChanged(object sender, EventArgs e)
        {
            Plot.LastRenderInfo = Plot.LastRenderInfo.WithSize(skglControl1.Width, skglControl1.Height);
            skglControl1.Invalidate();
        }

        private void skglControl1_MouseMove(object sender, MouseEventArgs e)
        {
            Pixel MouseNowPixel = new(e.X, e.Y);

            StringBuilder sb = new();
            sb.AppendLine($"Mouse pixel: {MouseNowPixel}");
            sb.AppendLine($"Mouse coordinate: {Plot.LastRenderInfo.GetCoordinate(MouseNowPixel)}");

            if (MouseDownPixel.HasValue)
            {
                Pixel mouseDownPixel = MouseDownPixel.Value;
                Pixel DeltaPixel = MouseNowPixel - MouseDownPixel.Value;
                sb.AppendLine($"Mouse dragged from {MouseDownPixel} to {MouseNowPixel} (delta {DeltaPixel})");
                ViewNow = MouseDownView?.WithPan(mouseDownPixel, MouseNowPixel);
                skglControl1.Invalidate();
            }
            else
            {
                sb.AppendLine("Mouse buton is not down");
            }

            richTextBox1.Text = sb.ToString();
        }

        private void skglControl1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownPixel = new Pixel(e.X, e.Y);
            MouseDownView = Plot.LastRenderInfo;
        }

        private void skglControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (ViewNow is not null)
                Plot.LastRenderInfo = ViewNow;

            MouseDownPixel = null;
            ViewNow = null;
        }
    }
}