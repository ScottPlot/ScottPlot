using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Legend
{
    class LegendTests
    {
        [Test]
        public void Test_Legend_LooksGoodInEveryPosition()
        {

            var mplt = new MultiPlot(1000, 800, 3, 3);

            legendLocation[] locs = Enum.GetValues(typeof(legendLocation)).Cast<legendLocation>().ToArray();
            for (int i = 0; i < locs.Length; i++)
            {
                var plt = mplt.subplots[i];
                plt.PlotScatter(DataGen.Consecutive(20), DataGen.Sin(20), markerShape: MarkerShape.filledSquare, label: "sin");
                plt.PlotScatter(DataGen.Consecutive(20), DataGen.Cos(20), markerShape: MarkerShape.openDiamond, label: "cos");
                plt.Legend(location: locs[i]);
                plt.Title(locs[i].ToString());
            }

            TestTools.SaveFig(mplt);
        }

        [Test]
        public void Test_Legend_ReverseOrder()
        {
            var plt1 = new ScottPlot.Plot();
            plt1.PlotSignal(DataGen.Sin(100), label: "sin");
            plt1.PlotSignal(DataGen.Cos(100), label: "cos");
            plt1.Legend();
            string hash1 = ScottPlot.Tools.BitmapHash(plt1.GetBitmap());

            var plt2 = new ScottPlot.Plot();
            plt2.PlotSignal(DataGen.Sin(100), label: "sin");
            plt2.PlotSignal(DataGen.Cos(100), label: "cos");
            plt2.Legend(reverseOrder: true);
            string hash2 = ScottPlot.Tools.BitmapHash(plt2.GetBitmap());

            TestTools.SaveFig(plt1, "standard");
            TestTools.SaveFig(plt2, "reversed");

            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Test_Signal_ColorAndStyle()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotSignal(DataGen.Sin(10), label: "sin", color: Color.Red, lineStyle: LineStyle.Dash, lineWidth: 1);
            plt.PlotSignal(DataGen.Cos(10), label: "cos", color: Color.Blue, lineStyle: LineStyle.Dot, lineWidth: 3, markerSize: 10);
            plt.Legend();
            TestTools.SaveFig(plt);
        }
    }
}
