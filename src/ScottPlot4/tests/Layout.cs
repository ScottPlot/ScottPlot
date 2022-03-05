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
            plt.AddScatter(dataXs, dataSin);
            plt.AddScatter(dataXs, dataCos);

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
            plt.AddScatter(dataXs, dataSin);
            plt.AddScatter(dataXs, dataCos);

            string labelWithLineBreak = "Line One\nLine Two";
            plt.XAxis2.Label(label: labelWithLineBreak, size: 30);
            plt.XAxis.Label(labelWithLineBreak);
            plt.YAxis.Label(labelWithLineBreak);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Layout_RotatedTicksWithRoom()
        {
            var plt = new ScottPlot.Plot(400, 300);
            var s1 = plt.AddSignal(DataGen.Sin(51));
            var s2 = plt.AddSignal(DataGen.Cos(51));
            s1.OffsetX = 1e6;
            s2.OffsetX = 1e6;

            // WARNING: this resets the layout, so you must call Layout() after this
            plt.XLabel("horizontal axis label");

            plt.XAxis.TickLabelStyle(rotation: 45);
            plt.Layout(bottom: 50);

            TestTools.SaveFig(plt);
        }
    }
}
