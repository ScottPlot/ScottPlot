namespace WinForms_Demo.Demos;

public partial class HeatmapLive : Form, IDemoWindow
{
    readonly ScottPlot.Plottables.Heatmap HMap;
    readonly System.Windows.Forms.Timer Timer;
    readonly double[,] HeatmapData;
    int UpdateCount = 0;

    public string Title => "Live Heatmap";

    public string Description => "Demonstrates how to display a heatmap with data that changes over time";


    public HeatmapLive()
    {
        InitializeComponent();

        HeatmapData = ScottPlot.Generate.Sin2D(23, 13, multiple: 3);
        HMap = new ScottPlot.Plottables.Heatmap(HeatmapData);
        formsPlot1.Plot.PlottableList.Add(HMap);

        Timer = new() { Enabled = true, Interval = 100 };
        Timer.Tick += (s, e) => ChangeData();

        formsPlot1.Refresh();
    }

    private void ChangeData()
    {
        Text = $"Updated {++UpdateCount} times";

        Random rand = new();
        for (int y = 0; y < HeatmapData.GetLength(0); y++)
        {
            for (int x = 0; x < HeatmapData.GetLength(1); x++)
            {
                HeatmapData[y, x] += rand.NextDouble() - .5;
            }
        }

        HMap.Update();
        formsPlot1.Refresh();
    }
}
