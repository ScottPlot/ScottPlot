namespace WinForms_Demo.Demos;
public partial class DataStreamer : Form, IDemoWindow
{
    public string Title => "Data Streamer";
    public string Description => "Plots live streaming data as a fixed-width line plot, " +
        "shifting old data out as new data comes in.";

    readonly System.Windows.Forms.Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly System.Windows.Forms.Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    readonly ScottPlot.Plottables.DataStreamer Streamer;

    public DataStreamer()
    {
        InitializeComponent();

        Streamer = formsPlot1.Plot.Add.DataStreamer(1000);

        // disable mouse interaction by default
        formsPlot1.Interaction.Disable();

        // setup a timer to add data to the streamer periodically
        AddNewDataTimer.Tick += (s, e) =>
        {
            var newValues = ScottPlot.Generate.RandomWalker.Next(10);
            Streamer.AddRange(newValues);
        };

        // setup a timer to request a render periodically
        UpdatePlotTimer.Tick += (s, e) =>
        {
            if (Streamer.HasNewData)
            {
                formsPlot1.Plot.Title($"Processed {Streamer.Data.CountTotal:N0} points");
                formsPlot1.Refresh();
            }
        };

        // setup configuration actions
        btnWipeRight.Click += (s, e) => Streamer.ViewWipeRight();
        btnScrollLeft.Click += (s, e) => Streamer.ViewScrollLeft();
        cbManageLimits.CheckedChanged += (s, e) =>
        {
            if (cbManageLimits.Checked)
            {
                Streamer.ManageAxisLimits = true;
                formsPlot1.Interaction.Disable();
                formsPlot1.Plot.Axes.AutoScale();
            }
            else
            {
                Streamer.ManageAxisLimits = false;
                formsPlot1.Interaction.Enable();
            }
        };
    }
}
