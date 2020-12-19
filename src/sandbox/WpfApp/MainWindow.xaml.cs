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

namespace WpfApp
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
            PlotRandomBars(WpfPlot1.plt);
            PlotRandomBars(WpfPlot2.plt);
            PlotRandomBars(WpfPlot3.plt);
        }

        private void PlotRandomBars(ScottPlot.Plot plt, int count = 10)
        {
            double[] values = ScottPlot.DataGen.Random(rand, count);
            string[] labels = Enumerable.Range(1, count).Select(x => $"bar number {x}").ToArray();
            plt.AddBar(values);
            plt.XTicks(labels);
            plt.XAxis.TickLabelStyle(rotation: 45);
            plt.SetAxisLimits(yMin: 0);
        }
    }
}
