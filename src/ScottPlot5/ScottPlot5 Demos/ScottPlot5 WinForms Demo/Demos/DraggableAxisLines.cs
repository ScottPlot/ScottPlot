using ScottPlot;
using System.Data;

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

        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin());
        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos());

        VLine = formsPlot1.Plot.Add.VerticalLine(23);
        VLine.Label.Text = "VLine";

        HLine = formsPlot1.Plot.Add.HorizontalLine(0.42);
        HLine.Label.Text = "HLine";

        formsPlot1.Refresh();

        formsPlot1.MouseMove += (s, e) =>
        {
            CoordinateRect rect = GetCoordinateRect(e.X, e.Y);

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

            if (PlottableBeingDragged is not null)
            {
                formsPlot1.Refresh();
            }
        };

        formsPlot1.MouseDown += (s, e) =>
        {
            CoordinateRect rect = GetCoordinateRect(e.X, e.Y);

            if (rect.ContainsX(VLine.X))
            {
                PlottableBeingDragged = VLine;
            }
            else if (rect.ContainsY(HLine.Y))
            {
                PlottableBeingDragged = HLine;
            }

            if (PlottableBeingDragged is not null)
            {
                formsPlot1.Interaction.Actions = ScottPlot.Control.PlotActions.NonInteractive();
            }
        };

        formsPlot1.MouseUp += (s, e) =>
        {
            PlottableBeingDragged = null;
            formsPlot1.Interaction.Actions = ScottPlot.Control.PlotActions.Standard();
            formsPlot1.Refresh();
        };
    }

    /// <summary>
    /// Return a rectangle around the mouse in Axis units.
    /// The size of the rectangle is defned in Pixel units.
    /// </summary>
    public CoordinateRect GetCoordinateRect(float x, float y, float radius = 10)
    {
        PixelRect dataRect = formsPlot1.Plot.RenderManager.LastRender.DataRect;
        double left = formsPlot1.Plot.XAxis.GetCoordinate(x - radius, dataRect);
        double right = formsPlot1.Plot.XAxis.GetCoordinate(x + radius, dataRect);
        double top = formsPlot1.Plot.YAxis.GetCoordinate(y - radius, dataRect);
        double bottom = formsPlot1.Plot.YAxis.GetCoordinate(y + radius, dataRect);
        return new CoordinateRect(left, right, bottom, top);
    }
}
