using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Statistics
{
    class Interpolation
    {
        [Test]
        public void Test_Interpolation_SmoothData()
        {
            Random rand = new(0);
            (double[] xs, double[] ys) = GetSmoothpData(161);
            (double[] xs2, double[] ys2) = ScottPlot.Statistics.Interpolation.Cubic.InterpolateXY(xs, ys, 20);

            ScottPlot.Plot plt = new();
            plt.AddScatterPoints(xs, ys, label: "original");
            plt.AddScatterLines(xs2, ys2, label: "interpolation");
            plt.Legend();

            TestTools.SaveFig(plt);
        }

        private (double[] xs, double[] ys) GetSharpData(int count)
        {
            // this dataset has been reported to cause problems
            // https://github.com/ScottPlot/ScottPlot/issues/1433
            Random rand = new(0);
            double[] xs = new double[count];
            double[] ys = new double[count];
            for (int i = 0; i < count; i++)
            {
                xs[i] = -85 + i * 11.43;
                ys[i] = rand.Next(-30, -20);
            }
            return (xs, ys);
        }

        private (double[] xs, double[] ys) GetSmoothpData(int count)
        {
            Random rand = new(0);
            double[] xs = ScottPlot.DataGen.RandomWalk(rand, count);
            double[] ys = ScottPlot.DataGen.RandomWalk(rand, count);
            return (xs, ys);
        }

        [Test]
        public void Test_Interpolation_ProblematicData()
        {
            (double[] xs, double[] ys) = GetSharpData(161);
            (double[] xs2, double[] ys2) = ScottPlot.Statistics.Interpolation.Cubic.InterpolateXY(xs, ys, 20);

            ScottPlot.Plot plt = new();
            plt.AddScatterPoints(xs, ys, label: "original");
            plt.AddScatterLines(xs2, ys2, label: "interpolation");
            plt.Legend();

            TestTools.SaveFig(plt);
        }
    }
}
