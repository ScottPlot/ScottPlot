using System.Windows;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for AxisLimits.xaml
    /// </summary>
    public partial class AxisLimits : Window
    {
        public AxisLimits()
        {
            InitializeComponent();
            wpfPlot1.Plot.AddSignal(DataGen.Sin(51));
            wpfPlot1.Plot.AddSignal(DataGen.Cos(51));
            wpfPlot1.Plot.AxisAuto();
            wpfPlot1.Plot.SetOuterViewLimits(0, 50, -1, 1);
            wpfPlot1.Refresh();
        }
    }
}
