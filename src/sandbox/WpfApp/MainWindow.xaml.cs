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
        public MainWindow()
        {
            InitializeComponent();

            Random rand = new Random(0);
            int pointCount = 1_000;
            double[] xs = ScottPlot.DataGen.Random(rand, pointCount);
            double[] ys = ScottPlot.DataGen.Random(rand, pointCount);
            WpfPlot1.Plot.AddScatter(xs, ys);
            WpfPlot1.Configuration.Quality = ScottPlot.Control.QualityMode.High;

            WpfPlot2.Plot.Title("Plot 2");
            WpfPlot2.Plot.AddSignal(ScottPlot.DataGen.Cos(51));
        }
    }
}
