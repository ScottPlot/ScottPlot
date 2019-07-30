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
        public MainWindow()
        {
            InitializeComponent();

            int pointCount = 100;
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] ys = ScottPlot.DataGen.Sin(pointCount);
            scottPlotUC1.plt.Title("WPF Demo");
            scottPlotUC1.plt.PlotScatter(xs, ys);
        }
    }
}
