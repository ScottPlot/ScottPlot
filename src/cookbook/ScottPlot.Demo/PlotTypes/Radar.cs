using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Radar
    {
        public class RadarQuickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Radar Chart Quickstart";
            public string description { get; } =
                "A radar chart concisely displays multiple values. " +
                "Radar plots are also called a spider charts or star charts.";

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

        public class RadarPolygon : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Straight Axis Lines";
            public string description { get; } = "Change the axis type to polygon to display radar charts with straight lines.";

            public void Render(Plot plt)
            {
                double[,] values = {
                    { 78, 83, 84, 76, 43 },
                    { 100, 50, 70, 60, 90 }
                };

                var radarPlot = plt.PlotRadar(values);
                radarPlot.axisType = RadarAxis.Polygon;

                // customize the plot
                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class RadarNoAxis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "No Axis Lines";
            public string description { get; } = "A radar chart can have no drawn axis as well.";

            public void Render(Plot plt)
            {
                double[,] values = {
                    { 78, 83, 84, 76, 43 },
                    { 100, 50, 70, 60, 90 }
                };

                var radarPlot = plt.PlotRadar(values);
                radarPlot.axisType = RadarAxis.None;
                radarPlot.showAxisLabels = false; // Looks weird without this

                // customize the plot
                plt.Grid(false);
                plt.Frame(false);
                plt.Ticks(false, false);
            }
        }

        public class RadarUnlabeledAxes : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Labeled Categories";
            public string description { get; } = "Category labels can be displayed on the radar chart.";

            public void Render(Plot plt)
            {
                double[,] values = { { 5, 3, 10, 15, 3, 2, 256 }, { 5, 2, 10, 10, 1, 4, 252 }, };
                string[] categories = { "Wins", "Poles", "Podiums", "Points Finishes", "DNFs", "Fastest Laps", "Points" };
                string[] groups = { "Sebastian Vettel", "Fernando Alonso" };

                var radarPlot = plt.PlotRadar(values, categories, groups, independentAxes: true);
                radarPlot.showAxisLabels = false;
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

        public class RadarWithLabels : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Labeled Values";
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

        public class RadarSeveralAxes : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Independent Axis Scaling";
            public string description { get; } =
                "Axis scaling can be independent, allowing values for each category to be displayed using a different scale. " +
                "When independent axis mode is enabled, axis limits are automatically adjusted to fit the range of the data.";

            public void Render(Plot plt)
            {
                double[,] values = { { 5, 3, 10, 15, 3, 2, 256 }, { 5, 2, 10, 10, 1, 4, 252 }, };
                string[] categories = { "Wins", "Poles", "Podiums", "Points Finishes", "DNFs", "Fastest Laps", "Points" };
                string[] groups = { "Sebastian Vettel", "Fernando Alonso" };

                plt.PlotRadar(values, categories, groups, independentAxes: true);
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

        public class RadarSeveralAxesWithMax : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Defined Axis Limits";
            public string description { get; } =
                "Radar charts with independent axis limits use scales fitted to the data by default, " +
                "but scaling can be controlled by defining the maximum value for each axis.";

            public void Render(Plot plt)
            {
                double[,] values = { { 5, 3, 10, 15, 3, 2, 256 }, { 5, 2, 10, 10, 1, 4, 252 }, };
                double[] maxValues = { 13, 15, 17, 15, 10, 10, 413 };
                string[] categories = { "Wins", "Poles", "Podiums", "Points Finishes", "DNFs", "Fastest Laps", "Points" };
                string[] groups = { "Sebastian Vettel", "Fernando Alonso" };

                plt.PlotRadar(values, categories, groups, independentAxes: true, maxValues: maxValues);
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
                 *
                 * Max values are based on https://en.wikipedia.org/wiki/List_of_Formula_One_World_Drivers%27_Champions.
                 */
            }
        }
    }
}
