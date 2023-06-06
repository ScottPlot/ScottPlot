using System;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos;
public partial class DataStreamer : Form
{
    readonly Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    readonly ScottPlot.Plottable.DataStreamer Streamer;

    readonly Random Rand = new();

    double LastPointValue = 0;

    public DataStreamer()
    {
        InitializeComponent();

        Streamer = formsPlot1.Plot.AddDataStreamer(1000);
        Streamer.ViewScrollLeft();

        formsPlot1.Refresh();

        AddNewDataTimer.Tick += (s, e) => AddRandomWalkData();
        UpdatePlotTimer.Tick += UpdatePlotTimer_Tick;
    }

    private void AddRandomWalkData()
    {
        int count = Rand.Next(10);
        for (int i = 0; i < count; i++)
        {
            LastPointValue = LastPointValue + Rand.NextDouble() - .5;
            Streamer.Add(LastPointValue);
        }
    }

    private void UpdatePlotTimer_Tick(object sender, EventArgs e)
    {
        if (Streamer.RenderNeeded)
            formsPlot1.Refresh();

        Text = $"DataStreamer Demo ({Streamer.TotalPoints:N0} points)";
    }

    private void btnWipeRight_Click(object sender, EventArgs e)
    {
        formsPlot1.Plot.Title("Wipe Right");
        Streamer.ViewWipeRight();
        formsPlot1.Refresh();
    }

    private void btnWipeLeft_Click(object sender, EventArgs e)
    {
        formsPlot1.Plot.Title("Wipe Left");
        Streamer.ViewWipeLeft();
        formsPlot1.Refresh();
    }

    private void btnScrollRight_Click(object sender, EventArgs e)
    {
        formsPlot1.Plot.Title("Scroll Right");
        Streamer.ViewScrollRight();
        formsPlot1.Refresh();
    }

    private void btnScrollLeft_Click(object sender, EventArgs e)
    {
        formsPlot1.Plot.Title("Scroll Left");
        Streamer.ViewScrollLeft();
        formsPlot1.Refresh();
    }
}
