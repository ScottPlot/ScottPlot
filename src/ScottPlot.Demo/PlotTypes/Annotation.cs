using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public static class Annotation
    {
        public class AnnotationQuickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Annotation Quickstart";
            public string description { get; } = "Annotations are labels fixed to the figure (not the data area), so they don't move around as the axes are adjusted.";

            public void Render(Plot plt)
            {
                double[] xs = DataGen.Range(0, 5, .1);
                plt.PlotScatter(xs, DataGen.Sin(xs));
                plt.PlotScatter(xs, DataGen.Cos(xs));

                // negative coordinates snap text to the lower or right edges
                plt.PlotAnnotation("Top Left", 10, 10);
                plt.PlotAnnotation("Lower Left", 10, -10);
                plt.PlotAnnotation("Top Right", -10, 10);
                plt.PlotAnnotation("Lower Right", -10, -10);

                // arguments allow customization of style
                plt.PlotAnnotation("Fancy Annotation", 10, 40,
                    fontSize: 24, fontName: "Impact", fontColor: Color.Red, shadow: true,
                    fill: true, fillColor: Color.White, fillAlpha: 1, lineWidth: 2);
            }
        }
    }
}
