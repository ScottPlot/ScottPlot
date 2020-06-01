using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Radar
    {
        public class RadarQuickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Quickstart";
            public string description { get; } = "A radar chart concisely displays multiple values. Radar plots are also called a spider charts or star charts.";

            public void Render(Plot plt)
            {
                double[,] values = {
                    { 78, 83, 84, 76, 43 },
                    { 100, 50, 70, 60, 90 }
                };

                plt.PlotRadar(values);

                // customize the plot
                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class RadarWithLabels : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Radar With Labels";
            public string description { get; } = "Labels can be displayed on the arms of the radar chart.";

            public void Render(Plot plt)
            {
                double[,] values = { { 5, 3, 10, 15, 3, 2 }, { 5, 2, 10, 10, 1, 4 }, };
                string[] categories = { "Wins", "Poles", "Podiums", "Points Finishes", "DNFs", "Fastest Laps" };
                string[] groups = { "Sebastian Vettel", "Fernando Alonso" };

                plt.PlotRadar(values, categories, groups);
                plt.Legend();

                // customize the plot
                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
                plt.Title("2010 Formula One World Championship");

                /* Data represents the 2010 Formula One World Championship
                 * https://en.wikipedia.org/wiki/2010_Formula_One_World_Championship
                 * Note: Alonso did not finish (DNF) in the Malaysian GP, but was included 
                 * here because he completed >90% of the race distance.
                 */
            }
        }
    }
}
