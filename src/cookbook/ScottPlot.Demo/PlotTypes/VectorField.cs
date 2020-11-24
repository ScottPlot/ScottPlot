using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using ScottPlot.Statistics;

#pragma warning disable CS0618 // Type or member is obsolete
namespace ScottPlot.Demo.PlotTypes
{
    public static class VectorField
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Quickstart";
            public string description { get; } = "A vector field can be useful to show data explained by differential equations";

            public void Render(Plot plt)
            {
                double[] xPositions = DataGen.Range(0, 10);
                double[] yPositions = DataGen.Range(0, 10);
                Vector2[,] vectors = new Vector2[xPositions.Length, yPositions.Length];

                for (int x = 0; x < xPositions.Length; x++)
                    for (int y = 0; y < yPositions.Length; y++)
                        vectors[x, y] = new Vector2(
                            x: Math.Sin(xPositions[x]),
                            y: Math.Sin(yPositions[y]));

                plt.PlotVectorField(vectors, xPositions, yPositions);
            }
        }

        public class WithChangeingMagnitude : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Angle and Magnitude";
            public string description { get; } = "This example demonstrates how to define vectors according to a given angle and magnitude.";

            public void Render(Plot plt)
            {
                double[] xs = DataGen.Range(-5, 6);
                double[] ys = DataGen.Range(-5, 6);
                Vector2[,] vectors = new Vector2[xs.Length, ys.Length];

                for (int i = 0; i < xs.Length; i++)
                {
                    for (int j = 0; j < ys.Length; j++)
                    {
                        double slope = -xs[i];
                        double magnitude = Math.Abs(xs[i]);
                        double angle = Math.Atan(slope);

                        vectors[i, j] = new Vector2(Math.Cos(angle) * magnitude, Math.Sin(angle) * magnitude);
                    }
                }

                plt.PlotVectorField(vectors, xs, ys);
            }
        }

        public class Pendulum : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Custom Colormap";
            public string description { get; } = "A colormap can be supplied to color arrows according to their magnitude";

            public void Render(Plot plt)
            {
                double[] xs = DataGen.Range(-5, 5, .5);
                double[] ys = DataGen.Range(-5, 5, .5);
                Vector2[,] vectors = new Vector2[xs.Length, ys.Length];
                double r = 0.5;


                for (int i = 0; i < xs.Length; i++)
                {
                    for (int j = 0; j < ys.Length; j++)
                    {
                        double x = ys[j];
                        double y = -9.81 / r * Math.Sin(xs[i]);

                        vectors[i, j] = new Vector2(x, y);
                    }
                }

                plt.PlotVectorField(vectors, xs, ys, colormap: Drawing.Colormap.Turbo);
                plt.XLabel("θ");
                plt.YLabel("dθ/dt");
            }
        }

        public class CustomScaleFactor : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Custom Scale Factor";
            public string description { get; } = "A custom scale factor can adjust the length of the arrows.";

            public void Render(Plot plt)
            {
                double[] xs = DataGen.Range(-1.5, 1.5, .25);
                double[] ys = DataGen.Range(-1.5, 1.5, .25);
                Vector2[,] vectors = new Vector2[xs.Length, ys.Length];

                for (int i = 0; i < xs.Length; i++)
                {
                    for (int j = 0; j < ys.Length; j++)
                    {
                        double x = xs[i];
                        double y = ys[j];
                        var e = Math.Exp(-x * x - y * y);
                        var dx = (1 - 2 * x * x) * e;
                        var dy = -2 * x * y * e;

                        vectors[i, j] = new Vector2(dx, dy);
                    }
                }

                plt.PlotVectorField(vectors, xs, ys, scaleFactor: 0.3);
            }
        }
    }
}
