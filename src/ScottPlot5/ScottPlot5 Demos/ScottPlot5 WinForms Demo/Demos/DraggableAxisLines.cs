using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class DraggableAxisLines : Form, IDemoWindow
{
    public string Title => "Draggable Axis Lines";
    public string Description => "Demonstrates how to add mouse interactivity to plotted objects";

    AxisLine? PlottableBeingDragged = null;

    public DraggableAxisLines()
    {
        InitializeComponent();

        // place axis lines on the plot
        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());

        var vl = formsPlot1.Plot.Add.VerticalLine(23);
        vl.IsDraggable = true;
        vl.Text = "VLine";

        var hl = formsPlot1.Plot.Add.HorizontalLine(0.42);
        hl.IsDraggable = true;
        hl.Text = "HLine";

        formsPlot1.Refresh();

        // use events for custom mouse interactivity
        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        var lineUnderMouse = GetLineUnderMouse(e.X, e.Y);
        if (lineUnderMouse is not null)
        {
            PlottableBeingDragged = lineUnderMouse;
            formsPlot1.Interaction.Disable(); // disable panning while dragging
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        PlottableBeingDragged = null;
        formsPlot1.Interaction.Enable(); // enable panning again
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        // this rectangle is the area around the mouse in coordinate units
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(e.X, e.Y, radius: 10);

        if (PlottableBeingDragged is null)
        {
            // set cursor based on what's beneath the plottable
            var lineUnderMouse = GetLineUnderMouse(e.X, e.Y);
            if (lineUnderMouse is null) Cursor = Cursors.Default;
            else if (lineUnderMouse.IsDraggable && lineUnderMouse is VerticalLine) Cursor = Cursors.SizeWE;
            else if (lineUnderMouse.IsDraggable && lineUnderMouse is HorizontalLine) Cursor = Cursors.SizeNS;
        }
        else
        {
            // update the position of the plottable being dragged
            if (PlottableBeingDragged is HorizontalLine hl)
            {
                hl.Y = rect.VerticalCenter;
                hl.Text = $"{hl.Y:0.00}";
            }
            else if (PlottableBeingDragged is VerticalLine vl)
            {
                vl.X = rect.HorizontalCenter;
                vl.Text = $"{vl.X:0.00}";
            }
            formsPlot1.Refresh();
        }
    }

    private AxisLine? GetLineUnderMouse(float x, float y)
    {
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(x, y, radius: 10);

        foreach (AxisLine axLine in formsPlot1.Plot.GetPlottables<AxisLine>().Reverse())
        {
            if (axLine.IsUnderMouse(rect))
                return axLine;
        }

        return null;
    }
}
