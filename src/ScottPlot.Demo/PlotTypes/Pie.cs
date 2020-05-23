using System;
using System.Collections.Generic;
using System.Linq;
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
                    proportions[i] = rand.NextDouble() * (1 - total); //Make sure we don't add up to more than 1
                    total += proportions[i];
                }
                if (total < 1) {
                    proportions[proportions.Length - 1] += 1 - total; //If we are less than 1, add the remnant to the last slice
                }

                plt.PlotPie(proportions);
                plt.Legend();
                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class ExplodingPie : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Exploding Pie";
            public string description { get; } = "There is an option to have an \"exploding\" pie chart.";
            Random rand = new Random();

            public void Render(Plot plt)
            {
                double[] proportions = new double[5];
                double total = 0;
                for (int i = 0; i < proportions.Length; i++)
                {
                    if (total >= 1)
                    {
                        break;
                    }
                    proportions[i] = rand.NextDouble() * (1 - total); //Make sure we don't add up to more than 1
                    total += proportions[i];
                }
                if (total < 1)
                {
                    proportions[proportions.Length - 1] += 1 - total; //If we are less than 1, add the remnant to the last slice
                }

                plt.PlotPie(proportions, explodedChart: true);
                plt.Legend();
                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class PieShownValues : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Pie With Shown Proportions";
            public string description { get; } = "There is an option to show the proportions on the chart.";
            Random rand = new Random();

            public void Render(Plot plt)
            {
                double[] proportions = new double[5];
                double total = 0;
                for (int i = 0; i < proportions.Length; i++) //Create 5 sectors with 20% each
                {
                    if (total >= 1)
                    {
                        break;
                    }
                    proportions[i] = (1.0 / proportions.Length + 0.01 * rand.NextDouble());
                    total += proportions[i];
                }
                if (total < 1)
                {
                    proportions[proportions.Length - 1] += 1 - total; //If we are less than 1, add the remnant to the last slice
                }
                else if (total > 1) {
                    proportions[proportions.Length - 1] -= total - 1; //Don't go over 100%
                }
                plt.PlotPie(proportions, showValues: true);
                plt.Legend();
                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }
    }
}
