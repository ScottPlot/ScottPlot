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
        string sourceCodePath;

        public LauncherWindow()
        {
            InitializeComponent();
            VersionLabel.Content = Tools.GetVersionString();

            string developerSourcePath = "../../../../ScottPlot.Demo/IPlotDemo.cs";
            developerSourcePath = System.IO.Path.GetFullPath(developerSourcePath);
            Debug.WriteLine($"Looking for source code: {developerSourcePath}");

            string distributedSourcePath = "./.source/IPlotDemo.cs";
            distributedSourcePath = System.IO.Path.GetFullPath(distributedSourcePath);
            Debug.WriteLine($"Looking for source code: {distributedSourcePath}");

            if (System.IO.File.Exists(developerSourcePath))
                sourceCodePath = System.IO.Path.GetFullPath(developerSourcePath);
            else if (System.IO.File.Exists(distributedSourcePath))
                sourceCodePath = System.IO.Path.GetFullPath(distributedSourcePath);
            else
                sourceCodePath = null;
            Debug.WriteLine($"Source code path: {sourceCodePath}");

            if (sourceCodePath is null)
            {
                GenerateButton.IsEnabled = false;
                GenerateDescription.Foreground = Brushes.Gray;
            }
        }

        private void LaunchCookbook(object sender, RoutedEventArgs e)
        {
            new CookbookWindow().ShowDialog();
        }

        private void LaunchCookbookGenerator(object sender, RoutedEventArgs e)
        {
            new CookbookGeneratorWindow().ShowDialog();
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
    }
}
