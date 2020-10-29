using ScottPlot.Config;
using ScottPlot.Drawing;
using System;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlot.Renderable
{
    public class Benchmark : IRenderable
    {
        private Stopwatch stopwatch = new Stopwatch();
        public double msec { get { return stopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency; } }
        public double hz { get { return (msec > 0) ? 1000.0 / msec : 0; } }
        public string text { get; private set; }

        public string FontName = Fonts.GetMonospaceFontName();
        public float FontSize = 10;
        public Color FillColor = Color.FromArgb(150, Color.LightYellow);
        public Color FontColor = Color.Black;

        public bool IsVisible { get; set; } = false;

        public void Start() => stopwatch.Restart();
        public void Stop() => stopwatch.Stop();
        public override string ToString() => text;

        public void UpdateMessage(int plottableCount)
        {
            text = $"Rendered {plottableCount} plot objects";
            text += string.Format(" in {0:0.000} ms ({1:0.00 Hz})", msec, hz);
            if (plottableCount == 1)
                text = text.Replace("objects", "object");
        }

        public void Render(Drawing.PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var font = GDI.Font(FontName, FontSize))
            using (var fontBrush = new SolidBrush(FontColor))
            using (var fillBrush = new SolidBrush(FillColor))
            using (var outline = new Pen(FontColor))
            {
                int debugPadding = 3;
                SizeF txtSize = GDI.MeasureString(gfx, text, font);
                PointF txtLoc = new PointF(
                    x: dims.DataWidth + dims.DataOffsetX - debugPadding - txtSize.Width,
                    y: dims.DataHeight + dims.DataOffsetY - debugPadding - txtSize.Height);
                RectangleF textRect = new RectangleF(txtLoc, txtSize);

                gfx.FillRectangle(fillBrush, textRect);
                gfx.DrawRectangle(outline, Rectangle.Round(textRect));
                gfx.DrawString(text, font, fontBrush, txtLoc);
            }
        }
    }
}
