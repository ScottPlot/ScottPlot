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
            if (width <= 0 || height <= 0)
                throw new ArgumentException("width and height must each be greater than 0");
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
            }

            if (settings.dataSize.Width > 0 && settings.dataSize.Height > 0)
            {
                settings.bmpData = new Bitmap(settings.dataSize.Width, settings.dataSize.Height);
                settings.gfxData = Graphics.FromImage(settings.bmpData);
            }

        }

        public void UpdateAntiAliasing()
        {

            if (settings.gfxFigure != null)
            {
                if (settings.antiAliasFigure)
                {
                    settings.gfxFigure.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    settings.gfxFigure.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                }
                else
                {
                    settings.gfxFigure.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    settings.gfxFigure.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                }
            }

            if (settings.gfxData != null)
            {
                if (settings.antiAliasData)
                {
                    settings.gfxData.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    settings.gfxData.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                }
                else
                {
                    settings.gfxData.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    settings.gfxData.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                }
            }
        }

        public void Render()
        {
            if (!settings.tighteningOccurred)
                TightenLayout();

            UpdateAntiAliasing();

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
            if (settings.displayLegend)
                Renderer.DataLegend(settings);
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
            filePath = System.IO.Path.GetFullPath(filePath);
            string folder = System.IO.Path.GetDirectoryName(filePath);
            if (System.IO.Directory.Exists(folder))
            {
                settings.bmpFigure.Save(filePath);
                Console.WriteLine($"Wrote: {filePath}");
            }
            else
            {
                throw new Exception($"ERROR: folder does not exist: {folder}");
            }
        }

        #endregion




        #region PLOTTABLE DATA

        public void Clear()
        {
            settings.plottables.Clear();
        }

        public void PlotText(string text, double x, double y, Color? color = null, double fontSize = 12, bool bold = false)
        {
            if (color == null)
                color = Color.Black;
            PlottableText txt = new PlottableText(text, x, y, color: (Color)color, fontSize: fontSize, bold: bold);
            settings.plottables.Add(txt);
        }

        public void PlotPoint(double x, double y, Color? color = null, double markerSize = 5, string label = null)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableScatter mark = new PlottableScatter(new double[] { x }, new double[] { y }, color: (Color)color, lineWidth: 0, markerSize: markerSize, label: label);
            settings.plottables.Add(mark);
        }

        public void PlotScatter(double[] xs, double[] ys, Color? color = null, double lineWidth = 1, double markerSize = 5, string label = null)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableScatter scat = new PlottableScatter(xs, ys, color: (Color)color, lineWidth: lineWidth, markerSize: markerSize, label: label);
            settings.plottables.Add(scat);
        }

        public void PlotSignal(double[] ys, double sampleRate = 1, double xOffset = 0, Color? color = null, double linewidth = 1, double markerSize = 5)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableSignal signal = new PlottableSignal(ys, sampleRate, xOffset, (Color)color, lineWidth: linewidth, markerSize: markerSize);
            settings.plottables.Add(signal);
        }

        public void PlotVLine(double x, Color? color = null, double lineWidth = 1)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableAxLine axLine = new PlottableAxLine(x, vertical: true, color: (Color)color, lineWidth: lineWidth);
            settings.plottables.Add(axLine);
        }
        public void PlotHLine(double x, Color? color = null, double lineWidth = 1)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableAxLine axLine = new PlottableAxLine(x, vertical: false, color: (Color)color, lineWidth: lineWidth);
            settings.plottables.Add(axLine);
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

        public void AxisZoom(double xFrac = 1, double yFrac = 1)
        {
            settings.AxisZoom(xFrac, yFrac);
        }
        public void AxisPan(double dx = 0, double dy = 0)
        {
            settings.AxisPan(dx, dy);
        }

        public void TightenLayout(int padding = 5)
        {
            settings.axisPadding = padding;
            settings.AxisTighen();
            Resize();
        }

        #endregion




        #region CUSTOMIZATION

        public void Title(string title = null, Color? color = null, bool? enable = true)
        {
            if (title != null)
                settings.title = title;
            if (color != null)
                settings.titleColor = (Color)color;
            if (enable != null)
                if (enable == false)
                    settings.title = "";
            settings.backgroundRenderNeeded = true;
            TightenLayout();
        }

        public void XLabel(string xLabel = null, Color? color = null, bool? enable = true)
        {
            if (xLabel != null)
                settings.axisLabelX = xLabel;
            if (enable == false)
                settings.axisLabelX = "";
            if (color != null)
                settings.axisLabelColor = (Color)color;
            settings.backgroundRenderNeeded = true;
            TightenLayout();
        }

        public void YLabel(string yLabel = null, Color? color = null, bool? enable = true)
        {
            if (yLabel != null)
                settings.axisLabelY = yLabel;
            if (enable == false)
                settings.axisLabelY = "";
            if (color != null)
                settings.axisLabelColor = (Color)color;
            settings.backgroundRenderNeeded = true;
            TightenLayout();
        }

        public void Legend(bool enableLegend = true)
        {
            settings.displayLegend = enableLegend;
        }

        public void Background(Color? figure = null, Color? data = null)
        {
            if (figure != null)
                settings.figureBackgroundColor = (Color)figure;
            if (data != null)
                settings.dataBackgroundColor = (Color)data;
            settings.backgroundRenderNeeded = true;
        }

        public void Grid(bool? enable = true, Color? color = null)
        {
            if (enable != null)
                settings.displayGrid = (bool)enable;
            if (color != null)
                settings.gridColor = (Color)color;
        }

        public void Ticks(bool? displayTicksX = true, bool? displayTicksY = true, Color? color = null)
        {
            if (displayTicksX != null)
                settings.displayTicksX = (bool)displayTicksX;
            if (displayTicksY != null)
                settings.displayTicksY = (bool)displayTicksY;
            if (color != null)
                settings.tickColor = (Color)color;
            settings.backgroundRenderNeeded = true;
        }

        public void Frame(bool? drawFrame = true, Color? frameColor = null, bool[] byAxis = null)
        {
            if (drawFrame != null)
                settings.displayAxisFrames = (bool)drawFrame;
            if (frameColor != null)
                settings.tickColor = (Color)frameColor;
            if (byAxis != null && byAxis.Length == 4)
                settings.displayFrameByAxis = byAxis;
            settings.backgroundRenderNeeded = true;
        }

        public void Benchmark(bool displayBenchmark = true)
        {
            settings.displayBenchmark = displayBenchmark;
        }

        public void AntiAlias(bool figure = true, bool data = false)
        {
            settings.antiAliasFigure = figure;
            settings.antiAliasData = data;
            settings.backgroundRenderNeeded = true;
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