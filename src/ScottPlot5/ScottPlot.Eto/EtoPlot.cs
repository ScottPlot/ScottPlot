using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using ScottPlot.Control;
using SkiaSharp;
using Eto.Drawing;
using System.Runtime.InteropServices;

namespace ScottPlot.Eto
{
    public class EtoPlot : ImageView, IPlotControl
    {
        public Plot Plot { get; } = new();

        public Backend Backend { get; set; }

        public EtoPlot()
        {
            Backend = new(this);

            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;
            this.MouseMove += OnMouseMove;
            this.MouseWheel += OnMouseWheel;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
            this.MouseDoubleClick += OnDoubleClick;
            this.SizeChanged += OnSizeChanged;
        }

        public void Refresh()
        {
            SKImageInfo imageInfo = new((int)Bounds.Width, (int)Bounds.Height);

            using var surface = SKSurface.Create(imageInfo);
            if (surface is null)
                return;

            Plot.Render(surface);

            // TODO: can this sequence be improved to reduce copying?
            SKImage img = surface.Snapshot();
            SKPixmap pixels = img.ToRasterImage().PeekPixels();
            byte[] bytes = pixels.GetPixelSpan().ToArray();

            var bmp = new Bitmap((int)Bounds.Width, (int)Bounds.Height, PixelFormat.Format32bppRgba);

            using (var data = bmp.Lock())
            {
                Marshal.Copy(bytes, 0, data.Data, bytes.Length);
            }

            this.Image = bmp;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Focus();

            Backend.MouseDown(e.Pixel(), e.ToButton());
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            Backend.MouseUp(e.Pixel(), e.ToButton());
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Backend.MouseMove(e.Pixel());
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            Backend.MouseWheelVertical(e.Pixel(), e.Delta.Height);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Backend.KeyDown(e.Key());
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Backend.KeyUp(e.Key());
        }

        private void OnDoubleClick(object sender, MouseEventArgs e)
        {
            Backend.DoubleClick();
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
