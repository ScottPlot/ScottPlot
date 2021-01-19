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
            int smallestValidN = 1;
            double nthValue = ScottPlot.Statistics.Common.NthOrderStatistic(RandomValues, smallestValidN);
            double minValue = SortedValues.First();
            Assert.AreEqual(minValue, nthValue);
            Assert.Throws<ArgumentException>(() => ScottPlot.Statistics.Common.NthOrderStatistic(RandomValues, smallestValidN - 1));
        }

        [Test]
        public void Test_NthOrderStatistic_MaxValue()
        {
            int largestValidN = RandomValues.Length;
            double nthValue = ScottPlot.Statistics.Common.NthOrderStatistic(RandomValues, largestValidN);
            double maxValue = SortedValues.Last();
            Assert.AreEqual(maxValue, nthValue);
            Assert.Throws<ArgumentException>(() => ScottPlot.Statistics.Common.NthOrderStatistic(RandomValues, largestValidN + 1));
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

        [Test]
        [TestCase(100)]
        [TestCase(50)]
        [TestCase(256)]
        [TestCase(77)]
        [TestCase(123)]
        public void Test_FloatPercentile(int n)
        {
            Random rand = new Random(0);
            double[] values = ScottPlot.DataGen.Random(rand, pointCount: n);
            Assert.AreEqual(ScottPlot.Statistics.Common.Percentile(values, 25), ScottPlot.Statistics.Common.Quartile(values, 1));
            Assert.AreEqual(ScottPlot.Statistics.Common.Percentile(values, 50), ScottPlot.Statistics.Common.Quartile(values, 2));
            Assert.AreEqual(ScottPlot.Statistics.Common.Percentile(values, 75), ScottPlot.Statistics.Common.Quartile(values, 3));
        }

        //[Test]
        public void Test_RandomIntGenerator_IncludesLowerAndExcludesUpper()
        {
            int replicates = 1000;
            int[] randomInts = new int[replicates];
            for (int i = 0; i < randomInts.Length; i++)
                randomInts[i] = ScottPlot.Statistics.Common.GetRandomInt(0, 5);

            Assert.AreEqual(0, randomInts.Min());
            Assert.AreEqual(4, randomInts.Max());
        }
    }
}
