using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ScottPlot.Demo.WPF
{
    /// <summary>
    /// Interaction logic for DemoListControl.xaml
    /// </summary>
    public partial class DemoListControl : UserControl
    {
        public DemoListControl()
        {
            InitializeComponent();
        }

        private void LaunchMouseTracker(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.MouseTracker().ShowDialog();
        private void LaunchToggleVisibility(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.ToggleVisibility().ShowDialog();
        private void LaunchWpfConfig(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.WpfConfig().ShowDialog();
        private void LaunchLinkedAxes(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.LinkedPlots().ShowDialog();
        private void LaunchLiveDataFixed(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.LiveDataFixed().ShowDialog();
        private void LaunchLiveDataIncoming(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.LiveDataGrowing().ShowDialog();
        private void LaunchShowValueUnderMouse(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.ShowValueOnHover().ShowDialog();
        private void LaunchTransparentBackground(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.TransparentBackground().ShowDialog();
        private void LaunchPlotViewer(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.PlotViewer().ShowDialog();
        private void LaunchManyPlot(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.ManyPlots().ShowDialog();
        private void LaunchCustomRightClick(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.RightClickMenu().ShowDialog();
        private void LaunchPlotInAScrollViewer(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.PlotInScrollViewer().ShowDialog();
        private void LaunchAxisLimits(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.AxisLimits().ShowDialog();
        private void LaunchLayout(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.Layout().ShowDialog();
        private void MultiAxisLock(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.MultiAxisLock().ShowDialog();
        private void StyleBrowser(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.StyleBrowser().ShowDialog();
        private void DisplayScaling(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.DisplayScaling().ShowDialog();
        private void LaunchHittableDemo(object sender, RoutedEventArgs e) => new ScottPlot.Demo.WPF.WpfDemos.HittableDemo().ShowDialog();

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {

        }
    }
}
