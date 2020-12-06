/* The Plot class is the primary public interface for ScottPlot.
 * State (plottables and configuration) is stored in the settings object 
 * which is private so it can be refactored without breaking the API.
 * Helper methods plottable creation and styling are in partial classes.
 */

using ScottPlot.Plottable;
using System;
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
