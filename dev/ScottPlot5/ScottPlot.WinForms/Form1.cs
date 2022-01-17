using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using System.Text;

namespace ScottPlot.WinForms
{
    public partial class Form1 : Form
    {
        readonly Plot Plot = new();

        public Form1()
        {
            InitializeComponent();
            Plot.AddDemoSinAndCos();
        }

        private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            ICanvas canvas = new SkiaCanvas() { Canvas = e.Surface.Canvas };
            Plot.Draw(canvas, skglControl1.Width, skglControl1.Height);
        }

        private void UpdateMessage()
        {
        }

        private void skglControl1_MouseMove(object sender, MouseEventArgs e)
        {
            StringBuilder sb = new();
            sb.AppendLine($"Mouse pixel: {e.Location}");
            sb.AppendLine($"Mouse coordinate: {Plot.LastView.GetCoordinate(e.Location.X, e.Location.Y)}");

            richTextBox1.Text = sb.ToString();
        }
    }
}