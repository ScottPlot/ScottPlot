using NUnit.Framework;
using ScottPlot;

namespace ScottPlotTests.Renderable
{
    class Axis
    {
        [Test]
        public void Test_Renderable_Basic()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotSignal(DataGen.Sin(51));

            double[] positions = { 1, 2, 3 };
            double[] minorPositions = { .5, 1.5, 2.5, 3.5 };
            string[] labels = { "1", "2", "3" };

            var newAxisX = new ScottPlot.Renderable.Axis(positions, labels, minorPositions)
            {

            };
            plt.Add(newAxisX);

            TestTools.SaveFig(plt);
        }
    }
}
