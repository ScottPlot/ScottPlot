using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.UserInputProcessor.EXPERIMENTAL_TAKEOVER();

        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());
    }
}
