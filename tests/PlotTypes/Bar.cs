using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.PlotTypes
{
    public class Bar
    {
        [Test]
        public void Test_barSingleSet_valuesOnly()
        {
            var votes = new double[] { 33706, 36813, 12496 };
            var groups = new string[] { "Debian", "SuSE", "Red Hat" };

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotBar(votes, groups);

            plt.Title("Favorite Linux Distribution");
            plt.YLabel("Respondants");
            plt.Grid(enableVertical: false);
            plt.Axis(y1: 0);
            plt.Ticks(useMultiplierNotation: false);
            plt.XTicks(labels: groups);
            plt.Legend();

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_barSingleSet_valuesAndErrors()
        {
            var votes = new double[] { 33706, 36813, 12496 };
            var errors = new double[] { 2660, 3580, 1950 };
            var groups = new string[] { "Debian", "SuSE", "Red Hat" };

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotBar(votes, groups, errors);

            plt.Title("Favorite Linux Distribution");
            plt.YLabel("Respondants");
            plt.Grid(enableVertical: false);
            plt.Axis(y1: 0);
            plt.Ticks(useMultiplierNotation: false);
            plt.XTicks(labels: groups);
            plt.Legend();

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_barMultipleSets_valuesOnly()
        {
            //var errorsWork = new double[] { 2660, 3580, 1950 };
            //var errorsFun = new double[] { 2456, 3456, 2345 };

            // series-level variables
            var votesWork = new ScottPlot.DataSet("Work", new double[] { 33706, 36813, 12496 });
            var votesHobby = new ScottPlot.DataSet("Hobby", new double[] { 34930, 33400, 12843 });

            // group-level variables
            var groups = new string[] { "Debian", "SuSE", "Red Hat" };
            ScottPlot.DataSet[] dataSets = new ScottPlot.DataSet[] { votesWork, votesHobby };

            // make the plot
            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotBar(dataSets, groups);

            plt.Title("Favorite Linux Distribution");
            plt.YLabel("Respondants");
            plt.Grid(enableVertical: false);
            plt.Axis(y1: 0);
            plt.Ticks(useMultiplierNotation: false);
            plt.XTicks(labels: groups);
            plt.Legend(location: ScottPlot.legendLocation.upperRight);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_barMultipleSets_valuesWithError()
        {
            // series-level variables
            var votesWork = new ScottPlot.DataSet(label: "Work",
                values: new double[] { 33706, 36813, 12496 },
                errors: new double[] { 2456, 3456, 2345 });

            var votesHobby = new ScottPlot.DataSet(label: "Hobby",
                new double[] { 34930, 33400, 12843 },
                new double[] { 2456, 3456, 2345 }
                );

            // group-level variables
            var groups = new string[] { "Debian", "SuSE", "Red Hat" };
            ScottPlot.DataSet[] dataSets = new ScottPlot.DataSet[] { votesWork, votesHobby };

            // make the plot
            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotBar(dataSets, groups);

            plt.Title("Favorite Linux Distribution");
            plt.YLabel("Respondants");
            plt.Grid(enableVertical: false);
            plt.Axis(y1: 0);
            plt.Ticks(useMultiplierNotation: false);
            plt.XTicks(labels: groups);
            plt.Legend(location: ScottPlot.legendLocation.upperRight);

            TestTools.SaveFig(plt);
        }
    }
}
