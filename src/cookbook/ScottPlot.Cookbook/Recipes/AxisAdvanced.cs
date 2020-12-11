using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes
{
    class GridAdvanced : IRecipe
    {
        public string Category => "Advanced Axis Features";
        public string ID => "asis_gridAdvanced";
        public string Title => "Advanced Grid Customization";
        public string Description => "Grid lines can be extensively customized using various configuration methods.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // advanced grid customizations are available by accessing Axes directly
            plt.XAxis.ConfigureMajorGrid(color: Color.FromArgb(100, Color.Black));
            plt.XAxis.ConfigureMinorGrid(enable: true, color: Color.FromArgb(20, Color.Black));
            plt.YAxis.ConfigureMajorGrid(lineWidth: 2, lineStyle: LineStyle.Dash, color: Color.Magenta);
        }
    }

    class TicksNumericFormatString : IRecipe
    {
        public string Category => "Advanced Axis Features";
        public string ID => "ticks_numericFormatString";
        public string Title => "Numeric Format String";
        public string Description => "Tick labels can be converted to text using a custom format string.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // See https://tinyurl.com/y86clj9k to learn about numeric format strings
            plt.XAxis.SetTickLabelFormat("E2", dateTimeFormat: false);
            plt.YAxis.SetTickLabelFormat("P1", dateTimeFormat: false);
        }
    }

    class TicksDefined : IRecipe
    {
        public string Category => "Advanced Axis Features";
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
            plt.XAxis.SetTickPositions(xPositions, xLabels);

            // manually define Y axis tick positions and labels
            double[] yPositions = { -1, 0, .5, 1 };
            string[] yLabels = { "bottom", "center", "half", "top" };
            plt.YAxis.SetTickPositions(yPositions, yLabels);
        }
    }

    class TicksNonLinearX : IRecipe
    {
        public string Category => "Advanced Axis Features";
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
            plt.XAxis.SetTickPositions(positions, labels);
            plt.XAxis.ConfigureTickLabelStyle(rotation: 45);
            plt.XAxis.PixelSizeMinimum = 50; // extra padding for rotated ticks

            // apply axis labels, trigging a layout reset
            plt.Title("Vibrational Coupling");
            plt.YLabel("Amplitude (dB)");
            plt.XLabel("Frequency (Hz)");
        }
    }

    class TicksDescending : IRecipe
    {
        public string Category => "Advanced Axis Features";
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
            sig.xOffset = -50;

            // then invert the sign of the axis tick labels
            plt.XAxis.ConfigureTickLabelStyle(invertSign: true);
            plt.YAxis.ConfigureTickLabelStyle(invertSign: true);
        }
    }

    class TicksDefinedSpacing : IRecipe
    {
        public string Category => "Advanced Axis Features";
        public string ID => "ticks_definedSpacing";
        public string Title => "Defined Tick Spacing";
        public string Description =>
            "The space between tick marks can be manually defined by setting the grid spacing.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot the positive data in the negative space
            double[] values = DataGen.Sin(50);
            var sig = plt.AddSignal(values);
            sig.xOffset = -50;

            // then invert the sign of the axis tick labels
            plt.XAxis.SetTickSpacing(2);
            plt.YAxis.SetTickSpacing(.1);
        }
    }

    class TicksCulture : IRecipe
    {
        public string Category => "Advanced Axis Features";
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
            sig.xOffset = start.ToOADate();

            // set the localization
            var culture = System.Globalization.CultureInfo.CreateSpecificCulture("hu"); // Hungarian
            plt.SetCulture(culture);

            // further decorate the plot
            plt.XAxis.DateTimeFormat(true);
            plt.YAxis.SetLabel("Price");
            plt.XAxis.SetLabel("Date and Time");
            plt.XAxis2.SetLabel("Hungarian Formatted DateTime Tick Labels");
        }
    }

    class TicksCultureCustom : IRecipe
    {
        public string Category => "Advanced Axis Features";
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
            plt.XAxis.SetTickLabelFormat("M\\/dd", dateTimeFormat: true);
        }
    }

    class TicksMultiplier : IRecipe
    {
        public string Category => "Advanced Axis Features";
        public string ID => "ticks_multiplier";
        public string Title => "Multiplier Notation";
        public string Description =>
            "Multiplier notation keeps tick labels small when plotting large data values.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] largeXs = DataGen.Consecutive(100, spacing: 1e6);
            double[] largeYs = DataGen.Random(null, 100, multiplier: 1e6);

            plt.AddScatter(largeXs, largeYs);
            plt.XAxis.TickLabelNotation(multiplier: true);
        }
    }

    class TicksOffset : IRecipe
    {
        public string Category => "Advanced Axis Features";
        public string ID => "ticks_offset";
        public string Title => "Offset Notation";
        public string Description =>
            "Offset notation keeps tick labels small when plotting large data values that are close together.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] largeXs = DataGen.Consecutive(100, spacing: 1e6);
            double[] largeYs = DataGen.Random(null, 100, multiplier: 1e6);

            plt.AddScatter(largeXs, largeYs);
            plt.XAxis.TickLabelNotation(offset: true);
        }
    }

    class TicksDefinedDateTimeSpace : IRecipe
    {
        public string Category => "Advanced Axis Features";
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
            plt.XAxis.SetTickSpacing(1, ScottPlot.Ticks.DateTimeUnit.Day);
            plt.XAxis.ConfigureTickLabelStyle(rotation: 45);

            // add some extra space for rotated ticks
            plt.XAxis.PixelSizeMinimum = 50;
        }
    }
}
