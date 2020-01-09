using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace ScottPlotTests
{
    class Statistics
    {
        private (double, double, double[], double[]) GetRandomRegressionData(Random rand)
        {
            double slope = rand.Next(-1000, 1000) / 100.0;
            double offset = rand.Next(-1000, 1000);
            int pointCount = rand.Next(50, 1000);
            double[] ys = ScottPlot.DataGen.NoisyLinear(
                rand: rand,
                pointCount: pointCount,
                slope: slope,
                offset: offset,
                noise: 1 // intentionally low noise to get a really good fit
            );
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);
            return (slope, offset, xs, ys);
        }

        [Test]
        public void Test_LinearRegression_EvenlySpacedXs()
        {
            Random rand = new Random(0);
            for (int i = 0; i < 1000; i++)
            {
                // create random data
                (double slope, double offset, double[] xs, double[] data) = GetRandomRegressionData(rand);


                // fit the random data with the linear regression model
                var model = new ScottPlot.Statistics.LinearRegressionLine(data, 0, 1);

                // ensure the model's coeffecients are similar to the real ones
                double slopeError = Math.Abs(slope - model.slope);
                double offsetError = Math.Abs(offset - model.offset);
                Assert.That(slopeError, Is.LessThan(.1));
                Assert.That(offsetError, Is.LessThan(10));
            }
        }
    }
}
