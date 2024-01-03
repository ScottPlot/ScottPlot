using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());

        AxisLimits limits = new(0, 50, -1, 1);

        ScottPlot.AxisRules.MinimumBoundary rule1 = new(formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Left, limits);
        formsPlot1.Plot.Axes.Rules.Add(rule1);

        ScottPlot.AxisRules.MaximumBoundary rule2 = new(formsPlot1.Plot.Axes.Bottom, formsPlot1.Plot.Axes.Left, limits);
        formsPlot1.Plot.Axes.Rules.Add(rule2);

        formsPlot1.Refresh();
    }
}
