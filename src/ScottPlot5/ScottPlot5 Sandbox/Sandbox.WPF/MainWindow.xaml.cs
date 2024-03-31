using System.Linq;
using System.Windows;
using System.Windows.Input;
using ScottPlot;
using ScottPlot.WPF;
using ScottPlot.Plottables;

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    SignalXY? PlottableBeingDragged = null;
    DataPoint StartingDragPosition = DataPoint.None;
    double StartingDragOffset = 0;
    double ScaleFactor => PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;

    public MainWindow()
    {
        InitializeComponent();

        double[] xs = Generate.Consecutive(100);
        double[] ys1 = Generate.Sin(100);
        double[] ys2 = Generate.Cos(100);

        WpfPlot1.Plot.Add.SignalXY(xs, ys1);
        WpfPlot1.Plot.Add.SignalXY(xs, ys2);

        WpfPlot1.MouseDown += FormsPlot1_MouseDown;
        WpfPlot1.MouseUp += FormsPlot1_MouseUp;
        WpfPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object sender, MouseButtonEventArgs e)
    {
        WpfPlot plot = (WpfPlot)sender;
        if (plot is null)
            return;

        double x = e.GetPosition(plot).X * ScaleFactor;
        double y = e.GetPosition(plot).Y * ScaleFactor;

        (var signalUnderMouse, DataPoint dataPoint) = GetSignalXYUnderMouse(plot, x, y);
        if (signalUnderMouse is null)
            return;

        PlottableBeingDragged = signalUnderMouse;
        StartingDragPosition = dataPoint;
        StartingDragOffset = signalUnderMouse.Data.XOffset;
        plot.Interaction.Disable(); // disable panning while dragging
    }

    private void FormsPlot1_MouseUp(object sender, MouseButtonEventArgs e)
    {
        var plot = (WpfPlot)sender;
        if (plot is null)
            return;

        PlottableBeingDragged = null;
        StartingDragPosition = DataPoint.None;
        plot.Interaction.Enable(); // enable panning again
        plot.Refresh();
    }

    /// <summary>
    /// Returns the SignalXY object and data point beneath the mouse,
    /// or null if nothing is beneath the mouse.
    /// </summary>
    private static (SignalXY? signalXY, DataPoint point) GetSignalXYUnderMouse(WpfPlot plot, double x, double y)
    {
        Pixel mousePixel = new(x, y);

        Coordinates mouseLocation = plot.Plot.GetCoordinates(mousePixel);

        foreach (SignalXY signal in plot.Plot.GetPlottables<SignalXY>().Reverse())
        {
            DataPoint nearest = signal.Data.GetNearest(mouseLocation, plot.Plot.LastRender);
            if (nearest.IsReal)
            {
                return (signal, nearest);
            }
        }

        return (null, DataPoint.None);
    }

    private void FormsPlot1_MouseMove(object sender, MouseEventArgs e)
    {
        var plotControl = (WpfPlot)sender;
        if (plotControl is null)
            return;

        // this rectangle is the area around the mouse in coordinate units
        float x = (float)(e.GetPosition(plotControl).X * ScaleFactor);
        float y = (float)(e.GetPosition(plotControl).Y * ScaleFactor);
        CoordinateRect rect = plotControl.Plot.GetCoordinateRect(x, y, radius: 5);

        // update the cursor to reflect what is beneath it
        if (PlottableBeingDragged is null)
        {
            (var signalUnderMouse, _) = GetSignalXYUnderMouse(plotControl, x, y);
            Cursor = signalUnderMouse is null ? Cursors.Arrow : Cursors.SizeWE;
            return;
        }

        // update the position of the plottable being dragged
        if (PlottableBeingDragged is SignalXY sigXY)
        {
            sigXY.Data.XOffset = rect.HorizontalCenter - StartingDragPosition.X + StartingDragOffset;
            plotControl.Refresh();
        }
    }
}