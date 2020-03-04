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
                GenericPlots.SinAndCos(plt);

                plt.PlotVSpan(y1: .15, y2: .85, label: "VSpan");
                plt.PlotHSpan(x1: 10, x2: 25, label: "HSpan");
                plt.Legend();
            }
        }

        public class Draggable : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Draggable Axis Spans";
            public string description { get; } = "Horizontal and vertical spans can be made draggable (with optional limits) using arguments.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);

                plt.PlotVSpan(y1: .15, y2: .85, label: "VSpan", draggable: true, dragLimitLower: -1, dragLimitUpper: 1);
                plt.PlotHSpan(x1: 10, x2: 25, label: "HSpan", draggable: true, dragLimitLower: 0, dragLimitUpper: 50);
                plt.Legend();
            }
        }
    }
}
