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

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class DonutPie : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Donut Pie";
            public string description { get; } = "There is an option to have a \"donut\" pie chart.";

            public void Render(Plot plt)
            {
                double[] values = { 778, 283, 184, 76, 43 };

                //Default donutSize = 0.6
                plt.PlotPie(values, explodedChart: true, donut: true, donutSize: 0.55);

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class DonutPieWithPercentageInDonut : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Donut Pie With Percentage in the Middle";
            public string description { get; } = "There is an option to have a \"donut\" pie chart with the largest proportion in the donut hole.";

            public void Render(Plot plt)
            {
                double[] values = { 778, 586 };

                System.Drawing.Color[] colors = new System.Drawing.Color[] { System.Drawing.Color.FromArgb(255, 0, 150, 200), System.Drawing.Color.FromArgb(100, 0, 150, 200) };

                plt.PlotPie(values, donut: true, showPercentageInDonut: true, colors: colors, drawOutline: false);

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class PieShowValues : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Show Values";
            public string description { get; } = "There is an option to show slice values on the chart.";

            public void Render(Plot plt)
            {
                double[] values = { 778, 43, 283, 76, 184 };

                plt.PlotPie(values, showValues: true);

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class PieShowPercentages : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Show Percentages";
            public string description { get; } = "There is an option to show slice percentages on the chart.";

            public void Render(Plot plt)
            {
                double[] values = { 778, 43, 283, 76, 184 };

                plt.PlotPie(values, showPercentages: true);

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class PieLabelSlices : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Show Labels in Slices";
            public string description { get; } = "Labels can also be shown on slices.";

            public void Render(Plot plt)
            {
                double[] values = { 778, 43, 283, 76, 184 };
                string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };

                plt.PlotPie(values, labels, showLabels: true);

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class PieLegend : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Labels in Legend";
            public string description { get; } = "You can label slices and show them in the legend.";

            public void Render(Plot plt)
            {
                double[] values = { 778, 43, 283, 76, 184 };
                string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };

                plt.PlotPie(values, labels, showLabels: false);
                plt.Legend();

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class PieShowEverything : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Show Everything";
            public string description { get; } = "Values, percentages, labels, and legend";

            public void Render(Plot plt)
            {
                double[] values = { 778, 43, 283, 76, 184 };
                string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };

                plt.PlotPie(values, labels, showPercentages: true, showValues: true, showLabels: true);
                plt.Legend();

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class PieCustomSliceLabels : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Custom Slice Labels";
            public string description { get; } = "The best way to show labels and data is to make the string labels yourself";

            public void Render(Plot plt)
            {
                double[] values = { 778, 43, 283, 76, 184 };
                string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };

                labels = Enumerable
                    .Range(0, values.Length)
                    .Select(i => $"{labels[i]}\n({values[i]})")
                    .ToArray();

                plt.PlotPie(values, labels);

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }
    }
}
