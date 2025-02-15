using ScottPlot;
using System.Windows;

namespace WPF_Demo.DemoWindows;

public partial class MultiThreading : Window, IDemoWindow
{
    public string DemoTitle => "WPF Multi-Threading";
    public string Description => "Demonstrate how to safely change data while rendering asynchronously.";

    System.Timers.Timer SystemTimer = new() { Interval = 10 };
    private readonly System.Windows.Threading.DispatcherTimer DispatcherTimer = new() { Interval = TimeSpan.FromMilliseconds(10) };

    private readonly List<double> Xs = [];
    private readonly List<double> Ys = [];

    public MultiThreading()
    {
        InitializeComponent();

        // pre-populate lists with valid data
        ChangeDataLength();

        // add the scatter plot
        WpfPlot1.Plot.Add.ScatterLine(Xs, Ys);

        SystemTimer.Elapsed += (s, e) =>
        {
            // Changing data length will throw an exception if it occurs mid-render.
            // Operations performed while the sync object will occur outside renders.
            lock (WpfPlot1.Plot.Sync)
            {
                ChangeDataLength();
            }
            WpfPlot1.Refresh();
        };

        DispatcherTimer.Tick += (s, e) =>
        {
            // Locking the sync object does not seem to be required
            // when data is changed using the dispatcher timer in WPF apps
            ChangeDataLength();
            WpfPlot1.Refresh();
        };

    }

    private void ChangeDataLength(int minLength = 10_000, int maxLength = 20_000)
    {
        int newLength = Random.Shared.Next(minLength, maxLength);
        Xs.Clear();
        Ys.Clear();
        Xs.AddRange(Generate.Consecutive(newLength));
        Ys.AddRange(Generate.RandomWalk(newLength));
        WpfPlot1.Plot.Axes.AutoScale(true);
    }

    private void StartTimer(object sender, RoutedEventArgs e)
    {
        SystemTimer.Start();
        ButtonStackPanel.IsEnabled = false;
    }

    private void StartDispatchTimer(object sender, RoutedEventArgs e)
    {
        DispatcherTimer.Start();
        ButtonStackPanel.IsEnabled = false;
    }
}
