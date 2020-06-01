using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Demo.PlotTypes
{
    public static class Polygon
    {
        public class BenchmarkPolygon : PlotDemo, IPlotDemo
        {
            public string name { get; } = "PlotPolygon with many polygons";
            public string description { get; } = "PlotPolygon can display thousands of polygons, but this is SLOW! Use PlotPolygons for this purpose instead.";

            public void Render(Plot plt)
            {
                int polygonCount = 5_000;
                int pointsPerPolygon = 100;
                Random rand = new Random(0);

                // create polygons and plot them one at a time
                for (int i = 0; i < polygonCount; i++)
                {
                    double polyX = rand.NextDouble() * 100;
                    double polyY = rand.NextDouble() * 100;
                    double polyR = rand.NextDouble();
                    double[] xs = Enumerable.Range(0, pointsPerPolygon).Select(x => polyR * Math.Cos(2.0 * Math.PI * x / pointsPerPolygon) + polyX).ToArray();
                    double[] ys = Enumerable.Range(0, pointsPerPolygon).Select(x => polyR * Math.Sin(2.0 * Math.PI * x / pointsPerPolygon) + polyY).ToArray();
                    plt.PlotPolygon(xs, ys, fillColor: Color.Green);
                }

                // customize the plot
                plt.EqualAxis = true;
                plt.Title($"PlotPolygon with {polygonCount:N0} {pointsPerPolygon}-sided polygons");
            }
        }

        public class BenchmarkPolygons : PlotDemo, IPlotDemo
        {
            public string name { get; } = "PlotPolygons with many polygons";
            public string description { get; } = "PlottablePolygons is a speed-optimized method of displaying large numbers of polygons.";

            public void Render(Plot plt)
            {
                int polygonCount = 5_000;
                int pointsPerPolygon = 100;
                Random rand = new Random(0);

                List<List<(double x, double y)>> polys = new List<List<(double x, double y)>>();

                // create a List of polygons
                for (int i = 0; i < polygonCount; i++)
                {
                    double polyX = rand.NextDouble() * 100;
                    double polyY = rand.NextDouble() * 100;
                    double polyR = rand.NextDouble();
                    double[] xs = Enumerable.Range(0, pointsPerPolygon).Select(x => polyR * Math.Cos(2.0 * Math.PI * x / pointsPerPolygon) + polyX).ToArray();
                    double[] ys = Enumerable.Range(0, pointsPerPolygon).Select(x => polyR * Math.Sin(2.0 * Math.PI * x / pointsPerPolygon) + polyY).ToArray();
                    polys.Add(xs.Zip(ys, (xp, yp) => (xp, yp)).ToList());
                }

                // then plot all the polygons with one command
                plt.PlotPolygons(polys, fillColor: Color.Green);

                // customize the plot
                plt.EqualAxis = true;
                plt.Title($"PlotPolygons with {polys.Count:N0} {pointsPerPolygon}-sided polygons");
            }
        }

        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Polygon Quickstart";
            public string description { get; } = "Pairs of X/Y points can be used to display polygons.";

            public void Render(Plot plt)
            {
                plt.PlotPolygon(
                    xs: new double[] { 2, 8, 6, 4 },
                    ys: new double[] { 3, 4, 0.5, 1 },
                    label: "polygon A", lineWidth: 2, fillAlpha: .8,
                    lineColor: System.Drawing.Color.Black);

                plt.PlotPolygon(
                    xs: new double[] { 3, 2.5, 5 },
                    ys: new double[] { 4.5, 1.5, 2.5 },
                    label: "polygon B", lineWidth: 2, fillAlpha: .8,
                    lineColor: System.Drawing.Color.Black);

                plt.Title($"Polygon Demonstration");
                plt.Legend();
            }
        }

        public class ShadedLineAboveAndBelow : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Shaded Line Plot";
            public string description { get; } = "Line plots can be shaded above/below zero by plotting two polygons.";

            public void Render(Plot plt)
            {
                // generate sample data
                Random rand = new Random(0);
                var dataY = DataGen.RandomWalk(rand, 1000, offset: -10);
                var dataX = DataGen.Consecutive(dataY.Length, spacing: 0.025);

                // create an array with an extra point on each side of the data
                var xs = new double[dataX.Length + 2];
                var ys = new double[dataY.Length + 2];
                Array.Copy(dataX, 0, xs, 1, dataX.Length);
                Array.Copy(dataY, 0, ys, 1, dataY.Length);
                xs[0] = dataX[0];
                xs[xs.Length - 1] = dataX[dataX.Length - 1];
                ys[0] = 0;
                ys[ys.Length - 1] = 0;

                // separate the data into two arrays (for positive and negative)
                double[] neg = new double[ys.Length];
                double[] pos = new double[ys.Length];
                for (int i = 0; i < ys.Length; i++)
                {
                    if (ys[i] < 0)
                        neg[i] = ys[i];
                    else
                        pos[i] = ys[i];
                }

                // now plot the arrays as polygons
                plt.PlotPolygon(xs, neg, "negative", lineWidth: 1,
                    lineColor: Color.Black, fillColor: Color.Red, fillAlpha: .5);
                plt.PlotPolygon(xs, pos, "positive", lineWidth: 1,
                    lineColor: Color.Black, fillColor: Color.Green, fillAlpha: .5);
                plt.Title("Shaded Line Plot (negative vs. positive)");
                plt.Legend(location: ScottPlot.legendLocation.lowerLeft);
                plt.AxisAuto(0);
            }
        }
    }
}
