using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());
        formsPlot1.Plot.Axes.Rules.Add(new ScottPlot.AxisRules.SnapToTicksX(formsPlot1.Plot.Axes.Bottom));
        formsPlot1.Plot.Axes.Rules.Add(new ScottPlot.AxisRules.SnapToTicksY(formsPlot1.Plot.Axes.Left));
    }
}
