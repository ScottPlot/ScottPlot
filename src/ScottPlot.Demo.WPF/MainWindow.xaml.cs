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
            VersionLabel.Content = Tools.GetVersionString();

            //bool sourceIsAvailable = System.IO.File.Exists("../../../ScottPlot.Demo.WPF.csproj");
            //GenerateButton.IsEnabled = sourceIsAvailable;
            //GenerateDescription.Foreground = (sourceIsAvailable) ? Brushes.Black : Brushes.Gray;
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
    }
}
