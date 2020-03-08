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

            wpfPlot1.plt.Title("ScottPlot QuickStart: WPF");
            wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.Sin(50));
            wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.Cos(50));
            wpfPlot1.Render();
        }
    }
}
