using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class SharedAxes : Form, IDemoWindow
{
    public string Title => "Shared Axes";

    public string Description => "Connect two controls together so they share an axis and have aligned layouts";

    public SharedAxes()
    {
        InitializeComponent();

        var sig1 = formsPlot1.Plot.Add.Signal(Generate.Sin());
        sig1.Color = Colors.C0;
        sig1.LineWidth = 2;

        var sig2 = formsPlot2.Plot.Add.Signal(Generate.Cos());
        sig2.Color = Colors.C1;
        sig2.LineWidth = 2;

        checkShareX.CheckedChanged += (s, e) => AutoscaleAllAxes();
        checkShareY.CheckedChanged += (s, e) => AutoscaleAllAxes();

        formsPlot1.Plot.RenderManager.AxisLimitsChanged += (s, e) => CopyLimits(formsPlot1, formsPlot2);
        formsPlot2.Plot.RenderManager.AxisLimitsChanged += (s, e) => CopyLimits(formsPlot2, formsPlot1);
    }

    private void CopyLimits(IPlotControl source, IPlotControl target)
    {
        AxisLimits sourceLimits = source.Plot.Axes.GetLimits();

        if (checkShareX.Checked)
            target.Plot.Axes.SetLimitsX(sourceLimits.Left, sourceLimits.Right);

        if (checkShareY.Checked)
            target.Plot.Axes.SetLimitsY(sourceLimits.Bottom, sourceLimits.Top);

        // prevent infinite loop
        target.Plot.RenderManager.DisableAxisLimitsChangedEventOnNextRender = true;

        target.Refresh();
    }

    private void AutoscaleAllAxes()
    {
        formsPlot1.Plot.Axes.AutoScale();
        formsPlot1.Refresh();
        formsPlot2.Plot.Axes.AutoScale();
        formsPlot2.Refresh();
    }
}
