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

            bool sourceIsAvailable = System.IO.File.Exists("../../../ScottPlot.Demo.WPF.csproj");
            GenerateButton.IsEnabled = sourceIsAvailable;
            GenerateDescription.Foreground = (sourceIsAvailable) ? Brushes.Black : Brushes.Gray;
        }

        private void LaunchCookbook(object sender, RoutedEventArgs e)
        {
            new DemoNavigator().ShowDialog();
        }

        private void LaunchWpfDemos(object sender, RoutedEventArgs e)
        {
            new WpfDemosWindow().ShowDialog();
        }

        private void LaunchCookbookGenerator(object sender, RoutedEventArgs e)
        {
            new CookbookGeneratorWindow().ShowDialog();
        }
    }
}
