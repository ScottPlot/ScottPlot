using ScottPlot;
using ScottPlot.Control;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        var sig1 = formsPlot1.Plot.Add.Signal(Generate.Sin());
        sig1.Axes.YAxis = formsPlot1.Plot.Axes.Left;
        formsPlot1.Plot.Axes.Color(sig1.Axes.YAxis, sig1.Color);

        var sig2 = formsPlot1.Plot.Add.Signal(Generate.Cos(mult: 1000));
        sig2.Axes.YAxis = formsPlot1.Plot.Axes.Right;
        formsPlot1.Plot.Axes.Color(sig2.Axes.YAxis, sig2.Color);

        checkBox1.CheckedChanged += (s, e) => formsPlot1.Interaction.ChangeOpposingAxesTogether = checkBox1.Checked;
    }
}
