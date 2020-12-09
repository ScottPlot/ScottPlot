/* The Plot class is the primary public interface for ScottPlot.
 * State (plottables and configuration) is stored in the settings object 
 * which is private so it can be refactored without breaking the API.
 * Helper methods plottable creation and styling are in partial classes.
 */

using ScottPlot.Plottable;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace ScottPlot
{
    public partial class Plot
    {
        // The settings object stores all state for a plot
        private readonly Settings settings = new Settings();

        /// <summary>
        /// A ScottPlot stores data in plottable objects and draws it on a bitmap when Render() is called
        /// </summary>
        public Plot(int width = 800, int height = 600)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("width and height must each be greater than 0");

            StyleTools.SetStyle(this, ScottPlot.Style.Default);
            Resize(width, height);
        }

        public override string ToString() =>
            $"ScottPlot ({settings.Width}x{settings.Height}) " +
            $"with {settings.Plottables.Count:n0} plottables";

        /// <summary>
        /// ScottPlot version in the format "1.2.3" ("1.2.3-beta" for pre-releases)
        /// </summary>
        public static string Version
        {
            get
            {
                Version v = typeof(Plot).Assembly.GetName().Version;
                string versionString = $"{v.Major}.{v.Minor}.{v.Build}-beta";
                return versionString;
            }
        }

        /// <summary>
        /// Return a new Plot with all the same Plottables (and some of the styles) of this one.
        /// </summary>
        public Plot Copy()
        {
            // This is typically only called when you right-click a plot in a control and hit "open in new window".
            // All state from the old plot must be copied to the new plot.

            Plot plt2 = new Plot(settings.Width, settings.Height);
            var settings2 = plt2.GetSettings(false);

            // Copying state of plottables is easy because they contain their own state.
            settings2.Plottables.AddRange(settings.Plottables);

            // TODO: copy axes, since they now carry their own state too.
            plt2.Title(settings.XAxis2.Title.Label);
            plt2.XLabel(settings.XAxis.Title.Label);
            plt2.YLabel(settings.YAxis.Title.Label);

            plt2.AxisAuto();
            return plt2;
        }

        #region plottable management

        /// <summary>
        /// Add a plottable to the plot
        /// </summary>
        public void Add(IPlottable plottable) => settings.Plottables.Add(plottable);

        /// <summary>
        /// Return a copy of the list of plottables
        /// </summary>
        /// <returns></returns>
        public IPlottable[] GetPlottables() => settings.Plottables.ToArray();

        /// <summary>
        /// Return a copy of the list of draggable plottables
        /// </summary>
        /// <returns></returns>
        public IDraggable[] GetDraggables() => settings.Plottables.Where(x => x is IDraggable).Select(x => (IDraggable)x).ToArray();

        /// <summary>
        /// Return the draggable plottable under the mouse cursor (or null if there isn't one)
        /// </summary>
        public IDraggable GetDraggableUnderMouse(double pixelX, double pixelY, int snapDistancePixels = 5)
        {
            double snapWidth = GetSettings(false).XAxis.Dims.UnitsPerPx * snapDistancePixels;
            double snapHeight = GetSettings(false).YAxis.Dims.UnitsPerPx * snapDistancePixels;

            foreach (IDraggable draggable in GetDraggables())
                if (draggable.IsUnderMouse(GetCoordinateX((float)pixelX), GetCoordinateY((float)pixelY), snapWidth, snapHeight))
                    if (draggable.DragEnabled)
                        return draggable;

            return null;
        }

        /// <summary>
        /// Throw an exception if any plottable contains an invalid state. Deep validation is more thorough but slower.
        /// </summary>
        public void Validate(bool deep = true)
        {
            foreach (var plottable in settings.Plottables)
                plottable.ValidateData(deep);
        }

        #endregion

        #region plot settings and styling

        /// <summary>
        /// Return a new color from the Pallette based on the number of plottables already in the plot.
        /// Use this to ensure every plottable gets a unique color.
        /// </summary>
        public Color GetNextColor(double alpha = 1) => Color.FromArgb((byte)(alpha * 255), settings.GetNextColor());

        /// <summary>
        /// Get access to the plot settings module (not exposed by default because its internal API changes frequently)
        /// </summary>
        public Settings GetSettings(bool showWarning = true)
        {
            if (showWarning)
                Debug.WriteLine("WARNING: GetSettings() is only for development and testing. " +
                                "Be aware its class structure changes frequently!");
            return settings;
        }

        /// <summary>
        /// Set the default size for new renders
        /// </summary>
        public void Resize(float width, float height) => settings.Resize(width, height);

        /// <summary>
        /// Display render benchmark information on the plot
        /// </summary>
        public void Benchmark(bool enable = true) => settings.BenchmarkMessage.IsVisible = enable;
        public void BenchmarkToggle() => settings.BenchmarkMessage.IsVisible = !settings.BenchmarkMessage.IsVisible;


        /// <summary>
        /// Change the default color palette for new plottables
        /// </summary>
        public void Colorset(Drawing.Palette colorset) => settings.PlottablePalette = colorset;

        /// <summary>
        /// Set colors of all plot components using pre-defined styles
        /// </summary>
        public void Style(Style style) => StyleTools.SetStyle(this, style);

        /// <summary>
        /// Set the color of specific plot components
        /// </summary>
        public void Style(
            System.Drawing.Color? figureBackgroundColor = null,
            System.Drawing.Color? dataBackgroundColor = null,
            System.Drawing.Color? gridColor = null,
            System.Drawing.Color? tickColor = null,
            System.Drawing.Color? axisLabelColor = null,
            System.Drawing.Color? titleLabelColor = null)
        {
            settings.FigureBackground.Color = figureBackgroundColor ?? settings.FigureBackground.Color;
            settings.DataBackground.Color = dataBackgroundColor ?? settings.DataBackground.Color;

            foreach (var axis in settings.Axes)
            {
                axis.Title.Font.Color = axisLabelColor ?? axis.Title.Font.Color;
                axis.Ticks.MajorLabelFont.Color = tickColor ?? axis.Ticks.MajorLabelFont.Color;
                axis.Ticks.MajorGridColor = gridColor ?? axis.Ticks.MajorGridColor;
                axis.Ticks.MinorGridColor = gridColor ?? axis.Ticks.MinorGridColor;
                axis.Ticks.Color = tickColor ?? axis.Ticks.Color;
                axis.Line.Color = tickColor ?? axis.Line.Color;
            }

            settings.XAxis2.Title.Font.Color = titleLabelColor ?? settings.XAxis2.Title.Font.Color;
        }

        /// <summary>
        /// Customize the default culture defining the display of numbers and dates in tick labels
        /// </summary>
        public void SetCulture(System.Globalization.CultureInfo culture)
        {
            foreach (var axis in settings.Axes)
                axis.Ticks.TickCollection.Culture = culture;
        }

        /// <summary>
        /// Customize the default culture defining the display of numbers and dates in tick labels
        /// </summary>
        /// <param name="shortDatePattern">
        /// https://docs.microsoft.com/dotnet/standard/base-types/custom-date-and-time-format-strings
        /// </param>
        /// <param name="decimalSeparator">
        /// Separates the decimal digits.
        /// </param>
        /// <param name="numberGroupSeparator">
        /// Separates large numbers ito groups of digits for readability.
        /// </param>
        /// <param name="decimalDigits">
        /// Number of digits after the numberDecimalSeparator.
        /// </param>
        /// <param name="numberNegativePattern">
        /// https://docs.microsoft.com/dotnet/api/system.globalization.numberformatinfo.numbernegativepattern
        /// </param>
        /// <param name="numberGroupSizes">
        /// Sizes of decimal groups which are separated by the numberGroupSeparator.
        /// https://docs.microsoft.com/dotnet/api/system.globalization.numberformatinfo.numbergroupsizes
        /// </param>
        public void SetCulture(string shortDatePattern = null, string decimalSeparator = null, string numberGroupSeparator = null,
            int? decimalDigits = null, int? numberNegativePattern = null, int[] numberGroupSizes = null)
        {
            foreach (var axis in settings.Axes)
                axis.Ticks.TickCollection.SetCulture(
                        shortDatePattern, decimalSeparator, numberGroupSeparator,
                        decimalDigits, numberNegativePattern, numberGroupSizes);
        }

        /// <summary>
        /// Customize legend visibility and styling
        /// </summary>
        public void Legend(
            bool enableLegend = true,
            string fontName = null,
            float? fontSize = null,
            bool? bold = null,
            System.Drawing.Color? fontColor = null,
            System.Drawing.Color? backColor = null,
            System.Drawing.Color? frameColor = null,
            Alignment location = Alignment.LowerRight,
            Alignment shadowDirection = Alignment.LowerRight,
            bool? fixedLineWidth = null,
            bool? reverseOrder = null
            )
        {
            settings.CornerLegend.IsVisible = enableLegend;
            settings.CornerLegend.FontName = fontName ?? settings.CornerLegend.Font.Name;
            settings.CornerLegend.FontSize = fontSize ?? settings.CornerLegend.Font.Size;
            settings.CornerLegend.FontColor = fontColor ?? settings.CornerLegend.Font.Color;
            settings.CornerLegend.FillColor = backColor ?? settings.CornerLegend.FillColor;
            settings.CornerLegend.OutlineColor = frameColor ?? settings.CornerLegend.OutlineColor;
            settings.CornerLegend.ReverseOrder = reverseOrder ?? settings.CornerLegend.ReverseOrder;
            settings.CornerLegend.FontBold = bold ?? settings.CornerLegend.Font.Bold;
            settings.CornerLegend.FixedLineWidth = fixedLineWidth ?? settings.CornerLegend.FixedLineWidth;
            settings.CornerLegend.Location = location;

            // TODO: support shadowDirection
        }

        #endregion

        #region Plottable Creation Helper Methods

        /* These methods allower users to create plottables and add them to the plot with a single line.
         * - Only the most common plottables have helper methods
         * - Only the most common styling options are configurable with optional arguments
         * - Methods return the plottables they create, so the user can further customize them if desired
         */

        /// <summary>
        /// Display text in the data area at a pixel location (not a X/Y coordinates)
        /// </summary>
        public Annotation AddAnnotation(string label, double x, double y, float size = 12, Color? color = null, Color? backColor = null)
        {
            var plottable = new Annotation()
            {
                label = label,
                xPixel = x,
                yPixel = y,
                FontSize = size
            };
            plottable.Font.Color = color ?? plottable.Font.Color;
            plottable.BackgroundColor = backColor ?? plottable.BackgroundColor;
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Display an arrow pointing to a spot in coordinate space
        /// </summary>
        public ScatterPlot AddArrow(double xTip, double yTip, double xBase, double yBase, float lineWidth = 5, Color? color = null)
        {
            double[] xs = { xBase, xTip };
            double[] ys = { yBase, yTip };
            var plottable = new ScatterPlot(xs, ys)
            {
                lineWidth = lineWidth,
                markerSize = 0,
                color = color ?? GetNextColor(),
                ArrowheadLength = 3,
                ArrowheadWidth = 3
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a bar plot for the given values. Bars will be placed at X positions 0, 1, 2, etc.
        /// </summary>
        public BarPlot AddBar(double[] values, Color? color = null)
        {
            double[] xs = DataGen.Consecutive(values.Length);
            var plottable = new BarPlot(xs, values, null, null)
            {
                fillColor = color ?? GetNextColor()
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a bar plot for the given values using defined bar positions
        /// </summary>
        public BarPlot AddBar(double[] values, double[] positions, Color? color = null)
        {
            var plottable = new BarPlot(positions, values, null, null)
            {
                fillColor = color ?? GetNextColor()
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a bar plot (values +/- errors) using defined positions
        /// </summary>
        public BarPlot AddBar(double[] values, double[] errors, double[] positions, Color? color = null)
        {
            var plottable = new BarPlot(positions, values, errors, null)
            {
                fillColor = color ?? GetNextColor()
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Create a series of bar plots and customize the ticks and legend
        /// </summary>
        public BarPlot[] AddBarGroups(string[] groupLabels, string[] seriesLabels, double[][] ys, double[][] yErr)
        {
            if (groupLabels is null || seriesLabels is null || ys is null)
                throw new ArgumentException("labels and ys cannot be null");

            if (seriesLabels.Length != ys.Length)
                throw new ArgumentException("groupLabels and ys must be the same length");

            foreach (double[] subArray in ys)
                if (subArray.Length != groupLabels.Length)
                    throw new ArgumentException("all arrays inside ys must be the same length as groupLabels");

            double groupWidthFraction = 0.8;
            double barWidthFraction = 0.8;
            double errorCapSize = 0.38;

            int seriesCount = ys.Length;
            double barWidth = groupWidthFraction / seriesCount;
            BarPlot[] bars = new BarPlot[seriesCount];
            bool containsNegativeY = false;
            for (int i = 0; i < seriesCount; i++)
            {
                double[] barYs = ys[i];
                double[] barYerr = yErr?[i];
                double[] barXs = DataGen.Consecutive(barYs.Length);
                containsNegativeY |= barYs.Where(y => y < 0).Any();
                var bar = new BarPlot(barXs, barYs, barYerr, null)
                {
                    label = seriesLabels[i],
                    barWidth = barWidth * barWidthFraction,
                    xOffset = i * barWidth,
                    errorCapSize = errorCapSize,
                    fillColor = GetNextColor()
                };
                Add(bar);
            }

            if (containsNegativeY)
                AxisAuto();

            double[] groupPositions = DataGen.Consecutive(groupLabels.Length, offset: (groupWidthFraction - barWidth) / 2);
            XTicks(groupPositions, groupLabels);

            return bars;
        }

        /// <summary>
        /// Add a line (a scatter plot with two points) to the plot
        /// </summary>
        public ScatterPlot AddLine(
            double x1,
            double y1,
            double x2,
            double y2,
            Color? color = null,
            float lineWidth = 1,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            return AddScatter(
                xs: new double[] { x1, x2 },
                ys: new double[] { y1, y2 },
                color: color,
                lineWidth: lineWidth,
                lineStyle: lineStyle,
                markerSize: 0);
        }

        /// <summary>
        /// Add candlesticks to the chart from OHLC (open, high, low, close) data
        /// </summary>
        public FinancePlot AddCandlesticks(OHLC[] ohlcs)
        {
            FinancePlot plottable = new FinancePlot()
            {
                ohlcs = ohlcs,
                Candle = true,
                ColorUp = ColorTranslator.FromHtml("#26a69a"),
                ColorDown = ColorTranslator.FromHtml("#ef5350"),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Create a polygon to fill the area between Y values and a baseline.
        /// </summary>
        public Polygon AddFill(double[] xs, double[] ys, double baseline = 0, Color? color = null)
        {
            var plottable = new Polygon(
                xs: Tools.Pad(xs, cloneEdges: true),
                ys: Tools.Pad(ys, 1, baseline, baseline))
            {
                fill = true,
                fillColor = color ?? GetNextColor(.5),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Create a polygon to fill the area between Y values of two curves.
        /// </summary>
        public Polygon AddFill(double[] xs1, double[] ys1, double[] xs2, double[] ys2, Color? color = null)
        {
            // combine xs and ys to make one big curve
            int pointCount = xs1.Length + xs2.Length;
            double[] bothX = new double[pointCount];
            double[] bothY = new double[pointCount];

            // copy the first dataset as-is
            Array.Copy(xs1, 0, bothX, 0, xs1.Length);
            Array.Copy(ys1, 0, bothY, 0, ys1.Length);

            // copy the second dataset in reverse order
            for (int i = 0; i < xs2.Length; i++)
            {
                bothX[xs1.Length + i] = xs2[xs2.Length - 1 - i];
                bothY[ys1.Length + i] = ys2[ys2.Length - 1 - i];
            }

            var plottable = new Polygon(bothX, bothY)
            {
                fill = true,
                fillColor = color ?? GetNextColor(.5),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Create a polygon to fill the area between Y values and a baseline
        /// that uses two different colors for area above and area below the baseline.
        /// </summary>
        public (Polygon polyAbove, Polygon polyBelow) AddFillAboveAndBelow(double[] xs, double[] ys, double baseline = 0, Color? colorAbove = null, Color? colorBelow = null)
        {
            // TODO: this almost works perfectly, but not quite.
            // look at scatter plots with low numbers of points
            // that cross the baseline a lot. The same X value appears
            // to have filled area both above and below the curve.

            double[] xs2 = Tools.Pad(xs, cloneEdges: true);
            double[] ys2 = Tools.Pad(ys, padWithLeft: baseline, padWithRight: baseline);
            double[] ys2below = new double[ys2.Length];
            double[] ys2above = new double[ys2.Length];

            for (int i = 0; i < ys2.Length; i++)
            {
                if (ys2[i] < baseline)
                {
                    ys2below[i] = ys2[i];
                    ys2above[i] = baseline;
                }
                else
                {
                    ys2above[i] = ys2[i];
                    ys2below[i] = baseline;
                }
            }

            Polygon polyAbove = new Polygon(xs2, ys2above);
            Polygon polyBelow = new Polygon(xs2, ys2below);

            polyAbove.fillColor = colorAbove ?? Color.Green;
            polyBelow.fillColor = colorBelow ?? Color.Red;

            Add(polyAbove);
            Add(polyBelow);

            return (polyAbove, polyBelow);
        }

        /// <summary>
        /// Add a line plot that uses a function (rather than X/Y points) to place the curve
        /// </summary>
        public FunctionPlot AddFunction(Func<double, double?> function, Color? color = null, double lineWidth = 1, LineStyle lineStyle = LineStyle.Solid)
        {
            FunctionPlot plottable = new FunctionPlot(function)
            {
                color = color ?? settings.GetNextColor(),
                lineWidth = lineWidth,
                lineStyle = lineStyle
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a horizontal axis line at a specific Y position
        /// </summary>
        public HLine AddHorizontalLine(double y, Color? color = null, float width = 1, LineStyle style = LineStyle.Solid)
        {
            HLine plottable = new HLine()
            {
                position = y,
                color = color ?? settings.GetNextColor(),
                lineWidth = width,
                lineStyle = style
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a horizontal span (shades the region between two X positions)
        /// </summary>
        public HSpan AddHorizontalSpan(double xMin, double xMax, Color? color = null)
        {
            var plottable = new HSpan()
            {
                position1 = xMin,
                position2 = xMax,
                color = color ?? GetNextColor(.5),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a straight line to the plot (really just a scatter plot with 2 points)
        /// </summary>
        public ScatterPlot AddLine(double x1, double y1, double x2, double y2, Color? color = null, float linewidth = 1)
        {
            double[] xs = { x1, x2 };
            double[] ys = { y1, y2 };
            var plottable = new ScatterPlot(xs, ys)
            {
                color = color ?? GetNextColor(),
                lineWidth = linewidth
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add OHLC (open, high, low, close) data to the plot
        /// </summary>
        public FinancePlot AddOHLCs(OHLC[] ohlcs)
        {
            FinancePlot plottable = new FinancePlot()
            {
                ohlcs = ohlcs,
                Candle = false,
                ColorUp = ColorTranslator.FromHtml("#26a69a"),
                ColorDown = ColorTranslator.FromHtml("#ef5350"),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a pie chart to the plot
        /// </summary>
        public PiePlot AddPie(double[] values, bool hideGridAndFrame = true)
        {
            Color[] colors = Enumerable.Range(0, values.Length)
                                       .Select(i => settings.PlottablePalette.GetColor(i))
                                       .ToArray();

            PiePlot pie = new PiePlot(values, null, colors);
            Add(pie);

            if (hideGridAndFrame)
            {
                XAxis.Grid = false;
                YAxis.Grid = false;
                LayoutFrameless();
            }

            return pie;
        }

        /// <summary>
        /// Add a point (a scatter plot with a single marker)
        /// </summary>
        public ScatterPlot AddPoint(double x, double y, Color? color = null, float size = 5, MarkerShape shape = MarkerShape.filledCircle)
        {
            var plottable = new ScatterPlot(new double[] { x }, new double[] { y })
            {
                color = color ?? settings.GetNextColor(),
                markerSize = size,
                markerShape = shape
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a polygon to the plot
        /// </summary>
        public Polygon AddPolygon(double[] xs, double[] ys, Color? fillColor = null, double lineWidth = 0, Color? lineColor = null)
        {
            var plottable = new Polygon(xs, ys)
            {
                lineWidth = lineWidth,
                lineColor = lineColor ?? Color.Black,
                fillColor = fillColor ?? settings.GetNextColor(),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add many polygons using an optimized rendering method
        /// </summary>
        public Polygons AddPolygons(List<List<(double x, double y)>> polys, Color? fillColor = null, double lineWidth = 0, Color? lineColor = null)
        {
            var plottable = new Polygons(polys)
            {
                lineWidth = lineWidth,
                lineColor = lineColor ?? Color.Black,
                fillColor = fillColor ?? settings.GetNextColor(),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a population to the plot
        /// </summary>
        public PopulationPlot AddPopulation(Population population, string label = null)
        {
            var plottable = new PopulationPlot(population, label, settings.GetNextColor());
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add multiple populations to the plot as a single series
        /// </summary>
        public PopulationPlot AddPopulations(Population[] populations, string label = null)
        {
            var plottable = new PopulationPlot(populations, label, settings.GetNextColor());
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add multiple populations to the plot as a single series
        /// </summary>
        public PopulationPlot AddPopulations(PopulationMultiSeries multiSeries)
        {
            for (int i = 0; i < multiSeries.multiSeries.Length; i++)
                multiSeries.multiSeries[i].color = settings.PlottablePalette.GetColor(i);

            var plottable = new PopulationPlot(multiSeries);
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a radar plot
        /// </summary>
        public RadarPlot AddRadar(double[,] values, bool independentAxes = false, double[] maxValues = null, bool disableFrameAndGrid = true)
        {

            Color[] colors = Enumerable.Range(0, values.Length)
                                       .Select(i => settings.PlottablePalette.GetColor(i))
                                       .ToArray();

            Color[] fills = colors.Select(x => Color.FromArgb(50, x)).ToArray();

            RadarPlot plottable = new RadarPlot(values, colors, fills, independentAxes, maxValues);
            Add(plottable);

            if (disableFrameAndGrid)
            {
                LayoutFrameless();
                DisableGrid();
            }

            return plottable;
        }

        /// <summary>
        /// Add an L-shaped scalebar to the corner of the plot
        /// </summary>
        public ScaleBar AddScaleBar(double width, double height, string xLabel = null, string yLabel = null)
        {
            var scalebar = new ScaleBar()
            {
                Width = width,
                Height = height,
                HorizontalLabel = xLabel,
                VerticalLabel = yLabel,
            };
            Add(scalebar);
            return scalebar;
        }

        /// <summary>
        /// Add a scatter plot from X/Y pairs. 
        /// Scatter plots are slower than Signal plots.
        /// </summary>
        public ScatterPlot AddScatter(
            double[] xs,
            double[] ys,
            Color? color = null,
            float lineWidth = 1,
            float markerSize = 5,
            string label = null,
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid)
        {
            var plottable = new ScatterPlot(xs, ys, null, null)
            {
                color = color ?? GetNextColor(),
                lineWidth = lineWidth,
                markerSize = markerSize,
                label = label,
                markerShape = markerShape,
                lineStyle = lineStyle
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Scatter plot with Add() and Clear() methods for updating data
        /// </summary>
        public ScatterPlotList AddScatterList(
            Color? color = null,
            float lineWidth = 1,
            float markerSize = 5,
            string label = null,
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid)
        {
            var spl = new ScatterPlotList()
            {
                Color = color ?? GetNextColor(),
                LineWidth = lineWidth,
                MarkerSize = markerSize,
                Label = label,
                MarkerShape = markerShape,
                LineStyle = lineStyle
            };

            Add(spl);
            return spl;
        }

        /// <summary>
        /// Signal plots have evenly-spaced X points and render very fast.
        /// </summary>
        public SignalPlot AddSignal(double[] ys, double sampleRate = 1, Color? color = null)
        {
            SignalPlot signal = new SignalPlot()
            {
                ys = ys,
                sampleRate = sampleRate,
                color = color ?? settings.GetNextColor(),

                // TODO: FIX THIS!!!
                minRenderIndex = 0,
                maxRenderIndex = ys.Length - 1,
            };
            Add(signal);
            return signal;
        }

        /// <summary>
        /// SignalConts plots have evenly-spaced X points and render faster than Signal plots
        /// but data in source arrays cannot be changed after it is loaded.
        /// Methods can be used to update all or portions of the data.
        /// </summary>
        public SignalPlotConst<T> AddSignalConst<T>(T[] ys, double sampleRate = 1, Color? color = null) where T : struct, IComparable
        {
            SignalPlotConst<T> plottable = new SignalPlotConst<T>()
            {
                ys = ys,
                sampleRate = sampleRate,
                color = color ?? settings.GetNextColor(),
                minRenderIndex = 0,
                maxRenderIndex = ys.Length - 1,
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Speed-optimized plot for Ys with unevenly-spaced ascending Xs
        /// </summary>
        public SignalPlotXY AddSignalXY(double[] xs, double[] ys, Color? color = null)
        {
            SignalPlotXY plottable = new SignalPlotXY()
            {
                xs = xs,
                ys = ys,
                color = color ?? settings.GetNextColor(),
                minRenderIndex = 0,
                maxRenderIndex = ys.Length - 1,
            };
            Add(plottable);
            return plottable;
        }


        /// <summary>
        /// Speed-optimized plot for Ys with unevenly-spaced ascending Xs.
        /// Faster than SignalXY but values cannot be modified after loading.
        /// </summary>
        public SignalPlotXYConst<TX, TY> AddSignalXYConst<TX, TY>(TX[] xs, TY[] ys, Color? color = null)
            where TX : struct, IComparable where TY : struct, IComparable
        {
            SignalPlotXYConst<TX, TY> signal = new SignalPlotXYConst<TX, TY>()
            {
                xs = xs,
                ys = ys,
                color = color ?? settings.GetNextColor(),
                minRenderIndex = 0,
                maxRenderIndex = ys.Length - 1,
            };
            Add(signal);
            return signal;
        }

        /// <summary>
        /// Display text at specific X/Y coordinates
        /// </summary>
        public Text AddText(string label, double x, double y, float size = 12, Color? color = null) =>
            AddText(label, x, y, new Drawing.Font() { Size = size, Color = color ?? GetNextColor() });

        /// <summary>
        /// Display text at specific X/Y coordinates
        /// </summary>
        public Text AddText(string label, double x, double y, Drawing.Font font)
        {
            var plottable = new Text()
            {
                text = label,
                x = x,
                y = y,
                Font = font
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a 2D vector field to the plot
        /// </summary>
        public VectorField AddVectorField(
            Vector2[,] vectors,
            double[] xs,
            double[] ys,
            string label = null,
            Color? color = null,
            Drawing.Colormap colormap = null,
            double scaleFactor = 1
            )
        {
            // TODO: refactor constructor to eliminate styling arguments
            var vectorField = new VectorField(vectors, xs, ys,
                colormap, scaleFactor, color ?? settings.GetNextColor())
            { label = label };

            Add(vectorField);
            return vectorField;
        }

        /// <summary>
        /// Add a vertical axis line at a specific Y position
        /// </summary>
        public VLine AddVerticalLine(double x, Color? color = null, float width = 1, LineStyle style = LineStyle.Solid)
        {
            VLine plottable = new VLine()
            {
                position = x,
                color = color ?? settings.GetNextColor(),
                lineWidth = width,
                lineStyle = style
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a horizontal span (shades the region between two X positions)
        /// </summary>
        public VSpan AddVerticalSpan(double yMin, double yMax, Color? color = null)
        {
            var plottable = new VSpan()
            {
                position1 = yMin,
                position2 = yMax,
                color = color ?? GetNextColor(.5),
            };
            Add(plottable);
            return plottable;
        }

        #endregion
    }
}
