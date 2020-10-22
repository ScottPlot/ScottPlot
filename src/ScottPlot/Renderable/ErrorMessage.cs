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

        public void Render(Settings settings)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;

            using (var font = GDI.Font(FontName, FontSize))
            using (var fontBrush = GDI.Brush(FontColor))
            using (var fillBrush = GDI.Brush(FillColor))
            using (var outlinePen = GDI.Pen(FontColor))
            {
                int debugPadding = 3;
                SizeF txtSize = settings.gfxData.MeasureString(Text, font);
                PointF txtLoc = new PointF(
                    x: settings.dataOrigin.X + debugPadding,
                    y: settings.dataOrigin.Y + debugPadding);
                RectangleF textRect = new RectangleF(txtLoc, txtSize);

                settings.gfxFigure.FillRectangle(fillBrush, textRect);
                settings.gfxFigure.DrawRectangle(outlinePen, Rectangle.Round(textRect));
                settings.gfxFigure.DrawString(Text, font, fontBrush, txtLoc);
            }
        }
    }
}
