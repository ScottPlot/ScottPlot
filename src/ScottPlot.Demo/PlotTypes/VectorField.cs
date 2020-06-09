using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

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
                double[] xPositions = DataGen.Range(0, 10, .5);
                double[] yPositions = DataGen.Range(0, 10, .5);
                Vector2[,] vectors = new Vector2[xPositions.Length, yPositions.Length];

                for (int x = 0; x < xPositions.Length; x++)
                    for (int y = 0; y < yPositions.Length; y++)
                        vectors[x, y] = new Vector2(
                            x: (float)Math.Sin(xPositions[x]),
                            y: (float)Math.Sin(yPositions[y]));

                plt.PlotVectorField(vectors, xPositions, yPositions);
            }
        }

        public class WithChangeingMagnitude : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Changing Magnitude";
            public string description { get; } = "A vector field can be useful to show data explained by differential equations";

            public void Render(Plot plt)
            {
                double[] xs = Enumerable.Range(-5, 11).Select(i => (double)i).ToArray();
                double[] ys = Enumerable.Range(-5, 11).Select(i => (double)i).ToArray();
                Vector2[,] vectors = new Vector2[xs.Length, ys.Length];

                for (int i = 0; i < xs.Length; i++)
                {
                    for (int j = 0; j < ys.Length; j++)
                    {
                        double slope = -xs[i];
                        double magnitude = Math.Abs(xs[i]);
                        double angle = Math.Atan(slope);

                        vectors[i, j] = new Vector2((float)(Math.Cos(angle) * magnitude), (float)(Math.Sin(angle) * magnitude));
                    }
                }

                plt.PlotVectorField(vectors, xs, ys);
            }
        }

        public class Pendulum : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Pendulum Example";
            public string description { get; } = "A vector field can be useful to show data explained by differential equations";

            public void Render(Plot plt)
            {
                double[] xs = Enumerable.Range(-10, 20).Select(i => (double)i / 2).ToArray();
                double[] ys = Enumerable.Range(-10, 20).Select(i => (double)i / 2).ToArray();
                Vector2[,] vectors = new Vector2[xs.Length, ys.Length];
                double r = 0.5;


                for (int i = 0; i < xs.Length; i++)
                {
                    for (int j = 0; j < ys.Length; j++)
                    {
                        double x = ys[j];
                        double y = -9.81 / r * Math.Sin(xs[i]);

                        vectors[i, j] = new Vector2((float)x, (float)y);
                    }
                }

                plt.PlotVectorField(vectors, xs, ys, colormap: new Config.ColorMaps.Turbo());
                plt.XLabel("θ");
                plt.YLabel("dθ/dt");
            }
        }

        public class ScaledPendulum : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Scale Factor Example";
            public string description { get; } = "Sometimes it can be useful to shrink or enlarge the arrows";

            public void Render(Plot plt)
            {
                double[] xs = Enumerable.Range(-10, 20).Select(i => (double)i / 2).ToArray();
                double[] ys = Enumerable.Range(-10, 20).Select(i => (double)i / 2).ToArray();
                Vector2[,] vectors = new Vector2[xs.Length, ys.Length];
                double r = 0.5;


                for (int i = 0; i < xs.Length; i++)
                {
                    for (int j = 0; j < ys.Length; j++)
                    {
                        double x = ys[j];
                        double y = -9.81 / r * Math.Sin(xs[i]);

                        vectors[i, j] = new Vector2((float)x, (float)y);
                    }
                }

                plt.PlotVectorField(vectors, xs, ys, colormap: new Config.ColorMaps.Turbo(), scaleFactor: 1.3);
                plt.XLabel("θ");
                plt.YLabel("dθ/dt");
            }
        }

        public class AnotherExample : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Another Example";
            public string description { get; } = "A vector field can be useful to show data explained by differential equations";

            public void Render(Plot plt)
            {
                //Thank you to https://github.com/hhubschle

                double[] xs = Enumerable.Range(-6, 12).Select(i => (double)i / 4).ToArray();
                double[] ys = Enumerable.Range(-6, 12).Select(i => (double)i / 4).ToArray();
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

                        vectors[i, j] = new Vector2((float)dx, (float)dy);
                    }
                }

                plt.PlotVectorField(vectors, xs, ys, scaleFactor: 0.3);
            }
        }
    }
}
