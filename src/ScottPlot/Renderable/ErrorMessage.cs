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

        public void Render(Settings settings)
        {
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

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            throw new System.NotImplementedException();
        }
    }
}
