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

        Logger = formsPlot1.Plot.AddDataLogger(label: "trace");

        AddRandomWalkData(1000);
        btnFull_Click(null, EventArgs.Empty);
        cbView_CheckedChanged(null, EventArgs.Empty);

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
        if (Logger.Count == Logger.CountOnLastRender)
            return;

        formsPlot1.Refresh();

        Text = $"DataLogger Demo ({Logger.Count:N0} points)";
    }

    private void cbView_CheckedChanged(object sender, EventArgs e)
    {
        Logger.ManageAxisLimits = cbEnableViewManagement.Checked;

        // disable mouse interaction if axis limits are managed automatically
        formsPlot1.Configuration.Pan = !cbEnableViewManagement.Checked;
        formsPlot1.Configuration.Zoom = !cbEnableViewManagement.Checked;
    }

    private void btnFull_Click(object sender, EventArgs e)
    {
        Logger.ViewFull();
        formsPlot1.Plot.Title("Full");
        formsPlot1.Refresh();
    }

    private void btnJump_Click(object sender, EventArgs e)
    {
        Logger.ViewJump();
        formsPlot1.Plot.Title("Jump");
        formsPlot1.Refresh();
    }

    private void btnSlide_Click(object sender, EventArgs e)
    {
        Logger.ViewSlide();
        formsPlot1.Plot.Title("Slide");
        formsPlot1.Refresh();
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        Logger.Clear();
        formsPlot1.Refresh();
    }

    private void chk_showLegend_CheckedChanged(object sender, EventArgs e)
    {
        formsPlot1.Plot.Legend(chk_showLegend.Checked);
        formsPlot1.Refresh();
    }
}
