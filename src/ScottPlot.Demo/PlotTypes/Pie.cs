using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Pie
    {
        public class PieQuickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Pie Quickstart";
            public string description { get; } = "Pie charts show proportions with corresponding proportions on a circle.";
            Random rand = new Random();

            public void Render(Plot plt)
            {
                double[] proportions = new double[5];
                double total = 0;
                for (int i = 0; i < proportions.Length; i++)
                {
                    if (total >= 1) {
                        break;
                    }
                    proportions[i] = rand.NextDouble() * (1 - total);   //Make sure we don't add up to more than 1
                    total += proportions[i];
                }
                if (total < 1) {
                    proportions[proportions.Length - 1] += 1 - total; //If we are less than 1, add the remnant to the last slice
                }

                plt.PlotPie(proportions);
                plt.Legend();
            }
        }
    }
}
