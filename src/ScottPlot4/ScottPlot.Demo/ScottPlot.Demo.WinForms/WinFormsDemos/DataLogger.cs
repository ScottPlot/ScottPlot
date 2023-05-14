using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ScottPlot.Demo.WinForms.WinFormsDemos;

public partial class DataLogger : Form
{
    readonly Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    readonly ScottPlot.Plottable.ScatterDataLogger Logger;

    readonly Random Rand = new();

    double LastPointValue = 0;

    public DataLogger()
    {
        InitializeComponent();

        comboBox1.Items.Add("Full");
        comboBox1.Items.Add("Sweeps");
        comboBox1.Items.Add("Latest");
        comboBox1.SelectedIndex = 0;

        Logger = formsPlot1.Plot.AddScatterLogger();

        AddRandomWalkData(1000);
        formsPlot1.Refresh();

        AddNewDataTimer.Tick += (s, e) => AddRandomWalkData(10);
        UpdatePlotTimer.Tick += UpdatePlotTimer_Tick;
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //throw new NotImplementedException();
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

        Text = $"DataLogger Demo ({Logger.Count:N0} points)";
    }
}
