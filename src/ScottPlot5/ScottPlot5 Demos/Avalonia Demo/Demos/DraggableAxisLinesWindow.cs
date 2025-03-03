using Avalonia.Controls;
using Avalonia.Input;
using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.Demos;

public class DraggableAxisLinesDemo : IDemo
{
    public string Title => "Draggable Axis Lines";
    public string Description => "Demonstrates how to add mouse interactivity to plotted objects";

    public Window GetWindow()
    {
        return new DraggableAxisLinesWindow();
    }

}

public class DraggableAxisLinesWindow : SimpleDemoWindow
{
    AxisLine? PlottableBeingDragged = null;

    public DraggableAxisLinesWindow() : base("Draggable Axis Lines")
    {

    }

    protected override void StartDemo()
    {
        // place axis lines on the plot
        AvaPlot.Plot.Add.Signal(Generate.Sin());
        AvaPlot.Plot.Add.Signal(Generate.Cos());

        var vl = AvaPlot.Plot.Add.VerticalLine(23);
        vl.IsDraggable = true;
        vl.Text = "VLine";

        var hl = AvaPlot.Plot.Add.HorizontalLine(0.42);
        hl.IsDraggable = true;
        hl.Text = "HLine";

        AvaPlot.Refresh();

        // use events for custom mouse interactivity
        AvaPlot.PointerPressed += OnMouseDown;
        AvaPlot.PointerReleased += OnMouseUp;
        AvaPlot.PointerMoved += OnMouseMove;
    }

    private void OnMouseDown(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(this);
        var lineUnderMouse = GetLineUnderMouse((float)pos.X, (float)pos.Y);
        if (lineUnderMouse is not null)
        {
            PlottableBeingDragged = lineUnderMouse;
            AvaPlot.UserInputProcessor.Disable(); // disable panning while dragging
        }
    }

    private void OnMouseUp(object? sender, PointerEventArgs e)
    {
        PlottableBeingDragged = null;
        AvaPlot.UserInputProcessor.Enable(); // enable panning again
        AvaPlot.Refresh();
    }

    private void OnMouseMove(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(this);
        // this rectangle is the area around the mouse in coordinate units
        CoordinateRect rect = AvaPlot.Plot.GetCoordinateRect((float)pos.X, (float)pos.Y, radius: 10);

        if (PlottableBeingDragged is null)
        {
            // set cursor based on what's beneath the plottable
            var lineUnderMouse = GetLineUnderMouse((float)pos.X, (float)pos.Y);
            if (lineUnderMouse is null) Cursor = new(StandardCursorType.Arrow);
            else if (lineUnderMouse.IsDraggable && lineUnderMouse is VerticalLine) Cursor = new(StandardCursorType.SizeWestEast);
            else if (lineUnderMouse.IsDraggable && lineUnderMouse is HorizontalLine) Cursor = new(StandardCursorType.SizeNorthSouth);
        }
        else
        {
            // update the position of the plottable being dragged
            if (PlottableBeingDragged is HorizontalLine hl)
            {
                hl.Y = rect.VerticalCenter;
                hl.Text = $"{hl.Y:0.00}";
            }
            else if (PlottableBeingDragged is VerticalLine vl)
            {
                vl.X = rect.HorizontalCenter;
                vl.Text = $"{vl.X:0.00}";
            }
            AvaPlot.Refresh();
        }
    }

    private AxisLine? GetLineUnderMouse(float x, float y)
    {
        CoordinateRect rect = AvaPlot.Plot.GetCoordinateRect(x, y, radius: 10);

        foreach (AxisLine axLine in AvaPlot.Plot.GetPlottables<AxisLine>().Reverse())
        {
            if (axLine.IsUnderMouse(rect))
                return axLine;
        }

        return null;
    }
}
