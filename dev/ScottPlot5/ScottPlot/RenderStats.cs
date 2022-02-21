using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot
{
    public class RenderStats
    {
        private readonly List<double> RenderTimesMsec = new();
        public int Count => RenderTimesMsec.Count;

        public int MaxRenderTimeCount = 10;

        Color FontColor = Colors.Black;

        // NOTE: when Maui matures, MeasureString()
        public Color BackColor = Colors.Yellow.WithAlpha(.5f);
        public Color BorderColor = Colors.Black;
        public float Padding = 5;
        public bool IsVisible = false;

        public void AddRenderTime(TimeSpan ts) => RenderTimesMsec.Add(ts.TotalMilliseconds);
        public void Clear() => RenderTimesMsec.Clear();
        public double[] GetRenderTimes() => RenderTimesMsec.ToArray();
        public (double mean, double stdErr, int n) GetStats()
        {
            double[] times = RenderTimesMsec.ToArray();
            int n = times.Length;
            double sum = times.Sum();
            double mean = sum / n;
            double stdErr = double.NaN;
            return (mean, stdErr, times.Length);
        }

        public void Draw(ICanvas canvas, PlotConfig info)
        {
            if (!IsVisible || !RenderTimesMsec.Any())
                return;

            string message = $"Render time: {RenderTimesMsec.Last()} ms ({1000.0 / RenderTimesMsec.Last():N2} FPS)";

            // NOTE: when Maui matures, MeasureString()

            canvas.FontColor = FontColor;
            canvas.DrawString(message, info.DataRect.Left + Padding, info.DataRect.Bottom - Padding, HorizontalAlignment.Left);
        }
    }
}
