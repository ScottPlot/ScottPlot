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

            avaPlot1.Plot.AddScatter(dataXs, dataSin);
            avaPlot1.Plot.AddScatter(dataXs, dataCos);
            avaPlot1.Refresh();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void PanEnable(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.LeftClickDragPan = true; }
        private void PanDisable(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.LeftClickDragPan = false; }
        private void ZoomEnable(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.RightClickDragZoom = true; avaPlot1.Configuration.ScrollWheelZoom = true; }
        private void ZoomDisable(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.RightClickDragZoom = false; avaPlot1.Configuration.ScrollWheelZoom = false; }
        private void DragLowQualityEnable(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.Quality = Control.QualityMode.LowWhileDragging; }
        private void DragLowQualityDisable(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.Quality = Control.QualityMode.High; }
        private void VerticalLock(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.LockVerticalAxis = true; }
        private void VerticalUnlock(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.LockVerticalAxis = false; }
        private void HorizontalLock(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.LockHorizontalAxis = true; }
        private void HorizontalUnlock(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.LockHorizontalAxis = false; }
        private void EqualAxisLock(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Plot.AxisScaleLock(true); avaPlot1.Refresh(); }
        private void EqualAxisUnlock(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Plot.AxisScaleLock(false); }
        private void DoubleClickBenchmarkEnable(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.DoubleClickBenchmark = true; avaPlot1.Refresh(); }
        private void DoubleClickBenchmarkDisable(object sender, RoutedEventArgs e) { if (avaPlot1 is null) return; avaPlot1.Configuration.DoubleClickBenchmark = false; avaPlot1.Refresh(); }

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
