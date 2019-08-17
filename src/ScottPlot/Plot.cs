/* The Plot class is the primary public interface to ScottPlot.
 * - This should be the only class the user interacts with.
 * - Internal refactoring can occur as long as these functions remain fixed.
 * - This file is intentionally spaced out to make code changes easier to review.
 * - Very little processing occurs here. This interface mostly calls private methods.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ScottPlot
{
    public class Plot
    {
        public PixelFormat pixelFormat = PixelFormat.Format32bppPArgb;
        private readonly Settings settings;
        public readonly MouseTracker mouseTracker;

        public Plot(int width = 800, int height = 600)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("width and height must each be greater than 0");
            settings = new Settings();
            mouseTracker = new MouseTracker(settings);
            Resize(width, height);
            TightenLayout();
        }

        public override string ToString()
        {
            string sizeString = $"{settings.figureSize.Width}x{settings.figureSize.Height}";
            return $"ScottPlot ({sizeString}) with {GetPlottables().Count} objects ({GetTotalPoints()} points)";
        }

        #region Bitmaps, Graphics, and Drawing Settings

        public void Resize(int? width = null, int? height = null)
        {
            if (width == null)
                width = settings.figureSize.Width;
            if (height == null)
                height = settings.figureSize.Height;

            if (width < 1)
                width = 1;
            if (height < 1)
                height = 1;

            settings.Resize((int)width, (int)height);
            InitializeBitmaps();
        }

        private void InitializeBitmaps()
        {
            settings.bmpFigure = null;
            settings.gfxFigure = null;
            settings.bmpData = null;
            settings.gfxData = null;

            if (settings.figureSize.Width > 0 && settings.figureSize.Height > 0)
            {
                settings.bmpFigure = new Bitmap(settings.figureSize.Width, settings.figureSize.Height, pixelFormat);
                settings.gfxFigure = Graphics.FromImage(settings.bmpFigure);
            }

            if (settings.dataSize.Width > 0 && settings.dataSize.Height > 0)
            {
                settings.bmpData = new Bitmap(settings.dataSize.Width, settings.dataSize.Height, pixelFormat);
                settings.gfxData = Graphics.FromImage(settings.bmpData);
            }

        }

        private void UpdateAntiAliasingSettings()
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

        private void RenderBitmap()
        {
            if (!settings.axisHasBeenIntentionallySet && settings.plottables.Count > 0)
                settings.AxisAuto();

            if (!settings.tighteningOccurred)
                TightenLayout();

            UpdateAntiAliasingSettings();

            settings.BenchmarkStart();
            if (settings.bmpFigureRenderRequired)
            {
                Renderer.FigureClear(settings);
                Renderer.FigureLabels(settings);
                Renderer.FigureTicks(settings);
                Renderer.FigureFrames(settings);
                settings.bmpFigureRenderRequired = false;
            }
            if (settings.gfxData != null)
            {
                Renderer.DataBackground(settings);
                Renderer.DataGrid(settings);
                Renderer.DataPlottables(settings);
                Renderer.DataLegend(settings);
                Renderer.DataPlaceOntoFigure(settings);
            }
            settings.BenchmarkEnd();
            Renderer.Benchmark(settings);
        }

        public Bitmap GetBitmap(bool renderFirst = true, bool lowQuality = false)
        {          
            if (lowQuality)
            {
                bool currentAAData = settings.antiAliasData; // save currently using AA setting
                settings.antiAliasData = false; // disable AA for render
                if (renderFirst)
                    RenderBitmap();
                settings.antiAliasData = currentAAData; // restore saved AA setting
            }
            else
            {
                if (renderFirst)
                    RenderBitmap();
            }                            
            return settings.bmpFigure;
        }

        public void SaveFig(string filePath, bool renderFirst = true)
        {
            if (renderFirst)
                RenderBitmap();
            filePath = System.IO.Path.GetFullPath(filePath);
            string folder = System.IO.Path.GetDirectoryName(filePath);
            if (System.IO.Directory.Exists(folder))
                settings.bmpFigure.Save(filePath);
            else
                throw new Exception($"ERROR: folder does not exist: {folder}");
        }

        #endregion

        #region Managing Plot Objects

        public void Clear(
            bool axisLines = true,
            bool scatterPlots = true,
            bool signalPlots = true,
            bool text = true,
            bool bar = true,
            bool finance = true
            )
        {
            settings.Clear(
                axLines: axisLines,
                scatters: scatterPlots,
                signals: signalPlots,
                text: text,
                bar: bar,
                finance: finance
                );
        }

        public PlottableText PlotText(
            string text,
            double x,
            double y,
            Color? color = null,
            string fontName = "Segoe UI",
            double fontSize = 12,
            bool bold = false,
            string label = null,
            TextAlignment alignment = TextAlignment.upperLeft
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            fontName = ScottPlot.Tools.VerifyFont(fontName);

            PlottableText plottableText = new PlottableText(
                text: text,
                x: x,
                y: y,
                color: (Color)color,
                fontName: fontName,
                fontSize: fontSize,
                bold: bold,
                label: label,
                alignment: alignment
                );

            settings.plottables.Add(plottableText);
            return plottableText;
        }

        public PlottableScatter PlotPoint(
            double x,
            double y,
            Color? color = null,
            double markerSize = 5,
            string label = null,
            double? errorX = null,
            double? errorY = null,
            double errorLineWidth = 1,
            double errorCapSize = 3,
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            double[] errorXarray = (errorX != null) ? new double[] { (double)errorX } : null;
            double[] errorYarray = (errorY != null) ? new double[] { (double)errorY } : null;

            PlottableScatter scatterPlot = new PlottableScatter(
                xs: new double[] { x },
                ys: new double[] { y },
                color: (Color)color,
                lineWidth: 0,
                markerSize: markerSize,
                label: label,
                errorX: errorXarray,
                errorY: errorYarray,
                errorLineWidth: errorLineWidth,
                errorCapSize: errorCapSize,
                stepDisplay: false,
                markerShape: markerShape,
                lineStyle: lineStyle
                );

            settings.plottables.Add(scatterPlot);
            return scatterPlot;
        }

        public PlottableScatter PlotScatter(
            double[] xs,
            double[] ys,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            double[] errorX = null,
            double[] errorY = null,
            double errorLineWidth = 1,
            double errorCapSize = 3,
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableScatter scatterPlot = new PlottableScatter(
                xs: xs,
                ys: ys,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                errorX: errorX,
                errorY: errorY,
                errorLineWidth: errorLineWidth,
                errorCapSize: errorCapSize,
                stepDisplay: false,
                markerShape: markerShape,
                lineStyle: lineStyle
                );

            settings.plottables.Add(scatterPlot);
            return scatterPlot;
        }

        public PlottableScatter PlotStep(
            double[] xs,
            double[] ys,
            Color? color = null,
            double lineWidth = 1,
            string label = null
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableScatter stepPlot = new PlottableScatter(
                xs: xs,
                ys: ys,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: 0,
                label: label,
                errorX: null,
                errorY: null,
                errorLineWidth: 0,
                errorCapSize: 0,
                stepDisplay: true,
                markerShape: MarkerShape.none,
                lineStyle: LineStyle.Solid
                );

            settings.plottables.Add(stepPlot);
            return stepPlot;
        }

        public PlottableSignal<T> PlotSignal<T>(
            T[] ys,
            double sampleRate = 1,
            double xOffset = 0,
            double yOffset = 0,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null
            ) where T: struct, IComparable
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableSignal<T> signal = new PlottableSignal<T>(
                ys: ys,
                sampleRate: sampleRate,
                xOffset: xOffset,
                yOffset: yOffset,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                useParallel: settings.useParallel
                );

            settings.plottables.Add(signal);
            return signal;
        }

        public PlottableSignalConst<T> PlotSignalConst<T>(
            T[] ys,
            double sampleRate = 1,
            double xOffset = 0,
            double yOffset = 0,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            bool singlePrecision = false
            ) where T: struct, IComparable
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableSignalConst<T> signal = new PlottableSignalConst<T>(
                ys: ys,
                sampleRate: sampleRate,
                xOffset: xOffset,
                yOffset: yOffset,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                useParallel: settings.useParallel,
                singlePrecision: singlePrecision
                );

            settings.plottables.Add(signal);
            return signal;
        }

        public PlottableBar PlotBar(
            double[] xs,
            double[] ys,
            double? barWidth = null,
            double xOffset = 0,
            Color? color = null,
            string label = null,
            double[] errorY = null,
            double errorLineWidth = 1,
            double errorCapSize = 3
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            if (barWidth == null)
                barWidth = (xs[1] - xs[0]) * .8;

            PlottableBar barPlot = new PlottableBar(
                xs: xs,
                ys: ys,
                barWidth: (double)barWidth,
                xOffset: xOffset,
                color: (Color)color,
                label: label,
                yErr: errorY,
                errorLineWidth: errorLineWidth,
                errorCapSize: errorCapSize
                );

            settings.plottables.Add(barPlot);
            return barPlot;
        }

        public PlottableOHLC PlotOHLC(OHLC[] ohlcs)
        {
            PlottableOHLC ohlc = new PlottableOHLC(ohlcs, displayCandles: false);
            settings.plottables.Add(ohlc);
            return ohlc;
        }

        public PlottableOHLC PlotCandlestick(OHLC[] ohlcs)
        {
            PlottableOHLC ohlc = new PlottableOHLC(ohlcs, displayCandles: true);
            settings.plottables.Add(ohlc);
            return ohlc;
        }

        public PlottableAxLine PlotVLine(
            double x,
            Color? color = null,
            double lineWidth = 1,
            string label = null,
            bool draggable = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableAxLine axLine = new PlottableAxLine(
                position: x,
                vertical: true,
                color: (Color)color,
                lineWidth: lineWidth,
                label: label,
                draggable: draggable,
                dragLimitLower: dragLimitLower,
                dragLimitUpper: dragLimitUpper,
                lineStyle: lineStyle
                );

            settings.plottables.Add(axLine);
            return axLine;
        }

        public PlottableAxSpan PlotVSpan(
            double y1,
            double y2,
            Color? color = null,
            double alpha = .5,
            string label = null,
            bool draggable = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            var axisSpan = new PlottableAxSpan(
                position1: y1,
                position2: y2,
                vertical: true,
                color: (Color)color,
                alpha: alpha,
                label: label,
                draggable: draggable,
                dragLimitLower: dragLimitLower,
                dragLimitUpper: dragLimitUpper
                );

            settings.plottables.Add(axisSpan);
            return axisSpan;
        }

        public PlottableAxLine PlotHLine(
            double x,
            Color? color = null,
            double lineWidth = 1,
            string label = null,
            bool draggable = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableAxLine axLine = new PlottableAxLine(
                position: x,
                vertical: false,
                color: (Color)color,
                lineWidth: lineWidth,
                label: label,
                draggable: draggable,
                dragLimitLower: dragLimitLower,
                dragLimitUpper: dragLimitUpper,
                lineStyle: lineStyle
                );

            settings.plottables.Add(axLine);
            return axLine;
        }

        public PlottableAxSpan PlotHSpan(
            double x1,
            double x2,
            Color? color = null,
            double alpha = .5,
            string label = null,
            bool draggable = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            var axisSpan = new PlottableAxSpan(
                    position1: x1,
                    position2: x2,
                    vertical: false,
                    color: (Color)color,
                    alpha: alpha,
                    label: label,
                    draggable: draggable,
                    dragLimitLower: dragLimitLower,
                    dragLimitUpper: dragLimitUpper
                    );

            settings.plottables.Add(axisSpan);
            return axisSpan;
        }

        public List<Plottable> GetPlottables()
        {
            // This function is useful because the end user really isn't 
            // intended to interact with the settincs class directly.
            return settings.plottables;
        }

        public Settings GetSettings()
        {
            // The user really should not interact with the settings class directly.
            // This is exposed here to aid in testing.
            Console.WriteLine("WARNING: GetSettings() is only for development and testing");
            return settings;
        }

        public int GetTotalPoints()
        {
            int totalPoints = 0;
            foreach (Plottable plottable in settings.plottables)
                totalPoints += plottable.pointCount;
            return totalPoints;
        }

        #endregion

        #region Axis Settings

        public double[] Axis(
            double? x1 = null, 
            double? x2 = null, 
            double? y1 = null, 
            double? y2 = null
            )
        {
            bool someValuesAreNull = (x1 == null) || (x2 == null) || (y1 == null) || (y2 == null);
            if (someValuesAreNull && !settings.axisHasBeenIntentionallySet)
                settings.AxisAuto();

            settings.AxisSet(x1, x2, y1, y2);
            return settings.axis;
        }

        public void AxisAuto(
            double horizontalMargin = .05, 
            double verticalMargin = .1, 
            int tightenPadding = 5
            )
        {
            settings.AxisAuto(horizontalMargin, verticalMargin);
            TightenLayout(tightenPadding);
        }

        public void AxisZoom(
            double xFrac = 1, 
            double yFrac = 1, 
            PointF? zoomCenter = null
            )
        {
            settings.AxisZoom(xFrac, yFrac, zoomCenter);
        }

        public void AxisPan(double dx = 0, double dy = 0)
        {
            settings.AxisPan(dx, dy);
        }

        public PointF CoordinateFromPixel(int pixelX, int pixelY)
        {
            return settings.GetLocation(pixelX, pixelY);
        }

        public PointF CoordinateFromPixel(Point pixel)
        {
            return CoordinateFromPixel(pixel.X, pixel.Y);
        }

        public PointF CoordinateToPixel(double locationX, double locationY)
        {
            PointF pixelLocation = settings.GetPixel(locationX, locationY);
            pixelLocation.X += settings.dataOrigin.X;
            pixelLocation.Y += settings.dataOrigin.Y;
            return pixelLocation;
        }

        public PointF CoordinateToPixel(PointF location)
        {
            return CoordinateToPixel(location.X, location.Y);
        }

        #endregion

        #region Labels

        public void Title(
            string title = null,
            bool? enable = true,
            string fontName = "Segoe UI",
            float fontSize = 12,
            Color? color = null,
            bool bold = true
            )
        {
            if (title != null)
                settings.title = title;
            if (color != null)
                settings.titleColor = (Color)color;
            if (enable != null)
                if (enable == false)
                    settings.title = "";

            fontName = ScottPlot.Tools.VerifyFont(fontName);
            FontStyle fontStyle = (bold) ? FontStyle.Bold : FontStyle.Regular;

            settings.titleFont = new Font(fontName, fontSize, fontStyle);
            settings.bmpFigureRenderRequired = true;
            TightenLayout();
        }

        public void XLabel(
            string xLabel = null,
            Color? color = null,
            bool? enable = true,
            string fontName = "Segoe UI",
            float fontSize = 12,
            bool bold = false
            )
        {
            if (xLabel != null)
                settings.axisLabelX = xLabel;
            if (enable == false)
                settings.axisLabelX = "";
            if (color != null)
                settings.axisLabelColorX = (Color)color;

            fontName = ScottPlot.Tools.VerifyFont(fontName);
            FontStyle fontStyle = (bold) ? FontStyle.Bold : FontStyle.Regular;
            settings.axisLabelFontX = new Font(fontName, fontSize, fontStyle);

            settings.bmpFigureRenderRequired = true;
            TightenLayout();
        }

        public void YLabel(
            string yLabel = null,
            bool? enable = true,
            string fontName = "Segoe UI",
            float fontSize = 12, Color?
            color = null,
            bool bold = false
            )
        {
            if (yLabel != null)
                settings.axisLabelY = yLabel;
            if (enable == false)
                settings.axisLabelY = "";
            if (color != null)
                settings.axisLabelColorY = (Color)color;

            fontName = ScottPlot.Tools.VerifyFont(fontName);
            FontStyle fontStyle = (bold) ? FontStyle.Bold : FontStyle.Regular;
            settings.axisLabelFontY = new Font(fontName, fontSize, fontStyle);

            settings.bmpFigureRenderRequired = true;
            TightenLayout();
        }

        public void Legend(
            bool enableLegend = true,
            string fontName = "Segoe UI",
            float fontSize = 12,
            bool bold = false,
            Color? fontColor = null,
            Color? backColor = null,
            Color? frameColor = null,
            legendLocation location = legendLocation.lowerRight,
            shadowDirection shadowDirection = shadowDirection.lowerRight,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            if (fontColor != null)
                settings.legendFontColor = (Color)fontColor;
            if (backColor != null)
                settings.legendBackColor = (Color)backColor;
            if (frameColor != null)
                settings.legendFrameColor = (Color)frameColor;

            fontName = ScottPlot.Tools.VerifyFont(fontName);
            FontStyle fontStyle = (bold) ? FontStyle.Bold : FontStyle.Regular;
            settings.legendFont = new Font(fontName, fontSize, fontStyle);

            if (enableLegend)
            {
                settings.legendLocation = location;
                settings.legendShadowDirection = shadowDirection;
            }
            else
            {
                settings.legendLocation = legendLocation.none;
                settings.legendShadowDirection = shadowDirection.none;
            }
        }

        #endregion

        #region Styling and Misc Graph Settings

        public void Ticks(
            bool? displayTicksX = true,
            bool? displayTicksY = true,
            Color? color = null,
            bool? useMultiplierNotation = null,
            bool? useOffsetNotation = null,
            bool? useExponentialNotation = null
            )
        {
            if (displayTicksX != null)
                settings.displayTicksX = (bool)displayTicksX;
            if (displayTicksY != null)
                settings.displayTicksY = (bool)displayTicksY;
            if (color != null)
                settings.tickColor = (Color)color;
            if (useMultiplierNotation != null)
                settings.useMultiplierNotation = (bool)useMultiplierNotation;
            if (useOffsetNotation != null)
                settings.useOffsetNotation = (bool)useOffsetNotation;
            if (useExponentialNotation != null)
                settings.useExponentialNotation = (bool)useExponentialNotation;

            settings.bmpFigureRenderRequired = true;
        }

        public void Grid(
            bool? enable = true,
            Color? color = null,
            double? xSpacing = null,
            double? ySpacing = null
            )
        {
            if (enable != null)
                settings.displayGrid = (bool)enable;
            if (color != null)
                settings.gridColor = (Color)color;
            settings.tickSpacingX = (xSpacing == null) ? 0 : (double)xSpacing;
            settings.tickSpacingY = (ySpacing == null) ? 0 : (double)ySpacing;
            settings.bmpFigureRenderRequired = true;
        }

        public void Frame(
            bool? drawFrame = true,
            Color? frameColor = null,
            bool? left = true,
            bool? right = true,
            bool? bottom = true,
            bool? top = true
            )
        {
            if (drawFrame != null)
                settings.displayAxisFrames = (bool)drawFrame;
            if (frameColor != null)
                settings.tickColor = (Color)frameColor;
            if (left != null)
                settings.displayFrameByAxis[0] = (bool)left;
            if (right != null)
                settings.displayFrameByAxis[1] = (bool)right;
            if (bottom != null)
                settings.displayFrameByAxis[2] = (bool)bottom;
            if (top != null)
                settings.displayFrameByAxis[3] = (bool)top;
            settings.bmpFigureRenderRequired = true;
        }

        public void Benchmark(bool show = true, bool toggle = false)
        {
            if (toggle)
                settings.displayBenchmark = (settings.displayBenchmark) ? false : true;
            else
                settings.displayBenchmark = show;
        }

        public void AntiAlias(bool figure = true, bool data = false)
        {
            settings.antiAliasFigure = figure;
            settings.antiAliasData = data;
            settings.bmpFigureRenderRequired = true;
        }

        public void TightenLayout(int padding = 5)
        {
            if (!settings.axisHasBeenIntentionallySet && settings.plottables.Count > 0)
                settings.AxisAuto();
            settings.axisPadding = padding;
            settings.AxisTighen();
            Resize();
        }

        public void MatchPadding(Plot sourcePlot, bool horizontal = true, bool vertical = true)
        {
            if (horizontal)
            {
                settings.axisLabelPadding[0] = sourcePlot.settings.axisLabelPadding[0];
                settings.axisLabelPadding[1] = sourcePlot.settings.axisLabelPadding[1];
            }
            if (vertical)
            {
                settings.axisLabelPadding[2] = sourcePlot.settings.axisLabelPadding[2];
                settings.axisLabelPadding[3] = sourcePlot.settings.axisLabelPadding[3];
            }
            Resize();
        }

        public void MatchAxis(Plot sourcePlot, bool horizontal = true, bool vertical = true)
        {
            if (horizontal)
            {
                settings.axis[0] = sourcePlot.settings.axis[0];
                settings.axis[1] = sourcePlot.settings.axis[1];
            }
            if (vertical)
            {
                settings.axis[2] = sourcePlot.settings.axis[2];
                settings.axis[3] = sourcePlot.settings.axis[3];
            }
            Resize();
        }

        public void Style(
            Color? figBg = null,
            Color? dataBg = null,
            Color? grid = null,
            Color? tick = null,
            Color? label = null,
            Color? title = null
            )
        {
            if (figBg != null)
                settings.figureBackgroundColor = (Color)figBg;
            if (dataBg != null)
                settings.dataBackgroundColor = (Color)dataBg;
            if (grid != null)
                settings.gridColor = (Color)grid;
            if (tick != null)
                settings.tickColor = (Color)tick;
            if (label != null)
                settings.axisLabelColorX = (Color)label;
            if (label != null)
                settings.axisLabelColorY = (Color)label;
            if (title != null)
                settings.titleColor = (Color)title;
            if (dataBg != null)
                settings.legendBackColor = (Color)dataBg;
            if (tick != null)
                settings.legendFrameColor = (Color)tick;
            if (label != null)
                settings.legendFontColor = (Color)label;
            settings.bmpFigureRenderRequired = true;
        }

        public void Style(Style style)
        {
            StyleTools.SetStyle(this, style);
        }

        public void Parallel(bool useParallel)
        {
            settings.useParallel = useParallel;
            foreach (var plottable in GetPlottables())
                plottable.useParallel = useParallel;
        }

        #endregion

    }
}