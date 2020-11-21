using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class AvaloniaConfig : Window
    {
        AvaPlot avaPlot1;
        public AvaloniaConfig()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            int pointCount = 51;
            double[] dataXs = DataGen.Consecutive(pointCount);
            double[] dataSin = DataGen.Sin(pointCount);
            double[] dataCos = DataGen.Cos(pointCount);

            avaPlot1 = this.Find<AvaPlot>("avaPlot1");

            avaPlot1.plt.PlotScatter(dataXs, dataSin);
            avaPlot1.plt.PlotScatter(dataXs, dataCos);
            avaPlot1.Render();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void PanEnable(object sender, RoutedEventArgs e) { avaPlot1?.Configure(enablePanning: true); }
        private void PanDisable(object sender, RoutedEventArgs e) { avaPlot1?.Configure(enablePanning: false); }
        private void ZoomEnable(object sender, RoutedEventArgs e) { avaPlot1?.Configure(enableRightClickZoom: true, enableScrollWheelZoom: true); }
        private void ZoomDisable(object sender, RoutedEventArgs e) { avaPlot1?.Configure(enableRightClickZoom: false, enableScrollWheelZoom: false); }
        private void DragLowQualityEnable(object sender, RoutedEventArgs e) { avaPlot1?.Configure(lowQualityWhileDragging: true); }
        private void DragLowQualityDisable(object sender, RoutedEventArgs e) { avaPlot1?.Configure(lowQualityWhileDragging: false); }
        private void VerticalLock(object sender, RoutedEventArgs e) { avaPlot1?.Configure(lockVerticalAxis: true); }
        private void VerticalUnlock(object sender, RoutedEventArgs e) { avaPlot1?.Configure(lockVerticalAxis: false); }
        private void HorizontalLock(object sender, RoutedEventArgs e) { avaPlot1?.Configure(lockHorizontalAxis: true); }
        private void HorizontalUnlock(object sender, RoutedEventArgs e) { avaPlot1?.Configure(lockHorizontalAxis: false); }
        private void EqualAxisLock(object sender, RoutedEventArgs e) { avaPlot1?.Configure(equalAxes: true); avaPlot1.Render(); }
        private void EqualAxisUnlock(object sender, RoutedEventArgs e) { avaPlot1?.Configure(equalAxes: false); }
        private void DoubleClickBenchmarkEnable(object sender, RoutedEventArgs e) { avaPlot1?.Configure(enableDoubleClickBenchmark: true); }
        private void DoubleClickBenchmarkDisable(object sender, RoutedEventArgs e) { avaPlot1?.Configure(enableDoubleClickBenchmark: false); avaPlot1.plt.Benchmark(false); avaPlot1.Render(); }

        private void RightClickMenuEnable(object sender, RoutedEventArgs e)
        {
            // TODO
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
