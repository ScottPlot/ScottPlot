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

            TestQuickstart();
            //TestDraggables();
        }

        private void TestQuickstart()
        {
            wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.Sin(50));
            wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.Cos(50));
            wpfPlot1.plt.Title("ScottPlot WPF Quickstart");
            wpfPlot1.Render();
        }

        private void TestDraggables()
        {
            wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.Sin(50));
            wpfPlot1.plt.PlotSignal(ScottPlot.DataGen.Cos(50));

            wpfPlot1.plt.PlotVLine(25, draggable: true, dragLimitLower: 0, dragLimitUpper: 50);
            wpfPlot1.plt.PlotHLine(0.25, draggable: true, dragLimitLower: -1, dragLimitUpper: 1);

            wpfPlot1.plt.PlotVSpan(35, 45, draggable: true, dragLimitLower: 0, dragLimitUpper: 50);
            wpfPlot1.plt.PlotHSpan(-.75, -.45, draggable: true, dragLimitLower: -1, dragLimitUpper: 1);

            wpfPlot1.plt.Title("WPF Test Draggables");
            wpfPlot1.Render();
        }
    }
}
