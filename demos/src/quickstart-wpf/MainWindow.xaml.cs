using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScottPlotQuickstartWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
            scottPlotUC1.plt.Title("WPF Demo");
            scottPlotUC1.plt.YLabel("signal level");
            scottPlotUC1.plt.XLabel("horizontal units");
        }

        private void AddPlot(object sender, RoutedEventArgs e)
        {
            scottPlotUC1.plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 1000));
            scottPlotUC1.plt.AxisAuto();
            scottPlotUC1.Render();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            scottPlotUC1.plt.Clear();
            scottPlotUC1.Render();
        }
    }
}
