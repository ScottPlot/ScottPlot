using NUnit.Framework;
using ScottPlot;

namespace ScottPlotTests
{
    public class Layout
    {
        [Test]
        public void Test_Layout()
        {
            int pointCount = 50;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot();
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);

            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Layout_LabelsWithLineBreaks()
        {
            int pointCount = 50;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot();
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);

            string labelWithLineBreak = "Line One\nLine Two";
            plt.Title(labelWithLineBreak, fontSize: 30);
            plt.XLabel(labelWithLineBreak);
            plt.YLabel(labelWithLineBreak);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Layout_RotatedTicksWithRoom()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotSignal(DataGen.Sin(51), xOffset: 1e6);
            plt.PlotSignal(DataGen.Cos(51), xOffset: 1e6);

            // WARNING: this resets the layout, so you must call Layout() after this
            plt.XLabel("horizontal axis label");

            plt.Ticks(xTickRotation: 45);
            plt.Layout(xScaleHeight: 50);

            TestTools.SaveFig(plt);
        }
    }
}
