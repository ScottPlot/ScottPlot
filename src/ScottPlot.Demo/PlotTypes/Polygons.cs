using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public class Polygons
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
    }
}
