using NUnit.Framework;
using ScottPlot;

namespace ScottPlotTests.Renderable
{
    class Axis
    {
        [Test]
        public void Test_Renderable_Basic()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotSignal(DataGen.Sin(51));

            // TODO: make this smart enough to calculate itself
            plt.Layout(xLabelHeight: 40);

            plt.XAxis.Title = "Horizontal Axis";
            plt.YAxis.Title = "Vertical Axis";

            var bmp = new System.Drawing.Bitmap(600, 400);
            plt.Render(bmp);

            TestTools.SaveFig(bmp);
        }
    }
}
