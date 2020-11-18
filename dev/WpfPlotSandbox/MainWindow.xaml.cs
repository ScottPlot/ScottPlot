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
            var YAxis3 = new ScottPlot.Renderable.AdditionalRightAxis(2, "Third Axis");
            YAxis3.Title.Label = "Tertiary Vertical Axis";
            wpfPlot1.plt.GetSettings(false).Axes.Add(YAxis3);

            var sig1 = wpfPlot1.plt.PlotSignal(DataGen.Sin(51, mult: 1, phase: 0));
            var sig2 = wpfPlot1.plt.PlotSignal(DataGen.Sin(51, mult: 10, phase: .2));
            var sig3 = wpfPlot1.plt.PlotSignal(DataGen.Sin(51, mult: 100, phase: .4));

            sig1.VerticalAxisIndex = 0;
            sig2.VerticalAxisIndex = 1;
            sig3.VerticalAxisIndex = 2;

            wpfPlot1.Render();
        }
    }
}
