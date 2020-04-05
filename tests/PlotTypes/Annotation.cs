using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlotTests.PlotTypes
{
    public class Annotation
    {
        [Test]
        public void Test_Annotation_Plottable()
        {
            var plottable = new ScottPlot.PlottableAnnotation(
                    xPixel: 50,
                    yPixel: 10,
                    label: "seems like it works",
                    fontSize: 16,
                    fontName: "Arial Narrow",
                    fontColor: Color.Magenta,
                    fill: true,
                    fillColor: Color.Green,
                    lineWidth: 2,
                    lineColor: Color.Blue,
                    shadow: true
                );

            var plt = new ScottPlot.Plot(400, 300);
            plt.Add(plottable);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Annotation_PlotModule()
        {
            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotAnnotation("Annotation Test");
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Annotation_Coordinates()
        {
            var plt = new ScottPlot.Plot(400, 300);

            // negative coordinates snap text to the lower or right edges
            plt.PlotAnnotation("Top Left", 10, 10);
            plt.PlotAnnotation("Lower Left", 10, -10);
            plt.PlotAnnotation("Top Right", -10, 10);
            plt.PlotAnnotation("Lower Right", -10, -10);

            // arguments allow customization of style
            plt.PlotAnnotation("Fancy Annotation", 10, 40,
                fontSize: 24, fontName: "Impact", fontColor: Color.Red, shadow: true,
                fill: true, fillColor: Color.White, fillAlpha: 1, lineWidth: 2);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Annotation_BesideLegend()
        {
            Random rand = new Random();
            double[] dataA = ScottPlot.DataGen.RandomNormal(rand, 50);
            double[] dataB = ScottPlot.DataGen.RandomNormal(rand, 50);

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotSignal(dataA, label: "data A");
            plt.PlotSignal(dataB, label: "data B");
            plt.Legend();

            plt.GetBitmap(true); // force a render to force drawing/placing the legend

            double legendLocationX = plt.GetSettings(false).legend.rect.Location.X;
            double legendDistanceFromRightEdge = plt.GetSettings(false).dataSize.Width - legendLocationX;
            plt.PlotAnnotation("text beside\nthe legend", 
                    xPixel: -legendDistanceFromRightEdge - 5, 
                    yPixel: -10,
                    fillColor: Color.White,
                    fillAlpha: 1,
                    shadow: true
                );

            TestTools.SaveFig(plt);
        }
    }
}
