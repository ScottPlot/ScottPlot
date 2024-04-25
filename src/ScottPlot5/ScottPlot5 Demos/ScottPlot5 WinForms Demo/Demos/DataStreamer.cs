namespace WinForms_Demo.Demos;
public partial class DataStreamer : Form, IDemoWindow
{
    public string Title => "Data Streamer";
    public string Description => "Plots live streaming data as a fixed-width line plot, " +
        "shifting old data out as new data comes in.";

    readonly System.Windows.Forms.Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly System.Windows.Forms.Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    readonly ScottPlot.Plottables.DataStreamer Streamer1;
    readonly ScottPlot.Plottables.DataStreamer Streamer2;

    readonly ScottPlot.DataGenerators.RandomWalker Walker1 = new(0);
    readonly ScottPlot.DataGenerators.RandomWalker Walker2 = new(1);

    readonly ScottPlot.Plottables.VerticalLine VLine;

    public DataStreamer()
    {
        InitializeComponent();

        Streamer1 = formsPlot1.Plot.Add.DataStreamer(1000);
        Streamer2 = formsPlot1.Plot.Add.DataStreamer(1000);
        VLine = formsPlot1.Plot.Add.VerticalLine(0, 2, ScottPlot.Colors.Red);

        // disable mouse interaction by default
        formsPlot1.Interaction.Disable();

        // setup a timer to add data to the streamer periodically
        AddNewDataTimer.Tick += (s, e) =>
        {
            Streamer1.AddRange(Walker1.Next(10));
            Streamer2.AddRange(Walker2.Next(10));
        };

        // setup a timer to request a render periodically
        UpdatePlotTimer.Tick += (s, e) =>
        {
            if (Streamer1.HasNewData)
            {
                formsPlot1.Plot.Title($"Processed {Streamer1.Data.CountTotal:N0} points");
                VLine.IsVisible = Streamer1.Renderer is ScottPlot.DataViews.Wipe;
                VLine.Position = Streamer1.Data.NextIndex * Streamer1.Data.SamplePeriod + Streamer1.Data.OffsetX;
                formsPlot1.Refresh();
            }
        };

        // setup configuration actions
        btnWipeRight.Click += (s, e) => Streamer1.ViewWipeRight(0.1);
        btnScrollLeft.Click += (s, e) => Streamer1.ViewScrollLeft();
        cbManageLimits.CheckedChanged += (s, e) =>
        {
            if (cbManageLimits.Checked)
            {
                Streamer1.ManageAxisLimits = true;
                Streamer2.ManageAxisLimits = true;
                formsPlot1.Interaction.Disable();
                formsPlot1.Plot.Axes.AutoScale();
            }
            else
            {
                Streamer1.ManageAxisLimits = false;
                Streamer2.ManageAxisLimits = false;
                formsPlot1.Interaction.Enable();
            }
        };

        cbContinuouslyAutoscale.CheckedChanged += (s, e) =>
        {
            Streamer1.ContinuouslyAutoscale = cbContinuouslyAutoscale.Checked;
            Streamer2.ContinuouslyAutoscale = cbContinuouslyAutoscale.Checked;
        };
    }
}
