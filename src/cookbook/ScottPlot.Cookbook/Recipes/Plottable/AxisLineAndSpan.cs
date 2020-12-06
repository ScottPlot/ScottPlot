using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class AxisLineAndSpan : IRecipe
    {
        public string Category => "Plottable: Axis Line and Span";
        public string ID => "axisLine_basics";
        public string Title => "Axis Line";
        public string Description =>
            "Axis lines are placed at a position on one axis and extend to " +
            "positive and negative infinity on the other axis.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.PlotSignal(DataGen.Sin(51));
            plt.PlotSignal(DataGen.Cos(51));

            // add axis lines
            plt.AddHorizontalLine(.85);
            plt.AddVerticalLine(23);

            // customize axis lines with optional arguments
            plt.AddVerticalLine(x: 33, color: Color.Magenta, width: 3, style: LineStyle.Dot);
        }
    }

    public class Draggable : IRecipe
    {
        public string Category => "Plottable: Axis Line and Span";
        public string ID => "axisLine_draggable";
        public string Title => "Draggable Axis Lines";
        public string Description => 
            "In GUI environments, axis lines can be draggable and moved with the mouse. " + 
            "Drag limits define the boundaries the lines can be dragged.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.PlotSignal(DataGen.Sin(51));
            plt.PlotSignal(DataGen.Cos(51));

            // add axis lines and configure their drag settings
            var hLine = plt.AddHorizontalLine(.85);
            hLine.DragEnabled = true;
            hLine.DragLimitMin = -1;
            hLine.DragLimitMax = 1;

            var vLine = plt.AddVerticalLine(23);
            vLine.DragEnabled = true;
            vLine.DragLimitMin = 0;
            vLine.DragLimitMax = 50;

            // you can access the position of an axis line at any time
            Console.WriteLine($"Vertical line is at X={vLine.position}");
        }
    }
}
