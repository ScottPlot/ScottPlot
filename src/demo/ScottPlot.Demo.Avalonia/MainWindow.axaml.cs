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
            this.Find<TextBlock>("VersionLabel").Text = Plot.Version;

            this.Find<Button>("WebsiteLabel").Click += WebsiteLabelClick;
            this.Find<Button>("LaunchCookbookButton").Click += LaunchCookbook;
            this.Find<Button>("LaunchPlotViewerButton").Click += LaunchPlotViewer;
            this.Find<Button>("LaunchMouseTrackerButton").Click += LaunchMouseTracker;
            this.Find<Button>("LaunchToggleVisibilityButton").Click += LaunchToggleVisibility;
            this.Find<Button>("LaunchAvaPlotConfigButton").Click += LaunchAvaloniaConfig;
            this.Find<Button>("LaunchLinkedAxesButton").Click += LaunchLinkedAxes;
            this.Find<Button>("LaunchLiveDataFixedButton").Click += LaunchLiveDataFixed;
            this.Find<Button>("LaunchLiveDataIncomingButton").Click += LaunchLiveDataIncoming;
            this.Find<Button>("LaunchShowValueUnderMouseButton").Click += LaunchShowValueUnderMouse;
            this.Find<Button>("LaunchTransparentBackgroundButton").Click += LaunchTransparentBackground;
            this.Find<Button>("LaunchCustomRightClickButton").Click += LaunchCustomRightClick;
            this.Find<Button>("LaunchPlotInAScrollViewerButton").Click += LaunchPlotInAScrollViewer;
            this.Find<Button>("LaunchAxisLimitsButton").Click += LaunchAxisLimits;
            this.Find<Button>("LaunchLayoutButton").Click += LaunchLayout;
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
            new AvaloniaDemos.MouseTracker().ShowDialog(this);
        }

        public void LaunchToggleVisibility(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.ToggleVisibility().ShowDialog(this);
        }

        public void LaunchAvaloniaConfig(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.AvaloniaConfig().ShowDialog(this);
        }

        public void LaunchLinkedAxes(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.LinkedPlots().ShowDialog(this);
        }

        public void LaunchLiveDataFixed(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.LiveDataFixed().ShowDialog(this);
        }

        public void LaunchLiveDataIncoming(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.LiveDataGrowing().ShowDialog(this);
        }

        public void LaunchShowValueUnderMouse(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.ShowValueOnHover().ShowDialog(this);
        }

        public void LaunchTransparentBackground(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.TransparentBackground().ShowDialog(this);
        }

        public void WebsiteLabelClick(object sender, RoutedEventArgs e)
        {
            Tools.LaunchBrowser("https://swharden.com/scottplot/demo");
        }

        public void LaunchPlotViewer(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.PlotViewer().ShowDialog(this);
        }

        public void LaunchManyPlot(object sender, RoutedEventArgs e)
        {
            //new WpfDemos.ManyPlots().ShowDialog();
        }

        public void LaunchCustomRightClick(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.RightClickMenu().ShowDialog(this);
        }

        public void LaunchPlotInAScrollViewer(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.PlotInScrollViewer().ShowDialog(this);
        }

        public void LaunchAxisLimits(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.AxisLimits().ShowDialog(this);
        }

        public void LaunchLayout(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.Layout().ShowDialog(this);
        }
    }
}
