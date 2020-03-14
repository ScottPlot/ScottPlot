using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Statistics
{
    public class Regression
    {
        private (double[] xs, double[] ys) GetNoisyLinearData_EvenlySpaced(int pointCount, double actualSlope, double actualOffset, bool display = true)
        {
            double[] ys = ScottPlot.DataGen.NoisyLinear(new Random(0), pointCount, actualSlope, actualOffset, noise: 50);
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);

            if (display)
                for (int i = 0; i < ys.Length; i++)
                    Console.WriteLine($"{xs[i]}, {ys[i]}");

            return (xs, ys);
        }

        [Test]
        public void Test_LinearRegression_EvenlySpacedXs()
        {
            int pointCount = 50;
            double actualSlope = 3;
            double actualOffset = 200;
            (double[] xs, double[] ys)  = GetNoisyLinearData_EvenlySpaced(pointCount, actualSlope, actualOffset);

            // fit the random data with the linear regression model
            var model = new ScottPlot.Statistics.LinearRegressionLine(ys, firstX: 0, xSpacing: 1);

            // plot to visually assess goodness of fit
            var plt = new ScottPlot.Plot(450, 300);
            plt.Title($"Y = {model.slope:0.0000}x + {model.offset:0.0}\nR² = {model.rSquared:0.0000}");
            plt.PlotScatter(xs, ys, lineWidth: 0);
            plt.PlotLine(model.slope, model.offset, (xs.First(), xs.Last()), lineWidth: 2);
            TestTools.SaveFig(plt);

            // ensure the fit is good
            Assert.AreEqual(actualSlope, model.slope, .1);
            Assert.AreEqual(actualOffset, model.offset, 10);
        }
    }
}
