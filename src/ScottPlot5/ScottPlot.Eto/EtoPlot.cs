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

#if false
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Keyboard.Focus(this);

            Backend.MouseDown(e.Pixel(this), e.ToButton());

            (sender as UIElement)?.CaptureMouse();

            if (e.ClickCount == 2)
            {
                Backend.DoubleClick();
            }

            base.OnMouseDown(e);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            Backend.MouseUp(e.Pixel(this), e.ToButton());
            (sender as UIElement)?.ReleaseMouseCapture();
            base.OnMouseUp(e);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Backend.MouseMove(e.Pixel(this));
            base.OnMouseMove(e);
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            Backend.MouseWheelVertical(e.Pixel(this), e.Delta);
            base.OnMouseWheel(e);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Backend.KeyDown(e.Key());
            base.OnKeyDown(e);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Backend.KeyUp(e.Key());
            base.OnKeyUp(e);
        }
#endif
    }
}
