using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.Customize
{
    class Figure
    {
        public class Background : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Background Colors";
            public string description { get; } = "Figure and data area background colors can be set individually.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Style(figBg: Color.LightBlue);
                plt.Style(dataBg: Color.LightYellow);
            }
        }

        public class Frame : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Corner Frame";
            public string description { get; } = "The data are is typically surrounded by a frame (a 1px line). This frame can be customized using arguments.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Frame(left: true, bottom: true, top: false, right: false);
            }
        }

        public class FigurePadding : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Figure Padding";
            public string description { get; } = "Extra padding can be added around the data area if desired.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                // custom colors are used to make it easier to see the data and figure areas
                plt.Style(figBg: Color.LightBlue);
                plt.Style(dataBg: Color.LightYellow);

                plt.Layout(yScaleWidth: 80, titleHeight: 50, xLabelHeight: 20, y2LabelWidth: 20);
            }
        }

        public class NoPad : PlotDemo, IPlotDemo
        {
            public string name { get; } = "No Padding";
            public string description { get; } = "This example shows how to only plot the data area (no axis labels, ticks, or tick labels)";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                // custom colors are used to make it easier to see the data and figure areas
                plt.Style(figBg: Color.LightBlue);
                plt.Style(dataBg: Color.LightYellow);

                plt.Ticks(false, false);
                plt.Frame(false);

                // Eliminate space between the data area and frame edge by setting padding to 0.
                // This must be repeated if the layout resets (such as when new items are added to the plot).
                plt.TightenLayout(padding: 0);
            }
        }

        public class AntiAliasing : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Anti-Aliasing";
            public string description { get; } = "Anti-aliasing makes plots look nicer but slightly reduces performance. Antialiasing of the Figure (containing the title, axis labels, and axis ticks) can be controlled independently from the data area and/or legend.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin, label: "Sin");
                plt.PlotScatter(x, cos, label: "Cos");

                plt.Title("Plot Title");
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
                plt.Legend();

                plt.AntiAlias(figure: false, data: false, legend: false);

                // NOTE: anti-aliasing is automatically in the user control
                // while the mouse button is held down to improve performance
                // while panning and zooming. You can disable this feature by:
                // formsPlot1.Configure(lowQualityWhileDragging = false);
            }
        }
    }
}
