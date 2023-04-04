using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos;

public partial class AxisLimits : Form
{
    public AxisLimits()
    {
        InitializeComponent();
        formsPlot1.Plot.AddSignal(DataGen.Sin(51));
        formsPlot1.Plot.AddSignal(DataGen.Cos(51));
        formsPlot1.Plot.AxisAuto();
        formsPlot1.Plot.SetOuterViewLimits(0, 50, -1, 1);
        formsPlot1.Refresh();
    }
}
