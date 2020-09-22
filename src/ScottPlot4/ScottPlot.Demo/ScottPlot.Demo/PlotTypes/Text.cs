using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Text
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Text Quickstart";
            public string description { get; } = "Text can be placed at any X/Y location and styled using arguments.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.PlotText("demo text", 10, .5, fontName: "comic sans ms", fontSize: 42, color: Color.Magenta, bold: true);
            }
        }

        public class Alignment : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Text Alignment";
            public string description { get; } = "Text alignment and rotation can be customized using arguments.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.PlotPoint(25, 0.8, color: Color.Green);
                plt.PlotText(" important point", 25, 0.8, color: Color.Green);

                plt.PlotPoint(30, 0.3, color: Color.Black, markerSize: 15);
                plt.PlotText(" default alignment", 30, 0.3, fontSize: 16, bold: true, color: Color.Magenta);

                plt.PlotPoint(30, 0, color: Color.Black, markerSize: 15);
                plt.PlotText("middle center", 30, 0, fontSize: 16, bold: true, color: Color.Magenta, alignment: TextAlignment.middleCenter);

                plt.PlotPoint(30, -0.3, color: Color.Black, markerSize: 15);
                plt.PlotText("upper left", 30, -0.3, fontSize: 16, bold: true, color: Color.Magenta, alignment: TextAlignment.upperLeft);

                plt.PlotPoint(5, -.5, color: Color.Blue, markerSize: 15);
                plt.PlotText(" Rotated Text", 5, -.5, fontSize: 16, color: Color.Blue, bold: true, rotation: -30);

                plt.PlotText("Framed Text", 15, -.6, fontSize: 16, color: Color.White, bold: true, frame: true, frameColor: Color.DarkRed);
            }
        }
    }
}
