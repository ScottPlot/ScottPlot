using ScottPlot.Drawing;
using System;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlot.Renderable
{
    public class Benchmark : IRenderable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.AboveData;

        public Color FillColor = Color.FromArgb(150, Color.LightYellow);
        public Color FontColor = Color.Black;
        public Color OutlineColor = Color.Black;

        private Stopwatch stopwatch = new Stopwatch();
        public void Start()
        {
            Text = "running...";
            stopwatch.Restart();
        }

        public void Stop()
        {
            stopwatch.Stop();
            Text = string.Format("Rendered in {0:0.000} ms ({1:0.00 Hz})", MSec, Hz);
        }

        public double MSec { get { return stopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency; } }
        public double Hz { get { return (MSec > 0) ? 1000.0 / MSec : 0; } }
        public string Text { get; private set; } = "benchmark not yet run...";

        public void Render(Bitmap bmp, PlotInfo info)
        {
            if (Visible == false)
                return;

            Stop();
            using (Graphics gfx = Graphics.FromImage(bmp))
            using (var font = SdFont.Monospace(11))
            using (var fontBrush = new SolidBrush(FontColor))
            using (var fillBrush = new SolidBrush(FillColor))
            using (var outline = new Pen(OutlineColor))
            {
                int debugPadding = 3;
                SizeF txtSize = gfx.MeasureString(Text, font);
                PointF txtLoc = new PointF(
                x: info.DataOffsetX + info.DataWidth - debugPadding - txtSize.Width,
                y: info.DataOffsetY + info.DataHeight - debugPadding - txtSize.Height);
                RectangleF textRect = new RectangleF(txtLoc, txtSize);

                gfx.FillRectangle(fillBrush, textRect);
                gfx.DrawRectangle(outline, Rectangle.Round(textRect));
                gfx.DrawString(Text, font, fontBrush, txtLoc);
            }
        }
    }
}
