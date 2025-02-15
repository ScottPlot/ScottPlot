using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class PlottableDrag : Form, IDemoWindow
{
    public string Title => "Draggable Plottables";

    public string Description => "Demonstrates how to create Plottables " +
        "which can be dragged with the mouse";

    DraggablePlottableDecorator? PlottableBeingDragged = null;

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
        formsPlot1.Plot.Add.Plottable(new DraggablePlottableDecorator(signal1));

        var signal2 = formsPlot1.Plot.Add.Signal(ys2);
        formsPlot1.Plot.Remove(signal2);
        formsPlot1.Plot.Add.Plottable(new DraggablePlottableDecorator(signal2));

        var signalXY = formsPlot1.Plot.Add.SignalXY(xsLarge, ysLarge);
        formsPlot1.Plot.Remove(signalXY);
        formsPlot1.Plot.Add.Plottable(new DraggablePlottableDecorator(signalXY));

        var scatter = formsPlot1.Plot.Add.Scatter(xs, ys2);
        formsPlot1.Plot.Remove(scatter);
        formsPlot1.Plot.Add.Plottable(new DraggablePlottableDecorator(scatter));

        var ellipse = formsPlot1.Plot.Add.Ellipse(25, 2, 5, 3, Angle.FromDegrees(-30));
        formsPlot1.Plot.Remove(ellipse);
        formsPlot1.Plot.Add.Plottable(new DraggablePlottableDecorator(ellipse));

        var arrow = formsPlot1.Plot.Add.Arrow(20, 1, 0, 0);
        formsPlot1.Plot.Remove(arrow);
        formsPlot1.Plot.Add.Plottable(new DraggablePlottableDecorator(arrow));

        var imageMarker = formsPlot1.Plot.Add.ImageMarker(new Coordinates(20, 1), SampleImages.ScottPlotLogo(), 0.1f);
        formsPlot1.Plot.Remove(imageMarker);
        formsPlot1.Plot.Add.Plottable(new DraggablePlottableDecorator(imageMarker));

        var marker = formsPlot1.Plot.Add.Marker(30, -1);
        formsPlot1.Plot.Remove(marker);
        formsPlot1.Plot.Add.Plottable(new DraggablePlottableDecorator(marker));

        var text = formsPlot1.Plot.Add.Text("Text", 12, 1);
        text.LabelFontSize = 50;
        formsPlot1.Plot.Remove(text);
        formsPlot1.Plot.Add.Plottable(new DraggablePlottableDecorator(text));

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        Pixel mousePixel = new(e.X, e.Y);

        foreach (DraggablePlottableDecorator dp in formsPlot1.Plot.GetPlottables<DraggablePlottableDecorator>().Reverse())
        {
            if (dp.IsHit(mousePixel, 10))
            {
                var PlottableCoordinates = formsPlot1.Plot.GetCoordinates(mousePixel, dp.Axes.XAxis, dp.Axes.YAxis);
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
        Pixel mousePixel = new(e.X, e.Y);

        if (PlottableBeingDragged is not null)
        {
            // update the position of the plottable being dragged
            Coordinates mouseCoordinates = formsPlot1.Plot.GetCoordinates(
                pixel: mousePixel,
                xAxis: PlottableBeingDragged.Axes.XAxis,
                yAxis: PlottableBeingDragged.Axes.YAxis);

            PlottableBeingDragged.DragTo(mouseCoordinates);
            formsPlot1.Refresh();
            return;
        }

        // update the cursor to reflect what is underneath it
        foreach (DraggablePlottableDecorator dp in formsPlot1.Plot.GetPlottables<DraggablePlottableDecorator>())
        {
            if (dp.IsHit(mousePixel, 10))
            {
                Cursor = Cursors.Hand;
                return;
            }
        }

        Cursor = Cursors.Arrow;
    }
}
