using ScottPlot.Drawing;
using System.Drawing;

namespace ScottPlot.Renderable
{
    public class FigureBackground : IRenderable
    {
        public Color Color { get; set; } = Color.White;
        public bool IsVisible { get; set; } = true;
        public Bitmap Bitmap { get; set; } = null;

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (Bitmap is null)
            {
                RenderSolidColorBackground(dims, bmp);
            }
            else
            {
                RenderImageBackground(dims, bmp);
            }
        }

        private void RenderSolidColorBackground(PlotDimensions dims, Bitmap bmp)
        {
            using var gfx = GDI.Graphics(bmp, dims, lowQuality: true, clipToDataArea: false);
            gfx.Clear(Color);
        }

        private void RenderImageBackground(PlotDimensions dims, Bitmap bmp)
        {
            using var gfx = GDI.Graphics(bmp, dims, lowQuality: false, clipToDataArea: false);

            float x = 0;
            float y = 0;
            float width = dims.Width;
            float height = dims.Height;

            // NOTE: increase size by 1 source pixel to prevent anti-aliasing transparency at edges
            x -= width / Bitmap.Width;
            y -= height / Bitmap.Height;
            width += 2 * width / Bitmap.Width;
            height += 2 * height / Bitmap.Height;

            gfx.DrawImage(Bitmap, x, y, width, height);
        }
    }
}
