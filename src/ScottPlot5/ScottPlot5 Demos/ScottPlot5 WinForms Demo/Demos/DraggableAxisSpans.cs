using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class DraggableAxisSpans : Form, IDemoWindow
{
    public string Title => "Draggable Axis Spans";

    public string Description => "Demonstrates how to create a mouse-interactive " +
        "axis span that can be resized or dragged";

    DraggedSpan? Dragging = null;

    public DraggableAxisSpans()
    {
        InitializeComponent();

        // place axis spans on the plot
        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());
        formsPlot1.Plot.Add.VerticalSpan(.23, .78);
        formsPlot1.Plot.Add.HorizontalSpan(23, 42);
        formsPlot1.Refresh();

        // use events for custom mouse interactivity
        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        Dragging = DraggedSpan.GetThingUnderMouse(formsPlot1, e.X, e.Y);

        // disable pan/zoom while a plottable is being dragged
        if (Dragging is not null)
        {
            formsPlot1.Interaction.Disable();
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        Dragging = null;
        formsPlot1.Interaction.Enable();
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        if (Dragging is not null)
        {
            // currently draggins something so update it
            Coordinates cs = formsPlot1.Plot.GetCoordinates(e.X, e.Y);
            Dragging.Update(cs);
            formsPlot1.Refresh();
        }
        else
        {
            // not dragging anything
            DraggedSpan? spanUnderMouse = DraggedSpan.GetThingUnderMouse(formsPlot1, e.X, e.Y);
            if (spanUnderMouse is null) Cursor = Cursors.Default;
            else if (spanUnderMouse.IsVerticalEdge) Cursor = Cursors.SizeWE;
            else if (spanUnderMouse.IsHorizontalEdge) Cursor = Cursors.SizeNS;
            else Cursor = Cursors.SizeAll;
        }
    }
}
