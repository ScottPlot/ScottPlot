using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        // work with the plot like normal
        formsPlot1.Plot.Add.Signal(Generate.Sin());

        // add a sub-plot and interact with it
        var plot2 = formsPlot1.Multiplot.AddPlot();
        plot2.Add.Signal(Generate.Cos());
    }
}
