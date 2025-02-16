using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(Generate.Sin());

        button1.Click += (s, e) =>
        {
            formsPlot1.Reset();
            formsPlot1.Plot.Add.Signal(Generate.RandomSample(100));
            formsPlot1.Refresh();
        };
    }
}
