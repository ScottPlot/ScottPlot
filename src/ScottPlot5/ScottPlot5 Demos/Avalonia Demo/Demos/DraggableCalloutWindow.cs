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

public class DraggableCalloutDemo : IDemo
{
    public string Title => "Draggable Callout";
    public string Description => "Demonstrates how to make a Callout mouse-interactive";

    public Window GetWindow()
    {
        return new DraggableCalloutWindow();
    }

}

public class DraggableCalloutWindow : SimpleDemoWindow
{
    Callout? CalloutBeingDragged = null;
    PixelOffset MouseDownOffset;

    public DraggableCalloutWindow() : base("Draggable Callout")
    {

    }

    protected override void StartDemo()
    {
        var fp = AvaPlot.Plot.Add.Function(SampleData.DunningKrugerCurve);
        fp.MinX = 0;
        fp.MaxX = 2;
        fp.LineWidth = 3;

        AvaPlot.Plot.Add.Callout(
            text: "Peak of \"Mount Stupid\"",
            textLocation: new(0.35, 1.05),
            tipLocation: new(0.2185, fp.GetY(0.2185)));

        AvaPlot.Plot.Add.Callout(
            text: "Valley of Despair",
            textLocation: new(0.35, 0.6),
            tipLocation: new(0.3885, fp.GetY(0.3885)));

        AvaPlot.Plot.Add.Callout(
            text: "Slope of Enlightenment",
            textLocation: new(0.9, 0.3),
            tipLocation: new(0.76935, fp.GetY(0.76935)));

        AvaPlot.Plot.Add.Callout(
            text: "Plateau of Sustainability",
            textLocation: new(1.4, 0.8),
            tipLocation: new(1.701, fp.GetY(1.701)));

        AvaPlot.Plot.YLabel("Confidence");
        AvaPlot.Plot.XLabel("Competence");
        AvaPlot.Plot.Title("Dunning-Kruger Effect", 24);

        AvaPlot.Plot.Axes.SetLimitsX(0, 2);
        AvaPlot.Plot.Axes.SetLimitsY(0, 1.2);

        AvaPlot.PointerPressed += OnMouseDown;
        AvaPlot.PointerReleased += OnMouseUp;
        AvaPlot.PointerMoved += OnMouseMove;
    }

    private void OnMouseDown(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(this);
        Callout? calloutUnderMouse = GetCalloutUnderMouse((float)pos.X, (float)pos.Y);
        if (calloutUnderMouse is null)
            return;

        CalloutBeingDragged = calloutUnderMouse;

        float dX = (float)(calloutUnderMouse!.TextPixel.X - pos.X);
        float dY = (float)(calloutUnderMouse!.TextPixel.Y - pos.Y);
        MouseDownOffset = new(dX, dY);

        AvaPlot.UserInputProcessor.Disable();
        OnMouseMove(sender, e);
    }

    private void OnMouseUp(object? sender, PointerEventArgs e)
    {
        CalloutBeingDragged = null;
        AvaPlot.UserInputProcessor.Enable();
        AvaPlot.Refresh();
    }

    private void OnMouseMove(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(this);
        if (CalloutBeingDragged is null)
        {
            Callout? calloutUnderMouse = GetCalloutUnderMouse((float)pos.X, (float)pos.Y);
            Cursor = calloutUnderMouse is null ? new(StandardCursorType.Arrow) : new(StandardCursorType.Hand);
            if (calloutUnderMouse is not null)
                AvaPlot.Plot.MoveToFront(calloutUnderMouse);
        }
        else
        {
            Pixel mousePixel = new(pos.X + MouseDownOffset.X, pos.Y + MouseDownOffset.Y);
            Coordinates mouseCoordinates = CalloutBeingDragged.Axes.GetCoordinates(mousePixel);
            CalloutBeingDragged.TextCoordinates = mouseCoordinates;
            AvaPlot.Refresh();
        }
    }

    private Callout? GetCalloutUnderMouse(float x, float y)
    {
        return AvaPlot.Plot
            .GetPlottables<Callout>()
            .Reverse()
            .Where(p => p.LastRenderRect.Contains(x, y))
            .FirstOrDefault();
    }
}
