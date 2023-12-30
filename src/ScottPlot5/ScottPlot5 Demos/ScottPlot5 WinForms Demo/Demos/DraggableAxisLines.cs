using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class DraggableAxisLines : Form, IDemoWindow
{
    public string Title => "Draggable Axis Lines";
    public string Description => "Demonstrates how to add mouse interactivity to plotted objects";

    ScottPlot.Plottables.VerticalLine VLine;
    ScottPlot.Plottables.HorizontalLine HLine;
    IPlottable? PlottableBeingDragged = null;

    public DraggableAxisLines()
    {
        InitializeComponent();

        // place axis lines on the plot
        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());
        VLine = formsPlot1.Plot.Add.VerticalLine(23);
        VLine.Text = "VLine";
        HLine = formsPlot1.Plot.Add.HorizontalLine(0.42);
        HLine.Text = "HLine";
        formsPlot1.Refresh();

        // use events for custom mouse interactivity
        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        // determine the range of coordinates around the mouse position
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(e.X, e.Y);

        // determine if a plottable is within the coordinate range near the mouse
        if (rect.ContainsX(VLine.X))
        {
            PlottableBeingDragged = VLine;
        }
        else if (rect.ContainsY(HLine.Y))
        {
            PlottableBeingDragged = HLine;
        }

        // disable pan/zoom while a plottable is being dragged
        if (PlottableBeingDragged is not null)
        {
            formsPlot1.Interaction.Disable();
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        PlottableBeingDragged = null;
        formsPlot1.Interaction.Enable();
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        // this rectangle is the area around the mouse in coordinate units
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(e.X, e.Y, radius: 10);

        if (PlottableBeingDragged is null)
        {
            if (rect.ContainsX(VLine.X))
            {
                Cursor = Cursors.SizeWE;
            }
            else if (rect.ContainsY(HLine.Y))
            {
                Cursor = Cursors.SizeNS;
            }
            else
            {
                Cursor = Cursors.Arrow;
            }

            return;
        }

        if (PlottableBeingDragged == VLine)
        {
            VLine.X = rect.HorizontalCenter;
            VLine.Text = $"{VLine.X:0.00}";
            formsPlot1.Refresh();
            return;
        }

        if (PlottableBeingDragged == HLine)
        {
            HLine.Y = rect.VerticalCenter;
            HLine.Text = $"{HLine.Y:0.00}";
            formsPlot1.Refresh();
            return;
        }
    }
}
