using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class SharedAxes : Form, IDemoWindow
{
    public string Title => "Shared Axes";

    public string Description => "Connect two controls together so they share an axis and have aligned layouts";

    readonly System.Windows.Forms.Timer LayoutCheckTimer = new() { Interval = 10 };

    public SharedAxes()
    {
        InitializeComponent();

        // plot a signal with small numbers
        var sig1 = formsPlot1.Plot.Add.Signal(Generate.Sin(51, mult: 1));
        sig1.Color = Colors.C0;
        sig1.LineWidth = 2;

        // plot a signal with big numbers
        var sig2 = formsPlot2.Plot.Add.Signal(Generate.Sin(51, mult: 100_000));
        sig2.Color = Colors.C1;
        sig2.LineWidth = 2;

        // add labels
        formsPlot1.Plot.Axes.Left.Label.Text = "Small Signal";
        formsPlot2.Plot.Axes.Left.Label.Text = "Big Signal";
        formsPlot2.Plot.Axes.Bottom.Label.Text = "Shared Horizontal Axis";

        // use a fixed size for the left axis panel to ensure it's always aligned
        float leftAxisSize = 90;
        formsPlot1.Plot.Axes.Left.MinimumSize = leftAxisSize;
        formsPlot1.Plot.Axes.Left.MaximumSize = leftAxisSize;
        formsPlot2.Plot.Axes.Left.MinimumSize = leftAxisSize;
        formsPlot2.Plot.Axes.Left.MaximumSize = leftAxisSize;

        // when one plot changes update the other plot
        formsPlot1.Plot.RenderManager.AxisLimitsChanged += (s, e) => PlotsToCopy = (formsPlot1, formsPlot2);
        formsPlot2.Plot.RenderManager.AxisLimitsChanged += (s, e) => PlotsToCopy = (formsPlot2, formsPlot1);

        // setup a timer to check when limits should be copied
        LayoutCheckTimer.Tick += (s, e) => CopyLimits();
        LayoutCheckTimer.Start();

        // initial render
        formsPlot1.Refresh();
        formsPlot2.Refresh();
    }

    private (IPlotControl from, IPlotControl to)? PlotsToCopy = null;

    private void CopyLimits()
    {
        if (PlotsToCopy is null)
            return;
        (IPlotControl source, IPlotControl target) = PlotsToCopy.Value;
        AxisLimits sourceLimits = source.Plot.Axes.GetLimits();
        target.Plot.Axes.SetLimitsX(sourceLimits);
        target.Refresh();
    }
}
