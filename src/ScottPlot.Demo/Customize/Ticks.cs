using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Demo.Customize
{
    class Ticks
    {
        public class Visibility : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Hide Tick Labels";
            public string description { get; } = "Tick label visibility can be controlled with arguments to the Ticks() method";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Ticks(displayTicksX: false);
            }
        }

        public class Formatting : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Numeric Tick Formatting";
            public string description { get; } = "Display format of numeric ticks can be customized using numeric format strings.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                // See https://tinyurl.com/y86clj9k to learn about numeric format strings
                plt.Ticks(numericFormatStringX: "E2", numericFormatStringY: "P1");
            }
        }

        public class DateAxis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "DateTime Axis";
            public string description { get; } = "Axis tick labels can be set to display date and time format if the values (double[]) are OATime values.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                double[] temperature = DataGen.RandomWalk(rand, 60 * 8);
                DateTime start = new DateTime(2019, 08, 25, 8, 30, 00);
                double pointsPerDay = 24 * 60;

                plt.PlotSignal(temperature, sampleRate: pointsPerDay, xOffset: start.ToOADate());
                plt.Ticks(dateTimeX: true);
                plt.YLabel("Temperature (C)");
            }
        }

        public class Positions : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Define Tick Positions";
            public string description { get; } = "An array of tick positions and labels can be manually defined.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                double[] xPositions = { 7, 21, 37, 46 };
                string[] xLabels = { "VII", "XXI", "XXXVII", "XLVI" };
                plt.XTicks(xPositions, xLabels);

                double[] yPositions = { -1, 0, .5, 1 };
                string[] yPabels = { "bottom", "center", "half", "top" };
                plt.YTicks(yPositions, yPabels);
            }
        }

        public class NonlinearTickSpacing : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Nonlinear Tick Spacing";
            public string description { get; } = "Customize tick labels to give a nonlinear axis appearance";

            public void Render(Plot plt)
            {
                // these are our nonlinear data values we wish to plot
                double[] amplitudes = { 23.9, 24.2, 24.3, 24.5, 25.3, 26.3, 27.6, 31.4, 33.7, 36,
                                        38.4, 42, 43.5, 46.1, 48.8, 51.5, 53.2, 55, 56.9, 58.7, 60.6 };
                double[] frequencies = { 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630,
                                         800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000 };

                // ignore the "real" X values and plot data at consecutive X values (0, 1, 2, 3...)
                double[] positions = DataGen.Consecutive(frequencies.Length);
                plt.PlotScatter(positions, amplitudes);

                // then define tick labels based on "real" X values, rotate them, and give them extra space
                string[] labels = frequencies.Select(x => x.ToString()).ToArray();
                plt.XTicks(positions, labels);
                plt.Ticks(xTickRotation: 45);

                // apply axis labels, trigging a layout reset
                plt.Title("Vibrational Coupling");
                plt.YLabel("Amplitude (dB)");
                plt.XLabel("Frequency (Hz)");

                // manually tweak the layout to add extra padding for rotated labels
                plt.Layout(xScaleHeight: 30);
            }
        }

        public class Inverted : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Descending Ticks";
            public string description { get; } = "ScottPlot will ALWAYS display data where X values ascend from left to right. To simulate an inverted axis (where numbers decrease from left to right) plot data in the NEGATIVE space, then use a Tick() argument to invert the sign of tick labels.";

            public void Render(Plot plt)
            {
                // plot in the negative space
                plt.PlotSignal(DataGen.Sin(50), xOffset: -50);

                // then invert the sign of the axis tick labels
                plt.Ticks(invertSignX: true);
                plt.Ticks(invertSignY: true);
            }
        }

        public class DefineSpacing : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Defined Tick Spacing";
            public string description { get; } = "The space between tick marks can be manually defined by setting the grid spacing.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Grid(xSpacing: 2, ySpacing: .1);
            }
        }

        public class LocalizedHungarian : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Localized Formatting (Hungarian)";
            public string description { get; } = "Large numbers and dates are formatted differently for different cultures. Hungarian is a good example of this: they use spaces to separate large numbers, and periods to separate fields in dates.";

            public void Render(Plot plt)
            {
                // generate some data
                Random rand = new Random(0);
                double[] price = ScottPlot.DataGen.RandomWalk(rand, 60 * 8, 10000);
                DateTime start = new DateTime(2019, 08, 25, 8, 30, 00);
                double pointsPerDay = 24 * 60;

                // create the plot
                plt.PlotSignal(price, sampleRate: pointsPerDay, xOffset: start.ToOADate());
                plt.Ticks(dateTimeX: true);
                plt.YLabel("Price");
                plt.XLabel("Date and Time");
                plt.Title("Hungarian Formatted DateTime Tick Labels");

                // set the localization
                var culture = System.Globalization.CultureInfo.CreateSpecificCulture("hu"); // Hungarian
                plt.SetCulture(culture);
            }
        }

        public class LocalizedGerman : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Localized Formatting (German)";
            public string description { get; } = "Large numbers and dates are formatted differently for different cultures. German is a good example of this: they use periods to separate large numbers, and periods to separate fields in dates.";

            public void Render(Plot plt)
            {
                // generate some data
                Random rand = new Random(0);
                double[] price = ScottPlot.DataGen.RandomWalk(rand, 60 * 8, 10000);
                DateTime start = new DateTime(2019, 08, 25, 8, 30, 00);
                double pointsPerDay = 24 * 60;

                // create the plot
                plt.PlotSignal(price, sampleRate: pointsPerDay, xOffset: start.ToOADate());
                plt.Ticks(dateTimeX: true);
                plt.YLabel("Price");
                plt.XLabel("Date and Time");
                plt.Title("German Formatted DateTime Tick Labels");

                // set the localization
                var culture = System.Globalization.CultureInfo.CreateSpecificCulture("de"); // German
                plt.SetCulture(culture);
            }
        }

        public class CustomCulture : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Format Ticks with Custom Culture";
            public string description { get; } = "SetCulture() as arguments to let the user manually define formatting strings which will be used globally to change how numbers and dates are formatted.";

            public void Render(Plot plt)
            {
                // generate 10 days of data
                int pointCount = 10;
                double[] values = DataGen.RandomWalk(null, pointCount);
                double[] days = new double[pointCount];
                DateTime day1 = new DateTime(1985, 09, 24);
                for (int i = 0; i < days.Length; i++)
                    days[i] = day1.AddDays(1).AddDays(i).ToOADate();

                // plot the data with custom tick format (https://tinyurl.com/ycwh45af)
                plt.PlotScatter(days, values);
                plt.Ticks(dateTimeX: true);
                plt.SetCulture(shortDatePattern: "M\\/dd");
            }
        }

        public class Large : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Accomodating Large Ticks";
            public string description { get; } = "The plot layout adjusts automatically to accomodate large tick labels.";

            public void Render(Plot plt)
            {
                // generate LARGE data
                Random rand = new Random(0);
                double[] xs = ScottPlot.DataGen.Consecutive(100);
                double[] ys = ScottPlot.DataGen.RandomWalk(rand, 100, 1e2, 1e15);
                plt.PlotScatter(xs, ys);
                plt.YLabel("vertical units");
                plt.XLabel("horizontal units");
            }
        }

        public class MultiplierNotation : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Multiplier Notation";
            public string description { get; } = "To keep tick labels small 'multiplier' notation can be used when their values are large.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 100;
                double[] largeXs = DataGen.Consecutive(pointCount, spacing: 1e6);
                double[] largeYs = DataGen.Random(rand, pointCount, multiplier: 1e6);

                plt.PlotScatter(largeXs, largeYs);
                plt.Ticks(useMultiplierNotation: true);
            }
        }

        public class OffsetNotation : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Offset Notation";
            public string description { get; } = "To keep tick labels small 'offset' notation can be used when their values are very far from zero.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 100;
                double[] largeXs = DataGen.Consecutive(pointCount, offset: 1e6);
                double[] largeYs = DataGen.Random(rand, pointCount, offset: 1e6);

                plt.PlotScatter(largeXs, largeYs);
                plt.Ticks(useOffsetNotation: true);
            }
        }

        public class RotatedTicks : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Rotated Ticks";
            public string description { get; } = "Horizontal ticks can be rotated an arbitrary amount.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                // setting the axis label resets the layout so call this first
                plt.XLabel("Horizontal Axis Label");

                // define axis tick rotation and tweak the layout to provide extra padding
                plt.Ticks(xTickRotation: 45);
            }
        }

        public class DateAxisFixedSpace : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Defined DateTime Tick Spacing";
            public string description { get; } = "This example shows how to use a fixed inter-tick distance.";

            public void Render(Plot plt)
            {
                int pointCount = 20;

                // create a series of dates
                double[] dates = new double[pointCount];
                var firstDay = new DateTime(2020, 1, 22);
                for (int i = 0; i < pointCount; i++)
                    dates[i] = firstDay.AddDays(i).ToOADate();

                // simulate data for each date
                double[] values = new double[pointCount];
                Random rand = new Random(0);
                for (int i = 1; i < pointCount; i++)
                    values[i] = values[i - 1] + rand.NextDouble();

                plt.PlotScatter(dates, values);
                plt.Ticks(dateTimeX: true);

                // define tick spacing as 1 day (every day will be shown)
                plt.Grid(xSpacing: 1, xSpacingDateTimeUnit: Config.DateTimeUnit.Day);
                plt.Ticks(dateTimeX: true, xTickRotation: 45);
                plt.Layout(xScaleHeight: 60);
            }
        }
    }
}
