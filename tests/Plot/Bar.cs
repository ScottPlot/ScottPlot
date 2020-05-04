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

            string[] groupNames = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
            int groupCount = groupNames.Length;

            double[] xs = DataGen.Consecutive(groupCount);
            double[] ys1 = DataGen.RandomNormal(rand, groupCount, 20, 5);
            double[] ys2 = DataGen.RandomNormal(rand, groupCount, 20, 5);
            double[] ys3 = DataGen.RandomNormal(rand, groupCount, 20, 5);
            double[] err1 = DataGen.RandomNormal(rand, groupCount, 5, 2);
            double[] err2 = DataGen.RandomNormal(rand, groupCount, 5, 2);
            double[] err3 = DataGen.RandomNormal(rand, groupCount, 5, 2);

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotMultiBar(
                xs: new double[][] { xs, xs, xs }, 
                ys: new double[][] { ys1, ys2, ys3 }, 
                yErr: new double[][] { err1, err2, err3 });

            plt.XTicks(xs, groupNames);
            plt.Axis(y1: 0);
            plt.Grid(enableVertical: false, lineStyle: LineStyle.Dot);
            plt.Legend(location: legendLocation.upperRight);
            TestTools.SaveFig(plt);
        }
    }
}
