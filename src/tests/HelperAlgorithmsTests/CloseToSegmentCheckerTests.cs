using NUnit.Framework;
using ScottPlot.Plottable.HelperAlgorithms;

namespace ScottPlotTests.HelperAlgorithmsTests
{
    [TestFixture]
    public class CloseToSegmentCheckerTests
    {
        [TestCase(9.9, 0.9)]
        [TestCase(5, 0.5)]
        [TestCase(5, -0.9)]
        [TestCase(3, 0.9)]
        [TestCase(0, 0)]
        public void IsClose_HorisontalSegmentPointsClose_ReturnTrue(double x, double y)
        {
            CloseToSegmentChecker checker = new CloseToSegmentChecker(x, y, 1, 1);
            var result = checker.IsClose(0, 0, 10, 0);
            Assert.True(result);
        }

        [TestCase(5, 5)]
        [TestCase(12, 0.3)]
        [TestCase(1000, 0)]
        [TestCase(0, 1000)]
        [TestCase(3, -5)]
        [TestCase(5, -1.05)]
        public void IsClose_HorisontalSegmentPointsFarFrom_ReturnFalse(double x, double y)
        {
            CloseToSegmentChecker checker = new CloseToSegmentChecker(x, y, 1, 1);
            var result = checker.IsClose(0, 0, 10, 0);
            Assert.False(result);
        }

        [TestCase(4.9, 9.7)]
        [TestCase(0, 20.5)]
        [TestCase(-5.5, 30)]
        [TestCase(-10, 39.3)]
        [TestCase(0.3, 20.7)]
        public void IsClose_SomeSegmentPointsClose_ReturnTrue(double x, double y)
        {
            CloseToSegmentChecker checker = new CloseToSegmentChecker(x, y, 1, 1);
            var result = checker.IsClose(5, 10, -10, 40);
            Assert.True(result);
        }

        [TestCase(0, 25.1)]
        [TestCase(5, 8)]
        [TestCase(-11.2, 40)]
        [TestCase(-5.5, 28.7)]
        [TestCase(10, 0)]
        [TestCase(1000, 0)]
        [TestCase(0, 1000)]
        public void IsClose_SomeSegmentPointsFarFrom_ReturnFalse(double x, double y)
        {
            CloseToSegmentChecker checker = new CloseToSegmentChecker(x, y, 1, 1);
            var result = checker.IsClose(5, 10, -10, 40);
            Assert.False(result);
        }
    }
}
