using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.Customize
{
    class PlotStyle
    {
        public class ModifyAfterPlot : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Modify styles after plotting";
            public string description { get; } = "Styles are typically defined as arguments when data is initially plotted. However, plotting functions return objects which contain style information that can be modified after it has been plotted. In some cases these properties allow more extensive customization than the initial function arguments.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                var thing1 = plt.PlotScatter(x, sin, label: "thing 1");
                var thing2 = plt.PlotScatter(x, cos, label: "thing 2");

                thing1.lineWidth = 5;
                thing1.markerShape = MarkerShape.openCircle;
                thing1.markerSize = 20;

                thing2.color = Color.Magenta;

                plt.Legend();
            }
        }

        public class StyledLabels : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Custom Fonts Everywhere";
            public string description { get; } = "Uses cutom font, color, and sizes for numerous types of labels";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Title("Impressive Graph", fontName: "courier new", fontSize: 24, color: Color.Purple, bold: true);
                plt.YLabel("vertical units", fontName: "impact", fontSize: 24, color: Color.Red, bold: true);
                plt.XLabel("horizontal units", fontName: "georgia", fontSize: 24, color: Color.Blue, bold: true);
                plt.PlotText("very graph", 25, .8, fontName: "comic sans ms", fontSize: 24, color: Color.Blue, bold: true);
                plt.PlotText("so data", 0, 0, fontName: "comic sans ms", fontSize: 42, color: Color.Magenta, bold: true);
                plt.PlotText("many documentation", 3, -.6, fontName: "comic sans ms", fontSize: 18, color: Color.DarkCyan, bold: true);
                plt.PlotText("wow.", 10, .6, fontName: "comic sans ms", fontSize: 36, color: Color.Green, bold: true);
                plt.PlotText("NuGet", 32, 0, fontName: "comic sans ms", fontSize: 24, color: Color.Gold, bold: true);
                plt.Legend(fontName: "comic sans ms", fontSize: 16, bold: true, fontColor: Color.DarkBlue);
                plt.Ticks(fontName: "comic sans ms", fontSize: 12, color: Color.DarkBlue);
            }
        }

        public class CustomLegend : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Legend";
            public string description { get; } = "A legend is available to display data that was plotted using the 'label' argument. Arguments for Legend() allow the user to define its position.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin, label: "sin");
                plt.PlotScatter(x, cos, label: "cos");
                plt.Legend();
            }
        }
    }
}
