using ScottPlot.Control.EventProcess.Events;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    class GridAdvanced : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "asis_gridAdvanced";
        public string Title => "Advanced Grid Customization";
        public string Description => "Grid lines can be extensively customized using various configuration methods.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // advanced grid customizations are available by accessing Axes directly
            plt.XAxis.MajorGrid(color: Color.FromArgb(100, Color.Black));
            plt.XAxis.MinorGrid(enable: true, color: Color.FromArgb(20, Color.Black));
            plt.YAxis.MajorGrid(lineWidth: 2, lineStyle: LineStyle.Dash, color: Color.Magenta);
        }
    }

    class TicksNumericFormatString : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_numericFormatString";
        public string Title => "Numeric Format String";
        public string Description => "Tick labels can be converted to text using a custom format string.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // See https://tinyurl.com/y86clj9k to learn about numeric format strings
            plt.XAxis.TickLabelFormat("E2", dateTimeFormat: false);
            plt.YAxis.TickLabelFormat("P1", dateTimeFormat: false);
        }
    }

    class TicksDefined : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_defined";
        public string Title => "Manual Tick Labels";
        public string Description =>
            "Tick positions and labels can be defined manually.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // manually define X axis tick positions and labels
            double[] xPositions = { 7, 21, 37, 46 };
            string[] xLabels = { "VII", "XXI", "XXXVII", "XLVI" };
            plt.XAxis.ManualTickPositions(xPositions, xLabels);

            // manually define Y axis tick positions and labels
            double[] yPositions = { -1, 0, .5, 1 };
            string[] yLabels = { "bottom", "center", "half", "top" };
            plt.YAxis.ManualTickPositions(yPositions, yLabels);
        }
    }

    class TicksDefinedAndUnioned : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_defined_and_unioned";
        public string Title => "Manual and Automatic Tick Labels";
        public string Description =>
            "Tick positions and labels can be defined manually, but also added alongside automatic tick labels.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(15), 2);
            plt.AddSignal(DataGen.Cos(15), 2);

            double[] positions = { Math.PI, 2 * Math.PI };
            string[] labels = { "π", "2π" };
            plt.XAxis.AutomaticTickPositions(positions, labels);
            plt.XAxis.TickDensity(0.5);
        }
    }

    class TicksNonLinearX : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_nonLinearX";
        public string Title => "NonLinear Tick Spacing";
        public string Description =>
            "Plot data on regular cartesian space then manually control axis labels " +
            "to give the appearance of non-linear spacing between points.";

        public void ExecuteRecipe(Plot plt)
        {
            // these are our nonlinear data values we wish to plot
            double[] amplitudes = { 23.9, 24.2, 24.3, 24.5, 25.3, 26.3, 27.6, 31.4, 33.7, 36,
                        38.4, 42, 43.5, 46.1, 48.8, 51.5, 53.2, 55, 56.9, 58.7, 60.6 };
            double[] frequencies = { 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630,
                         800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000 };

            // ignore the "real" X values and plot data at consecutive X values (0, 1, 2, 3...)
            double[] positions = DataGen.Consecutive(frequencies.Length);
            plt.AddScatter(positions, amplitudes);

            // then define tick labels based on "real" X values, rotate them, and give them extra space
            string[] labels = frequencies.Select(x => x.ToString()).ToArray();
            plt.XAxis.ManualTickPositions(positions, labels);
            plt.XAxis.TickLabelStyle(rotation: 45);
            plt.XAxis.SetSizeLimit(min: 50); // extra space for rotated ticks

            // apply axis labels, trigging a layout reset
            plt.Title("Vibrational Coupling");
            plt.YLabel("Amplitude (dB)");
            plt.XLabel("Frequency (Hz)");
        }
    }

    class TicksDescending : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_descending";
        public string Title => "Descending Ticks";
        public string Description =>
            "ScottPlot will always display data where X values ascend from left to right. " +
            "To simulate an inverted axis (where numbers decrease from left to right) plot " +
            "data in the negative space, then invert the sign of tick labels.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot the positive data in the negative space
            double[] values = DataGen.Sin(50);
            var sig = plt.AddSignal(values);
            sig.OffsetX = -50;

            // then invert the sign of the axis tick labels
            plt.XAxis.TickLabelNotation(invertSign: true);
            plt.YAxis.TickLabelNotation(invertSign: true);
        }
    }

    class TicksDefinedSpacing : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_definedSpacing";
        public string Title => "Defined Tick Spacing";
        public string Description =>
            "The space between tick marks can be manually defined by setting the grid spacing.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot the positive data in the negative space
            double[] values = DataGen.Sin(50);
            var sig = plt.AddSignal(values);
            sig.OffsetX = -50;

            // then invert the sign of the axis tick labels
            plt.XAxis.ManualTickSpacing(2);
            plt.YAxis.ManualTickSpacing(.1);
        }
    }

    class TicksCulture : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_culture";
        public string Title => "Tick Label Culture";
        public string Description =>
            "Large numbers and dates are formatted differently for different cultures. " +
            "Hungarian uses spaces to separate large numbers and periods to separate fields in dates.";

        public void ExecuteRecipe(Plot plt)
        {
            // generate some data
            double[] price = DataGen.RandomWalk(null, 60 * 8, 10000);
            DateTime start = new DateTime(2019, 08, 25, 8, 30, 00);
            double pointsPerDay = 24 * 60;

            // create the plot
            var sig = plt.AddSignal(price, pointsPerDay);
            sig.OffsetX = start.ToOADate();

            // set the localization
            var culture = System.Globalization.CultureInfo.CreateSpecificCulture("hu"); // Hungarian
            plt.SetCulture(culture);

            // further decorate the plot
            plt.XAxis.DateTimeFormat(true);
            plt.YAxis.Label("Price");
            plt.XAxis.Label("Date and Time");
            plt.XAxis2.Label("Hungarian Formatted DateTime Tick Labels");
        }
    }

    class TicksCultureCustom : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_cultureCustom";
        public string Title => "Custom Tick Label Culture";
        public string Description =>
            "SetCulture() as arguments to let the user manually define " +
            "formatting strings which will be used globally to change how numbers and dates are formatted.";

        public void ExecuteRecipe(Plot plt)
        {
            // generate 10 days of data
            int pointCount = 10;
            double[] values = DataGen.RandomWalk(null, pointCount);
            double[] days = new double[pointCount];
            DateTime day1 = new DateTime(1985, 09, 24);
            for (int i = 0; i < days.Length; i++)
                days[i] = day1.AddDays(1).AddDays(i).ToOADate();

            // plot the data with custom tick format (https://tinyurl.com/ycwh45af)
            plt.AddScatter(days, values);
            plt.XAxis.TickLabelFormat("M\\/dd", dateTimeFormat: true);
        }
    }

    class TicksMultiplier : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_multiplier";
        public string Title => "Multiplier Notation";
        public string Description =>
            "Multiplier notation keeps tick labels small when plotting large data values. " +
            "This style is also called engineering notation or scientific notation.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddLine(-1e5, -1e10, 1e5, 1e10);

            plt.XAxis.TickLabelNotation(multiplier: true);
            plt.YAxis.TickLabelNotation(multiplier: true);
        }
    }

    class TicksOffset : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_offset";
        public string Title => "Offset Notation";
        public string Description =>
            "Offset notation keeps tick labels small when plotting large data values that are close together.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddLine(1e5 + 111, 1e10 + 111, 1e5 + 222, 1e10 + 222);

            plt.XAxis.TickLabelNotation(offset: true);
            plt.YAxis.TickLabelNotation(offset: true);
        }
    }

    class TicksDefinedDateTimeSpace : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_definedDateTimeSpace";
        public string Title => "Defined DateTime Spacing";
        public string Description => "This example shows how to use a fixed inter-tick distance for a DateTime axis";

        public void ExecuteRecipe(Plot plt)
        {
            // create a series of dates
            int pointCount = 20;
            double[] dates = new double[pointCount];
            var firstDay = new DateTime(2020, 1, 22);
            for (int i = 0; i < pointCount; i++)
                dates[i] = firstDay.AddDays(i).ToOADate();

            // simulate data for each date
            double[] values = new double[pointCount];
            Random rand = new Random(0);
            for (int i = 1; i < pointCount; i++)
                values[i] = values[i - 1] + rand.NextDouble();

            plt.AddScatter(dates, values);
            plt.XAxis.DateTimeFormat(true);

            // define tick spacing as 1 day (every day will be shown)
            plt.XAxis.ManualTickSpacing(1, ScottPlot.Ticks.DateTimeUnit.Day);
            plt.XAxis.TickLabelStyle(rotation: 45);

            // add some extra space for rotated ticks
            plt.XAxis.SetSizeLimit(min: 50);
        }
    }

    class LogScale : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "asis_log";
        public string Title => "Log Scale";
        public string Description =>
            "ScottPlot is designed to display 2D data on linear X and Y axes, but you can log-transform " +
            "data before plotting it and customize the ticks and grid to give the appearance of logarithmic scales.";

        public void ExecuteRecipe(Plot plt)
        {
            // These are the dat we will plot with a linear X scale but log Y scale
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 10, 2_000, 50_000, 1_000_000, 1_500_000 };

            // Plot the Log10 of all the Y values
            double[] logYs = ys.Select(y => Math.Log10(y)).ToArray();
            var scatter = plt.AddScatter(xs, logYs, lineWidth: 2, markerSize: 10);

            // Use a custom formatter to control the label for each tick mark
            static string logTickLabels(double y) => Math.Pow(10, y).ToString("N0");
            plt.YAxis.TickLabelFormat(logTickLabels);

            // Use log-spaced minor tick marks and grid lines to make it more convincing
            plt.YAxis.MinorLogScale(true);
            plt.YAxis.MajorGrid(true, Color.FromArgb(80, Color.Black));
            plt.YAxis.MinorGrid(true, Color.FromArgb(20, Color.Black));
            plt.XAxis.MajorGrid(true, Color.FromArgb(80, Color.Black));

            // Set the axis limits manually to ensure edges terminate at desirable locations
            plt.SetAxisLimits(0, 6, 0, Math.Log10(10_000_000));
        }
    }

    class LogScaleTickDensity : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "asis_logTickDensity";
        public string Title => "Log Scale Tick Density";
        public string Description =>
            "Numer of minor ticks between major ticks can be customized.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] ys = ScottPlot.DataGen.Range(100, 10_000, 100, true);
            double[] xs = ScottPlot.DataGen.Consecutive(ys.Length);
            double[] logYs = ys.Select(y => Math.Log10(y)).ToArray();

            var scatter = plt.AddScatter(xs, logYs);

            static string logTickLabels(double y) => Math.Pow(10, y).ToString("N0");
            plt.YAxis.TickLabelFormat(logTickLabels);

            // set the number of minor ticks per major tick here
            plt.YAxis.MinorLogScale(true, minorTickCount: 20);

            // darken grid line colors
            plt.YAxis.MinorGrid(true);
            plt.YAxis.MinorGrid(true, Color.FromArgb(20, Color.Black));
            plt.YAxis.MajorGrid(true, Color.FromArgb(80, Color.Black));
            plt.XAxis.MajorGrid(true, Color.FromArgb(80, Color.Black));
        }
    }

    class Ruler : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "asis_ruler";
        public string Title => "Ruler mode";
        public string Description =>
            "Ruler mode is an alternative way to display axis ticks. " +
            "It draws long ticks and offsets the tick labels to give the appearance of a ruler.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.XAxis.RulerMode(true);
            plt.YAxis.RulerMode(true);
        }
    }

    class Polar : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "asis_polar";
        public string Title => "Polar Coordinates";
        public string Description =>
            "A helper function converts radius and theta arrays into Cartesian " +
            "coordinates suitable for plotting with traditioanl plot types.";

        public void ExecuteRecipe(Plot plt)
        {
            // create data with polar coordinates
            int count = 400;
            double step = 0.01;

            double[] rs = new double[count];
            double[] thetas = new double[count];

            for (int i = 0; i < rs.Length; i++)
            {
                rs[i] = 1 + i * step;
                thetas[i] = i * 2 * Math.PI * step;
            }

            // convert polar data to Cartesian data
            (double[] xs, double[] ys) = ScottPlot.Tools.ConvertPolarCoordinates(rs, thetas);

            // plot the Cartesian data
            plt.AddScatter(xs, ys);

            // decorate the plot
            plt.Title("Scatter Plot of Polar Data");
            plt.AxisScaleLock(true);
        }
    }

    class Image : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "asis_image";
        public string Title => "Images as Axis Labels";
        public string Description => "Images can be used as axis labels to allow for things like LaTeX axis labels.";

        public void ExecuteRecipe(Plot plt)
        {
            // create an interesting plot
            double[] xs = DataGen.Range(-5, 5, .5);
            double[] ys = DataGen.Range(-5, 5, .5);
            Vector2[,] vectors = new Vector2[xs.Length, ys.Length];
            for (int i = 0; i < xs.Length; i++)
                for (int j = 0; j < ys.Length; j++)
                    vectors[i, j] = new Vector2(ys[j], -15 * Math.Sin(xs[i]));
            plt.AddVectorField(vectors, xs, ys, colormap: Drawing.Colormap.Turbo);

            // use images as axis labels
            plt.XAxis.ImageLabel(new Bitmap("Images/theta.png"));
            plt.YAxis.ImageLabel(new Bitmap("Images/d_theta_dt.png"));
        }
    }

    class ImageTransparent : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "asis_imageTransparent";
        public string Title => "Transparent Images Axis Labels";
        public string Description =>
            "Transparency in PNGs is respected, but JPEG files do not support transparency.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Style(Style.Light2);
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // vertical axis label uses a transparent PNG
            plt.YAxis.ImageLabel(new Bitmap("Images/d_theta_dt.png"));

            // horizontal axis label uses a non-transparent JPEG
            plt.XAxis.ImageLabel(new Bitmap("Images/theta.jpg"));
        }
    }

    class TickDensity : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "asis_tickDensity";
        public string Title => "Tick Density";
        public string Description => "Axis tick density can be adjusted by the user. " +
            "The largest the density is, the more ticks are displayed. Setting this value too high " +
            "will result in overlapping tick labels.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.XAxis.Label("Lower Density Ticks");
            plt.XAxis.TickDensity(0.2);

            plt.YAxis.Label("Higher Density Ticks");
            plt.YAxis.TickDensity(3);
        }
    }

    class MinimumTickSpacing : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "asis_minimumTickSpacing";
        public string Title => "Minimum Tick Spacing";
        public string Description => "Minimum tick spacing can be defined such that zooming in " +
            "does not produce more grid lines, ticks, and tick labels beyond the defined limit.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.YAxis.MinimumTickSpacing(1);
            plt.XAxis.MinimumTickSpacing(25);
        }
    }

    class CustomTickFormatter : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "asis_custom_tick_formatter";
        public string Title => "Custom Tick Formatter";
        public string Description => "For ultimate control over tick label format you can create " +
            "a custom formatter function and use that to convert positions to labels. " +
            "This allows logic to be used to format tick labels.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            // create a custom formatter as a static class
            static string customTickFormatter(double position)
            {
                if (position == 0)
                    return "zero";
                else if (position > 0)
                    return $"+{position:F2}";
                else
                    return $"({Math.Abs(position):F2})";
            }

            // use the custom formatter for horizontal and vertical tick labels
            plt.XAxis.TickLabelFormat(customTickFormatter);
            plt.YAxis.TickLabelFormat(customTickFormatter);
        }
    }

    class TickMarksInvertDirection : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "ticks_invert_tick_mark_direction";
        public string Title => "Invert tick mark direction";
        public string Description =>
            "Tick marks can be outward (default) or inverted to appear as " +
            "inward lines relative to the edge of the plot area.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.XAxis.TickMarkDirection(outward: false);
            plt.YAxis.TickMarkDirection(outward: false);
        }
    }

    class AdvancedAxisCustomization : IRecipe
    {
        public ICategory Category => new Categories.AdvancedAxis();
        public string ID => "advanced_axis_customization";
        public string Title => "Advanced Axis Customization";
        public string Description =>
            "Axis labels, tick marks, and axis lines can all be extensively customized " +
            "by interacting directly with axis configuration objects.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.XAxis.AxisTicks.MajorTickLength = 10;
            plt.XAxis.AxisTicks.MinorTickLength = 5;

            plt.XAxis.AxisTicks.MajorTickColor = Color.Magenta;
            plt.XAxis.AxisTicks.MinorTickColor = Color.LightSkyBlue;

            plt.YAxis.AxisLine.Width = 3;
        }
    }
}
