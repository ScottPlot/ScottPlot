using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlot
{
    public class Plot
    {
        /* This class provides a simple way to accumulate plottables.
         * Plottables may have more features in them than ScottPlot provides access to.
         * Efforts will be taken to stabalize this API (though plottables may change).
         */

        public readonly Settings settings = new Settings();

        public Plot(int width = 600, int height = 800)
        {
            Resize(width, height);
        }

        public override string ToString()
        {
            return $"ScottPlot ({settings.figureSize.Width}x{settings.figureSize.Height}) with {settings.plottables.Count} objects ({GetTotalPoints()} points)";
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
            settings.bmpFigure = null;
            settings.gfxFigure = null;

            if (settings.figureSize.Width > 0 && settings.figureSize.Height > 0)
            {
                settings.bmpFigure = new Bitmap(settings.figureSize.Width, settings.figureSize.Height);
                settings.gfxFigure = Graphics.FromImage(settings.bmpFigure);
                settings.gfxFigure.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                settings.gfxFigure.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            }

            if (settings.dataSize.Width > 0 && settings.dataSize.Height > 0)
            {
                settings.bmpData = new Bitmap(settings.dataSize.Width, settings.dataSize.Height);
                settings.gfxData = Graphics.FromImage(settings.bmpData);
                settings.gfxData.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                settings.SetAntiAlilasing();
            }

        }

        public void Render()
        {
            if (!titenHappened)
                AxisTighten();

            settings.BenchmarkStart();
            if (settings.backgroundRenderNeeded)
            {
                Debug.WriteLine("Rendering background");
                Renderer.FigureClear(settings);
                Renderer.FigureLabels(settings);
                Renderer.FigureTicks(settings);
                Renderer.FigureFrames(settings);
            }
            Debug.WriteLine("Rendering data");
            Renderer.DataBackground(settings);
            Renderer.DataGrid(settings);
            Renderer.DataPlottables(settings);
            Renderer.DataPlaceOntoFigure(settings);
            settings.BenchmarkEnd();
            Renderer.Benchmark(settings);
            settings.backgroundRenderNeeded = false;

        }

        public Bitmap GetBitmap(bool renderFirst = true)
        {
            if (renderFirst)
                Render();
            return settings.bmpFigure;
        }

        public void SaveFig(string filePath, bool renderFirst = true)
        {
            if (renderFirst)
                Render();
            string folder = System.IO.Path.GetDirectoryName(filePath);
            if (System.IO.Directory.Exists(folder))
                settings.bmpFigure.Save(filePath);
            else
                throw new Exception($"ERROR: folder does not exist: {folder}");
        }

        #endregion




        #region PLOTTABLE DATA

        public void Clear()
        {
            Debug.WriteLine("Clearing plottables");
            settings.plottables.Clear();
        }

        public void PlotText(string text, double x, double y)
        {
            PlottableText txt = new PlottableText(text, x, y);
            settings.plottables.Add(txt);
        }

        public void PlotMarker(double x, double y)
        {
            PlottableMarker mark = new PlottableMarker(x, y);
            settings.plottables.Add(mark);
        }

        public void PlotScatter(double[] xs, double[] ys, Color? color = null)
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableScatter scat = new PlottableScatter(xs, ys, (Color)color);
            settings.plottables.Add(scat);
        }

        #endregion




        #region AXIS LIMIS

        public double[] Axis(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
        {
            settings.AxisSet(x1, x2, y1, y2);
            return settings.axis;
        }

        private bool titenHappened = false;
        public void AxisTighten(int padding = 5)
        {
            settings.axisPadding = padding;
            settings.AxisTighen();
            Resize();
            titenHappened = true;
        }

        #endregion




        #region CUSTOMIZATION

        public string title
        {
            get { return settings.title; }
            set { settings.title = value; }
        }

        public string xLabel
        {
            get { return settings.axisLabelX; }
            set { settings.axisLabelX = value; }
        }

        public string yLabel
        {
            get { return settings.axisLabelY; }
            set { settings.axisLabelY = value; }
        }

        #endregion




        #region MISC

        public int GetTotalPoints()
        {
            int totalPoints = 0;
            foreach (Plottable plottable in settings.plottables)
                totalPoints += plottable.pointCount;
            return totalPoints;
        }

        #endregion
    }
}