using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Point
    {
        public class Quickstart : RecipeBase, IRecipe
        {
            public string name { get; } = "Plot points";
            public string description { get; } = "Points are essentially scatter plots with a single point.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                // draw something to make the plot interesting
                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                // add a few points
                plt.AddPoint(25, 0.8);
                plt.AddPoint(30, 0.3, color: Color.Magenta, markerSize: 15);
            }
        }
    }
}
