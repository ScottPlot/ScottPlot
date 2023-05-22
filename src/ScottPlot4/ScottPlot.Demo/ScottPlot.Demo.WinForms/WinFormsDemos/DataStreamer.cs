using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos;
public partial class DataStreamer : Form
{
    readonly Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    readonly ScottPlot.Plottable.ScatterDataLogger Logger;

    readonly Random Rand = new();

    double LastPointValue = 0;

    public DataStreamer()
    {
        InitializeComponent();

        Logger = formsPlot1.Plot.AddScatterLogger();

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
            Logger.Add(Logger.Count, LastPointValue);
        }
    }

    private void UpdatePlotTimer_Tick(object sender, EventArgs e)
    {
        if (Logger.Count == Logger.LastRenderCount)
            return;

        formsPlot1.Refresh();

        Text = $"DataStreamer Demo ({Logger.Count:N0} points)";
    }
}
