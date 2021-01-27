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
        public void Test_Annotation_Coordinates()
        {
            var plt = new ScottPlot.Plot(400, 300);

            // negative coordinates snap text to the lower or right edges
            plt.AddAnnotation("Top Left", 10, 10);
            plt.AddAnnotation("Lower Left", 10, -10);
            plt.AddAnnotation("Top Right", -10, 10);
            plt.AddAnnotation("Lower Right", -10, -10);

            // customization of style
            var a = plt.AddAnnotation("Fancy Annotation", 10, 40);
            a.Font.Size = 24;
            a.Font.Name = "Impact";
            a.Font.Color = Color.Red;
            a.Shadow = true;
            a.Background = true;
            a.BackgroundColor = Color.White;
            a.BorderWidth = 2;

            TestTools.SaveFig(plt);
        }
    }
}
