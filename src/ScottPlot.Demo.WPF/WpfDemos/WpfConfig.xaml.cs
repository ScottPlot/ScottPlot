using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for WpfConfig.xaml
    /// </summary>
    public partial class WpfConfig : Window
    {
        public WpfConfig()
        {
            InitializeComponent();

            int pointCount = 51;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            wpfPlot1.plt.PlotScatter(dataXs, dataSin);
            wpfPlot1.plt.PlotScatter(dataXs, dataCos);
            wpfPlot1.Render();
        }

        private void PanEnable(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(enablePanning: true); }
        private void PanDisable(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(enablePanning: false); }
        private void ZoomEnable(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(enableRightClickZoom: true, enableScrollWheelZoom: true); }
        private void ZoomDisable(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(enableRightClickZoom: false, enableScrollWheelZoom: false); }
        private void DragLowQualityEnable(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(lowQualityWhileDragging: true); }
        private void DragLowQualityDisable(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(lowQualityWhileDragging: false); }
        private void VerticalLock(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(lockVerticalAxis: true); }
        private void VerticalUnlock(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(lockVerticalAxis: false); }
        private void HorizontalLock(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(lockHorizontalAxis: true); }
        private void HorizontalUnlock(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(lockHorizontalAxis: true); }
        private void EqualAxisLock(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(equalAxes: true); wpfPlot1.Render(); }
        private void EqualAxisUnlock(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(equalAxes: false); }
        private void DoubleClickBenchmarkEnable(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(enableDoubleClickBenchmark: true); }
        private void DoubleClickBenchmarkDisable(object sender, RoutedEventArgs e) { wpfPlot1?.Configure(enableDoubleClickBenchmark: false); wpfPlot1.plt.Benchmark(false); wpfPlot1.Render(); }

        private void RightClickMenuEnable(object sender, RoutedEventArgs e)
        {
            // TODO - currently WPF control has no right-click menu
        }

        private void RightClickMenuDisable(object sender, RoutedEventArgs e)
        {

        }

        private void CustomRightClickEnable(object sender, RoutedEventArgs e)
        {

        }

        private void CustomRightClickDisable(object sender, RoutedEventArgs e)
        {

        }

    }
}
