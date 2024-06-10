using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class DraggablePoints : Form, IDemoWindow
{
    public string Title => "Draggable Data Points";

    public string Description => "GUI events can be used to interact with data " +
        "drawn on the plot. This example shows how to achieve drag-and-drop behavior " +
        "for points of a scatter plot. Extra code may be added to limit how far points may be moved.";

    readonly double[] Xs = Generate.RandomAscending(10);
    readonly double[] Ys = Generate.RandomSample(10);
    readonly ScottPlot.Plottables.Scatter Scatter;
    int? IndexBeingDragged = null;

    public DraggablePoints()
    {
        InitializeComponent();

        Scatter = formsPlot1.Plot.Add.Scatter(Xs, Ys);
        Scatter.LineWidth = 2;
        Scatter.MarkerSize = 10;
        Scatter.Smooth = true;

        formsPlot1.MouseMove += FormsPlot1_MouseMove;
        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        Pixel mousePixel = new(e.Location.X, e.Location.Y);
        Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);
        DataPoint nearest = Scatter.Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender);
        IndexBeingDragged = nearest.IsReal ? nearest.Index : null;

        if (IndexBeingDragged.HasValue)
            formsPlot1.Interaction.Disable();
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        IndexBeingDragged = null;
        formsPlot1.Interaction.Enable();
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        Pixel mousePixel = new(e.Location.X, e.Location.Y);
        Coordinates mouseLocation = formsPlot1.Plot.GetCoordinates(mousePixel);
        DataPoint nearest = Scatter.Data.GetNearest(mouseLocation, formsPlot1.Plot.LastRender);
        formsPlot1.Cursor = nearest.IsReal ? Cursors.Hand : Cursors.Arrow;

        if (IndexBeingDragged.HasValue)
        {
            Xs[IndexBeingDragged.Value] = mouseLocation.X;
            Ys[IndexBeingDragged.Value] = mouseLocation.Y;
            formsPlot1.Refresh();
        }
    }
}
