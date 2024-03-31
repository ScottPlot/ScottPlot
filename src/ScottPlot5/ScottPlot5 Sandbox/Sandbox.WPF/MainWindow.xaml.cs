using System.Linq;
using System.Windows;
using System.Windows.Input;
using ScottPlot;
using ScottPlot.WPF;
using ScottPlot.Plottables;
using System.Collections.Generic;

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    SignalXY? PlottableBeingDragged = null;
    DataPoint StartingDragPosition = DataPoint.None;
    double StartingDragOffset = 0;

    public MainWindow()
    {
        InitializeComponent();
        double[] xAxis = new List<double>(Enumerable.Range(0, 100).Select(x => (double)x)).ToArray();
        WpfPlot1.Plot.Add.SignalXY(xAxis,Generate.Sin(100));
        WpfPlot1.Plot.Add.SignalXY(xAxis,Generate.Cos(100));

        WpfPlot1.MouseDown += FormsPlot1_MouseDown;
        WpfPlot1.MouseUp += FormsPlot1_MouseUp;
        WpfPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var plot = (WpfPlot)sender;
        if (plot != null)
        {
            double factor = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;
            double x = e.GetPosition(plot).X * factor;
            double y = e.GetPosition(plot).Y * factor;
            DataPoint dataPoint;
            var signalUnderMouse = GetSignalUnderMouse(plot, x, y, out dataPoint);
            if (signalUnderMouse != null)
            {
                PlottableBeingDragged = signalUnderMouse;
                StartingDragPosition = dataPoint;
                StartingDragOffset = signalUnderMouse.Data.XOffset;
                plot.Interaction.Disable(); // disable panning while dragging
            }
        }
    }
    private void FormsPlot1_MouseUp(object sender, MouseButtonEventArgs e)
    {
        var plot = (WpfPlot)sender;
        if (plot != null)
        {
            PlottableBeingDragged = null;
            StartingDragPosition = DataPoint.None;
            plot.Interaction.Enable(); // enable panning again
            plot.Refresh();
        }
    }

    private SignalXY GetSignalUnderMouse(WpfPlot plot, double x, double y, out DataPoint datapoint)
    {
        Pixel mousePixel = new(x, y);
        datapoint = DataPoint.None;

        Coordinates mouseLocation = plot.Plot.GetCoordinates(mousePixel);

        foreach (SignalXY signal in plot.Plot.GetPlottables<SignalXY>().Reverse())
        {
            DataPoint nearest = signal.Data.GetNearest(mouseLocation, plot.Plot.LastRender);
            if (nearest.IsReal)
            {
                datapoint = nearest;
                return signal;
            }
        }

        return null;
    }

    private void FormsPlot1_MouseMove(object sender, MouseEventArgs e)
    {
        var plot = (WpfPlot)sender;
        if (plot != null)
        {
            double factor = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;

            // this rectangle is the area around the mouse in coordinate units
            CoordinateRect rect = plot.Plot.GetCoordinateRect((float)(e.GetPosition(plot).X * factor), (float)(e.GetPosition(plot).Y * factor), radius: 5);

            if (PlottableBeingDragged is null)
            {
                var signalUnderMouse = GetSignalUnderMouse(plot, e.GetPosition(plot).X * factor, e.GetPosition(plot).Y * factor, out _);
                if (signalUnderMouse is null) Cursor = Cursors.Arrow;
                else Cursor = Cursors.SizeWE;
            }
            else
            {
                // update the position of the plottable being dragged
                if (PlottableBeingDragged is SignalXY vl)
                {
                    vl.Data.XOffset = rect.HorizontalCenter - StartingDragPosition.X + StartingDragOffset;
                }
                plot.Refresh();
            }
        }
    }
}