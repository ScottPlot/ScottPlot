using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WpfPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51, 2));
            WpfPlot2.Plot.AddSignal(ScottPlot.DataGen.Cos(51, 2));

            WpfPlot1.Refresh();
            WpfPlot2.Refresh();

            WpfPlot1.Configuration.AddLinkedControl(WpfPlot2); // update plot 2 when plot 1 changes
            WpfPlot2.Configuration.AddLinkedControl(WpfPlot1); // update plot 1 when plot 2 changes
        }
    }
}
