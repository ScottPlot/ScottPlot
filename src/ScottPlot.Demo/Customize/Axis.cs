using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Customize
{
    class Axis
    {
        public class AxisLabels : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Title and Axis Labels";
            public string description { get; } = "Title and axis labels can be defined and custoized using arguments.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Title("Plot Title");
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
            }
        }

        public class RulerMode : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Ruler Mode";
            public string description { get; } = "Ruler mode is an alternative way to display axis tick labels";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Ticks(rulerModeX: true, rulerModeY: true);
            }
        }

        public class RulerModeXOnly : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Ruler Mode (X only)";
            public string description { get; } = "Display a ruler on just one axis";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Ticks(rulerModeX: true, displayTicksY: false);
                plt.Frame(left: false, right: false, top: false);
                plt.TightenLayout(padding: 0, render: true);
            }
        }

        public class LogAxis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Log Axis";
            public string description { get; } = "ScottPlot can only display data on a linear 2D plane, " +
                "however you can log-transform your data before plotting it to give the appearance of log scales. " +
                "Customizing tick options for log-spaced minor ticks further improves appearance of these graphs.";

            public void Render(Plot plt)
            {
                // generate some interesting log-distributed data
                int pointCount = 200;
                double[] dataXs = new double[pointCount];
                double[] dataYs = new double[pointCount];
                Random rand = new Random(0);
                for (int i = 0; i < pointCount; i++)
                {
                    double x = 10.0 * i / pointCount;
                    dataXs[i] = x;
                    dataYs[i] = Math.Pow(2, x) + rand.NextDouble() * i;
                }

                // this tool can convert linear data to log data
                double[] dataYsLog = ScottPlot.Tools.Log10(dataYs);
                plt.PlotScatter(dataXs, dataYsLog, lineWidth: 0);

                // call this to move minor ticks to simulate a log scale
                plt.Ticks(logScaleY: true);

                plt.Title("Data (Log Scale)");
                plt.YLabel("Vertical Units (10^x)");
                plt.XLabel("Horizontal Units");
            }
        }

        public class PolarAxis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Polar Axis";
            public string description { get; } = "A helper function converts radius and theta arrays " +
                "into Cartesian coordinates suitable for plotting with traditioanl plot types.";

            public void Render(Plot plt)
            {
                // create some data with polar coordinates
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
                plt.PlotScatter(xs, ys);
                plt.Title("Scatter Plot of Polar Data");
                plt.EqualAxis = true;
            }
        }

        public class DateTimeAxis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "DateTime Axis";
            public string description { get; } = "Axis tick labels can show DateTime format if DateTime.ToOADate() was used to plot the data";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                double[] ys = DataGen.RandomWalk(rand, 100);
                double[] xs = new double[ys.Length];

                DateTime dtStart = new DateTime(1985, 9, 24);
                for (int i = 0; i < ys.Length; i++)
                {
                    DateTime dtNow = dtStart.AddSeconds(i);
                    xs[i] = dtNow.ToOADate();
                }

                plt.PlotScatter(xs, ys);
                plt.Ticks(dateTimeX: true);
                plt.Title("DateTime Axis Labels");
            }
        }

        public class TimeOnly : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Time Only";
            public string description { get; } = "Typically DateTime tick labels show date and time, " +
                "but by defining the format yourself you can customize this behavior.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                double[] ys = DataGen.RandomWalk(rand, 50);
                double[] xs = new double[ys.Length];

                DateTime start = new DateTime(1985, 9, 24);
                for (int i = 0; i < ys.Length; i++)
                {
                    DateTime dtNow = start.AddMinutes(i * 15);
                    xs[i] = dtNow.ToOADate();
                }

                plt.PlotScatter(xs, ys);
                plt.Ticks(dateTimeX: true, dateTimeFormatStringX: "HH:mm:ss");
                plt.Title("Time Axis Labels");
            }
        }

        public class TimeCodeAxis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Time Code Axis";
            public string description { get; } = "Axis tick labels show HH:MM:SS.SSS timecodes (useful for audio and video editing)";

            public void Render(Plot plt)
            {
                // simulate 10 seconds of audio data
                int pointsPerSecond = 44100;
                Random rand = new Random(0);
                double[] ys = DataGen.RandomWalk(rand, pointsPerSecond * 10);

                // For DateTime compatibility, sample rate must be points/day.
                // Also, avoid negative dates by offsetting the plot by today's date.
                double secondsPerDay = 24 * 60 * 60;
                double pointsPerDay = secondsPerDay * pointsPerSecond;
                double today = DateTime.Today.ToOADate();

                plt.PlotSignal(ys, sampleRate: pointsPerDay, xOffset: today);
                plt.Ticks(dateTimeX: true, dateTimeFormatStringX: "HH:mm:ss.fff");
            }
        }

        public class HexadecimalAxis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Hexadecimal Axis";
            public string description { get; } = "Axis tick labels can be in any base, not just base 10";

            public void Render(Plot plt)
            {
                // create some sample data
                double[] xs = { 0 };
                double[] valuesA = { 0x40000000 };
                double[] valuesB = { 0x40100000 };
                double[] valuesC = { 0xA0000000 };

                // to simulate stacking B on A, shift B up by A
                double[] valuesB2 = new double[valuesB.Length];
                double[] valuesC2 = new double[valuesC.Length];
                for (int i = 0; i < valuesB.Length; i++)
                {
                    valuesB2[i] = valuesA[i] + valuesB[i];
                    valuesC2[i] = valuesC[i] + valuesB2[i];
                }

                // plot the bar charts in reverse order (highest first)
                plt.PlotBar(xs, valuesC2, label: "Process C");
                plt.PlotBar(xs, valuesB2, label: "Process B");
                plt.PlotBar(xs, valuesA, label: "Process A");

                // configure ticks for base 16 Y-axis
                plt.Ticks(baseY: 16, prefixY: "0x");
                plt.Axis(-1, 1, 0, 0x1A0000000);

                // further customize the plot
                plt.Ticks(displayTicksX: false, displayTicksY: true);
                plt.Title("Memory Consumption");
                plt.YLabel("Memory (Bytes)");
                plt.Legend();
            }
        }
    }
}
