using NUnit.Framework;
using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlotTests.MinMaxSearchStrategies
{
    public class LinearDoubleOnlyMinMaxSearchStrategyTests
    {
        public virtual IMinMaxSearchStrategy<double> CreateStrategy()
        {
            return new LinearDoubleOnlyMinMaxStrategy();
        }

        [Test]
        public void SourceArray_SetThenGetBack_ReturnSameArray()
        {
            double[] sourceArray = new double[] { 5.92, 19.12 - 34, 91, -12 };
            var strategy = CreateStrategy();
            strategy.SourceArray = sourceArray;
            Assert.AreEqual(sourceArray, strategy.SourceArray);
        }


        [Test]
        public void SourceElement_RequestSomeElements_ReturnCorrect()
        {
            double[] sourceArray = new double[] { 1.0, 2, 3, 4.12, 5, -42.55, 7 };
            var strategy = CreateStrategy();
            strategy.SourceArray = sourceArray;
            Assert.AreEqual(1.0, strategy.SourceElement(0));
            Assert.AreEqual(3, strategy.SourceElement(2));
            Assert.AreEqual(-42.55, strategy.SourceElement(5));
        }

        [Test]
        public void UpdateElement_SomeUpdates_UpdatedCorrect()
        {
            double[] sourceArray = new double[] { 1.0, 2, 3, 4, 5, 6, 7 };
            var strategy = CreateStrategy();
            strategy.SourceArray = sourceArray;
            strategy.updateElement(3, 7.5);
            strategy.updateElement(0, -15);
            strategy.updateElement(6, 0);
            double[] expected = new double[] { -15, 2, 3, 7.5, 5, 6, 0 };
            Assert.AreEqual(expected, strategy.SourceArray);
        }


        [Test]
        public void UpdateRange_SomeUpdate_UpdatedCorrect()
        {
            double[] sourceArray = new double[] { 1.0, 2, 3, 4, 5, 6, 7 };
            double[] range1 = new double[] { -1, -2, -3, -5, -9 };
            double[] range2 = new double[] { 1_000_000, -500_000 };
            var strategy = CreateStrategy();
            strategy.SourceArray = sourceArray;

            strategy.updateRange(1, 4, range1, 1);
            double[] expectedRange1Update = new double[] { 1.0, -2, -3, -5, 5, 6, 7 };
            Assert.AreEqual(expectedRange1Update, strategy.SourceArray);

            strategy.updateRange(4, 6, range2);
            double[] expectedRange2Update = new double[] { 1, -2, -3, -5, 1_000_000, -500_000, 7 };
            Assert.AreEqual(expectedRange2Update, strategy.SourceArray);
        }


        [Test]
        public void MinMaxRangeQuerry_SmallTestArrayQuerry1_ReturnCorrect()
        {
            double[] sourceArray = new double[] { 1, -5, 15, 16, 14, 3, -1000, 19, 19, 18, 19 };
            var strategy = CreateStrategy();
            strategy.SourceArray = sourceArray;

            double min, max;
            strategy.MinMaxRangeQuery(0, 1, out min, out max);
            Assert.AreEqual(-5, min);
            Assert.AreEqual(1, max);
        }

        [Test]
        public void MinMaxRangeQuerry_SmallTestSomeQuerry2_ReturnCorrect()
        {
            double[] sourceArray = new double[] { 1, -5, 15, 16, 14, 3, -1000, 19, 19, 18, 19 };
            var strategy = CreateStrategy();
            strategy.SourceArray = sourceArray;

            double min, max;
            strategy.MinMaxRangeQuery(2, 5, out min, out max);
            Assert.AreEqual(3, min);
            Assert.AreEqual(16, max);
        }

        [Test]
        public void MinMaxRangeQuerry_SmallTestSomeQuerry3_ReturnCorrect()
        {
            double[] sourceArray = new double[] { 1, -5, 15, 16, 14, 3, -1000, 19, 19, 18, 19 };
            var strategy = CreateStrategy();
            strategy.SourceArray = sourceArray;

            double min, max;
            strategy.MinMaxRangeQuery(6, 6, out min, out max);
            Assert.AreEqual(-1000, min);
            Assert.AreEqual(-1000, max);
        }
    }
}
