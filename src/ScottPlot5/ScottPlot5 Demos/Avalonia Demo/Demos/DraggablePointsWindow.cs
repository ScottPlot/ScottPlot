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

public class DraggablePointsDemo : IDemo
{
    public string Title => "Draggable Data Points";
    public string Description => "GUI events can be used to interact with data " +
        "drawn on the plot. This example shows how to achieve drag-and-drop behavior " +
        "for points of a scatter plot. Extra code may be added to limit how far points may be moved.";

    public Window GetWindow()
    {
        return new DraggablePointsWindow();
    }

}

public class DraggablePointsWindow : SimpleDemoWindow
{
    private readonly double[] Xs = Generate.RandomAscending(10);
    private readonly double[] Ys = Generate.RandomSample(10);
    private ScottPlot.Plottables.Scatter? Scatter;
    private int? IndexBeingDragged = null;

    public DraggablePointsWindow() : base("Draggable Data Points")
    {

    }

    protected override void StartDemo()
    {
        if (Scatter is null)
            return;

        Scatter = AvaPlot.Plot.Add.Scatter(Xs, Ys);
        Scatter.LineWidth = 2;
        Scatter.MarkerSize = 10;
        Scatter.Smooth = true;

        AvaPlot.PointerPressed += OnMouseDown;
        AvaPlot.PointerReleased += OnMouseUp;
        AvaPlot.PointerMoved += OnMouseMove;
    }

    private void OnMouseDown(object? sender, PointerEventArgs e)
    {
        if (Scatter is null)
            return;

        var pos = e.GetPosition(this);
        Pixel mousePixel = new(pos.X, pos.Y);
        Coordinates mouseLocation = AvaPlot.Plot.GetCoordinates(mousePixel);
        DataPoint nearest = Scatter.Data.GetNearest(mouseLocation, AvaPlot.Plot.LastRender);
        IndexBeingDragged = nearest.IsReal ? nearest.Index : null;

        if (IndexBeingDragged.HasValue)
            AvaPlot.UserInputProcessor.Disable();
    }

    private void OnMouseUp(object? sender, PointerEventArgs e)
    {
        IndexBeingDragged = null;
        AvaPlot.UserInputProcessor.Enable();
        AvaPlot.Refresh();
    }

    private void OnMouseMove(object? sender, PointerEventArgs e)
    {
        if (Scatter is null)
            return;

        var pos = e.GetPosition(this);
        Pixel mousePixel = new(pos.X, pos.Y);
        Coordinates mouseLocation = AvaPlot.Plot.GetCoordinates(mousePixel);
        DataPoint nearest = Scatter.Data.GetNearest(mouseLocation, AvaPlot.Plot.LastRender);
        AvaPlot.Cursor = nearest.IsReal ? new(StandardCursorType.Hand) : new(StandardCursorType.Arrow);

        if (IndexBeingDragged.HasValue)
        {
            Xs[IndexBeingDragged.Value] = mouseLocation.X;
            Ys[IndexBeingDragged.Value] = mouseLocation.Y;
            AvaPlot.Refresh();
        }
    }
}
