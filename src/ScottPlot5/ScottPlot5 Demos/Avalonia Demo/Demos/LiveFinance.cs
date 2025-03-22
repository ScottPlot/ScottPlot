using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace Avalonia_Demo.Demos;

public class LiveFinanceDemo : IDemo
{
    public string Title => "Live Financial Plot";
    public string Description => "Demonstrates how to display price OHLC data in real time, " +
        "modifying the last bar for live updates and adding new bars as time progresses.";

    public Window GetWindow()
    {
        return new LiveFinanceWindow();
    }

}

public class LiveFinanceWindow : SimpleDemoWindow
{
    private DispatcherTimer Timer;
    private List<OHLC> OHLCs = new();

    public LiveFinanceWindow() : base("Live Financial Plot")
    {

    }

    protected override void StartDemo()
    {
        int historicalPointCount = 10;
        DateTime now = DateTime.Now;
        TimeSpan timeSpan = TimeSpan.FromSeconds(5);
        DateTime oldest = now - historicalPointCount * timeSpan;
        for (int i = 0; i < historicalPointCount; i++)
        {
            DateTime start = oldest + timeSpan * i;
            OHLC ohlc = GetRandomOhlc(start, timeSpan);
            OHLCs.Add(ohlc);
        }

        // plot the OHLC list
        AvaPlot.Plot.Add.Candlestick(OHLCs);

        // setup the plot to display X axis tick labels using date time format
        AvaPlot.Plot.Axes.DateTimeTicksBottom();

        // setup a timer to update the chart every second
        Timer = new() { Interval = TimeSpan.FromSeconds(1) };
        Timer.Start();
        Timer.Tick += Timer_Tick;
    }

    private OHLC GetRandomOhlc(DateTime dt, TimeSpan ts)
    {
        double open = Generate.RandomNumber(95, 105);
        double close = Generate.RandomNumber(95, 105);
        double low = 95 - Generate.RandomNumber(5);
        double high = 105 + Generate.RandomNumber(5);
        return new OHLC(open, high, low, close, dt, ts);
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        DateTime now = DateTime.Now;

        lock (AvaPlot.Plot.Sync)
        {
            if (now.Second % 5 == 0)
            {
                // start a new OHLC every 5 seconds
                OHLC newPrice = GetRandomOhlc(DateTime.Now, TimeSpan.FromSeconds(5));
                OHLCs.Add(newPrice);
            }
            else
            {
                // modify the last OHLC by increasing its closing price
                int lastOhlcIndex = OHLCs.Count - 1;
                OHLC updatedOhlc = OHLCs[lastOhlcIndex];
                updatedOhlc.Close += .5;
                OHLCs[lastOhlcIndex] = updatedOhlc;
            }
        }

        AvaPlot.Plot.Axes.AutoScaleExpand();
        AvaPlot.Refresh();
    }
}
