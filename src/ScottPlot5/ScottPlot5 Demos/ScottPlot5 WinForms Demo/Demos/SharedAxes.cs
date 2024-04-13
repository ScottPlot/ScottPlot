using ScottPlot;
using System.Diagnostics;

namespace WinForms_Demo.Demos;

public partial class SharedAxes : Form, IDemoWindow
{
    public string Title => "Shared Axes";

    public string Description => "Connect two controls together so they share an axis and have aligned layouts";

    public SharedAxes()
    {
        InitializeComponent();

        // plot sample data
        formsPlot1.Plot.Add.Signal(Generate.Sin(51, mult: 1));
        formsPlot2.Plot.Add.Signal(Generate.Sin(51, mult: 100_000));

        // add labels
        formsPlot1.Plot.Axes.Left.Label.Text = "Vertical Axis";
        formsPlot2.Plot.Axes.Left.Label.Text = "Vertical Axis";
        formsPlot1.Plot.Axes.Bottom.Label.Text = "Horizontal Axis";
        formsPlot2.Plot.Axes.Bottom.Label.Text = "Horizontal Axis";

        // use a fixed size for the left axis panel to ensure it's always aligned
        float leftAxisSize = 90;
        formsPlot1.Plot.Axes.Left.MinimumSize = leftAxisSize;
        formsPlot1.Plot.Axes.Left.MaximumSize = leftAxisSize;
        formsPlot2.Plot.Axes.Left.MinimumSize = leftAxisSize;
        formsPlot2.Plot.Axes.Left.MaximumSize = leftAxisSize;

        // when one plot changes update the other plot
        formsPlot1.Plot.RenderManager.AxisLimitsChanged += (s, e) =>
        {
            ApplyLayoutToOtherPlot(formsPlot1, formsPlot2);
        };
        formsPlot2.Plot.RenderManager.AxisLimitsChanged += (s, e) =>
        {
            ApplyLayoutToOtherPlot(formsPlot2, formsPlot1);
        };

        // initial render
        formsPlot1.Refresh();
        formsPlot2.Refresh();
    }

    private void ApplyLayoutToOtherPlot(IPlotControl source, IPlotControl dest)
    {
        AxisLimits axesBefore = dest.Plot.Axes.GetLimits();
        dest.Plot.Axes.SetLimitsX(source.Plot.Axes.GetLimits());
        AxisLimits axesAfter = dest.Plot.Axes.GetLimits();
        if (axesBefore != axesAfter)
        {
            dest.Refresh();
        }
    }
}
