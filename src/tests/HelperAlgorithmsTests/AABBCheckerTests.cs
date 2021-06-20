using NUnit.Framework;
using ScottPlot.Plottable.HelperAlgorithms;

namespace ScottPlotTests.HelperAlgorithmsTests
{
    [TestFixture]
    public class AABBCheckerTests
    {
        [TestCase(5, 2)]
        [TestCase(0, 0)]
        [TestCase(1, -7)]
        [TestCase(5, 10)]
        [TestCase(-5, 10)]
        [TestCase(-5, -10)]
        public void CheckInsideAABB_SomeAABBPointsInside_ReturnTrue(double x, double y)
        {
            AABBChecker checker = new AABBChecker(0, 0, 5, 10);
            var result = checker.CheckInsideAABB(x, y);
            Assert.True(result);
        }

        [TestCase(6, 0)]
        [TestCase(-1000, 1)]
        [TestCase(42, 42)]
        [TestCase(12, 3)]
        [TestCase(5.0001, 0)]
        public void CheckInsideAABB_SomeAABBPointsOutside_ReturnFalse(double x, double y)
        {
            AABBChecker checker = new AABBChecker(0, 0, 5, 10);
            var result = checker.CheckInsideAABB(x, y);
            Assert.False(result);
        }

        [TestCase(11, -7)]
        [TestCase(15, -10)]
        [TestCase(19, -11)]
        [TestCase(15, -9)]
        [TestCase(20, -12)]
        [TestCase(10, -6)]
        public void CheckInsideAABB_OffsetedAABBPointsInside_ReturnTrue(double x, double y)
        {
            AABBChecker checker = new AABBChecker(15, -9, 5, 3);
            var result = checker.CheckInsideAABB(x, y);
            Assert.True(result);
        }

        [TestCase(0, 0)]
        [TestCase(9, -9)]
        [TestCase(42, 42)]
        [TestCase(20, 20)]
        [TestCase(20.001, -9)]
        public void CheckInsideAABB_OffsetedAABBPointsOutside_ReturnFalse(double x, double y)
        {
            AABBChecker checker = new AABBChecker(15, -9, 5, 3);
            var result = checker.CheckInsideAABB(x, y);
            Assert.False(result);
        }
    }
}
