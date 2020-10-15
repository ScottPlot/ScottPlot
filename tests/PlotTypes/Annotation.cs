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
    }
}
