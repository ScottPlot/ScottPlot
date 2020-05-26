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
            public string description { get; } = "A radar chart (or spider or star chart) is good for showing multiple variables concisely.";

            public void Render(Plot plt)
            {
                double[,] values = { { 78, 83, 84, 76, 43 }, { 100, 50, 70, 60, 90 } };

                plt.PlotRadar(values);

                //These aren't necessary, but they look nice
                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class RadarWithLabels : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Radar With Labels";
            public string description { get; } = "A radar chart (or spider or star chart) is good for showing multiple variables concisely.";

            public void Render(Plot plt)
            {
                //In case you're interested, this is about the 2010 F1 Season
                //Note, Alonso did not finish the Malaysian GP, but was classified because he completed 90% race distance
                //Same for Hamilton in the Spanish GP
                //Both are counted as DNFs
                double[,] values = {
                    { 5, 3, 10, 15, 3, 2},
                    { 5, 2, 10, 10, 1, 4},
                };
                string[] categories = { "Wins", "Poles", "Podiums", "Points Finishes", "DNFs", "Fastest Laps" };
                string[] groups = { "Vettel", "Alonso" };

                plt.PlotRadar(values, categories, groups);
                plt.Legend();

                //These aren't necessary, but they look nice
                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }
    }
}
