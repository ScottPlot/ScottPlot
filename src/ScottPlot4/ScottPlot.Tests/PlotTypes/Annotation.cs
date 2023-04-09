using NUnit.Framework;
using ScottPlot;
using System;
using System.Drawing;

namespace ScottPlotTests.PlotTypes
{
    public class Annotation
    {
        [Test]
        public void Test_Annotation_Coordinates()
        {
            ScottPlot.Plot plt = new(400, 300);

            foreach (Alignment alignment in Enum.GetValues(typeof(Alignment)))
            {
                plt.AddAnnotation(alignment.ToString(), alignment);
            }

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Annotation_Styling()
        {
            ScottPlot.Plot plt = new(400, 300);

            var a = plt.AddAnnotation("Fancy Annotation");
            a.Font.Size = 24;
            a.Font.Name = "Impact";
            a.Font.Color = Color.Red;
            a.Shadow = true;
            a.Background = true;
            a.BackgroundColor = Color.Orange;
            a.BorderWidth = 2;

            TestTools.SaveFig(plt);
        }
    }
}
