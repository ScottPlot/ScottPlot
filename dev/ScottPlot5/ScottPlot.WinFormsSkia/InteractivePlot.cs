using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.WinFormsSkia
{
    public partial class InteractivePlot : UserControl
    {
        public Plot Plot;
        public readonly MouseTracker MouseTracker;

        public InteractivePlot()
        {
            InitializeComponent();
            skglControl1.MouseWheel += skglControl1_MouseWheel;

            Plot = new Plot();
            Plot.FigureBackground.Color = Renderer.Color.Convert(SystemColors.Control);
            Plot.DataBackground.Color = Renderer.Colors.White;
            MouseTracker = new MouseTracker(Plot);
            Render();
        }

        public void Render(bool recalculateLayout = true, bool lowQuality = false)
        {
            skglControl1.Invalidate();
        }

        private void skglControl1_SizeChanged(object sender, EventArgs e)
        {
            Render();
        }

        private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            using (var renderer = new SkiaSharpRenderer(e.Surface.Canvas, skglControl1.Width, skglControl1.Height))
                Plot.Render(renderer, true, false);
        }

        private void skglControl1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseTracker.MouseMove(e.X, e.Y);
            if (e.Button != MouseButtons.None)
                Render(recalculateLayout: false, lowQuality: true);
        }

        private void skglControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) MouseTracker.MouseDownLeft(e.X, e.Y);
            else if (e.Button == MouseButtons.Middle) MouseTracker.MouseDownCenter(e.X, e.Y);
            else if (e.Button == MouseButtons.Right) MouseTracker.MouseDownRight(e.X, e.Y);
        }

        private void skglControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) MouseTracker.MouseUpLeft(e.X, e.Y);
            else if (e.Button == MouseButtons.Middle) MouseTracker.MouseUpCenter(e.X, e.Y);
            else if (e.Button == MouseButtons.Right) MouseTracker.MouseUpRight(e.X, e.Y);
            Render();
        }

        private void skglControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            MouseTracker.MouseWheel(e.Delta > 0, e.X, e.Y);
            Render();
        }

        private void skglControl1_DoubleClick(object sender, EventArgs e)
        {
            MouseTracker.DoubleClick();
            Render();
        }
    }
}
