using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;

namespace ScottPlot.Demo.Avalonia
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Find<TextBlock>("VersionLabel").Text = Tools.GetVersionString();

            this.Find<Button>("WebsiteLabel").Click += WebsiteLabelClick;
            this.Find<Button>("LaunchCookbookButton").Click += LaunchCookbook;
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void LaunchCookbook(object sender, RoutedEventArgs e)
        {
            new CookbookWindow().ShowDialog(this);
        }

        public void LaunchMouseTracker(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.MouseTracker().ShowDialog();
        }

        public void LaunchToggleVisibility(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.ToggleVisibility().ShowDialog();
        }

        public void LaunchWpfConfig(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.WpfConfig().ShowDialog();
        }

        public void LaunchLinkedAxes(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.LinkedPlots().ShowDialog();
        }

        public void LaunchLiveDataFixed(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.LiveDataFixed().ShowDialog();
        }

        public void LaunchLiveDataIncoming(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.LiveDataGrowing().ShowDialog();
        }

        public void LaunchShowValueUnderMouse(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.ShowValueOnHover().ShowDialog();
        }

        public void LaunchTransparentBackground(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.TransparentBackground().ShowDialog();
        }

        public void WebsiteLabelClick(object sender, RoutedEventArgs e)
        {
            Tools.LaunchBrowser();
        }

        public void LaunchPlotViewer(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.PlotViewer().ShowDialog();
        }

        public void LaunchManyPlot(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.ManyPlots().ShowDialog();
        }

        public void LaunchCustomRightClick(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.RightClickMenu().ShowDialog();
        }

        public void LaunchPlotInAScrollViewer(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.PlotInScrollViewer().ShowDialog();
        }

        public void LaunchAxisLimits(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.AxisLimits().ShowDialog();
        }

        public void LaunchLayout(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.Layout().ShowDialog();
        }
    }
}
