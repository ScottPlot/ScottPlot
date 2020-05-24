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
            public string name { get; } = "Quickstart";
            public string description { get; } = "A pie chart (or a circle chart) illustrates numerical proportions as slices of a circle.";

            public void Render(Plot plt)
            {
                double[] values = { 778, 283, 184, 76, 43 };

                plt.PlotPie(values);
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

            public void Render(Plot plt)
            {
                double[] values = { 778, 283, 184, 76, 43 };

                plt.PlotPie(values, explodedChart: true);
                plt.Legend();

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class PieShownValues : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Labeled Slices";
            public string description { get; } = "There is an option to show the proportions on the chart.";

            public void Render(Plot plt)
            {
                double[] values = { 778, 43, 283, 76, 184 };

                plt.PlotPie(values, showValues: true);
                plt.Legend();

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }
    }
}
