using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace Avalonia_Demo.Demos;

public class LiveHeatmapDemo : IDemo
{
    public string Title => "Live Heatmap";
    public string Description => "Demonstrates how to display a heatmap with data that changes over time";

    public Window GetWindow()
    {
        return new LiveHeatmapWindow();
    }

}

public class LiveHeatmapWindow : SimpleDemoWindow
{
    private ScottPlot.Plottables.Heatmap HMap;
    private DispatcherTimer Timer;
    private double[,] HeatmapData;
    private int UpdateCount = 0;

    public LiveHeatmapWindow() : base("Live Heatmap")
    {

    }

    protected override void StartDemo()
    {
        HeatmapData = ScottPlot.Generate.Sin2D(23, 13, multiple: 3);
        HMap = new ScottPlot.Plottables.Heatmap(HeatmapData);
        AvaPlot.Plot.PlottableList.Add(HMap);

        Timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100) };
        Timer.Start();
        Timer.Tick += (s, e) => ChangeData();

        AvaPlot.Refresh();
    }

    private void ChangeData()
    {
        Title = $"Updated {++UpdateCount} times";

        Random rand = new();
        for (int y = 0; y < HeatmapData.GetLength(0); y++)
        {
            for (int x = 0; x < HeatmapData.GetLength(1); x++)
            {
                HeatmapData[y, x] += rand.NextDouble() - .5;
            }
        }

        lock (AvaPlot.Plot.Sync)
        {
            HMap.Update();
        }
        AvaPlot.Refresh();
    }
}
