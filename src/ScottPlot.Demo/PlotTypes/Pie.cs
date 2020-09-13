using System;
using System.Collections.Generic;
using System.Drawing;
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
            public string name { get; } = "Donut Plot";
            public string description { get; } = "Donut plots are circle plots with a hollow center.";

            public void Render(Plot plt)
            {
                double[] values = { 778, 283, 184, 76, 43 };

                var pie = plt.PlotPie(values);
                pie.donutSize = .6;
                pie.explodedChart = true;

                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class DonutPieWithPercentageInDonut : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Donut Plot With Text";
            public string description { get; } = "Custom text can be displayed inside the donut.";

            public void Render(Plot plt)
            {
                double[] values = { 779, 586 };
                string centerText = $"{values[0] / values.Sum() * 100:00.0}%";

                Color color1 = Color.FromArgb(255, 0, 150, 200);
                Color color2 = Color.FromArgb(100, 0, 150, 200);

                var pie = plt.PlotPie(values);
                pie.donutSize = .6;
                pie.centerText = centerText;
                pie.centerTextColor = color1;
                pie.outlineSize = 2;
                pie.colors = new Color[] { color1, color2 };

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
