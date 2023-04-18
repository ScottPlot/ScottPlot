using System;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos;

public partial class DataLogger : Form
{
    readonly Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    readonly ScottPlot.Plottable.DataLogger Logger = new();

    readonly Random Rand = new();

    double LastPointValue = 0;

    public DataLogger()
    {
        InitializeComponent();
        formsPlot1.Plot.Add(Logger);

        AddRandomWalkData(1000);
        formsPlot1.Refresh();

        AddNewDataTimer.Tick += (s, e) => AddRandomWalkData(10);
        UpdatePlotTimer.Tick += UpdatePlotTimer_Tick;
    }

    private void AddRandomWalkData(int count)
    {
        for (int i = 0; i < count; i++)
        {
            LastPointValue = LastPointValue + Rand.NextDouble() - .5;
            Logger.Add(LastPointValue);
        }
    }

    private void UpdatePlotTimer_Tick(object sender, EventArgs e)
    {
        if (!Logger.CountChangedSinceLastRender)
            return;

        Logger.UpdateAxisLimits(formsPlot1.Plot);

        formsPlot1.Refresh();

        Text = $"DataLogger Demo ({Logger.Count:N0} points)";
    }
}
