using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.WinForms
{
    public partial class FormsPlot : UserControl
    {
        public readonly Plot Plot = new();
        Pixel? MouseDownPixel = null;
        PlotInfo? MouseDownView = null;
        PlotInfo? InfoNow = null;

        public FormsPlot()
        {
            InitializeComponent();
            Plot.Info.Style.FigureBackgroundColor = Microsoft.Maui.Graphics.Color.FromInt(SystemColors.Control.ToArgb());

            skglControl1.MouseWheel += SkglControl1_MouseWheel; ;
            skglControl1.PaintSurface += SkglControl1_PaintSurface;
            skglControl1.SizeChanged += SkglControl1_SizeChanged;
            skglControl1.MouseMove += SkglControl1_MouseMove;
            skglControl1.MouseUp += SkglControl1_MouseUp;
            skglControl1.MouseDown += SkglControl1_MouseDown;
            skglControl1.MouseWheel += SkglControl1_MouseWheel1;
            skglControl1.DoubleClick += SkglControl1_DoubleClick;

            Plot.Info = Plot.Info.WithSize(skglControl1.Width, skglControl1.Height);
        }

        public void Redraw()
        {
            skglControl1.Refresh();
        }

        private void SkglControl1_DoubleClick(object? sender, EventArgs e)
        {
            Plot.BenchmarkToggle();
            skglControl1.Invalidate();
        }

        private void SkglControl1_MouseWheel1(object? sender, MouseEventArgs e)
        {
            double fraction = e.Delta > 0 ? 1.15 : 0.85;
            Pixel MouseNowPixel = new(e.X, e.Y);
            Plot.Info = Plot.Info.WithZoom(MouseNowPixel, fraction);

            skglControl1.Refresh();
        }

        private void SkglControl1_MouseDown(object? sender, MouseEventArgs e)
        {
            MouseDownPixel = new Pixel(e.X, e.Y);
            MouseDownView = Plot.Info;
        }

        private void SkglControl1_MouseUp(object? sender, MouseEventArgs e)
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

        private void SkglControl1_MouseMove(object? sender, MouseEventArgs e)
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

        private void SkglControl1_SizeChanged(object? sender, EventArgs e)
        {
            Plot.Info = Plot.Info.WithSize(skglControl1.Width, skglControl1.Height);
            skglControl1.Invalidate();
        }

        private void SkglControl1_PaintSurface(object? sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            ICanvas canvas = new SkiaCanvas() { Canvas = e.Surface.Canvas };

            if (InfoNow is null)
                Plot.Draw(canvas);
            else
                Plot.Draw(canvas, InfoNow);
        }

        private void SkglControl1_MouseWheel(object? sender, MouseEventArgs e)
        {
            Plot.Info = Plot.Info.WithSize(skglControl1.Width, skglControl1.Height);
            skglControl1.Invalidate();
        }

        #region obsolete

        [Obsolete("call Redraw()", true)]
        public new void Refresh() => base.Refresh();

        #endregion
    }
}
