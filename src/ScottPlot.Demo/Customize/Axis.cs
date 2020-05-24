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
            public string description { get; } = "Ruler mode only on one axis";

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
            public string description { get; } = "";

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
            public string description { get; } = "";

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
    }
}
