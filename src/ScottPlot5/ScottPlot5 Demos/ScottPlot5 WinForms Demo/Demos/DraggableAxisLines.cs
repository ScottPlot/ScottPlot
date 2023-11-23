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
        VLine.Label.Text = "VLine";
        HLine = formsPlot1.Plot.Add.HorizontalLine(0.42);
        HLine.Label.Text = "HLine";
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
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(e.X, e.Y);

        if (PlottableBeingDragged is null)
        {
            // nothing is being dragged, but set the cursor based on what's beneath it
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
        }
        else if (PlottableBeingDragged == VLine)
        {
            VLine.X = rect.HorizontalCenter;
            VLine.Label.Text = $"{VLine.X:0.00}";
        }
        else if (PlottableBeingDragged == HLine)
        {
            HLine.Y = rect.VerticalCenter;
            HLine.Label.Text = $"{HLine.Y:0.00}";
        }

        // if something is being dragged, force a render
        if (PlottableBeingDragged is not null)
        {
            formsPlot1.Refresh();
        }
    }
}
