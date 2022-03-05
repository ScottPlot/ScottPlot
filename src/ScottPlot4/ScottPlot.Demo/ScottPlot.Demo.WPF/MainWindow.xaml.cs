using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF
{
    /// <summary>
    /// Interaction logic for LauncherWindow.xaml
    /// </summary>
    public partial class LauncherWindow : Window
    {
        public LauncherWindow()
        {
            InitializeComponent();
            VersionLabel.Content = Plot.Version;
        }

        private void LaunchCookbook(object sender, RoutedEventArgs e)
        {
            new CookbookWindow().ShowDialog();
        }

        private void LaunchMouseTracker(object sender, RoutedEventArgs e)
        {
            new WpfDemos.MouseTracker().ShowDialog();
        }

        private void LaunchToggleVisibility(object sender, RoutedEventArgs e)
        {
            new WpfDemos.ToggleVisibility().ShowDialog();
        }

        private void LaunchWpfConfig(object sender, RoutedEventArgs e)
        {
            new WpfDemos.WpfConfig().ShowDialog();
        }

        private void LaunchLinkedAxes(object sender, RoutedEventArgs e)
        {
            new WpfDemos.LinkedPlots().ShowDialog();
        }

        private void LaunchLiveDataFixed(object sender, RoutedEventArgs e)
        {
            new WpfDemos.LiveDataFixed().ShowDialog();
        }

        private void LaunchLiveDataIncoming(object sender, RoutedEventArgs e)
        {
            new WpfDemos.LiveDataGrowing().ShowDialog();
        }

        private void LaunchShowValueUnderMouse(object sender, RoutedEventArgs e)
        {
            new WpfDemos.ShowValueOnHover().ShowDialog();
        }

        private void LaunchTransparentBackground(object sender, RoutedEventArgs e)
        {
            new WpfDemos.TransparentBackground().ShowDialog();
        }

        private void WebsiteLabelMouseEnter(object sender, MouseEventArgs e)
        {
            WebsiteLabel.Foreground = Brushes.Blue;
        }

        private void WebsiteLabelMouseLeave(object sender, MouseEventArgs e)
        {
            WebsiteLabel.Foreground = Brushes.Gray;
        }

        private void WebsiteLabelMouseDown(object sender, MouseButtonEventArgs e)
        {
            WebsiteLabel.Foreground = Brushes.Orange;
        }

        private void WebsiteLabelMouseUp(object sender, MouseButtonEventArgs e)
        {
            WebsiteLabel.Foreground = Brushes.Blue;
            Tools.LaunchBrowser();
        }

        private void LaunchPlotViewer(object sender, RoutedEventArgs e)
        {
            new WpfDemos.PlotViewer().ShowDialog();
        }

        private void LaunchManyPlot(object sender, RoutedEventArgs e)
        {
            new WpfDemos.ManyPlots().ShowDialog();
        }

        private void LaunchCustomRightClick(object sender, RoutedEventArgs e)
        {
            new WpfDemos.RightClickMenu().ShowDialog();
        }

        private void LaunchPlotInAScrollViewer(object sender, RoutedEventArgs e)
        {
            new WpfDemos.PlotInScrollViewer().ShowDialog();
        }

        private void LaunchAxisLimits(object sender, RoutedEventArgs e)
        {
            new WpfDemos.AxisLimits().ShowDialog();
        }

        private void LaunchLayout(object sender, RoutedEventArgs e)
        {
            new WpfDemos.Layout().ShowDialog();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Tools.LaunchBrowser("https://ScottPlot.NET/demo");
        }

        private void MultiAxisLock(object sender, RoutedEventArgs e)
        {
            new WpfDemos.MultiAxisLock().ShowDialog();
        }

        private void StyleBrowser(object sender, RoutedEventArgs e)
        {
            new WpfDemos.StyleBrowser().ShowDialog();
        }

        private void DisplayScaling(object sender, RoutedEventArgs e)
        {
            new WpfDemos.DisplayScaling().ShowDialog();
        }
    }
}
