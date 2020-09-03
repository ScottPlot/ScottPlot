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
        public string Message { get => $"Rendered in {MSec:00.00} ms ({Hz:00.00} Hz)"; }
        public int Padding = 3;

        public void Render(IRenderer renderer, Dimensions dims, bool lowQuality)
        {
            if (Visible == false)
                return;
            
            Font fnt = new Font(FontName, FontSize);
            Size txtSize = renderer.MeasureText(Message, fnt);

            Point txtPoint = new Point(
            x: dims.DataOffsetX + dims.DataWidth - Padding - txtSize.Width,
            y: dims.DataOffsetY + dims.DataHeight - Padding - txtSize.Height);

            renderer.FillRectangle(txtPoint, txtSize, FillColor);
            renderer.DrawRectangle(txtPoint, txtSize, OutlineColor, 1);
            renderer.DrawText(txtPoint, Message, FontColor, fnt);
        }
    }
}
