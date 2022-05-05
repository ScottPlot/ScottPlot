using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class AxisLineAndSpan : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.AxisLineAndSpan();
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

    public class FiniteAxisLine : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.AxisLineAndSpan();
        public string ID => "axisLine_finite";
        public string Title => "Finite Axis Line";
        public string Description =>
            "Axis lines can have lower and/or upper bounds. This can be useful for labeling points of interest.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddFunction(x => Math.Pow(x, 2), lineStyle: LineStyle.Dash);
            plt.AddFunction(x => Math.Sqrt(x), lineStyle: LineStyle.Dash);

            // mark a coordinate from the lower left
            var point1 = plt.AddPoint(1, 1, size: 10, shape: MarkerShape.openCircle);
            var hLine1 = plt.AddHorizontalLine(1, width: 2);
            hLine1.Max = 1;
            hLine1.Color = point1.Color;
            var vLine1 = plt.AddVerticalLine(1, width: 2);
            vLine1.Max = 1;
            vLine1.Color = point1.Color;

            // use finate upper and lower limits draw a cross on a point
            var point2 = plt.AddPoint(4, 2, size: 10, shape: MarkerShape.openCircle);
            var vLine2 = plt.AddVerticalLine(4, width: 2);
            vLine2.Min = 1.5;
            vLine2.Max = 2.5;
            vLine2.Color = point2.Color;
            var hLine2 = plt.AddHorizontalLine(2, width: 2);
            hLine2.Min = 3.5;
            hLine2.Max = 4.5;
            hLine2.Color = point2.Color;

            // mark a coordinate from the top right
            var point3 = plt.AddPoint(2, 4, size: 10, shape: MarkerShape.openCircle);
            var hLine3 = plt.AddHorizontalLine(4, width: 2);
            hLine3.Min = 2;
            hLine3.Color = point3.Color;
            var vLine3 = plt.AddVerticalLine(2, width: 2);
            vLine3.Min = 4;
            vLine3.Color = point3.Color;

            plt.SetAxisLimits(0, 5, 0, 5);
        }
    }

    public class Draggable : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.AxisLineAndSpan();
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
            string message = $"Vertical line is at X={vLine.X}";
        }
    }

    public class AxisLineWithPositionLabels : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.AxisLineAndSpan();
        public string ID => "axisLine_positionLabels";
        public string Title => "Position Labels";
        public string Description =>
            "Axis line positions can be labeled on the axis on top of axis ticks and tick labels. " +
            "Custom position formatters allow for full customization of the text displayed in these labels. " +
            "If using a DateTime axis, implement a custom formatter that uses DateTime.FromOADate().";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            var hline = plt.AddHorizontalLine(.85);
            hline.LineWidth = 2;
            hline.PositionLabel = true;
            hline.PositionLabelBackground = hline.Color;
            hline.DragEnabled = true;

            var vline = plt.AddVerticalLine(23);
            vline.LineWidth = 2;
            vline.PositionLabel = true;
            vline.PositionLabelBackground = vline.Color;
            vline.DragEnabled = true;

            Func<double, string> xFormatter = x => $"X={x:F2}";
            vline.PositionFormatter = xFormatter;
        }
    }

    public class AxisSpan : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.AxisLineAndSpan();
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
        public ICategory Category => new Categories.PlotTypes.AxisLineAndSpan();
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
            vSpan.BorderColor = Color.Red;
            vSpan.BorderLineStyle = LineStyle.Dot;
            vSpan.BorderLineWidth = 2;
            vSpan.HatchColor = Color.FromArgb(100, Color.Blue);
            vSpan.HatchStyle = Drawing.HatchStyle.SmallCheckerBoard;
            vSpan.Label = "Customized vSpan";


            // spans can be configured to allow dragging but disallow resizing
            var hSpan = plt.AddHorizontalSpan(10, 25);
            hSpan.DragEnabled = true;
            hSpan.DragFixedSize = true;
            hSpan.Label = "Standard hSpan";
            plt.Legend(true);
        }
    }

    public class AxisLineAndSpanIgnore : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.AxisLineAndSpan();
        public string ID => "axisSpan_ignore";
        public string Title => "Ignore Axis Limits";
        public string Description =>
            "Calling Plot.AxisAuto (or middle-clicking the plot) will set the axis limits " +
            "automatically to fit the data on the plot. By default the position of axis lines and spans are " +
            "included in automatic axis limit calculations, but setting the '' flag can disable this behavior.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            var hline = plt.AddHorizontalLine(0.23);
            hline.DragEnabled = true;
            hline.IgnoreAxisAuto = true;

            var hSpan = plt.AddHorizontalSpan(-10, 20);
            hSpan.DragEnabled = true;
            hSpan.IgnoreAxisAuto = true;
        }
    }

    public class RepeatingAxisLine : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.AxisLineAndSpan();
        public string ID => "repeatingAxisLine_basics";
        public string Title => "Repeating Axis Line";
        public string Description =>
            "Repeating axis lines allows to plot several axis lines, " +
            "either horizontal or vertical, draggable or not, whose positions are linked";

        public void ExecuteRecipe(Plot plt)
        {
            //Generate a single signal containing 3 harmonic signals
            int sampleCount = 500;
            double[] signal1 = ScottPlot.DataGen.Sin(sampleCount, 10);
            double[] signal2 = ScottPlot.DataGen.Sin(sampleCount, 20);
            double[] signal3 = ScottPlot.DataGen.Sin(sampleCount, 30);

            double[] signal = new double[sampleCount];
            for (int index = 0; index < sampleCount; index++)
            {
                signal[index] = signal1[index] + signal2[index] + signal3[index];
            }

            // Plot the signal
            plt.AddSignal(signal);

            // Create a draggable RepeatingVLine with 5 lines spaced evenly by 50 X units, starting at position 0
            // The first line will be thicker than the others
            ScottPlot.Plottable.RepeatingVLine vlines1 = new();
            vlines1.DragEnabled = true;
            vlines1.count = 5;
            vlines1.shift = 50;
            vlines1.Color = System.Drawing.Color.Magenta;
            vlines1.LineWidth = 2;
            vlines1.LineStyle = LineStyle.Dash;
            vlines1.PositionLabel = true;
            vlines1.PositionLabelBackground = vlines1.Color;
            vlines1.relativeposition = false;
            plt.Add(vlines1);

            // Create a draggable RepeatingVLine with 5 lines spaced evenly by 50 X units, starting at position 0, with a -4 offset
            // The first line will be thicker than the others
            ScottPlot.Plottable.RepeatingVLine vlines2 = new();
            vlines2.DragEnabled = true;
            vlines2.count = 3;
            vlines2.shift = 50;
            vlines2.offset = -1;
            vlines2.Color = System.Drawing.Color.DarkGreen;
            vlines2.LineWidth = 2;
            vlines2.LineStyle = LineStyle.Dot;
            vlines2.PositionLabel = true;
            vlines2.PositionLabelBackground = vlines2.Color;
            vlines2.relativeposition = false;
            plt.Add(vlines2);

            plt.SetAxisLimitsX(-100, 300);
        }
    }

    public class AxisLineVector : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.AxisLineAndSpan();
        public string ID => "axisLine_Vector";
        public string Title => "Axis Line Vector";
        public string Description =>
            "An AxisLineVector allows to setup a series of VLines or HLines, without hassle." +
            "These lines can optionally be dragged as their counterparts";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            double[] xs = DataGen.Random(rand, 50);
            double[] ys = DataGen.Random(rand, 50);

            var scatter = plt.AddScatterPoints(xs, ys, Color.Blue, 10);

            var vlines = new ScottPlot.Plottable.VLineVector();
            vlines.Xs = new double[] { xs[1], xs[12], xs[35] };
            vlines.Color = Color.Red;
            vlines.PositionLabel = true;
            vlines.PositionLabelBackground = vlines.Color;

            var hlines = new ScottPlot.Plottable.HLineVector();
            hlines.Ys = new double[] { ys[1], ys[12], ys[35] };
            hlines.Color = Color.DarkCyan;
            hlines.PositionLabel = true;
            hlines.PositionLabelBackground = hlines.Color;
            hlines.DragEnabled = true;

            plt.Add(scatter);
            plt.Add(vlines);
            plt.Add(hlines);
        }
    }
}
