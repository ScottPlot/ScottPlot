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
        PlotConfig? MouseDownView = null;
        PlotConfig? InfoNow = null;

        public FormsPlot()
        {
            InitializeComponent();

            skglControl1.MouseWheel += SkglControl1_MouseWheel;
            skglControl1.PaintSurface += SkglControl1_PaintSurface;
            skglControl1.SizeChanged += SkglControl1_SizeChanged;
            skglControl1.MouseMove += SkglControl1_MouseMove;
            skglControl1.MouseUp += SkglControl1_MouseUp;
            skglControl1.MouseDown += SkglControl1_MouseDown;
            skglControl1.DoubleClick += SkglControl1_DoubleClick;

            Plot.Config.Style.FigureBackgroundColor = Microsoft.Maui.Graphics.Color.FromInt(SystemColors.Control.ToArgb());
            Plot.Config = Plot.Config.WithSize(skglControl1.Width, skglControl1.Height);
        }

        public void Redraw(bool force = false)
        {
            skglControl1.Invalidate();
            if (force)
            {
                skglControl1.Refresh();
                skglControl1.Refresh();
            }
        }

        private void SkglControl1_DoubleClick(object? sender, EventArgs e)
        {
            Plot.BenchmarkToggle();
            Redraw();
        }

        private void SkglControl1_MouseWheel(object? sender, MouseEventArgs e)
        {
            double fraction = e.Delta > 0 ? 1.15 : 0.85;
            Pixel MouseNowPixel = new(e.X, e.Y);
            Plot.Config = Plot.Config.WithZoom(MouseNowPixel, fraction);
            Redraw();
        }

        private void SkglControl1_MouseDown(object? sender, MouseEventArgs e)
        {
            MouseDownPixel = new Pixel(e.X, e.Y);
            MouseDownView = Plot.Config;
        }

        private void SkglControl1_MouseUp(object? sender, MouseEventArgs e)
        {
            if (InfoNow is not null)
                Plot.Config = InfoNow;

            if (e.Button == MouseButtons.Middle)
            {
                Plot.Autoscale();
                skglControl1.Invalidate();
            }

            MouseDownPixel = null;
            InfoNow = null;

            Redraw(true);
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

            Redraw();
        }

        private void SkglControl1_SizeChanged(object? sender, EventArgs e)
        {
            Plot.Config = Plot.Config.WithSize(skglControl1.Width, skglControl1.Height);
            Redraw();
        }

        private void SkglControl1_PaintSurface(object? sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            ICanvas canvas = new SkiaCanvas() { Canvas = e.Surface.Canvas };

            if (InfoNow is null)
                Plot.Draw(canvas);
            else
                Plot.Draw(canvas, InfoNow);
        }

        #region obsolete

        [Obsolete("call Redraw()", true)]
        public new void Refresh() => base.Refresh();

        #endregion
    }
}
