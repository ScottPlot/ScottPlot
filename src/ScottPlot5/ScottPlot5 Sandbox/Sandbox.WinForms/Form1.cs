using ScottPlot;
using ScottPlot.Plottables;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    LabelPlot? labelBeingDragged = null;
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.Axes.SetLimits(-1, 10, -1, 1);
        var label = formsPlot1.Plot.Add.LabelPlot("Une belle étiquette\nAvec sa description\nEt d'autres informations", 3, 0, 3, 0);

        label.Label.BorderColor = Colors.Blue;
        label.Label.BackColor = Colors.Blue.WithAlpha(.5);
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
        if (labelBeingDragged is null) return;
        labelBeingDragged.Move(e.X, e.Y);
        formsPlot1.Refresh();
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
            labelBeingDragged.StartMove(e.X, e.Y);
            formsPlot1.Interaction.Disable();
        }
    }

    private LabelPlot? GetLabelUnderMouse(float x, float y)
    {
        foreach (LabelPlot label in formsPlot1.Plot.GetPlottables<LabelPlot>())
        {
            if (label.IsUnderMouse(x, y)) return label;
        }

        return null;
    }
}
