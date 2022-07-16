using ScottPlot.Drawing;
using System.Drawing;

namespace ScottPlot.Renderable
{
    public class ZoomRectangle : IRenderable
    {
        private float X;
        private float Y;
        private float Width;
        private float Height;

        public System.Drawing.Color FillColor { get; set; } = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Red);
        public System.Drawing.Color BorderColor { get; set; } = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Red);
        public bool IsVisible { get; set; } = true;

        public void Clear() => IsVisible = false;

        public void Set(float x, float y, float width, float height) =>
            (X, Y, Width, Height, IsVisible) = (x, y, width, height, true);

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!IsVisible)
                return;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality: true, false))
            using (var fillBrush = GDI.Brush(FillColor))
            using (var borderPen = GDI.Pen(BorderColor))
            {
                gfx.FillRectangle(fillBrush, X + dims.DataOffsetX, Y + dims.DataOffsetY, Width, Height);
                gfx.DrawRectangle(borderPen, X + dims.DataOffsetX, Y + dims.DataOffsetY, Width, Height);
            }
        }
    }
}
