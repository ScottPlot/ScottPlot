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
            wpfPlot1.Plot.AddSignal(DataGen.Sin(51));
            wpfPlot1.Plot.AddSignal(DataGen.Cos(51));
            wpfPlot1.Refresh();
        }

        // TODO: use proper binding (perhaps with the configuration object itself?)
        private void PanEnable(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.LeftClickDragPan = true; }
        private void PanDisable(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.LeftClickDragPan = false; }
        private void ZoomEnable(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.RightClickDragZoom = true; wpfPlot1.Configuration.ScrollWheelZoom = true; }
        private void ZoomDisable(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.RightClickDragZoom = false; wpfPlot1.Configuration.ScrollWheelZoom = false; }
        private void DragLowQualityEnable(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.Quality = Control.QualityMode.LowWhileDragging; }
        private void DragLowQualityDisable(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.Quality = Control.QualityMode.High; }
        private void VerticalLock(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.LockVerticalAxis = true; }
        private void VerticalUnlock(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.LockVerticalAxis = false; }
        private void HorizontalLock(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.LockHorizontalAxis = true; }
        private void HorizontalUnlock(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.LockHorizontalAxis = false; }
        private void EqualAxisLock(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Plot.AxisScaleLock(true); wpfPlot1.Refresh(); }
        private void EqualAxisUnlock(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Plot.AxisScaleLock(false); }
        private void DoubleClickBenchmarkEnable(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.DoubleClickBenchmark = true; wpfPlot1.Refresh(); }
        private void DoubleClickBenchmarkDisable(object sender, RoutedEventArgs e) { if (wpfPlot1 is null) return; wpfPlot1.Configuration.DoubleClickBenchmark = false; wpfPlot1.Refresh(); }

        private void RightClickMenuEnable(object sender, RoutedEventArgs e)
        {

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
