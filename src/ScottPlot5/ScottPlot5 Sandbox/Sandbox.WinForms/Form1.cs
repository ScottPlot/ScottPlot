namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin(51));
        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos(51));
        formsPlot1.Refresh();
    }
}
