using ScottPlot.Renderer;
using System;
using System.Diagnostics;

namespace ScottPlot.Renderable
{
    public class Benchmark : IRenderable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.AboveData;

        public Color FillColor = Colors.Yellow;
        public Color FontColor = Colors.Red;
        public Color OutlineColor = Colors.Black;
        public string FontName = "consolas";
        public float FontSize = 10;

        private readonly Stopwatch Stopwatch = new Stopwatch();
        public void Start() => Stopwatch.Restart();
        public void Stop() => Stopwatch.Stop();
        public double MSec { get { return Stopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency; } }
        public double Hz { get { return (MSec > 0) ? 1000.0 / MSec : 0; } }

        public void Render(IRenderer renderer, PlotInfo info)
        {
            if (Visible == false)
                return;

            int debugPadding = 3;

            string message = $"Rendered in {MSec:0.000} ms ({Hz:0.00 Hz})";

            Font fnt = new Font(FontName, FontSize);
            Size txtSize = renderer.MeasureText(message, fnt);

            Point txtPoint = new Point(
            x: info.DataOffsetX + info.DataWidth - debugPadding - txtSize.Width,
            y: info.DataOffsetY + info.DataHeight - debugPadding - txtSize.Height);

            renderer.FillRectangle(txtPoint, txtSize, FillColor);
            renderer.DrawRectangle(txtPoint, txtSize, OutlineColor, 1);
            renderer.DrawText(txtPoint, message, FontColor, fnt);
        }
    }
}
