using NUnit.Framework;
using System;
using System.Linq;

namespace ScottPlotTests.Statistics
{
    class OrderStatistics
    {
        double[] RandomValues;
        double[] SortedValues;

        [OneTimeSetUp]
        public void SetUp()
        {
            Random rand = new Random(0);
            RandomValues = ScottPlot.DataGen.Random(rand, pointCount: 5000);
            SortedValues = RandomValues.OrderBy(x => x).ToArray();
        }

        [Test]
        public void Test_NthOrderStatistic_ReturnsNthElementForRandomNs()
        {
            Random rand = new Random(0);
            var randomNs = Enumerable.Range(0, 100).Select(x => rand.Next(1, RandomValues.Length - 1));

            foreach (int n in randomNs)
            {
                double nthValue = ScottPlot.Statistics.Common.NthOrderStatistic(RandomValues, n);
                double expectedNthValue = SortedValues[n - 1];
                Assert.AreEqual(expectedNthValue, nthValue);
            }
        }

        [Test]
        public void Test_NthOrderStatistic_MinValue()
        {
            int n = 1;
            double nthValue = ScottPlot.Statistics.Common.NthOrderStatistic(RandomValues, n);
            double minValue = SortedValues.First();
            Assert.AreEqual(minValue, nthValue);
        }

        [Test]
        public void Test_NthOrderStatistic_MaxValue()
        {
            int n = RandomValues.Length;
            double nthValue = ScottPlot.Statistics.Common.NthOrderStatistic(RandomValues, n);
            double maxValue = SortedValues.Last();
            Assert.AreEqual(maxValue, nthValue);
        }

        [Test]
        public void Test_0thPercentileIsMin()
        {
            int n = 0;
            double nthValue = ScottPlot.Statistics.Common.Percentile(RandomValues, n);
            double minValue = SortedValues.First();
            Assert.AreEqual(minValue, nthValue);
        }

        [Test]
        public void Test_100thPercentileIsMax()
        {
            int n = 100;
            double nthValue = ScottPlot.Statistics.Common.Percentile(RandomValues, n);
            double maxValue = SortedValues.Last();
            Assert.AreEqual(maxValue, nthValue);
        }

        [Test]
        public void Test_MedianOdd()
        {
            int n = 101;
            double[] values = ScottPlot.DataGen.Random(new Random(0), n);
            double[] valuesSorted = values.OrderBy(x => x).ToArray();

            Assert.AreEqual(ScottPlot.Statistics.Common.Median(values), valuesSorted[n / 2]);
        }

        [Test]
        public void Test_MedianEven()
        {
            int n = 100;
            double[] values = ScottPlot.DataGen.Random(new Random(0), n);
            double[] valuesSorted = values.OrderBy(x => x).ToArray();

            Assert.AreEqual(ScottPlot.Statistics.Common.Median(values), (valuesSorted[n / 2] + valuesSorted[n / 2 - 1]) / 2);
        }
    }
}
