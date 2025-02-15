using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class MultiplotGrowingAxis : Form, IDemoWindow
{
    public string Title => "Multiplot with Growing Axes";

    public string Description => "Demonstrates how to allow axes to grow to fit long tick labels " +
        "while maintaining layout alignment across subplots in a multiplot.";

    public MultiplotGrowingAxis()
    {
        InitializeComponent();

        // setup multiplot
        formsPlot1.Multiplot.AddPlots(2);
        ScottPlot.Plot plotA = formsPlot1.Multiplot.GetPlot(0);
        ScottPlot.Plot plotB = formsPlot1.Multiplot.GetPlot(1);

        // add sample data
        plotA.Add.Signal(ScottPlot.Generate.Sin());
        plotB.Add.Signal(ScottPlot.Generate.Cos());

        // when a render starts tell all subplots to expand if necessary
        plotA.RenderManager.RenderFinished += (object? sender, ScottPlot.RenderDetails rd) =>
            SetMinimumLeftAxisSize(rd.Layout.PanelSizes[plotA.Axes.Left]);

        plotB.RenderManager.RenderFinished += (object? sender, ScottPlot.RenderDetails rd) =>
            SetMinimumLeftAxisSize(rd.Layout.PanelSizes[plotB.Axes.Left]);
    }

    void SetMinimumLeftAxisSize(float leftAxisSize)
    {
        foreach (ScottPlot.Plot plot in formsPlot1.Multiplot.GetPlots())
        {
            plot.Axes.Left.MinimumSize = leftAxisSize;
        }
    }
}
