using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.PlotTypes
{
    public class Polygon
    {
        [Test]
        public void Test_ManualPolygon_Render()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();

            for (int i = 0; i < 5; i++)
            {
                var color = plt.GetSettings(false).GetNextColor();

                var plottable = new ScottPlot.PlottablePolygon(
                        xs: ScottPlot.DataGen.Random(rand, 3, 100),
                        ys: ScottPlot.DataGen.Random(rand, 3, 100),
                        label: $"polygon {i + 1}",
                        lineWidth: 2, 
                        lineColor: color, 
                        fill: true, 
                        fillColor: color,
                        fillAlpha: .5
                    );

                plt.Add(plottable);
            }

            plt.Title("Polygon Example");
            plt.Legend(location: ScottPlot.legendLocation.lowerLeft);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_PlotPolygon_Render()
        {
            Random rand = new Random(0);

            var plt = new ScottPlot.Plot();

            for (int i = 0; i < 5; i++)
            {
                double[] xs = ScottPlot.DataGen.Random(rand, 3, 100);
                double[] ys = ScottPlot.DataGen.Random(rand, 3, 100);
                plt.PlotPolygon(xs, ys, $"polygon {i + 1}", 
                    lineWidth: 2, lineColor: Color.Black, fillAlpha: .5);
            }

            plt.Title("Polygon Example");
            plt.Legend(location: ScottPlot.legendLocation.lowerLeft);
            TestTools.SaveFig(plt);
        }
    }
}
