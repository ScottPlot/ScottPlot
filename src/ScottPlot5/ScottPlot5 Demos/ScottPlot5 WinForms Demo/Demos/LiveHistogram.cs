using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.Statistics;

namespace WinForms_Demo.Demos;

public partial class LiveHistogram : Form, IDemoWindow
{
    public string Title => "Live Histogram";

    public string Description => "A continuously updating histogram that expands binned counts as new values are added.";

    readonly System.Windows.Forms.Timer Timer = new() { Enabled = true, Interval = 20 };

    public LiveHistogram()
    {
        InitializeComponent();

        var histogram = Histogram.WithBinCount(count: 50, minValue: 0, maxValue: 100);
        var histogramPlot = formsPlot1.Plot.Add.Histogram(histogram);

        // simulate live incoming data
        Timer.Tick += (s, e) =>
        {
            int numberOfNewValues = Generate.RandomInteger(50, 100);
            for (int i = 0; i < numberOfNewValues; i++)
            {
                double randomValue = Generate.RandomNormalNumber(mean: 50, stdDev: 10);
                histogram.Add(randomValue);
            }

            formsPlot1.Plot.Title($"Total: {histogram.Counts.Sum():N0}");
            formsPlot1.Plot.Axes.AutoScale();
            formsPlot1.Refresh();
        };
    }
}
