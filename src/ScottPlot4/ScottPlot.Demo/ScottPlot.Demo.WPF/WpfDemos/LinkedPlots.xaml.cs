using System.Windows;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for LinkedPlots.xaml
    /// </summary>
    public partial class LinkedPlots : Window
    {
        public LinkedPlots()
        {
            InitializeComponent();

            wpfPlot1.Plot.AddSignal(DataGen.Sin(51, 2), color: System.Drawing.Color.Blue);
            wpfPlot2.Plot.AddSignal(DataGen.Cos(51, 2), color: System.Drawing.Color.Red);

            wpfPlot1.Refresh();
            wpfPlot2.Refresh();

            wpfPlot1.Configuration.AddLinkedControl(wpfPlot2); // update plot 2 when plot 1 changes
            wpfPlot2.Configuration.AddLinkedControl(wpfPlot1); // update plot 1 when plot 2 changes
        }
    }
}
