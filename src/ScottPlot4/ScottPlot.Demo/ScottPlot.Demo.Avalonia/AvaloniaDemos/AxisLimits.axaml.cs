using Avalonia.Controls;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public partial class AxisLimits : Window
    {
        public AxisLimits()
        {
            this.InitializeComponent();

            avaPlot1.Plot.AddSignal(DataGen.Sin(51));
            avaPlot1.Plot.AddSignal(DataGen.Cos(51));
            avaPlot1.Plot.AxisAuto();
            avaPlot1.Plot.XAxis.SetBoundary(0, 50);
            avaPlot1.Plot.YAxis.SetBoundary(-1, 1);
            avaPlot1.Refresh();
        }
    }
}
