using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Populations
    {
        public class PlotPopulation : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot a Population";
            public string description { get; } = "Population objects can be plotted with Plot.Populations(). The default display format is to show a box-and-whisker plot (showing outliers, quartiles, and median) next to a scatter plot of the original data values and the distribution curve.";

            public void Render(Plot plt)
            {
                // create some sample data to represent test scores
                Random rand = new Random(0);
                double[] scores = DataGen.RandomNormal(rand, 35, 85, 5);

                // create a Population object and plot it
                var pop = new ScottPlot.Statistics.Population(scores);
                plt.PlotPopulations(pop, "scores");

                // improve the style of the plot
                plt.Title($"Test Scores (mean: {pop.mean:0.00} +/- {pop.stDev:0.00}, n={pop.n})");
                plt.YLabel("Score");
                plt.Ticks(displayTicksX: false);
                plt.Legend();
                plt.Grid(lineStyle: LineStyle.Dot, enableVertical: false);
            }
        }

        public class PlotPopulationSeriesUniform : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Uniform Population Series";
            public string description { get; } = "A series of populations can be plotted as a single object. Every population in a series has the same style, and a series will appear only once in the legend.";

            public void Render(Plot plt)
            {
                // create some sample data to represent test scores
                Random rand = new Random(0);
                double[] scoresA = DataGen.RandomNormal(rand, 35, 85, 5);
                double[] scoresB = DataGen.RandomNormal(rand, 42, 87, 3);
                double[] scoresC = DataGen.RandomNormal(rand, 23, 92, 3);

                // collect multiple populations into a PopulationSeries
                var poulations = new Statistics.Population[] {
                    new Statistics.Population(scoresA),
                    new Statistics.Population(scoresB),
                    new Statistics.Population(scoresC)
                };

                // Plot these as a single series (all styled the same, appearing once in legend)
                var popSeries = new Statistics.PopulationSeries(poulations);
                plt.PlotPopulations(popSeries, "scores");

                // improve the style of the plot
                plt.Title($"Test Scores by Class");
                plt.YLabel("Score");
                plt.XTicks(new string[] { "Class A", "Class B", "Class C" });
                plt.Legend();
                plt.Grid(lineStyle: LineStyle.Dot, enableVertical: false);
            }
        }

        public class PlotPopulationSeriesUnique : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Unique Population Series";
            public string description { get; } = "To give every population in a series a different style, plot it as a MultiSeries where each group only contains 1 series. This way every population will have a unique style, and each population will be listed in the legend.";

            public void Render(Plot plt)
            {
                // create some sample data to represent test scores
                Random rand = new Random(0);
                double[] scoresA = DataGen.RandomNormal(rand, 35, 85, 5);
                double[] scoresB = DataGen.RandomNormal(rand, 42, 87, 3);
                double[] scoresC = DataGen.RandomNormal(rand, 23, 92, 3);

                // create a unique PopulationSeries for each set of scores
                var seriesA = new Statistics.PopulationSeries(new Statistics.Population[] { new Statistics.Population(scoresA) }, "Class A");
                var seriesB = new Statistics.PopulationSeries(new Statistics.Population[] { new Statistics.Population(scoresB) }, "Class B");
                var seriesC = new Statistics.PopulationSeries(new Statistics.Population[] { new Statistics.Population(scoresC) }, "Class C");

                // create a MultiSeries from all the individual series
                var multiSeries = new Statistics.PopulationMultiSeries(new Statistics.PopulationSeries[] { seriesA, seriesB, seriesC });
                plt.PlotPopulations(multiSeries);

                // improve the style of the plot
                plt.Title($"Test Scores by Class");
                plt.YLabel("Score");
                plt.Ticks(displayTicksX: false);
                plt.Legend();
                plt.Grid(lineStyle: LineStyle.Dot, enableVertical: false);
            }
        }

        public class PlotPopulationMultiSeries : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Plot a Population Multi-Series";
            public string description { get; } = "To compare groups of population series, construct a PopulationMultiSeries object and pot it. Each series within the MultiSeries will appear once in the legend.";

            public void Render(Plot plt)
            {
                // create some sample data to represent test scores.
                Random rand = new Random(0);

                // Each class (A, B, C) is a series.
                // Each semester (fall, spring, summer A, summer B) is a group.

                double[] scoresAfall = DataGen.RandomNormal(rand, 35, 85, 5);
                double[] scoresBfall = DataGen.RandomNormal(rand, 42, 87, 5);
                double[] scoresCfall = DataGen.RandomNormal(rand, 23, 82, 5);

                double[] scoresAspring = DataGen.RandomNormal(rand, 35, 84, 3);
                double[] scoresBspring = DataGen.RandomNormal(rand, 42, 88, 3);
                double[] scoresCspring = DataGen.RandomNormal(rand, 23, 84, 3);

                double[] scoresAsumA = DataGen.RandomNormal(rand, 35, 80, 5);
                double[] scoresBsumA = DataGen.RandomNormal(rand, 42, 90, 5);
                double[] scoresCsumA = DataGen.RandomNormal(rand, 23, 85, 5);

                double[] scoresAsumB = DataGen.RandomNormal(rand, 35, 91, 2);
                double[] scoresBsumB = DataGen.RandomNormal(rand, 42, 93, 2);
                double[] scoresCsumB = DataGen.RandomNormal(rand, 23, 90, 2);

                // Collect multiple populations into a PopulationSeries.
                // All populations in a series will be styled the same and appear once in the legend.

                var popsA = new Statistics.Population[] {
                    new Statistics.Population(scoresAfall),
                    new Statistics.Population(scoresAspring),
                    new Statistics.Population(scoresAsumA),
                    new Statistics.Population(scoresAsumB)
                };

                var popsB = new Statistics.Population[] {
                    new Statistics.Population(scoresBfall),
                    new Statistics.Population(scoresBspring),
                    new Statistics.Population(scoresBsumA),
                    new Statistics.Population(scoresBsumB)
                };

                var popsC = new Statistics.Population[] {
                    new Statistics.Population(scoresCfall),
                    new Statistics.Population(scoresCspring),
                    new Statistics.Population(scoresCsumA),
                    new Statistics.Population(scoresCsumB)
                };

                var seriesA = new Statistics.PopulationSeries(popsA, "Class A");
                var seriesB = new Statistics.PopulationSeries(popsB, "Class B");
                var seriesC = new Statistics.PopulationSeries(popsC, "Class C");
                var allSeries = new Statistics.PopulationSeries[] { seriesA, seriesB, seriesC };

                // create a MultiSeries from multiple population series and plot it
                var multiSeries = new Statistics.PopulationMultiSeries(allSeries);
                plt.PlotPopulations(multiSeries);

                // improve the style of the plot
                plt.Title($"Test Scores by Class");
                plt.YLabel("Score");
                plt.XTicks(new string[] { "Fall", "Spring", "Summer A", "Summer B" });
                plt.Legend();
                plt.Grid(lineStyle: LineStyle.Dot, enableVertical: false);
            }
        }

        public class AdvancedStyling : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Advanced Styling";
            public string description { get; } = "Store the object returned by Plot.Populations() and modify its properties to further customize how group plots are displayed.";

            public void Render(Plot plt)
            {
                // create some sample data to represent test scores
                Random rand = new Random(0);
                double[] scoresA = DataGen.RandomNormal(rand, 35, 72, 7);
                double[] scoresB = DataGen.RandomNormal(rand, 42, 57, 10);
                double[] scoresC = DataGen.RandomNormal(rand, 23, 79, 5);

                // create a unique PopulationSeries for each set of scores
                var seriesA = new Statistics.PopulationSeries(new Statistics.Population[] { new Statistics.Population(scoresA) }, "Class A");
                var seriesB = new Statistics.PopulationSeries(new Statistics.Population[] { new Statistics.Population(scoresB) }, "Class B");
                var seriesC = new Statistics.PopulationSeries(new Statistics.Population[] { new Statistics.Population(scoresC) }, "Class C");

                // create a MultiSeries from all the individual series
                var multiSeries = new Statistics.PopulationMultiSeries(new Statistics.PopulationSeries[] { seriesA, seriesB, seriesC });

                // save the plottable and modify its properties to customize display style
                var popPlot = plt.PlotPopulations(multiSeries);
                popPlot.displayDistributionCurve = true;
                popPlot.distributionCurveLineStyle = LineStyle.Dash;
                popPlot.scatterOutlineColor = System.Drawing.Color.Transparent;
                popPlot.displayItems = PlottablePopulations.DisplayItems.ScatterAndBox;
                popPlot.boxStyle = PlottablePopulations.BoxStyle.BarMeanStDev;
                plt.Axis(y1: 0);

                // colors are managed at the population series level:
                foreach (var popSeries in popPlot.popMultiSeries.multiSeries)
                    popSeries.color = Tools.GetRandomColor(rand);

                // improve the style of the plot
                plt.Title($"Test Scores by Class");
                plt.YLabel("Score");
                plt.Legend(location: legendLocation.lowerLeft);
                plt.Ticks(displayTicksX: false);
                plt.Grid(lineStyle: LineStyle.Dot, enableVertical: false);
            }
        }
    }
}
