using NUnit.Framework;
using ScottPlot.MinMaxSearchStrategies;
using System;

namespace ScottPlotTests.MinMaxSearchStrategies
{
    [TestFixture]
    public class SegmentedTreeMinMaxSearchStrategyTests
    {
        public virtual IMinMaxSearchStrategy<T> CreateStrategy<T>() where T : struct, IComparable
        {
            return new SegmentedTreeMinMaxSearchStrategy<T>();
        }

        [Test]
        public void SourceArrayDouble_SetThenGetBack_ReturnSameArray()
        {
            double[] sourceArray = new double[] { 5.92, 19.12 - 34, 91, -12 };
            var strategy = CreateStrategy<double>();
            strategy.SourceArray = sourceArray;
            Assert.AreEqual(sourceArray, strategy.SourceArray);
        }

        [Test]
        public void SourceArrayInt_SetThenGetBack_ReturnSameArray()
        {
            int[] sourceArray = new int[] { 5, 19, -34, 91, -12 };
            var strategy = CreateStrategy<int>();
            strategy.SourceArray = sourceArray;
            Assert.AreEqual(sourceArray, strategy.SourceArray);
        }

        [Test]
        public void SourceElementDouble_RequestSomeElements_ReturnCorrect()
        {
            double[] sourceArray = new double[] { 1.0, 2, 3, 4.12, 5, -42.55, 7 };
            var strategy = CreateStrategy<double>();
            strategy.SourceArray = sourceArray;
            Assert.AreEqual(1.0, strategy.SourceElement(0));
            Assert.AreEqual(3, strategy.SourceElement(2));
            Assert.AreEqual(-42.55, strategy.SourceElement(5));
        }

        [Test]
        public void SourceElementInt_RequestSomeElements_ReturnCorrectDouble()
        {
            int[] sourceArray = new int[] { 1, 2, 3, 7, 5, -42, 7 };
            var strategy = CreateStrategy<int>();
            strategy.SourceArray = sourceArray;
            Assert.AreEqual(1.0, strategy.SourceElement(0));
            Assert.AreEqual(3.0, strategy.SourceElement(2));
            Assert.AreEqual(-42.0, strategy.SourceElement(5));
        }

        [Test]
        public void UpdateElementDouble_SomeUpdates_UpdatedCorrect()
        {
            double[] sourceArray = new double[] { 1.0, 2, 3, 4, 5, 6, 7 };
            var strategy = CreateStrategy<double>();
            strategy.SourceArray = sourceArray;
            strategy.updateElement(3, 7.5);
            strategy.updateElement(0, -15);
            strategy.updateElement(6, 0);
            double[] expected = new double[] { -15, 2, 3, 7.5, 5, 6, 0 };
            Assert.AreEqual(expected, strategy.SourceArray);
        }

        [Test]
        public void UpdateElementFloat_SomeUpdates_UpdatedCorrect()
        {
            float[] sourceArray = new float[] { 1.0f, 2, 3, 4, 5, 6, 7 };
            var strategy = CreateStrategy<float>();
            strategy.SourceArray = sourceArray;
            strategy.updateElement(3, 7.5f);
            strategy.updateElement(0, -15);
            strategy.updateElement(6, 0);
            float[] expected = new float[] { -15, 2, 3, 7.5f, 5, 6, 0 };
            Assert.AreEqual(expected, strategy.SourceArray);
        }

        [Test]
        public void UpdateRangeDouble_SomeUpdate_UpdatedCorrect()
        {
            double[] sourceArray = new double[] { 1.0, 2, 3, 4, 5, 6, 7 };
            double[] range1 = new double[] { -1, -2, -3, -5, -9 };
            double[] range2 = new double[] { 1_000_000, -500_000 };
            var strategy = CreateStrategy<double>();
            strategy.SourceArray = sourceArray;

            strategy.updateRange(1, 4, range1, 1);
            double[] expectedRange1Update = new double[] { 1.0, -2, -3, -5, 5, 6, 7 };
            Assert.AreEqual(expectedRange1Update, strategy.SourceArray);

            strategy.updateRange(4, 6, range2);
            double[] expectedRange2Update = new double[] { 1, -2, -3, -5, 1_000_000, -500_000, 7 };
            Assert.AreEqual(expectedRange2Update, strategy.SourceArray);
        }

        [Test]
        public void UpdateRangeInt_SomeUpdate_UpdatedCorrect()
        {
            int[] sourceArray = new int[] { 1, 2, 3, 4, 5, 6, 7 };
            int[] range1 = new int[] { -1, -2, -3, -5, -9 };
            int[] range2 = new int[] { 1_000_000, -500_000 };
            var strategy = CreateStrategy<int>();
            strategy.SourceArray = sourceArray;

            strategy.updateRange(1, 4, range1, 1);
            int[] expectedRange1Update = new int[] { 1, -2, -3, -5, 5, 6, 7 };
            Assert.AreEqual(expectedRange1Update, strategy.SourceArray);

            strategy.updateRange(4, 6, range2);
            int[] expectedRange2Update = new int[] { 1, -2, -3, -5, 1_000_000, -500_000, 7 };
            Assert.AreEqual(expectedRange2Update, strategy.SourceArray);
        }

        [Test]
        public void MinMaxRangeQuerryDouble_SmallTestArrayQuerry1_ReturnCorrect()
        {
            double[] sourceArray = new double[] { 1, -5, 15, 16, 14, 3, -1000, 19, 19, 18, 19 };
            var strategy = CreateStrategy<double>();
            strategy.SourceArray = sourceArray;

            double min, max;
            strategy.MinMaxRangeQuery(0, 1, out min, out max);
            Assert.AreEqual(-5, min);
            Assert.AreEqual(1, max);
        }

        [Test]
        public void MinMaxRangeQuerryDouble_SmallTestSomeQuerry2_ReturnCorrect()
        {
            double[] sourceArray = new double[] { 1, -5, 15, 16, 14, 3, -1000, 19, 19, 18, 19 };
            var strategy = CreateStrategy<double>();
            strategy.SourceArray = sourceArray;

            double min, max;
            strategy.MinMaxRangeQuery(2, 5, out min, out max);
            Assert.AreEqual(3, min);
            Assert.AreEqual(16, max);
        }

        [Test]
        public void MinMaxRangeQuerryDouble_SmallTestSomeQuerry3_ReturnCorrect()
        {
            double[] sourceArray = new double[] { 1, -5, 15, 16, 14, 3, -1000, 19, 19, 18, 19 };
            var strategy = CreateStrategy<double>();
            strategy.SourceArray = sourceArray;

            double min, max;
            strategy.MinMaxRangeQuery(6, 6, out min, out max);
            Assert.AreEqual(-1000, min);
            Assert.AreEqual(-1000, max);
        }

        [Test]
        public void MinMaxRangeQuerryDouble_Issue783Bug_ReturnCorrect()
        {
            double[] sourceArray = new double[] { 105.02, 104.82, 104.84, 104.84 };
            var strategy = CreateStrategy<double>();
            strategy.SourceArray = sourceArray;

            double min, max;
            strategy.MinMaxRangeQuery(1, 2, out min, out max);
            Assert.AreEqual(104.82, min);
            Assert.AreEqual(104.84, max);
        }
    }
}
