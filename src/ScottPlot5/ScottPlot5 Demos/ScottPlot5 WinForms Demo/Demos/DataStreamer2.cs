namespace WinForms_Demo.Demos;

public partial class DataStreamer2 : Form, IDemoWindow
{
    public string Title => "Data Streamer (Experimental)";
    public string Description => "Experimental data streamer that uses a circular buffer for improved performance.";

    readonly System.Windows.Forms.Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly System.Windows.Forms.Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    readonly ScottPlot.DataGenerators.RandomWalker Walker1 = new(0, multiplier: 0.01);
    readonly ScottPlot.DataGenerators.RandomWalker Walker2 = new(1, multiplier: 1000);

    public DataStreamer2()
    {
        InitializeComponent();

        AddNewDataTimer.Tick += (s, e) =>
        {
            int count = 5;
            for (int i = 0; i < count; i++)
            {
                var val1 = Walker1.Next();
                loggerPlotHorz.Logger1.Add(val1);
                loggerPlotVert.Logger1.Add(val1);

                var val2 = Walker2.Next();
                loggerPlotHorz.Logger2.Add(val2);
                loggerPlotVert.Logger2.Add(val2);
            }
        };

        UpdatePlotTimer.Tick += (s, e) =>
        {
            loggerPlotHorz.RefreshPlot();
            loggerPlotVert.RefreshPlot();
        };
    }

    private void cbRunning_CheckedChanged(object sender, EventArgs e)
    {
        AddNewDataTimer.Enabled = cbRunning.Checked;
        UpdatePlotTimer.Enabled = cbRunning.Checked;
    }
}
