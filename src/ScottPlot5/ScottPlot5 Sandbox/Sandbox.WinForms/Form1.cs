using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        // the Plot is actually the single Plot inside a Multiplot
        formsPlot1.Plot.Add.Signal(Generate.Sin());

        // subplots can be created and added to the plot
        var plot2 = formsPlot1.Multiplot.AddPlot();
        plot2.Add.Signal(Generate.Cos());
    }
}
