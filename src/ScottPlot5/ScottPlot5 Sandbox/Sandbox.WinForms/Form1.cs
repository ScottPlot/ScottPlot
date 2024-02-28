using ScottPlot;
using ScottPlot.Plottables;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    LabelPlot? labelBeingDragged = null;
    Coordinates delta = new ();
    public Form1()
    {
        InitializeComponent();

        var ohlcs = Generate.RandomOHLCs(12_000);
        formsPlot1.Plot.Add.Candlestick(ohlcs);

        var label = formsPlot1.Plot.Add.LabelPlot("Test", 55_000, 300, 55_000, 320);

        label.Label.BorderColor = Colors.Blue;
        label.Label.BackColor = Colors.Blue.WithAlpha(.5);
        label.Label.Padding = 5;

        label.MarkerStyle = new()
        {
            IsVisible = true,
            Shape = MarkerShape.FilledTriangleDown,
            Size = 10
        };

        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        CoordinateRect rect = formsPlot1.Plot.GetCoordinateRect(e.X, e.Y);

        if (labelBeingDragged is null)
        {
            //Cursor = Cursors.Cross;
        }
        else
        {
            labelBeingDragged.Location = new Coordinates(rect.Center.X - delta.X, rect.Center.Y - delta.Y);
            formsPlot1.Refresh();
        }
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        labelBeingDragged = null;
        formsPlot1.Interaction.Enable();
        formsPlot1.Refresh();
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        LabelPlot? labelUnderMouse = GetLabelUnderMouse(e.X, e.Y);
        if (labelUnderMouse is not null)
        {
            labelBeingDragged = labelUnderMouse;
            formsPlot1.Interaction.Disable();
        }
    }

    private LabelPlot? GetLabelUnderMouse(float x, float y)
    {
        foreach (LabelPlot label in formsPlot1.Plot.GetPlottables<LabelPlot>().Reverse())
        {
            if (label.IsUnderMouse(x, y))
            {
                return label;
            }
        }

        return null;
    }
}
