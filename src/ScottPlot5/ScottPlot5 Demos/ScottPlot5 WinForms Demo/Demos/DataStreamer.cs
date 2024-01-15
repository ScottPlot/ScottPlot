namespace WinForms_Demo.Demos;
public partial class DataStreamer : Form, IDemoWindow
{
    public string Title => "Data Streamer";
    public string Description => "Plots live streaming data as a fixed-width line plot, " +
        "shifting old data out as new data comes in.";

    readonly System.Windows.Forms.Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly System.Windows.Forms.Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    readonly ScottPlot.Plottables.DataStreamer Streamer;

    readonly Random Rand = new();

    double LastPointValue = 0;

    public DataStreamer()
    {
        InitializeComponent();

        double[] data = new double[1000];
        Streamer = new ScottPlot.Plottables.DataStreamer(formsPlot1.Plot, data);
        formsPlot1.Plot.Add.Plottable(Streamer);

        //btnWipeRight_Click(null, EventArgs.Empty);
        //cbManageLimits_CheckedChanged(null, EventArgs.Empty);

        AddNewDataTimer.Tick += (s, e) => AddRandomWalkData();
        UpdatePlotTimer.Tick += (s, e) => UpdatePlotTimer_Tick(this, EventArgs.Empty);
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
        if (Streamer.CountTotal != Streamer.CountTotalOnLastRender)
            formsPlot1.Refresh();

        Text = $"DataStreamer Demo ({Streamer.CountTotal:N0} points)";
    }
}
