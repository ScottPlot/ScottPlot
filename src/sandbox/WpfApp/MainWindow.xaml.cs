using System;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WpfPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51));
            WpfPlot1.Plot.AddSignal(ScottPlot.DataGen.Cos(51));
            WpfPlot1.Refresh();
        }
    }
}
