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
        comboBox1.Items.Add("Slide (smooth)");
        comboBox1.Items.Add("Slide (jump)");
        comboBox1.SelectedIndex = 0;

        Logger = formsPlot1.Plot.AddScatterLogger();

        AddRandomWalkData(1000);
        formsPlot1.Refresh();

        AddNewDataTimer.Tick += (s, e) => AddRandomWalkData(10);
        UpdatePlotTimer.Tick += UpdatePlotTimer_Tick;
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Logger is null)
            return;

        Logger.LoggerView = comboBox1.Text switch
        {
            "Full" => new ScottPlot.Plottable.DataLoggerViews.Full(),
            "Slide (smooth)" => new ScottPlot.Plottable.DataLoggerViews.Slide(),
            "Slide (jump)" => new ScottPlot.Plottable.DataLoggerViews.Slide() { PaddingFraction = .75 },
            _ => throw new NotImplementedException(comboBox1.Text)
        };
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

    private void cbView_CheckedChanged(object sender, EventArgs e)
    {
        Logger.ManageAxisLimits = cbEnableViewManagement.Checked;
    }
}
