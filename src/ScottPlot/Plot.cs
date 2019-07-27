using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlot
{
    /// <summary>
    /// The <c>Plot</c> class is used to collect and graph data.
    /// </summary>
    public class Plot
    {

        private readonly Settings settings;
        public readonly MouseTracker mouseTracker;

        /// <summary>
        /// Create a new ScottPlot with the given dimensions
        /// </summary>
        /// <param name="width">image width (pixels)</param>
        /// <param name="height">image height (pixels)</param>
        public Plot(int width = 600, int height = 800)
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
            return $"ScottPlot ({settings.figureSize.Width}x{settings.figureSize.Height}) with {GetPlottables().Count} objects ({GetTotalPoints()} points)";
        }

        #region Bitmaps, Graphics, and Drawing Settings

        /// <summary>
        /// Resize the ScottPlot to the given dimensions
        /// </summary>
        /// <param name="width">image width (pixels)</param>
        /// <param name="height">image height (pixels)</param>
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
                settings.bmpFigure = new Bitmap(settings.figureSize.Width, settings.figureSize.Height);
                settings.gfxFigure = Graphics.FromImage(settings.bmpFigure);
            }

            if (settings.dataSize.Width > 0 && settings.dataSize.Height > 0)
            {
                settings.bmpData = new Bitmap(settings.dataSize.Width, settings.dataSize.Height);
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
            {
                settings.AxisAuto();
                TightenLayout();
            }

            if (!settings.tighteningOccurred)
            {
                TightenLayout();
            }

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

        /// <summary>
        /// Return the ScottPlot as a Bitmap
        /// </summary>
        public Bitmap GetBitmap(bool renderFirst = true)
        {
            if (renderFirst)
                RenderBitmap();
            return settings.bmpFigure;
        }

        /// <summary>
        /// Save the ScottPlot as an image file
        /// </summary>
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

        #region Managing Plottable Data

        /// <summary>
        /// Clear all plot objects
        /// </summary>
        public void Clear(bool axisLines = true, bool scatterPlots = true, bool signalPlots = true, bool text = true, bool bar = true, bool finance = true)
        {
            settings.Clear(axisLines, scatterPlots, signalPlots, text, bar, finance);
        }

        /// <summary>
        /// Plot text at a specific location
        /// </summary>
        public void PlotText(string text, double x, double y, Color? color = null, string fontName = "Arial", double fontSize = 12, bool bold = false, string label = null)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableText txt = new PlottableText(text, x, y, color: (Color)color, fontName: fontName, fontSize: fontSize, bold: bold, label: label);
            settings.plottables.Add(txt);
        }

        /// <summary>
        /// Plot a single point at a specific location
        /// </summary>
        public void PlotPoint(double x, double y, Color? color = null, double markerSize = 5, string label = null, double? errorX = null, double? errorY = null, double errorLineWidth = 1, double errorCapSize = 3, MarkerShape markerShape = MarkerShape.filledCircle)
        {
            if (color == null)
                color = settings.GetNextColor();

            double[] errorXarray = (errorX != null) ? new double[] { (double)errorX } : null;
            double[] errorYarray = (errorY != null) ? new double[] { (double)errorY } : null;

            PlottableScatter mark = new PlottableScatter(new double[] { x }, new double[] { y }, color: (Color)color, lineWidth: 0, markerSize: markerSize, label: label, errorX: errorXarray, errorY: errorYarray, errorLineWidth: errorLineWidth, errorCapSize: errorCapSize, stepDisplay: false, markerShape: markerShape);
            settings.plottables.Add(mark);
        }

        /// <summary>
        /// Plot a scatter plot from X and Y points
        /// </summary>
        public void PlotScatter(double[] xs, double[] ys, Color? color = null, double lineWidth = 1, double markerSize = 5, string label = null, double[] errorX = null, double[] errorY = null, double errorLineWidth = 1, double errorCapSize = 3, MarkerShape markerShape = MarkerShape.filledCircle)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableScatter scat = new PlottableScatter(xs, ys, color: (Color)color, lineWidth: lineWidth, markerSize: markerSize, label: label, errorX: errorX, errorY: errorY, errorLineWidth: errorLineWidth, errorCapSize: errorCapSize, stepDisplay: false, markerShape: markerShape);
            settings.plottables.Add(scat);
        }

        /// <summary>
        /// Create a step plot from X and Y points
        /// </summary>
        public void PlotStep(double[] xs, double[] ys, Color? color = null, double lineWidth = 1, string label = null)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableScatter step = new PlottableScatter(xs, ys, color: (Color)color, lineWidth: lineWidth, markerSize: 0, label: label, errorX: null, errorY: null, errorLineWidth: 0, errorCapSize: 0, stepDisplay: true, markerShape: MarkerShape.none);
            settings.plottables.Add(step);
        }

        /// <summary>
        /// Plot evenly-spaced data (optimized for speed)
        /// </summary>
        public void PlotSignal(double[] ys, double sampleRate = 1, double xOffset = 0, double yOffset = 0, Color? color = null, double lineWidth = 1, double markerSize = 5, string label = null)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableSignal signal = new PlottableSignal(ys, sampleRate, xOffset, yOffset, (Color)color, lineWidth: lineWidth, markerSize: markerSize, label: label);
            settings.plottables.Add(signal);
        }

        /// <summary>
        /// Plot data for a bar graph
        /// </summary>
        public void PlotBar(double[] xs, double[] ys, double? barWidth = null, double xOffset = 0, Color? color = null, string label = null, double[] errorY = null, double errorLineWidth = 1, double errorCapSize = 3)
        {
            if (color == null)
                color = settings.GetNextColor();
            if (barWidth == null)
                barWidth = (xs[1] - xs[0]) * .8;
            PlottableBar bar = new PlottableBar(xs, ys, (double)barWidth, xOffset, (Color)color, label: label, yErr: errorY, errorLineWidth: errorLineWidth, errorCapSize: errorCapSize);
            settings.plottables.Add(bar);
        }

        /// <summary>
        /// Plot open/high/low/close data as a traditional OHLC plot
        /// </summary>
        public void PlotOHLC(OHLC[] ohlcs)
        {
            PlottableOHLC ohlc = new PlottableOHLC(ohlcs, displayCandles: false);
            settings.plottables.Add(ohlc);
        }

        /// <summary>
        /// Plot open/high/low/close data as a candlestick plot
        /// </summary>
        public void PlotCandlestick(OHLC[] ohlcs)
        {
            PlottableOHLC ohlc = new PlottableOHLC(ohlcs, displayCandles: true);
            settings.plottables.Add(ohlc);
        }

        /// <summary>
        /// Plot a vertical line at the given X position
        /// </summary>
        public void PlotVLine(double x, Color? color = null, double lineWidth = 1, string label = null, bool draggable = false, double dragLimitLower = double.NegativeInfinity, double dragLimitUpper = double.PositiveInfinity)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableAxLine axLine = new PlottableAxLine(x, vertical: true, color: (Color)color, lineWidth: lineWidth, label: label, draggable: draggable, dragLimitLower: dragLimitLower, dragLimitUpper: dragLimitUpper);
            settings.plottables.Add(axLine);
        }

        /// <summary>
        /// Plot a horizontal line at the given Y position
        /// </summary>
        public void PlotHLine(double x, Color? color = null, double lineWidth = 1, string label = null, bool draggable = false, double dragLimitLower = double.NegativeInfinity, double dragLimitUpper = double.PositiveInfinity)
        {
            if (color == null)
                color = settings.GetNextColor();
            PlottableAxLine axLine = new PlottableAxLine(x, vertical: false, color: (Color)color, lineWidth: lineWidth, label: label, draggable: draggable, dragLimitLower: dragLimitLower, dragLimitUpper: dragLimitUpper);
            settings.plottables.Add(axLine);
        }

        /// <summary>
        /// Get a list of all plotted objects (useful for modifying their properties)
        /// </summary>
        public List<Plottable> GetPlottables()
        {
            return settings.plottables;
        }

        /// <summary>
        /// Get the total number of data points across all plottable objects
        /// </summary>
        public int GetTotalPoints()
        {
            int totalPoints = 0;
            foreach (Plottable plottable in settings.plottables)
                totalPoints += plottable.pointCount;
            return totalPoints;
        }

        #endregion

        #region Axis Settings

        /// <summary>
        /// Manually set axis limits
        /// </summary>
        /// <returns>axis limits [x1, x2, y1, y2]</returns>
        public double[] Axis(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
        {
            bool someValuesAreNull = (x1 == null) || (x2 == null) || (y1 == null) || (y2 == null);
            if (someValuesAreNull && !settings.axisHasBeenIntentionallySet)
                settings.AxisAuto();

            settings.AxisSet(x1, x2, y1, y2);
            return settings.axis;
        }

        /// <summary>
        /// Automatically adjust axis limits to fit the data
        /// </summary>
        /// <param name="horizontalMargin">fraction to pad data horizontally</param>
        /// <param name="verticalMargin">fraction to pad data vertically</param>
        public void AxisAuto(double horizontalMargin = .05, double verticalMargin = .1, int tightenPadding = 5)
        {
            settings.AxisAuto(horizontalMargin, verticalMargin);
            TightenLayout(tightenPadding);
        }

        /// <summary>
        /// Zoom in or out by a given fraction (greater than 1 zooms in)
        /// </summary>
        public void AxisZoom(double xFrac = 1, double yFrac = 1, PointF? zoomCenter = null)
        {
            settings.AxisZoom(xFrac, yFrac, zoomCenter);
        }

        /// <summary>
        /// Pan by a given amount (in X/Y units)
        /// </summary>
        public void AxisPan(double dx = 0, double dy = 0)
        {
            settings.AxisPan(dx, dy);
        }

        /// <summary>
        /// Return the axis coordinates of a pixel location (relative to the ScottPlot)
        /// </summary>
        public PointF CoordinateFromPixel(int pixelX, int pixelY)
        {
            return settings.GetLocation(pixelX, pixelY);
        }

        /// <summary>
        /// Return the axis coordinates of a pixel location (relative to the ScottPlot)
        /// </summary>
        public PointF CoordinateFromPixel(Point pixel)
        {
            return CoordinateFromPixel(pixel.X, pixel.Y);
        }

        /// <summary>
        /// Return the pixel position for the given coordinate on the axis
        /// </summary>
        public PointF CoordinateToPixel(double locationX, double locationY)
        {
            PointF pixelLocation = settings.GetPixel(locationX, locationY);
            pixelLocation.X += settings.dataOrigin.X;
            pixelLocation.Y += settings.dataOrigin.Y;
            return pixelLocation;
        }

        /// <summary>
        /// Return the pixel position for the given coordinate on the axis
        /// </summary>
        public PointF CoordinateToPixel(PointF location)
        {
            return CoordinateToPixel(location.X, location.Y);
        }

        #endregion

        #region Labels

        /// <summary>
        /// Set the title of the ScottPlot
        /// </summary>
        public void Title(string title = null, Color? color = null, bool? enable = true, float? fontSize = 12)
        {
            if (title != null)
                settings.title = title;
            if (color != null)
                settings.titleColor = (Color)color;
            if (enable != null)
                if (enable == false)
                    settings.title = "";
            if (fontSize != null)
                settings.titleFont = new Font(settings.titleFont.FontFamily, (float)fontSize, settings.titleFont.Style);
            settings.bmpFigureRenderRequired = true;
            TightenLayout();
        }

        /// <summary>
        /// Set the horizontal axis label
        /// </summary>
        public void XLabel(string xLabel = null, Color? color = null, bool? enable = true, float? fontSize = 12)
        {
            if (xLabel != null)
                settings.axisLabelX = xLabel;
            if (enable == false)
                settings.axisLabelX = "";
            if (color != null)
                settings.axisLabelColor = (Color)color;
            if (fontSize != null)
                settings.axisLabelFont = new Font(settings.axisLabelFont.FontFamily, (float)fontSize, settings.axisLabelFont.Style);
            settings.bmpFigureRenderRequired = true;
            TightenLayout();
        }

        /// <summary>
        /// Set the vertical axis label
        /// </summary>
        public void YLabel(string yLabel = null, Color? color = null, bool? enable = true, float? fontSize = 12)
        {
            if (yLabel != null)
                settings.axisLabelY = yLabel;
            if (enable == false)
                settings.axisLabelY = "";
            if (color != null)
                settings.axisLabelColor = (Color)color;
            if (fontSize != null)
                settings.axisLabelFont = new Font(settings.axisLabelFont.FontFamily, (float)fontSize, settings.axisLabelFont.Style);
            settings.bmpFigureRenderRequired = true;
            TightenLayout();
        }

        /// <summary>
        /// Add a legend made from the labels given to plot objects
        /// </summary>
        public void Legend(bool enableLegend = true, Color? fontColor = null, Color? backColor = null, Color? frameColor = null, legendLocation location = legendLocation.lowerRight, shadowDirection shadowDirection = shadowDirection.lowerRight)
        {
            if (fontColor != null)
                settings.legendFontColor = (Color)fontColor;
            if (backColor != null)
                settings.legendBackColor = (Color)backColor;
            if (frameColor != null)
                settings.legendFrameColor = (Color)frameColor;

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

        /// <summary>
        /// Control axis tick visibility and styling
        /// </summary>
        public void Ticks(bool? displayTicksX = true, bool? displayTicksY = true, Color? color = null)
        {
            if (displayTicksX != null)
                settings.displayTicksX = (bool)displayTicksX;
            if (displayTicksY != null)
                settings.displayTicksY = (bool)displayTicksY;
            if (color != null)
                settings.tickColor = (Color)color;
            settings.bmpFigureRenderRequired = true;
        }

        /// <summary>
        /// Control grid visibility and color
        /// </summary>
        public void Grid(bool? enable = true, Color? color = null, double? xSpacing = null, double? ySpacing = null)
        {
            if (enable != null)
                settings.displayGrid = (bool)enable;
            if (color != null)
                settings.gridColor = (Color)color;
            settings.gridSpacingX = (xSpacing == null) ? 0 : (double)xSpacing;
            settings.gridSpacingY = (ySpacing == null) ? 0 : (double)ySpacing;
            settings.bmpFigureRenderRequired = true;
        }

        /// <summary>
        /// Control visibility and style of the data area frame
        /// </summary>
        public void Frame(bool? drawFrame = true, Color? frameColor = null, bool? left = true, bool? right = true, bool? bottom = true, bool? top = true)
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

        /// <summary>
        /// Control visibility of benchmark information on the data area
        /// </summary>
        public void Benchmark(bool show = true, bool toggle = false)
        {
            if (toggle)
                settings.displayBenchmark = (settings.displayBenchmark) ? false : true;
            else
                settings.displayBenchmark = show;
        }

        /// <summary>
        /// Set anti-aliasing of the figure and the data
        /// </summary>
        public void AntiAlias(bool figure = true, bool data = false)
        {
            settings.antiAliasFigure = figure;
            settings.antiAliasData = data;
            settings.bmpFigureRenderRequired = true;
        }

        /// <summary>
        /// Reduce space between data and labels (title, ticks, axis legends, etc)
        /// </summary>
        public void TightenLayout(int padding = 5)
        {
            if (!settings.axisHasBeenIntentionallySet && settings.plottables.Count > 0)
                settings.AxisAuto();
            settings.axisPadding = padding;
            settings.AxisTighen();
            Resize();
        }

        /// <summary>
        /// Use frame padding from an existing ScottPlot
        /// </summary>
        public void MatchPadding(ScottPlot.Plot sourcePlot, bool horizontal = true, bool vertical = true)
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

        /// <summary>
        /// Use axis limits from an existing ScottPlot
        /// </summary>
        public void MatchAxis(ScottPlot.Plot sourcePlot, bool horizontal = true, bool vertical = true)
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

        /// <summary>
        /// Set colors of many of the common elements with named elements
        /// </summary>
        public void Style(Color? figBg = null, Color? dataBg = null, Color? grid = null, Color? tick = null, Color? label = null, Color? title = null)
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
                settings.axisLabelColor = (Color)label;
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

        /// <summary>
        /// Set the style using saved color combinations
        /// </summary>
        public void Style(Style style)
        {
            switch (style)
            {
                case (ScottPlot.Style.Black):
                    Style(figBg: Color.Black,
                        dataBg: Color.Black,
                        grid: ColorTranslator.FromHtml("#2d2d2d"),
                        tick: ColorTranslator.FromHtml("#757575"),
                        label: ColorTranslator.FromHtml("#b9b9ba"),
                        title: ColorTranslator.FromHtml("#FFFFFF"));
                    break;
                case (ScottPlot.Style.Blue1):
                    Style(figBg: ColorTranslator.FromHtml("#07263b"),
                        dataBg: ColorTranslator.FromHtml("#0b3049"),
                        grid: ColorTranslator.FromHtml("#0e3d54"),
                        tick: ColorTranslator.FromHtml("#145665"),
                        label: ColorTranslator.FromHtml("#b5bec5"),
                        title: ColorTranslator.FromHtml("#d0dae2"));
                    break;
                case (ScottPlot.Style.Blue2):
                    Style(figBg: ColorTranslator.FromHtml("#1b2138"),
                        dataBg: ColorTranslator.FromHtml("#252c48"),
                        grid: ColorTranslator.FromHtml("#2c334e"),
                        tick: ColorTranslator.FromHtml("#bbbdc4"),
                        label: ColorTranslator.FromHtml("#bbbdc4"),
                        title: ColorTranslator.FromHtml("#d8dbe3"));
                    break;
                case (ScottPlot.Style.Blue3):
                    Style(figBg: ColorTranslator.FromHtml("#001021"),
                        dataBg: ColorTranslator.FromHtml("#021d38"),
                        grid: ColorTranslator.FromHtml("#273c51"),
                        tick: ColorTranslator.FromHtml("#d3d3d3"),
                        label: ColorTranslator.FromHtml("#d3d3d3"),
                        title: ColorTranslator.FromHtml("#FFFFFF"));
                    break;
                case (ScottPlot.Style.Gray1):
                    Style(figBg: ColorTranslator.FromHtml("#31363a"),
                        dataBg: ColorTranslator.FromHtml("#3a4149"),
                        grid: ColorTranslator.FromHtml("#444b52"),
                        tick: ColorTranslator.FromHtml("#757a80"),
                        label: ColorTranslator.FromHtml("#d6d7d8"),
                        title: ColorTranslator.FromHtml("#FFFFFF"));
                    break;
                case (ScottPlot.Style.Gray2):
                    Style(figBg: ColorTranslator.FromHtml("#131519"),
                        dataBg: ColorTranslator.FromHtml("#262626"),
                        grid: ColorTranslator.FromHtml("#2d2d2d"),
                        tick: ColorTranslator.FromHtml("#757575"),
                        label: ColorTranslator.FromHtml("#b9b9ba"),
                        title: ColorTranslator.FromHtml("#FFFFFF"));
                    break;
                case (ScottPlot.Style.Light1):
                    Style(figBg: ColorTranslator.FromHtml("#FFFFFF"),
                        dataBg: ColorTranslator.FromHtml("#FFFFFF"),
                        grid: ColorTranslator.FromHtml("#ededed "),
                        tick: ColorTranslator.FromHtml("#7f7f7f"),
                        label: ColorTranslator.FromHtml("#000000"),
                        title: ColorTranslator.FromHtml("#000000"));
                    break;
                case (ScottPlot.Style.Light2):
                    Style(figBg: ColorTranslator.FromHtml("#e4e6ec"),
                        dataBg: ColorTranslator.FromHtml("#f1f3f7"),
                        grid: ColorTranslator.FromHtml("#e5e7ea"),
                        tick: ColorTranslator.FromHtml("#77787b"),
                        label: ColorTranslator.FromHtml("#000000"),
                        title: ColorTranslator.FromHtml("#000000"));
                    break;
                case (ScottPlot.Style.Control):
                    Style(figBg: SystemColors.Control,
                        dataBg: Color.White,
                        grid: Color.LightGray,
                        tick: Color.Black,
                        label: Color.Black,
                        title: Color.Black);
                    break;
                default:
                    Style(figBg: Color.White,
                        dataBg: Color.White,
                        grid: Color.LightGray,
                        tick: Color.Black,
                        label: Color.Black,
                        title: Color.Black);
                    break;
            }
        }

        #endregion

    }
}