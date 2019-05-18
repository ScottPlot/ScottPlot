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

        private Bitmap bmpFigure;
        private Bitmap bmpData;
        private Graphics gfxFigure;
        private Graphics gfxData;

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
            return $"ScottPlot ({settings.figureSize.Width}x{settings.figureSize.Height}) with {plottables.Count} objects ({GetTotalPoints()} points)";
        }

        #region BITMAP AND GRAPHICS

        public void Resize(int width, int height)
        {
            settings.Resize(width, height);
            InitializeBitmaps();
        }

        private void Resize()
        {
            Resize(settings.figureSize.Width, settings.figureSize.Height);
        }

        public void InitializeBitmaps()
        {
            Debug.WriteLine("reinitializing bitmaps and graphics objects");

            if (settings.figureSize.Width > 0 && settings.figureSize.Height > 0)
            {
                bmpFigure = new Bitmap(settings.figureSize.Width, settings.figureSize.Height);
                gfxFigure = Graphics.FromImage(bmpFigure);
            }
            else
            {
                Debug.WriteLine("WARNING: figure area is 0px");
                bmpFigure = null;
                gfxFigure = null;
            }

            if (settings.dataSize.Width > 0 && settings.dataSize.Height > 0)
            {
                bmpData = new Bitmap(settings.dataSize.Width, settings.dataSize.Height);
                gfxData = Graphics.FromImage(bmpData);
            }
            else
            {
                Debug.WriteLine("WARNING: data area is 0px");
                bmpData = null;
                gfxData = null;
            }
        }

        public void Render(bool forceBackgroundRender = false, bool benchmark = true)
        {
            if (!titenHappened)
                AxisTighten();

            bool fullRender = (settings.axisRenderNeeded || forceBackgroundRender);
            stopwatch.Restart();

            if (fullRender)
            {
                renderMessage = "Full render";
                Renderer.Background(gfxFigure, settings);
                Renderer.Labels(gfxFigure, settings);
                Renderer.Ticks(gfxFigure, settings);
                Renderer.AxisFrame(gfxFigure, settings);
                settings.axisRenderNeeded = false;
            }

            Renderer.DataPlottables(gfxData, settings, plottables);
            Renderer.PlaceDataOnFigure(gfxFigure, settings, bmpData);

            stopwatch.Stop();
            renderTimeMs = stopwatch.ElapsedTicks * 1000.0 / System.Diagnostics.Stopwatch.Frequency;
            renderRateHz = 1000.0 / renderTimeMs;

            if (benchmark)
                Renderer.Benchmark(gfxFigure, settings, plottables.Count, GetTotalPoints(), renderTimeMs, renderRateHz, fullRender);
        }

        public Bitmap GetBitmap(bool renderFirst = true)
        {
            if (renderFirst)
                Render();
            return bmpFigure;
        }

        #endregion

        #region PLOTTABLE DATA

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

        #region AXES

        public double[] Axis(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
        {
            settings.AxisSet(x1, x2, y1, y2);
            return settings.axis;
        }

        private bool titenHappened = false;
        public void AxisTighten(int padding = 5)
        {
            settings.axisPadding = padding;
            settings.AxisTighen(gfxFigure);
            Resize();
            titenHappened = true;
        }

        #endregion

        #region MISC

        public int GetTotalPoints()
        {
            int totalPoints = 0;
            foreach (Plottable plottable in plottables)
                totalPoints += plottable.pointCount;
            return totalPoints;
        }

        #endregion
    }
}