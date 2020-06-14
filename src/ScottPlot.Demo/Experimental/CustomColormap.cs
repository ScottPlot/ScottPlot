using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CS0618 // Type or member is obsolete
namespace ScottPlot.Demo.Experimental
{
    class CustomColormap
    {
        public class Red : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Red";
            public string description { get; } = "Demonstrates making a custom colormap";

            public void Render(Plot plt)
            {
                double[,] intensities = new double[10, 256];
                for (int i = 0; i < intensities.GetLength(0); i++)
                {
                    for (int j = 0; j < intensities.GetLength(1); j++)
                    {
                        intensities[i, j] = j;
                    }
                }
                byte[,] cmap = new byte[256, 3];
                for (int i = 0; i < cmap.GetLength(0); i++)
                {
                    cmap[i, 0] = (byte)i;
                    cmap[i, 1] = 0;
                    cmap[i, 2] = 0;
                }

                plt.PlotHeatmap(intensities, new Config.ColorMaps.CustomColormap(cmap), drawAxisLabels: false);
            }
        }
    }
}
