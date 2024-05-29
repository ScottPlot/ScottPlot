using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class DraggableCallout : Form, IDemoWindow
{
    public string Title => "Draggable Callout";
    public string Description => "Demonstrates how to make a Callout mouse-interactive";

    Callout? CalloutBeingDragged = null;
    PixelOffset MouseDownOffset;

    public DraggableCallout()
    {
        InitializeComponent();

        var fp = formsPlot1.Plot.Add.Function(SampleData.DunningKrugerCurve);
        fp.MinX = 0;
        fp.MaxX = 2;
        fp.LineWidth = 3;

        formsPlot1.Plot.Add.Callout(
            text: "Peak of \"Mount Stupid\"",
            textLocation: new(0.35, 1.05),
            tipLocation: new(0.2185, fp.GetY(0.2185)));

        formsPlot1.Plot.Add.Callout(
            text: "Valley of Despair",
            textLocation: new(0.35, 0.6),
            tipLocation: new(0.3885, fp.GetY(0.3885)));

        formsPlot1.Plot.Add.Callout(
            text: "Slope of Enlightenment",
            textLocation: new(0.9, 0.3),
            tipLocation: new(0.76935, fp.GetY(0.76935)));

        formsPlot1.Plot.Add.Callout(
            text: "Plateau of Sustainability",
            textLocation: new(1.4, 0.8),
            tipLocation: new(1.701, fp.GetY(1.701)));

        formsPlot1.Plot.YLabel("Confidence");
        formsPlot1.Plot.XLabel("Competence");
        formsPlot1.Plot.Title("Dunning-Kruger Effect", 24);

        formsPlot1.Plot.Axes.SetLimitsX(0, 2);
        formsPlot1.Plot.Axes.SetLimitsY(0, 1.2);

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }


    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        if (CalloutBeingDragged is null)
        {
            Callout? calloutUnderMouse = GetCalloutUnderMouse(e.X, e.Y);
            Cursor = calloutUnderMouse is null ? Cursors.Arrow : Cursors.Hand;
            if (calloutUnderMouse is not null)
                formsPlot1.Plot.MoveToFront(calloutUnderMouse);
        }
        else
        {
            Pixel mousePixel = new(e.X + MouseDownOffset.X, e.Y + MouseDownOffset.Y);
            Coordinates mouseCoordinates = CalloutBeingDragged.Axes.GetCoordinates(mousePixel);
            CalloutBeingDragged.TextCoordinates = mouseCoordinates;
            formsPlot1.Refresh();
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        CalloutBeingDragged = null;
        formsPlot1.Interaction.Enable();
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        Callout? calloutUnderMouse = GetCalloutUnderMouse(e.X, e.Y);
        if (calloutUnderMouse is null)
            return;

        CalloutBeingDragged = calloutUnderMouse;

        float dX = calloutUnderMouse!.TextPixel.X - e.X;
        float dY = calloutUnderMouse!.TextPixel.Y - e.Y;
        MouseDownOffset = new(dX, dY);

        formsPlot1.Interaction.Disable();
        FormsPlot1_MouseMove(sender, e);
    }

    private Callout? GetCalloutUnderMouse(float x, float y)
    {
        return formsPlot1.Plot
            .GetPlottables<Callout>()
            .Reverse()
            .Where(p => p.LastRenderRect.Contains(x, y))
            .FirstOrDefault();
    }
}
