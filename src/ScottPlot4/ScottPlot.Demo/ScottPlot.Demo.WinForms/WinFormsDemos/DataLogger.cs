using System;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos;

public partial class DataLogger : Form
{
    readonly Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };
    readonly ScottPlot.Plottable.ScatterPlotList<double> Scatter;
    readonly Random Rand = new();

    double LastPointValue = 0;
    int PointsOnLastRefresh = 0;

    public DataLogger()
    {
        InitializeComponent();

        Scatter = formsPlot1.Plot.AddScatterList();
        Scatter.MarkerSize = 0;
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
            Scatter.Add(Scatter.Count, LastPointValue);
        }
    }

    private void UpdatePlotTimer_Tick(object sender, EventArgs e)
    {
        if (Scatter.Count == PointsOnLastRefresh)
            return;

        PointsOnLastRefresh = Scatter.Count;
        formsPlot1.Plot.AxisAuto();
        formsPlot1.Refresh();
    }
}