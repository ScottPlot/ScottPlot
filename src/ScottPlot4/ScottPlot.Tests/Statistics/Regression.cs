using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Statistics
{
    public class Regression
    {
        private (double[] xs, double[] ys) GetNoisyLinearData_EvenlySpaced(int pointCount, double actualSlope, double actualOffset, bool display = true, double noiseLevel = 50)
        {
            Random rand = new Random(0);
            double[] ys = ScottPlot.DataGen.NoisyLinear(rand, pointCount, actualSlope, actualOffset, noise: noiseLevel);
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);

            if (display)
                for (int i = 0; i < ys.Length; i++)
                    Console.WriteLine($"{xs[i]}, {ys[i]}");

            return (xs, ys);
        }

        private (double[] xs, double[] ys) GetNoisyLinearData_RandomlySpaced(int pointCount, double actualSlope, double actualOffset, bool display = true, double noiseLevel = 50)
        {
            Random rand = new Random(0);
            double[] xs = ScottPlot.DataGen.RandomNormal(rand, pointCount, mean: pointCount / 2, stdDev: Math.Sqrt(pointCount));

            double[] ys = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
                ys[i] = actualSlope * xs[i] + actualOffset + (rand.NextDouble() - .5) * noiseLevel;

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
            (double[] xs, double[] ys) = GetNoisyLinearData_EvenlySpaced(pointCount, actualSlope, actualOffset, noiseLevel: 15);

            // fit the random data with the linear regression model
            var model = new ScottPlot.Statistics.LinearRegressionLine(ys, firstX: 0, xSpacing: 1);

            // plot to visually assess goodness of fit
            var plt = new ScottPlot.Plot(450, 300);
            plt.Title($"Y = {model.slope:0.0000}x + {model.offset:0.0}\nR² = {model.rSquared:0.0000}");
            plt.AddScatterPoints(xs, ys);
            // ADD THIS BACK IN!
            //plt.PlotLine(model.slope, model.offset, (xs.Min(), xs.Max()), lineWidth: 2, label: "model", lineStyle: ScottPlot.LineStyle.Dash);
            //plt.PlotLine(actualSlope, actualOffset, (xs.Min(), xs.Max()), lineWidth: 2, label: "actual");
            plt.Legend();
            TestTools.SaveFig(plt);

            // ensure the fit is good
            Assert.AreEqual(actualSlope, model.slope, .25);
            Assert.AreEqual(actualOffset, model.offset, 10);
        }

        [Test]
        public void Test_LinearRegression_RandomXs()
        {
            int pointCount = 50;
            double actualSlope = 3;
            double actualOffset = 200;
            (double[] xs, double[] ys) = GetNoisyLinearData_RandomlySpaced(pointCount, actualSlope, actualOffset, noiseLevel: 15);

            // fit the random data with the linear regression model
            var model = new ScottPlot.Statistics.LinearRegressionLine(xs, ys);

            // plot to visually assess goodness of fit
            var plt = new ScottPlot.Plot(450, 300);
            plt.Title($"Y = {model.slope:0.0000}x + {model.offset:0.0}\nR² = {model.rSquared:0.0000}");
            plt.AddScatterPoints(xs, ys);
            // ADD THIS BACK IN!
            //plt.PlotLine(model.slope, model.offset, (xs.Min(), xs.Max()), lineWidth: 2, label: "model", lineStyle: ScottPlot.LineStyle.Dash);
            //plt.PlotLine(actualSlope, actualOffset, (xs.Min(), xs.Max()), lineWidth: 2, label: "actual");
            plt.Legend();
            TestTools.SaveFig(plt);

            // ensure the fit is good
            Assert.AreEqual(actualSlope, model.slope, .25);
            Assert.AreEqual(actualOffset, model.offset, 10);
        }

        [Test]
        public void GetCoefficients_FirstXNotZero_ResultEqualtoTI_84_Plus()
        {
            double[] x = new double[] { 1, 2, 3, 4, 5, 6, 7 };
            double[] y = new double[] { 2, 2, 3, 3, 3.8, 4.2, 4 };
            var reg = new ScottPlot.Statistics.LinearRegressionLine(x, y);
            Assert.AreEqual(0.4, reg.slope, 0.0001);
            Assert.AreEqual(1.5428, reg.offset, 0.0001);
        }
    }
}
