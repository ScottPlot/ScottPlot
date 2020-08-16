using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;

namespace ScottPlot.Renderables
{
    public class Benchmark : IRenderable
    {
        private readonly Stopwatch sw = Stopwatch.StartNew();
        public void Restart() => sw.Restart();
        public void Stop() => sw.Stop();
        public int debugPadding = 3;
        Color fontColor = Color.Black;
        public string fontName = Fonts.GetMonospaceFontName();
        public float fontSize = 8;
        Color fillColor = Color.FromArgb(150, Color.LightYellow);

        public void Render(Bitmap bmp, Experimental.FigureInfo fig)
        {
            double elapsedSec = (double)sw.ElapsedTicks / Stopwatch.Frequency;
            string message = $"{elapsedSec * 1000:0.00} ms ({1 / elapsedSec:0.00} Hz)";

            using (Graphics gfx = Graphics.FromImage(bmp))
            using (Font font = new Font(fontName, fontSize))
            using (Brush fillBrush = new SolidBrush(fillColor))
            using (Brush fontBrush = new SolidBrush(fontColor))
            using (Pen outline = new Pen(fontColor))
            {
                var size = gfx.MeasureString(message, font);
                var loc = new PointF(
                    x: fig.DataR - debugPadding - size.Width,
                    y: fig.DataB - debugPadding - size.Height);

                RectangleF textRect = new RectangleF(loc, size);
                gfx.FillRectangle(fillBrush, textRect);
                gfx.DrawRectangle(outline, Rectangle.Round(textRect));
                gfx.DrawString(message, font, fontBrush, loc);
            }
        }
    }
}
