using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        Plot plot1 = multiFormsPlot1.SubPlots.Add();
        plot1.Add.Signal(Generate.Sin());
        plot1.Title("Plot 1");

        Plot plot2 = multiFormsPlot1.SubPlots.Add();
        plot2.Add.Signal(Generate.Cos());
        plot2.Title("Plot 2");
    }
}
