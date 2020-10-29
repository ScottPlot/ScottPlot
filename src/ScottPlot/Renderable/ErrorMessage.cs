using ScottPlot.Drawing;
using System.Drawing;

namespace ScottPlot.Renderable
{
    public class ErrorMessage : IRenderable
    {
        public string FontName = Config.Fonts.GetMonospaceFontName();
        public float FontSize = 12;

        public Color FillColor = Color.FromArgb(50, Color.Red);
        public Color FontColor = Color.Black;

        public string Text = "BIG ERROR";

        private bool _IsVisible = true;
        public bool IsVisible
        {
            get => _IsVisible && !string.IsNullOrWhiteSpace(Text);
            set => _IsVisible = value;
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var font = GDI.Font(FontName, FontSize))
            using (var fontBrush = GDI.Brush(FontColor))
            using (var fillBrush = GDI.Brush(FillColor))
            using (var outlinePen = GDI.Pen(FontColor))
            {
                int debugPadding = 3;
                SizeF txtSize = GDI.MeasureString(gfx, Text, font);
                PointF txtLoc = new PointF(
                    x: dims.DataOffsetX + debugPadding,
                    y: dims.DataOffsetY + debugPadding);
                RectangleF textRect = new RectangleF(txtLoc, txtSize);

                gfx.FillRectangle(fillBrush, textRect);
                gfx.DrawRectangle(outlinePen, Rectangle.Round(textRect));
                gfx.DrawString(Text, font, fontBrush, txtLoc);
            }
        }
    }
}
