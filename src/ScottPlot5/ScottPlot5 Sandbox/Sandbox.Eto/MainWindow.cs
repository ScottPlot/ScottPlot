using Eto.Forms;
using ScottPlot;

namespace Sandbox.Eto;

partial class MainWindow : Form
{
    public MainWindow()
    {
        InitializeComponent();
        etoPlot.Plot.Add.Signal(Generate.Sin(51));
        etoPlot.Plot.Add.Signal(Generate.Cos(51));
        etoPlot.Refresh();
    }
}
