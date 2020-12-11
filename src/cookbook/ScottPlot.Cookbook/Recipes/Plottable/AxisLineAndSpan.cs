using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            "An axis line marks a position on an axis. " +
            "Axis lines extend to positive and negative infinity on the other axis.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

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
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

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
            Debug.WriteLine($"Vertical line is at X={vLine.X}");
        }
    }

    public class AxisSpan : IRecipe
    {
        public string Category => "Plottable: Axis Line and Span";
        public string ID => "axisSpan_quickstart";
        public string Title => "Axis Span";
        public string Description =>
            "Axis spans shade a portion of one axis. " +
            "Axis spans extend to negative and positive infinity on the other axis.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // add axis spans
            plt.AddVerticalSpan(.15, .85);
            plt.AddHorizontalSpan(10, 25);
        }
    }

    public class AxisSpanDraggable : IRecipe
    {
        public string Category => "Plottable: Axis Line and Span";
        public string ID => "axisSpan_draggable";
        public string Title => "Draggable Axis Span";
        public string Description =>
            "Axis spans can be dragged using the mouse. " +
            "Drag limits are boundaries over which the edges of spans cannot cross.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // dragging can be enabled and optionally limited to a range
            var vSpan = plt.AddVerticalSpan(.15, .85);
            vSpan.DragEnabled = true;
            vSpan.DragLimitMin = -1;
            vSpan.DragLimitMax = 1;

            // spans can be configured to allow dragging but disallow resizing
            var hSpan = plt.AddHorizontalSpan(10, 25);
            hSpan.DragEnabled = true;
            hSpan.DragFixedSize = true;
        }
    }
}
