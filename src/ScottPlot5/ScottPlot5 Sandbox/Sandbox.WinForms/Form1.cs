using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        // default
        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());

        // multiplot
        formsPlot2.Plot.Add.Signal(Generate.Sin());
        var plot2 = formsPlot2.Multiplot.AddPlot();
        plot2.Add.Signal(Generate.Cos());
    }
}
