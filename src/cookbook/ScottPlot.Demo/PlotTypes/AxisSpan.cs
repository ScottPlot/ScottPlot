using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class AxisSpan
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Axis Span Quickstart";
            public string description { get; } = "Horizontal and vertical spans can be placed using VSpan() and HSpan(). Styling can be customized using arguments.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.PlotVSpan(y1: .15, y2: .85, label: "VSpan");
                plt.PlotHSpan(x1: 10, x2: 25, label: "HSpan");

                plt.Grid(lineStyle: LineStyle.Dot);
                plt.Legend();
            }
        }

        public class Draggable : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Draggable Axis Spans";
            public string description { get; } = "Horizontal and vertical spans can be made draggable " +
                "(with optional limits) using arguments. Hold SHIFT while dragging an adjustable span's edge " +
                "to shift it rather than resizing it.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.PlotVSpan(y1: .15, y2: .85, label: "Adjustable VSpan",
                    draggable: true, dragLimitLower: -1, dragLimitUpper: 1);

                plt.PlotHSpan(x1: 10, x2: 25, label: "Adjustable HSpan",
                    draggable: true, dragLimitLower: 0, dragLimitUpper: 50);

                plt.PlotVSpan(y1: -.25, y2: -.05, label: "Fixed Size VSpan",
                    draggable: true, dragLimitLower: -1, dragLimitUpper: 1,
                    dragFixedSize: true);

                plt.PlotHSpan(x1: 5, x2: 7, label: "Fixed Size HSpan",
                    draggable: true, dragLimitLower: 0, dragLimitUpper: 50,
                    dragFixedSize: true);

                plt.Grid(lineStyle: LineStyle.Dot);
                plt.Legend();
            }
        }
    }
}
