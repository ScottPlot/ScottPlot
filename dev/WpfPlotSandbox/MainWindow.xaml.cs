using ScottPlot;
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

namespace WpfPlotSandbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            wpfPlot1.plt.PlotSignal(DataGen.Sin(51));
            wpfPlot1.plt.PlotSignal(DataGen.Cos(51));

            wpfPlot1.plt.Ticks(false, false);
            wpfPlot1.plt.Frame(false);
            wpfPlot1.plt.TightenLayout(padding: 0);
            wpfPlot1.Configure(recalculateLayoutOnMouseUp: false);
            wpfPlot1.plt.Style(ScottPlot.Style.Gray2);

            wpfPlot1.Render();
        }
    }
}
