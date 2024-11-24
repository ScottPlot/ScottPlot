using ScottPlot;
using ScottPlot.Plottables;

namespace WinForms_Demo.Demos;

public partial class LiveHistogram : Form, IDemoWindow
{
    public string Title => "Live Histogram";

    public string Description => "A continuously updating histogram that expands binned counts as new values are added.";

    readonly System.Windows.Forms.Timer Timer = new() { Enabled = true, Interval = 20 };

    int Total = 0;

    public LiveHistogram()
    {
        InitializeComponent();

        // create a histogram with 100 bins
        int binCount = 100;
        double[] initialHeights = Generate.Zeros(binCount);
        BarPlot bp = formsPlot1.Plot.Add.Bars(initialHeights);

        // style bars so there are no gaps between them
        foreach (var bar in bp.Bars)
        {
            bar.LineWidth = 0;
            bar.Size = 1;
            bar.FillStyle.AntiAlias = false;
        }

        // prevent AutoScale() from adding space below the bars
        formsPlot1.Plot.Axes.Margins(bottom: 0);

        // simulate live incoming data
        Timer.Tick += (s, e) =>
        {
            int newValueCount = Generate.RandomInteger(50, 100);
            for (int i = 0; i < newValueCount; i++)
            {
                double randomValue = Generate.RandomNormalNumber(mean: 50, stdDev: 10);
                int binIndex = (int)NumericConversion.Clamp(randomValue, 0, binCount - 1);
                bp.Bars[binIndex].Value += 1;
                Total += 1;
            }

            formsPlot1.Plot.Title($"Total: {Total:N0}");
            formsPlot1.Plot.Axes.AutoScale();
            formsPlot1.Refresh();
        };
    }
}
