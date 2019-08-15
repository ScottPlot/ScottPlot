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
            wpfPlot1.plt.Title("WPF Demo");
            wpfPlot1.plt.YLabel("signal level");
            wpfPlot1.plt.XLabel("horizontal units");
        }

        private void AddPlot(object sender, RoutedEventArgs e)
        {
            wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 1000));
            wpfPlot1.plt.AxisAuto();
            wpfPlot1.Render();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            wpfPlot1.plt.Clear();
            wpfPlot1.Render();
        }
    }
}
