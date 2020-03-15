using System;
using System.Collections.Generic;
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
                GenericPlots.SinAndCos(plt);
                plt.Ticks(displayTicksX: false);
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
                GenericPlots.SinAndCos(plt);

                double[] xPositions = { 7, 21, 37, 46 };
                string[] xLabels = { "VII", "XXI", "XXXVII", "XLVI" };
                plt.XTicks(xPositions, xLabels);

                double[] yPositions = { -1, 0, .5, 1 };
                string[] yPabels = { "bottom", "center", "half", "top" };
                plt.YTicks(yPositions, yPabels);
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
                GenericPlots.SinAndCos(plt);
                plt.Grid(xSpacing: 2, ySpacing: .1);
            }
        }

        public class LocalizedHungarian : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Localized Formatting (Hungarian)";
            public string description { get; } = "Large numbers and dates are formatted differently for different cultures. This example demonstrates how to use a specific culture for formatting.";

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
                plt.Ticks(useMultiplierNotation: false);

                // set the localization
                var culture = System.Globalization.CultureInfo.CreateSpecificCulture("hu"); // Hungarian
                plt.SetCulture(culture);
            }
        }

        public class LocalizedGerman : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Localized Formatting (German)";
            public string description { get; } = "Large numbers and dates are formatted differently for different cultures. This example demonstrates how to use a specific culture for formatting.";

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
                plt.Ticks(useMultiplierNotation: false);

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
            public string description { get; } = "Large tick labels come with special layout considerations.";

            public void Render(Plot plt)
            {
                // generate LARGE data
                Random rand = new Random(0);
                double[] xs = ScottPlot.DataGen.Consecutive(100);
                double[] ys = ScottPlot.DataGen.RandomWalk(rand, 100, 1e2, 1e15);
                plt.PlotScatter(xs, ys);
                plt.YLabel("vertical units");
                plt.XLabel("horizontal units");

                // turn off features which typically shorten tick label size
                plt.Ticks(useOffsetNotation: false, useMultiplierNotation: false);

                // tightening with a render is the best way to get the axes right
                plt.TightenLayout(render: true);

            }
        }

        public class MultiplierNotation : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Multiplier Notation";
            public string description { get; } = "To keep tick labels small 'multiplier' notation is used when their values are large.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 100;
                double[] largeXs = DataGen.Consecutive(pointCount, spacing: 1e6);
                double[] largeYs = DataGen.Random(rand, pointCount, multiplier: 1e6);

                plt.PlotScatter(largeXs, largeYs);
            }
        }

        public class DisableMultiplierNotation : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Disable Multiplier Notation";
            public string description { get; } = "Multiplier notation can be disabled so tick labels are never abbreviated in this way.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 100;
                double[] largeXs = DataGen.Consecutive(pointCount, spacing: 1e6);
                double[] largeYs = DataGen.Random(rand, pointCount, multiplier: 1e6);

                plt.PlotScatter(largeXs, largeYs);
                plt.Ticks(useMultiplierNotation: false);
            }
        }

        public class OffsetNotation : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Offset Notation";
            public string description { get; } = "To keep tick labels small 'offset' notation is used when their values are very far from zero.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 100;
                double[] largeXs = DataGen.Consecutive(pointCount, offset: 1e6);
                double[] largeYs = DataGen.Random(rand, pointCount, offset: 1e6);

                plt.PlotScatter(largeXs, largeYs);
            }
        }

        public class DisableOffsetNotation : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Disable Offset Notation";
            public string description { get; } = "Offset notation can be disabled so tick labels are never abbreviated in this way.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 100;
                double[] largeXs = DataGen.Consecutive(pointCount, offset: 1e6);
                double[] largeYs = DataGen.Random(rand, pointCount, offset: 1e6);

                plt.PlotScatter(largeXs, largeYs);
                plt.Ticks(useOffsetNotation: false);
            }
        }
    }
}
