using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlot
{
    public class ScottPlot
    {
        /* This class provides a simple way to accumulate plottables.
         * Plottables may have more features in them than ScottPlot provides access to.
         * Efforts will be taken to stabalize this API (though plottables may change).
         */

        // objects requiring initialization
        private Bitmap figureBmp;
        private Graphics figureGfx;

        // set and forget
        public readonly Settings settings = new Settings();
        private readonly Stopwatch stopwatch = Stopwatch.StartNew();
        public readonly List<Plottable> plottables = new List<Plottable>();

        public double renderTimeMs { get; private set; }
        public double renderRateHz { get; private set; }
        public string renderMessage { get; private set; }

        public ScottPlot(int width = 600, int height = 800)
        {
            Debug.WriteLine($"creating new ScottPlot");
            Resize(width, height);
        }

        public override string ToString()
        {
            return $"ScottPlot ({settings.figureSize.Width}x{settings.figureSize.Height})";
        }

        #region bitmap and graphics

        public void Resize(int width, int height)
        {
            Debug.WriteLine($"resizing to ({width}, {height})");
            settings.Resize(width, height);
            InitializeBitmaps();
        }

        public void InitializeBitmaps()
        {
            Debug.WriteLine("reinitializing bitmaps and graphics objects");
            figureBmp = new Bitmap(settings.figureSize.Width, settings.figureSize.Height);
            figureGfx = Graphics.FromImage(figureBmp);
        }

        public void Render()
        {
            stopwatch.Restart();
            figureGfx.Clear(settings.figureBackgroundColor);

            int pointsPlotted = 0;
            for (int i = 0; i < plottables.Count; i++)
            {
                Plottable pltThing = plottables[i];
                try
                {
                    pltThing.Render(settings, figureGfx);
                    pointsPlotted += pltThing.pointCount;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"EXCEPTION while rendering {pltThing}:\n{ex}");
                }
            }

            figureGfx.DrawRectangle(Pens.Black, 0, 0, settings.figureSize.Width - 1, settings.figureSize.Height - 1);
            figureGfx.DrawRectangle(Pens.Black, settings.dataOrigin.X, settings.dataOrigin.Y, settings.dataSize.Width - 1, settings.dataSize.Height - 1);

            Ticks.DrawTicks(figureGfx, settings);

            stopwatch.Stop();
            renderTimeMs = stopwatch.ElapsedTicks * 1000.0 / System.Diagnostics.Stopwatch.Frequency;
            renderRateHz = 1000.0 / renderTimeMs;
            renderMessage = string.Format("Rendered {4}x{5} Bitmap with {2} objects ({3} points) in {0:00.000} ms ({1:0.00 Hz})",
                renderTimeMs, renderRateHz, plottables.Count, pointsPlotted, settings.figureSize.Width, settings.figureSize.Height);
            Debug.WriteLine(renderMessage);
        }

        public Bitmap GetBitmap(bool renderFirst = true)
        {
            if (renderFirst)
                Render();
            return figureBmp;
        }

        #endregion

        #region plottable data

        public void Clear()
        {
            Debug.WriteLine("Clearing plottables");
            plottables.Clear();
        }

        public void PlotText(string text, double x, double y)
        {
            PlottableText txt = new PlottableText(text, x, y);
            plottables.Add(txt);
        }

        public void PlotMarker(double x, double y)
        {
            PlottableMarker mark = new PlottableMarker(x, y);
            plottables.Add(mark);
        }

        public void PlotScatter(double[] xs, double[] ys)
        {
            PlottableScatter scat = new PlottableScatter(xs, ys);
            plottables.Add(scat);
        }

        #endregion

        #region axes

        public double[] Axes(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
        {
            settings.AxisSet(x1, x2, y1, y2);
            return settings.axis;
        }

        #endregion
    }
}