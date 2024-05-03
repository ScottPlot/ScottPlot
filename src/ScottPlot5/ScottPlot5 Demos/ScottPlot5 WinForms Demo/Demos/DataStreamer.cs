using ScottPlot;
using ScottPlot.Plottables;
using System.Diagnostics;

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

        // only show marker button in scroll mode
        btnMark.Visible = false;

        // setup a timer to add data to the streamer periodically
        AddNewDataTimer.Tick += (s, e) =>
        {
            int count = 5;

            // add new sample data
            Streamer1.AddRange(Walker1.Next(count));
            Streamer2.AddRange(Walker2.Next(count));

            // slide marker to the left
            formsPlot1.Plot.GetPlottables<Marker>()
                .ToList()
                .ForEach(m => m.X -= count);

            // remove off-screen marks
            formsPlot1.Plot.GetPlottables<Marker>()
                .Where(m => m.X < 0)
                .ToList()
                .ForEach(m => formsPlot1.Plot.Remove(m));
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
        btnWipeRight.Click += (s, e) =>
        {
            Streamer1.ViewWipeRight(0.1);
            Streamer2.ViewWipeRight(0.1);
            btnMark.Visible = false;
            formsPlot1.Plot.Remove<Marker>();
        };

        btnScrollLeft.Click += (s, e) =>
        {
            Streamer1.ViewScrollLeft();
            Streamer2.ViewScrollLeft();
            btnMark.Visible = true;
        };

        btnMark.Click += (s, e) =>
        {
            double x1 = Streamer1.Count;
            double y1 = Streamer1.Data.NewestPoint;
            var marker1 = formsPlot1.Plot.Add.Marker(x1, y1);
            marker1.Size = 20;
            marker1.Shape = MarkerShape.OpenCircle;
            marker1.Color = Streamer1.LineColor;
            marker1.LineWidth = 2;

            double x2 = Streamer2.Count;
            double y2 = Streamer2.Data.NewestPoint;
            var marker2 = formsPlot1.Plot.Add.Marker(x2, y2);
            marker2.Size = 20;
            marker2.Shape = MarkerShape.OpenCircle;
            marker2.Color = Streamer2.LineColor;
            marker2.LineWidth = 2;
        };

        rbManage.CheckedChanged += (s, e) =>
        {
            bool manageAxisLimits = ((RadioButton)s!).Checked;

            if (manageAxisLimits)
            {
                formsPlot1.Plot.Axes.ContinuouslyAutoscale = false;
                Streamer1.ManageAxisLimits = true;
                Streamer2.ManageAxisLimits = true;
            }
            else
            {
                formsPlot1.Plot.Axes.ContinuouslyAutoscale = true;
                Streamer1.ManageAxisLimits = false;
                Streamer2.ManageAxisLimits = false;
            }
        };
    }
}
