using Eto.Forms;
using ScottPlot;

namespace Sandbox.Eto;

partial class MainWindow : Form
{
    public MainWindow()
    {
        InitializeComponent();
        EtoPlot1.Plot.Add.Signal(Generate.Sin());
        EtoPlot1.Plot.Add.Signal(Generate.Cos());
        EtoPlot1.Refresh();
    }
}
