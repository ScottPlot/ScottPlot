using NUnit.Framework;

namespace ScottPlotTests.Plot
{
    internal class GetCoordinate
    {
        [Test]
        public void Test_GetUnit_ReturnsCorrectValue()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.Render();
            var limits = plt.GetAxisLimits();

            // right edge
            Assert.Greater(plt.GetCoordinateX(plt.Width), limits.XCenter);

            // left edge
            Assert.Less(plt.GetCoordinateX(0), limits.XCenter);

            // top edge
            Assert.Greater(plt.GetCoordinateY(0), limits.YCenter);

            // bottom edge
            Assert.Less(plt.GetCoordinateY(plt.Height), limits.YCenter);
        }
    }
}
