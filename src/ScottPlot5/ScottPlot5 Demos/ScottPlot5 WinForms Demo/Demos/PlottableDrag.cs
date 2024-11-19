using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class PlottableDrag : Form, IDemoWindow
{
    public string Title => "Draggable Plottables";

    public string Description => "Demonstrates how to create Plottables" +
        "which can be dragged with the mouse";

    DragablePlottableDecorator? PlottableBeingDragged = null;

    public PlottableDrag()
    {
        InitializeComponent();

        double[] xs = Generate.Consecutive(100);
        double[] ys1 = Generate.Sin(100);
        double[] ys2 = Generate.Cos(100);

        double[] xsLarge = Generate.Consecutive(1_000_000, 0.0001);
        double[] ysLarge = Generate.RandomWalk(1_000_000, 0.01);

        var signal1 = formsPlot1.Plot.Add.Signal(ys1);
        formsPlot1.Plot.Remove(signal1);
        formsPlot1.Plot.Add.Plottable(new DragablePlottableDecorator(signal1));

        var signal2 = formsPlot1.Plot.Add.Signal(ys2);
        formsPlot1.Plot.Remove(signal2);
        formsPlot1.Plot.Add.Plottable(new DragablePlottableDecorator(signal2));

        var signalXY = formsPlot1.Plot.Add.SignalXY(xsLarge, ysLarge);
        formsPlot1.Plot.Remove(signalXY);
        formsPlot1.Plot.Add.Plottable(new DragablePlottableDecorator(signalXY));

        var scatter = formsPlot1.Plot.Add.Scatter(xs, ys2);
        formsPlot1.Plot.Remove(scatter);
        formsPlot1.Plot.Add.Plottable(new DragablePlottableDecorator(scatter));

        var ellipse = formsPlot1.Plot.Add.Ellipse(25, 2, 5, 3, 30);
        formsPlot1.Plot.Remove(ellipse);
        formsPlot1.Plot.Add.Plottable(new DragablePlottableDecorator(ellipse));

        var arrow = formsPlot1.Plot.Add.Arrow(20, 1, 0, 0);
        formsPlot1.Plot.Remove(arrow);
        formsPlot1.Plot.Add.Plottable(new DragablePlottableDecorator(arrow));

        var imageMarker = formsPlot1.Plot.Add.ImageMarker(new Coordinates(20, 1), SampleImages.ScottPlotLogo(), 0.1f);
        formsPlot1.Plot.Remove(imageMarker);
        formsPlot1.Plot.Add.Plottable(new DragablePlottableDecorator(imageMarker));

        var marker = formsPlot1.Plot.Add.Marker(30, -1);
        formsPlot1.Plot.Remove(marker);
        formsPlot1.Plot.Add.Plottable(new DragablePlottableDecorator(marker));

        var text = formsPlot1.Plot.Add.Text("Text", 12, 1);
        text.LabelFontSize = 50;
        formsPlot1.Plot.Remove(text);
        formsPlot1.Plot.Add.Plottable(new DragablePlottableDecorator(text));

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        foreach (DragablePlottableDecorator dp in formsPlot1.Plot.GetPlottables<DragablePlottableDecorator>().Reverse())
        {
            if (dp.IsHit(e.X, e.Y, 10))
            {
                var PlottableCoordinates = formsPlot1.Plot.GetCoordinates(e.X, e.Y, dp.Axes.XAxis, dp.Axes.YAxis);
                dp.StartDrag(PlottableCoordinates);
                PlottableBeingDragged = dp;
                formsPlot1.UserInputProcessor.Disable(); // disable panning while dragging
                return;
            }
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        PlottableBeingDragged = null;
        formsPlot1.UserInputProcessor.Enable(); // enable panning again
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        // update the cursor to reflect what is beneath it
        if (PlottableBeingDragged is null)
        {
            foreach (DragablePlottableDecorator dp in formsPlot1.Plot.GetPlottables<DragablePlottableDecorator>())
            {
                if (dp.IsHit(e.X, e.Y, 10))
                {
                    Cursor = Cursors.Hand;
                    return;
                }
            }
            Cursor = Cursors.Arrow;
            return;
        }
        else // update the position of the plottable being dragged
        {
            var PlottableCoordinates = formsPlot1.Plot.GetCoordinates(e.X, e.Y, PlottableBeingDragged.Axes.XAxis, PlottableBeingDragged.Axes.YAxis);
            PlottableBeingDragged.DragTo(PlottableCoordinates);
            formsPlot1.Refresh();
        }
    }
}
