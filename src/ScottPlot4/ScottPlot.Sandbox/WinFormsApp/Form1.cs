using ScottPlot;
using System.Windows.Forms;

namespace WinFormsApp;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.AddSignal(DataGen.Sin(51));
        formsPlot1.Plot.AddSignal(DataGen.Cos(51));
        formsPlot1.Refresh();
    }
}
