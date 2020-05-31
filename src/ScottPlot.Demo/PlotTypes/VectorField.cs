using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class VectorField
    {
        class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Quickstart";
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
                        double slope = 0.5 * xs[i];
                        double magnitude = 1;
                        double angle = Math.Atan(slope);

                        vectors[i, j] = new Vector2((float)(Math.Cos(angle) * magnitude), (float)(Math.Sin(angle) * magnitude));
                    }
                }

                plt.PlotVectorField(vectors, xs, ys);
                plt.Title("Slope Field for y' = x^2");
            }
        }

        class WithChangeingMagnitude : PlotDemo, IPlotDemo
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
    }
}
