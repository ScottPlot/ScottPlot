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
            TightenLayout();
        }

        public override string ToString()
        {
            return $"ScottPlot ({settings.figureSize.Width}x{settings.figureSize.Height}) with {settings.plottables.Count} objects ({GetTotalPoints()} points)";
        }




        #region BITMAP AND GRAPHICS

        public void Resize(int? width = null, int? height = null)
        {
            if (width == null)
                width = settings.figureSize.Width;
            if (height == null)
                height = settings.figureSize.Height;
            settings.Resize((int)width, (int)height);
            InitializeBitmaps();
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
            if (!settings.tighteningOccurred)
                TightenLayout();

            settings.BenchmarkStart();
            if (settings.backgroundRenderNeeded)
            {
                Renderer.FigureClear(settings);
                Renderer.FigureLabels(settings);
                Renderer.FigureTicks(settings);
                Renderer.FigureFrames(settings);
            }
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

        public void PlotText(string text, double x, double y, Color? color = null, float fontSize = 12, bool bold = false)
        {
            if (color == null)
                color = Color.Black;
            PlottableText txt = new PlottableText(text, x, y, color: (Color)color, fontSize: fontSize, bold: bold);
            settings.plottables.Add(txt);
        }

        public void PlotPoint(double x, double y, Color? color = null, float markerSize = 5)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableScatter mark = new PlottableScatter(new double[] { x }, new double[] { y }, color: (Color)color, lineWidth: 0, markerSize: markerSize);
            settings.plottables.Add(mark);
        }

        public void PlotScatter(double[] xs, double[] ys, Color? color = null, float lineWidth = 1, float markerSize = 5)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableScatter scat = new PlottableScatter(xs, ys, color: (Color)color, lineWidth: lineWidth, markerSize: markerSize);
            settings.plottables.Add(scat);
        }

        #endregion




        #region AXIS LIMIS

        public void Axis(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
        {
            settings.AxisSet(x1, x2, y1, y2);
        }

        public void AxisAuto(double horizontalMargin = .05, double verticalMargin = .1)
        {
            settings.AxisAuto(horizontalMargin, verticalMargin);
        }

        public void TightenLayout(int padding = 5)
        {
            settings.axisPadding = padding;
            settings.AxisTighen();
            Resize();
        }

        #endregion




        #region CUSTOMIZATION

        public void Title(string title = "")
        {
            settings.title = title;
            TightenLayout();
        }

        public void XLabel(string xLabel = "")
        {
            settings.axisLabelX = xLabel;
            TightenLayout();
        }

        public void YLabel(string yLabel = "")
        {
            settings.axisLabelY = yLabel;
            TightenLayout();
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