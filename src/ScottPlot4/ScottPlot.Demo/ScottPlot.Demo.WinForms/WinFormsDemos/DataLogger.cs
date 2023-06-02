using System;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos;

public partial class DataLogger : Form
{
    readonly Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    readonly ScottPlot.Plottable.DataLogger Logger;

    readonly Random Rand = new();

    double LastPointValue = 0;

    public DataLogger()
    {
        InitializeComponent();

        comboBox1.Items.Add("Full");
        comboBox1.Items.Add("Slide (smooth)");
        comboBox1.Items.Add("Slide (jump)");
        comboBox1.Items.Add("Fixed Width");
        comboBox1.Items.Add("Wipe");
        comboBox1.SelectedIndex = 0;

        Logger = formsPlot1.Plot.AddDataLogger();

        AddRandomWalkData(1000);
        formsPlot1.Refresh();

        AddNewDataTimer.Tick += (s, e) => AddRandomWalkData(10);
        UpdatePlotTimer.Tick += UpdatePlotTimer_Tick;
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Logger is null)
            return;

        // TODO: these flags are too complex. Make the view style an enum.
        // Refactor the configuration system after the basic functionality is working.

        if (comboBox1.Text == "Full")
        {
            Logger.LoggerView = new ScottPlot.Plottable.DataLoggerViews.Full();
            Logger.FixedWidth = null;
            Logger.SamplePeriod = null;
        }
        else if (comboBox1.Text == "Slide (smooth)")
        {
            Logger.LoggerView = new ScottPlot.Plottable.DataLoggerViews.Slide() { PaddingFraction = 0 };
            Logger.FixedWidth = null;
            Logger.SamplePeriod = null;
        }
        else if (comboBox1.Text == "Slide (jump)")
        {
            Logger.LoggerView = new ScottPlot.Plottable.DataLoggerViews.Slide() { PaddingFraction = .75 };
            Logger.FixedWidth = null;
            Logger.SamplePeriod = null;
        }
        else if (comboBox1.Text == "Fixed Width")
        {
            Logger.FixedWidth = 1000;
            Logger.SamplePeriod = null;
        }
        else if (comboBox1.Text == "Wipe")
        {
            Logger.FixedWidth = null;
            Logger.SamplePeriod = 1;
        }
        else
        {
            throw new NotImplementedException(comboBox1.Text);
        }
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
