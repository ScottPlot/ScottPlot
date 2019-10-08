﻿/* The Plot class is the primary public interface to ScottPlot.
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

        public Plot(int width = 800, int height = 600, IGraphicBackend backendFigure = null, IGraphicBackend backendData = null, IGraphicBackend backendLegend = null)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("width and height must each be greater than 0");
            settings = new Settings();
            mouseTracker = new MouseTracker(settings);
            if (backendFigure == null)
                settings.figureBackend = new GDIbackend(width, height, pixelFormat);
            else
                settings.figureBackend = backendFigure;
            if (backendData == null)
                settings.dataBackend = new GDIbackend(width, height, pixelFormat);
            else
                settings.dataBackend = backendData;
            if (backendLegend == null)
                settings.legendBackend = new GDIbackend(1, 1, pixelFormat);
            else
                settings.legendBackend = backendLegend;
            Resize(width, height);
            TightenLayout();
        }

        public override string ToString()
        {
            return string.Format($"ScottPlot ({0:n0} x {1:n0}) with {2:n0} objects ({3:n0} points)",
                settings.figureSize.Width, settings.figureSize.Height,
                GetPlottables().Count, GetTotalPoints());
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

        private void InitializeLegend(Size size)
        {
            settings.legendBackend.Resize(size.Width, size.Height);
        }

        private void InitializeBitmaps()
        {
            settings.bmpFigure = null;
            settings.gfxFigure = null;

            if (settings.figureSize.Width > 0 && settings.figureSize.Height > 0)
            {
                settings.figureBackend.Resize(settings.figureSize.Width, settings.figureSize.Height);
                settings.bmpFigure = new Bitmap(settings.figureSize.Width, settings.figureSize.Height, pixelFormat);
                settings.gfxFigure = Graphics.FromImage(settings.bmpFigure);
            }


            settings.dataBackend.Resize(settings.dataSize.Width, settings.dataSize.Height);

            InitializeLegend(new Size(1, 1));
        }

        private void UpdateAntiAliasingSettings()
        {

            if (settings.gfxFigure != null)
            {
                if (settings.misc.antiAliasFigure)
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
            settings.figureBackend.SetAntiAlias(settings.misc.antiAliasFigure);
            settings.dataBackend.SetAntiAlias(settings.misc.antiAliasData);
            settings.legendBackend.SetAntiAlias(settings.legend.antiAlias);
        }

        private void RenderBitmap()
        {
            if (!settings.axes.hasBeenSet && settings.plottables.Count > 0)
                settings.AxisAuto();

            if (!settings.layout.tighteningOccurred)
            {
                // ticks must be populated before the layout can be tightened
                Renderer.FigureTicks(settings);
                TightenLayout();
                // only after the layout is tightened can the ticks be properly decided
            }

            settings.legend.rect = LegendTools.GetLegendFrame(settings);

            // TODO: this only re-renders the legend if the size changes. What if the colors change?
            if (settings.legend.rect.Size != settings.legendBackend.GetSize())
                InitializeLegend(settings.legend.rect.Size);

            UpdateAntiAliasingSettings();

            settings.benchmark.Start();
            if (settings.figureBackend != null)
            {
                // TODO: I removed "settings.bmpFigureRenderRequired" so the frame is currently being redrawn every time
                //settings.figureBackend.SetDrawRect(new Rectangle(new Point(0, 0), settings.figureSize));
                Renderer.FigureClear(settings);
                Renderer.FigureLabels(settings);
                Renderer.FigureTicks(settings);
                Renderer.FigureFrames(settings);
                //settings.figureBackend.ClearDrawRect();
            }
            if (settings.dataBackend != null)
            {
                settings.dataBackend.SetDrawRect(new Rectangle(settings.dataOrigin, settings.dataSize));
                Renderer.DataBackground(settings);
                Renderer.DataGrid(settings);
                Renderer.DataPlottables(settings);
                settings.dataBackend.ClearDrawRect();
                Renderer.PlaceDataOntoFigure(settings);
                Renderer.MouseZoomRectangle(settings);
                Renderer.CreateLegendBitmap(settings);
                Renderer.PlaceLegendOntoFigure(settings);
            }
            settings.benchmark.Stop();
            settings.benchmark.UpdateMessage(settings.plottables.Count, settings.GetTotalPointCount());
            Renderer.Benchmark(settings);
        }

        public Bitmap GetBitmap(bool renderFirst = true, bool lowQuality = false)
        {
            if (lowQuality)
            {
                bool currentAAData = settings.misc.antiAliasData; // save currently using AA setting
                settings.misc.antiAliasData = false; // disable AA for render
                if (renderFirst)
                    RenderBitmap();
                settings.misc.antiAliasData = currentAAData; // restore saved AA setting
            }
            else
            {
                if (renderFirst)
                    RenderBitmap();
            }
            return settings.figureBackend.GetBitmap();
        }

        public void SaveFig(string filePath, bool renderFirst = true)
        {
            if (renderFirst)
                RenderBitmap();

            filePath = System.IO.Path.GetFullPath(filePath);
            string fileFolder = System.IO.Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(fileFolder))
                throw new Exception($"ERROR: folder does not exist: {fileFolder}");

            ImageFormat imageFormat;
            string extension = System.IO.Path.GetExtension(filePath).ToUpper();
            if (extension == ".JPG" || extension == ".JPEG")
                imageFormat = ImageFormat.Jpeg; // TODO: use jpgEncoder to set custom compression level
            else if (extension == ".PNG")
                imageFormat = ImageFormat.Png;
            else if (extension == ".TIF" || extension == ".TIFF")
                imageFormat = ImageFormat.Tiff;
            else if (extension == ".BMP")
                imageFormat = ImageFormat.Bmp;
            else
                throw new NotImplementedException("Extension not supported: " + extension);

            settings.figureBackend.GetBitmap().Save(filePath, imageFormat);
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

        public PlottableSignal PlotSignal(
            double[] ys,
            double sampleRate = 1,
            double xOffset = 0,
            double yOffset = 0,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableSignal signal = new PlottableSignal(
                ys: ys,
                sampleRate: sampleRate,
                xOffset: xOffset,
                yOffset: yOffset,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                useParallel: settings.misc.useParallel
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
            string label = null
            ) where T : struct, IComparable
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
                useParallel: settings.misc.useParallel
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
            if (someValuesAreNull && !settings.axes.hasBeenSet)
                settings.AxisAuto();

            settings.axes.Set(x1, x2, y1, y2);
            return settings.axes.limits;
        }

        public void AxisAuto(
            double horizontalMargin = .05,
            double verticalMargin = .1,
            int tightenPadding = 5,
            bool xExpandOnly = false,
            bool yExpandOnly = false
            )
        {
            settings.AxisAuto(horizontalMargin, verticalMargin, xExpandOnly, yExpandOnly);
            TightenLayout(tightenPadding);
        }

        public void AxisAutoX(
            double margin = .05,
            bool expandOnly = false
            )
        {
            double oldY1 = settings.axes.y.min;
            double oldY2 = settings.axes.y.max;
            AxisAuto(horizontalMargin: margin, xExpandOnly: expandOnly);
            Axis(y1: oldY1, y2: oldY2);
        }

        public void AxisAutoY(
            double margin = .1,
            bool expandOnly = false
            )
        {
            double oldX1 = settings.axes.x.min;
            double oldX2 = settings.axes.x.max;
            AxisAuto(verticalMargin: margin, yExpandOnly: expandOnly);
            Axis(x1: oldX1, x2: oldX2);
        }

        public void AxisZoom(
            double xFrac = 1,
            double yFrac = 1,
            PointF? zoomCenter = null
            )
        {
            if (!settings.axes.hasBeenSet)
                settings.AxisAuto();
            settings.axes.Zoom(xFrac, yFrac, zoomCenter);
        }

        public void AxisPan(double dx = 0, double dy = 0)
        {
            if (!settings.axes.hasBeenSet)
                settings.AxisAuto();
            settings.axes.x.Pan(dx);
            settings.axes.y.Pan(dy);
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
            bool? enable = null,
            string fontName = null,
            float? fontSize = null,
            Color? color = null,
            bool? bold = null
            )
        {

            settings.title.text = title ?? settings.title.text;
            settings.title.visible = enable ?? settings.title.visible;
            settings.title.fontName = fontName ?? settings.title.fontName;
            settings.title.fontSize = fontSize ?? settings.title.fontSize;
            settings.title.color = color ?? settings.title.color;
            settings.title.bold = bold ?? settings.title.bold;

            TightenLayout();
        }

        public void XLabel(
            string xLabel = null,
            Color? color = null,
            bool? enable = null,
            string fontName = null,
            float? fontSize = null,
            bool? bold = null
            )
        {
            settings.xLabel.text = xLabel ?? settings.xLabel.text;
            settings.xLabel.color = color ?? settings.xLabel.color;
            settings.xLabel.visible = enable ?? settings.xLabel.visible;
            settings.xLabel.fontName = fontName ?? settings.xLabel.fontName;
            settings.xLabel.fontSize = fontSize ?? settings.xLabel.fontSize;
            settings.xLabel.bold = bold ?? settings.xLabel.bold;

            TightenLayout();
        }

        public void YLabel(
            string yLabel = null,
            bool? enable = null,
            string fontName = null,
            float? fontSize = null,
            Color? color = null,
            bool? bold = null
            )
        {
            settings.yLabel.text = yLabel ?? settings.yLabel.text;
            settings.yLabel.color = color ?? settings.yLabel.color;
            settings.yLabel.visible = enable ?? settings.yLabel.visible;
            settings.yLabel.fontName = fontName ?? settings.yLabel.fontName;
            settings.yLabel.fontSize = fontSize ?? settings.yLabel.fontSize;
            settings.yLabel.bold = bold ?? settings.yLabel.bold;

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
                settings.legend.colorText = (Color)fontColor;
            if (backColor != null)
                settings.legend.colorBackground = (Color)backColor;
            if (frameColor != null)
                settings.legend.colorFrame = (Color)frameColor;

            fontName = ScottPlot.Tools.VerifyFont(fontName);
            FontStyle fontStyle = (bold) ? FontStyle.Bold : FontStyle.Regular;
            settings.legend.font = new Font(fontName, fontSize, fontStyle);

            if (enableLegend)
            {
                settings.legend.location = location;
                settings.legend.shadow = shadowDirection;
            }
            else
            {
                settings.legend.location = legendLocation.none;
                settings.legend.shadow = shadowDirection.none;
            }
        }

        public Bitmap GetLegendBitmap()
        {
            return settings.legendBackend.GetBitmap();
        }

        #endregion

        #region Styling and Misc Graph Settings

        public void Ticks(
            bool? displayTicksX = null,
            bool? displayTicksY = null,
            bool? displayTicksXminor = null,
            bool? displayTicksYminor = null,
            Color? color = null,
            bool? useMultiplierNotation = null,
            bool? useOffsetNotation = null,
            bool? useExponentialNotation = null,
            bool? dateTimeX = null,
            bool? dateTimeY = null
            )
        {
            if (displayTicksX != null)
                settings.ticks.displayXmajor = (bool)displayTicksX;
            if (displayTicksY != null)
                settings.ticks.displayYmajor = (bool)displayTicksY;
            if (color != null)
                settings.ticks.color = (Color)color;
            if (useMultiplierNotation != null)
                settings.ticks.useMultiplierNotation = (bool)useMultiplierNotation;
            if (useOffsetNotation != null)
                settings.ticks.useOffsetNotation = (bool)useOffsetNotation;
            if (useExponentialNotation != null)
                settings.ticks.useExponentialNotation = (bool)useExponentialNotation;
            if (displayTicksXminor != null)
                settings.ticks.displayXminor = (bool)displayTicksXminor;
            if (displayTicksYminor != null)
                settings.ticks.displayYminor = (bool)displayTicksYminor;
            if (dateTimeX != null)
                settings.ticks.timeFormatX = (bool)dateTimeX;
            if (dateTimeY != null)
                settings.ticks.timeFormatY = (bool)dateTimeY;

            if (dateTimeX != null || dateTimeY != null)
            {
                // why these in this order? voodoo magic
                TightenLayout();
                RenderBitmap();
                TightenLayout();
            }
        }

        public void Grid(
            bool? enable = null,
            Color? color = null,
            double? xSpacing = null,
            double? ySpacing = null
            )
        {
            settings.grid.visible = enable ?? settings.grid.visible;
            settings.grid.color = color ?? settings.grid.color;

            settings.ticks.manualSpacingX = (xSpacing == null) ? 0 : (double)xSpacing;
            settings.ticks.manualSpacingY = (ySpacing == null) ? 0 : (double)ySpacing;
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
                settings.layout.displayAxisFrames = (bool)drawFrame;
            if (frameColor != null)
                settings.ticks.color = (Color)frameColor;
            if (left != null)
                settings.layout.displayFrameByAxis[0] = (bool)left;
            if (right != null)
                settings.layout.displayFrameByAxis[1] = (bool)right;
            if (bottom != null)
                settings.layout.displayFrameByAxis[2] = (bool)bottom;
            if (top != null)
                settings.layout.displayFrameByAxis[3] = (bool)top;
        }

        public void Benchmark(bool show = true, bool toggle = false)
        {
            if (toggle)
                settings.benchmark.visible = !settings.benchmark.visible;
            else
                settings.benchmark.visible = show;
        }

        public void AntiAlias(bool figure = true, bool data = false, bool legend = false)
        {
            settings.misc.antiAliasFigure = figure;
            settings.misc.antiAliasData = data;
            settings.legend.antiAlias = legend;
        }

        public void TightenLayout(int? padding = null)
        {
            if (!settings.axes.hasBeenSet && settings.plottables.Count > 0)
                settings.AxisAuto();
            if (padding != null)
                settings.layout.padOnAllSides = (int)padding;
            settings.ticks?.x?.Recalculate(settings, false);
            settings.ticks?.y?.Recalculate(settings, true);
            settings.layout.Tighten(settings.ticks, settings.title, settings.xLabel, settings.yLabel);
            Resize();
        }

        public void MatchPadding(Plot sourcePlot, bool horizontal = true, bool vertical = true)
        {
            if (horizontal)
            {
                settings.layout.paddingBySide[0] = sourcePlot.settings.layout.paddingBySide[0];
                settings.layout.paddingBySide[1] = sourcePlot.settings.layout.paddingBySide[1];
            }
            if (vertical)
            {
                settings.layout.paddingBySide[2] = sourcePlot.settings.layout.paddingBySide[2];
                settings.layout.paddingBySide[3] = sourcePlot.settings.layout.paddingBySide[3];
            }
            Resize();
        }

        public void MatchAxis(Plot sourcePlot, bool horizontal = true, bool vertical = true)
        {
            if (horizontal)
            {
                settings.axes.x.min = sourcePlot.settings.axes.x.min;
                settings.axes.x.max = sourcePlot.settings.axes.x.max;
            }
            if (vertical)
            {
                settings.axes.y.min = sourcePlot.settings.axes.y.min;
                settings.axes.y.max = sourcePlot.settings.axes.y.max;
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
                settings.misc.figureBackgroundColor = (Color)figBg;
            if (dataBg != null)
                settings.misc.dataBackgroundColor = (Color)dataBg;
            if (grid != null)
                settings.grid.color = (Color)grid;
            if (tick != null)
                settings.ticks.color = (Color)tick;
            if (label != null)
                settings.xLabel.color = (Color)label;
            if (label != null)
                settings.yLabel.color = (Color)label;
            if (title != null)
                settings.title.color = (Color)title;
            if (dataBg != null)
                settings.legend.colorBackground = (Color)dataBg;
            if (tick != null)
                settings.legend.colorFrame = (Color)tick;
            if (label != null)
                settings.legend.colorText = (Color)label;
        }

        public void Style(Style style)
        {
            StyleTools.SetStyle(this, style);
        }

        public void Parallel(bool useParallel)
        {
            // after refactoring the settings module this seems to due to axis/bitmap interactions
            throw new NotImplementedException("parallel processing should not be enabled at this time");
            /*
            settings.useParallel = useParallel;
            foreach (var plottable in GetPlottables())
                plottable.useParallel = useParallel;
            */
        }

        #endregion

    }
}