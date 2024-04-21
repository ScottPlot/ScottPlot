namespace WinForms_Demo.Demos;

public partial class DataLogger : Form, IDemoWindow
{
    public string Title => "Data Logger";
    public string Description => "Plots live streaming data as a growing line plot.";

    readonly System.Windows.Forms.Timer AddNewDataTimer = new() { Interval = 10, Enabled = true };
    readonly System.Windows.Forms.Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    readonly ScottPlot.Plottables.DataLogger Logger1;
    readonly ScottPlot.Plottables.DataLogger Logger2;

    public DataLogger()
    {
        InitializeComponent();

        Logger1 = formsPlot1.Plot.Add.DataLogger();
        Logger2 = formsPlot1.Plot.Add.DataLogger();

        var axis1 = formsPlot1.Plot.Axes.Right;
        Logger1.Axes.YAxis = axis1;

        var axis2 = formsPlot1.Plot.Axes.AddRightAxis();
        Logger2.Axes.YAxis = axis2;

        formsPlot1.Plot.RenderInMemory(); // force a single render
        formsPlot1.Plot.Axes.Left.Range.Reset();

        AddNewDataTimer.Tick += (s, e) =>
        {
            var newValues = ScottPlot.Generate.RandomWalker.Next(10);
            Logger1.Add(newValues);
            Logger2.Add(newValues.Select(x => x * 4));
        };

        UpdatePlotTimer.Tick += (s, e) =>
        {
            if (Logger1.HasNewData || Logger2.HasNewData)
                formsPlot1.Refresh();
        };
    }
}
