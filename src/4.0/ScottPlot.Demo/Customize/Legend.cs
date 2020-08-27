using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Customize
{
    class Legend
    {
        public class LegendDemo : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Legend Demo";
            public string description { get; } = "Demonstrates how various plot types appear in the legend.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);

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
                plt.PlotFunction(func, label: "function", lineWidth: 2);

                var func2 = new Func<double, double?>((x) => Math.Sin(x) * Math.Sin(10 * x) + 5);
                plt.PlotFunction(func2, label: null); // null labels will not appear in legend

                plt.PlotHLine(7.75, label: "horizontal line", lineStyle: LineStyle.Dot);
                plt.PlotVLine(7.75, label: "vertical line", lineStyle: LineStyle.Dash);

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

                plt.PlotPoint(1, 9, label: "point");
                plt.PlotArrow(8, 8, 8.5, 8.5, label: "arrow");

                plt.Axis(0, 13, -1, 11);
                plt.Legend();
                plt.Grid(false);
            }
        }
    }
}
