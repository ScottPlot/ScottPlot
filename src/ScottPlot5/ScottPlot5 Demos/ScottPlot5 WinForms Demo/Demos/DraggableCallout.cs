using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class DraggableCallout : Form, IDemoWindow
{
    public string Title => "Draggable Callout";
    public string Description => "Demonstrates how to make a Callout mouse-interactive";

    LabelPlot? LabelBeingDragged = null;

    public DraggableCallout()
    {
        InitializeComponent();

        formsPlot1.Plot.Axes.SetLimits(-1, 10, -1, 1);
        var label = formsPlot1.Plot.Add.LabelPlot(
            text: "A beautiful draggable label\nCan be link with an element",
            xLabel: 3,
            yLabel: 0,
            xLine: 3,
            yLine: 0);

        label.Label.BorderColor = Colors.Blue;
        label.Label.BackgroundColor = Colors.Blue.WithAlpha(.5);
        label.Label.Padding = 5;

        label.LineStyle = new LineStyle()
        {
            Color = label.Label.BorderColor,
            Width = label.Label.BorderWidth,
        };

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        if (LabelBeingDragged is null)
        {
            LabelPlot? labelUnderMouse = GetLabelUnderMouse(e.X, e.Y);
            Cursor = labelUnderMouse is null ? Cursors.Arrow : Cursors.Hand;
        }
        else
        {
            LabelBeingDragged.Move(e.X, e.Y);
            formsPlot1.Refresh();
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        LabelBeingDragged = null;
        formsPlot1.Interaction.Enable();
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        LabelPlot? labelUnderMouse = GetLabelUnderMouse(e.X, e.Y);
        if (labelUnderMouse is null)
            return;

        LabelBeingDragged = labelUnderMouse;
        LabelBeingDragged.StartMove(e.X, e.Y);
        formsPlot1.Interaction.Disable();
    }

    private LabelPlot? GetLabelUnderMouse(float x, float y)
    {
        return formsPlot1.Plot
            .GetPlottables<LabelPlot>()
            .Where(p => p.IsUnderMouse(x, y))
            .FirstOrDefault();
    }
}
