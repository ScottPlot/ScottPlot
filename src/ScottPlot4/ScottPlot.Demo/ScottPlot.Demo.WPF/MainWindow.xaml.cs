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

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) => Tools.LaunchBrowser("https://ScottPlot.NET/demo");

        private void LaunchCookbook(object sender, RoutedEventArgs e) => new CookbookWindow().ShowDialog();
    }
}
