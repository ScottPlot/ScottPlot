using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        SetupTab1();
        SetupTab2();

        tabControl1.SelectedIndex = 1;
    }

    private void SetupTab1()
    {
        // add sample data
        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());
    }

    private void SetupTab2()
    {
        // add sample data
        formsPlot2.Plot.Add.Signal(Generate.Sin());
        formsPlot3.Plot.Add.Signal(Generate.Cos());

        // link axes of two controls together bidirectionally
        formsPlot2.Plot.Axes.Link(formsPlot3);
        formsPlot3.Plot.Axes.Link(formsPlot2);
    }
}
