using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
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
        public void Test_Legend_ForEveryPlottableType()
        {
            Random rand = new Random(0);
            var plt = new ScottPlot.Plot();

            plt.PlotErrorBars(
                    xs: DataGen.Random(rand, 10, 10),
                    ys: DataGen.Random(rand, 10, 10),
                    xPositiveError: DataGen.Random(rand, 10),
                    xNegativeError: DataGen.Random(rand, 10),
                    yPositiveError: DataGen.Random(rand, 10),
                    yNegativeError: DataGen.Random(rand, 10),
                    label: "error bars"
                );

            var func = new Func<double, double?>((x) => Math.Sin(x) * Math.Sin(10 * x) + 3);
            plt.PlotFunction(func, label: "Sin(x) * Sin(10 * x) + 3");

            plt.PlotHLine(7.75, label: "horizontal line");
            plt.PlotVLine(7.75, label: "vertical line");
            plt.PlotHSpan(1.5, 2.5, label: "horizontal span");
            plt.PlotVSpan(1.5, 2.5, label: "vertical span");

            plt.PlotOHLC(new OHLC[]{
                new OHLC(5, 6, 4, 5.5, 1),
                new OHLC(6, 7.5, 3.5, 4.75, 1.5),
                new OHLC(5.5, 6, 3, 4.5, 2)
                });

            plt.PlotCandlestick(new OHLC[]{
                new OHLC(5, 6, 4, 5.5, 3),
                new OHLC(6, 7.5, 3.5, 4.75, 3.5),
                new OHLC(5.5, 6, 3, 4.5, 4)
                });

            plt.PlotScatter(
                xs: new double[] { 5, 5.5, 6, 7, 7, 6 },
                ys: new double[] { 7, 8, 7, 9, 7, 8 },
                lineStyle: LineStyle.Dash,
                lineWidth: 2,
                markerShape: MarkerShape.openCircle,
                markerSize: 10,
                label: "Scatter Plot"
                );

            plt.PlotSignal(
                ys: DataGen.RandomNormal(rand, 10),
                sampleRate: 5,
                xOffset: 3,
                yOffset: 8,
                label: "Signal Plot"
                );

            plt.PlotText("ScottPlot", 6, 6, rotation: 25, fontSize: 14, bold: true);

            plt.Axis(0, 10, 0, 10);
            plt.Legend();

            TestTools.SaveFig(plt);
        }
    }
}
