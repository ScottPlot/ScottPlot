using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Plot
{
    public class Bar
    {
        [Test]
        public void Test_Bar_MultiSeries()
        {
            Random rand = new Random(0);

            string[] groupNames = { "one", "two", "three", "four", "five" };
            string[] seriesNames = { "alpha", "beta", "gamma" };

            int groupCount = groupNames.Length;

            double[] xs = DataGen.Consecutive(groupCount);
            double[] ys1 = DataGen.RandomNormal(rand, groupCount, 20, 5);
            double[] ys2 = DataGen.RandomNormal(rand, groupCount, 20, 5);
            double[] ys3 = DataGen.RandomNormal(rand, groupCount, 20, 5);
            double[] err1 = DataGen.RandomNormal(rand, groupCount, 5, 2);
            double[] err2 = DataGen.RandomNormal(rand, groupCount, 5, 2);
            double[] err3 = DataGen.RandomNormal(rand, groupCount, 5, 2);

            var plt = new ScottPlot.Plot(600, 400);
            plt.AddBarGroups(
                groupLabels: groupNames,
                seriesLabels: seriesNames,
                ys: new double[][] { ys1, ys2, ys3 },
                yErr: new double[][] { err1, err2, err3 });

            plt.SetAxisLimits(yMin: 0);
            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            plt.XAxis.Grid(false);
            plt.XAxis.Grid(false);
            plt.Legend(location: Alignment.UpperRight);
            TestTools.SaveFig(plt);
        }
    }
}
