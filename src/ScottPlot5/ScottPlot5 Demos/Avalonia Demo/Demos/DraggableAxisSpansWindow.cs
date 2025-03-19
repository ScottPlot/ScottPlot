using Avalonia.Controls;
using Avalonia.Input;
using ScottPlot;
using ScottPlot.Plottables;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.Demos;

public class DraggableAxisSpansDemo : IDemo
{
    public string Title => "Draggable Axis Spans";

    public string Description => "Demonstrates how to create a mouse-interactive " +
        "axis span that can be resized or dragged";

    public Window GetWindow()
    {
        return new DraggableAxisSpansWindow();
    }

}

public class DraggableAxisSpansWindow : SimpleDemoWindow
{
    AxisSpanUnderMouse? SpanBeingDragged = null;
    public DraggableAxisSpansWindow() : base("Draggable Axis Spans")
    {

    }

    protected override void StartDemo()
    {
        // place axis spans on the plot
        AvaPlot.Plot.Add.Signal(Generate.Sin());
        AvaPlot.Plot.Add.Signal(Generate.Cos());

        var vs = AvaPlot.Plot.Add.VerticalSpan(.23, .78);
        vs.IsDraggable = true;
        vs.IsResizable = true;

        var hs = AvaPlot.Plot.Add.HorizontalSpan(23, 42);
        hs.IsDraggable = true;
        hs.IsResizable = true;

        AvaPlot.Refresh();

        // use events for custom mouse interactivity
        AvaPlot.PointerPressed += OnMouseDown;
        AvaPlot.PointerReleased += OnMouseUp;
        AvaPlot.PointerMoved += OnMouseMove;
    }

    private void OnMouseDown(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(this);
        var thingUnderMouse = GetSpanUnderMouse((float)pos.X, (float)pos.Y);
        if (thingUnderMouse is not null)
        {
            SpanBeingDragged = thingUnderMouse;
            AvaPlot.UserInputProcessor.Disable(); // disable panning while dragging
        }
    }

    private void OnMouseUp(object? sender, PointerEventArgs e)
    {
        SpanBeingDragged = null;
        AvaPlot.UserInputProcessor.Enable(); // enable panning
        AvaPlot.Refresh();
    }

    private void OnMouseMove(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(this);
        if (SpanBeingDragged is not null)
        {
            // currently dragging something so update it
            Coordinates mouseNow = AvaPlot.Plot.GetCoordinates(new Pixel(pos.X, pos.Y));
            SpanBeingDragged.DragTo(mouseNow);
            AvaPlot.Refresh();
        }
        else
        {
            // not dragging anything so just set the cursor based on what's under the mouse
            var spanUnderMouse = GetSpanUnderMouse((float)pos.X, (float)pos.Y);
            if (spanUnderMouse is null) Cursor = new(StandardCursorType.Arrow);
            else if (spanUnderMouse.IsResizingHorizontally) Cursor = new(StandardCursorType.SizeWestEast);
            else if (spanUnderMouse.IsResizingVertically) Cursor = new(StandardCursorType.SizeNorthSouth);
            else if (spanUnderMouse.IsMoving) Cursor = new(StandardCursorType.SizeAll);
        }
    }

    private AxisSpanUnderMouse? GetSpanUnderMouse(float x, float y)
    {
        CoordinateRect rect = AvaPlot.Plot.GetCoordinateRect(x, y, radius: 10);

        foreach (AxisSpan span in AvaPlot.Plot.GetPlottables<AxisSpan>().Reverse())
        {
            AxisSpanUnderMouse? spanUnderMouse = span.UnderMouse(rect);
            if (spanUnderMouse is not null)
                return spanUnderMouse;
        }

        return null;
    }
}
