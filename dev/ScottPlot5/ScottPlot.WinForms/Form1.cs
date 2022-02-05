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
        PlotView? MouseDownView = null;
        PlotView? ViewNow = null;

        public Form1()
        {
            InitializeComponent();
            Plot.AddDemoSinAndCos();
        }

        private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            ICanvas canvas = new SkiaCanvas() { Canvas = e.Surface.Canvas };

            if (ViewNow is not null)
            {
                Plot.Draw(canvas, ViewNow.Value);
            }
            else
            {
                PlotView view = Plot.GetLastView().WithSize(skglControl1.Width, skglControl1.Height);
                Plot.Draw(canvas, view);
            }
        }

        private void skglControl1_MouseMove(object sender, MouseEventArgs e)
        {
            Pixel MouseNowPixel = new(e.X, e.Y);

            StringBuilder sb = new();
            sb.AppendLine($"Mouse pixel: {MouseNowPixel}");
            sb.AppendLine($"Mouse coordinate: {Plot.GetLastView().GetCoordinate(MouseNowPixel)}");

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
            MouseDownView = Plot.GetLastView();
        }

        private void skglControl1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDownPixel = null;
            ViewNow = null;
        }
    }
}