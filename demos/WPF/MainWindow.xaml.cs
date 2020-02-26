using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Version version = typeof(ScottPlot.WpfPlot).Assembly.GetName().Version;
            VersionLabel.Content = $"version {version.Major}.{version.Minor}.{version.Build}";
        }

        private void LaunchViewer(ScottPlot.Plot plt)
        {
            var viewer = new WPF.PlotViewer(plt);
            viewer.ShowDialog();
        }

        private void LaunchQuickstart(object sender, RoutedEventArgs e) { new WPF.Examples.Quickstart().ShowDialog(); }
        private void LaunchProceduralPlots(object sender, RoutedEventArgs e) { new WPF.Examples.ProceduralPlots().ShowDialog(); }

        private void LaunchBoxAndWhisker(object sender, RoutedEventArgs e)
        {
            //LaunchViewer(ScottPlot.Demo.PlotTypes.BoxAndWhisker.Quickstart());
        }

        private void LaunchFunction(object sender, RoutedEventArgs e)
        {
            //LaunchViewer(ScottPlot.Demo.PlotTypes.Function.Quickstart());
        }

        private void LaunchSignalWithMarkers(object sender, RoutedEventArgs e)
        {
            //LaunchViewer(ScottPlot.Demo.PlotTypes.Signal.CustomLineAndMarkers());
        }
    }
}
