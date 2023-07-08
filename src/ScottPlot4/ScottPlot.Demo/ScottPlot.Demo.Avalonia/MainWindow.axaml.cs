using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ScottPlot.Demo.Avalonia
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.VersionLabel.Text = Plot.Version;

            this.WebsiteLabel.Click += WebsiteLabelClick;
            this.LaunchCookbookButton.Click += LaunchCookbook;
            this.LaunchPlotViewerButton.Click += LaunchPlotViewer;
            this.LaunchMouseTrackerButton.Click += LaunchMouseTracker;
            this.LaunchToggleVisibilityButton.Click += LaunchToggleVisibility;
            this.LaunchAvaPlotConfigButton.Click += LaunchAvaloniaConfig;
            this.LaunchLinkedAxesButton.Click += LaunchLinkedAxes;
            this.LaunchLiveDataFixedButton.Click += LaunchLiveDataFixed;
            this.LaunchLiveDataIncomingButton.Click += LaunchLiveDataIncoming;
            this.LaunchShowValueUnderMouseButton.Click += LaunchShowValueUnderMouse;
            this.LaunchTransparentBackgroundButton.Click += LaunchTransparentBackground;
            this.LaunchCustomRightClickButton.Click += LaunchCustomRightClick;
            this.LaunchPlotInAScrollViewerButton.Click += LaunchPlotInAScrollViewer;
            this.LaunchAxisLimitsButton.Click += LaunchAxisLimits;
            this.LaunchLayoutButton.Click += LaunchLayout;
            this.LaunchMultiAxisLockButton.Click += LaunchMultiAxisLock;
            this.LaunchStyleBrowserButton.Click += LaunchStyleBrowser;
            this.LaunchDisplayScalingButton.Click += LaunchDisplayScaling;
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
            Tools.LaunchBrowser("https://ScottPlot.NET/demo");
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
        public void LaunchMultiAxisLock(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.MultiAxisLock().ShowDialog(this);
        }

        public void LaunchStyleBrowser(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.StyleBrowser().ShowDialog(this);
        }

        public void LaunchDisplayScaling(object sender, RoutedEventArgs e)
        {
            new AvaloniaDemos.DisplayScaling().ShowDialog(this);
        }
    }
}
