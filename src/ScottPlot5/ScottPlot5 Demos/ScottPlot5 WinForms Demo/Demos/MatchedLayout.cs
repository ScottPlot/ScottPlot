using ScottPlot;
using System.Diagnostics;

namespace WinForms_Demo.Demos;

public partial class MatchedLayout : Form, IDemoWindow
{
    public string Title => "Matched Axes and Layouts";

    public string Description => "Connect two controls together so they share an axis and have aligned layouts";

    public MatchedLayout()
    {
        InitializeComponent();

        // plot sample data
        formsPlot1.Plot.Add.Signal(Generate.Sin(51, mult: 1));
        formsPlot2.Plot.Add.Signal(Generate.Sin(51, mult: 100_000));

        // use a fixed size for the left axis panel to ensure it's always aligned
        float leftAxisSize = 70;
        formsPlot1.Plot.LeftAxis.MinimumSize = leftAxisSize;
        formsPlot1.Plot.LeftAxis.MaximumSize = leftAxisSize;
        formsPlot2.Plot.LeftAxis.MinimumSize = leftAxisSize;
        formsPlot2.Plot.LeftAxis.MaximumSize = leftAxisSize;

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
        AxisLimits axesBefore = dest.Plot.GetAxisLimits();
        dest.Plot.SetAxisLimitsX(source.Plot.GetAxisLimits());
        AxisLimits axesAfter = dest.Plot.GetAxisLimits();
        if (axesBefore != axesAfter)
        {
            dest.Refresh();
        }
    }
}
