namespace WinForms_Demo.Demos;

public partial class MultiAxis : Form, IDemoWindow
{
    public string Title => "Multi-Axis";

    public string Description => "Display data which visually overlaps but is plotted on different axes";

    readonly ScottPlot.IYAxis YAxis1;

    readonly ScottPlot.IYAxis YAxis2;

    public MultiAxis()
    {
        InitializeComponent();

        // Store the primary Y axis so we can refer to it later
        YAxis1 = formsPlot1.Plot.LeftAxis;

        // Create a second Y axis, add it to the plot, and save it for later
        ScottPlot.AxisPanels.LeftAxis yAxis2 = new();
        formsPlot1.Plot.YAxes.Add(yAxis2);
        YAxis2 = yAxis2;

        // setup button actions
        btnRandomize.Click += (s, e) => PlotRandomData();

        // plot random data to start
        PlotRandomData();
    }

    private void PlotRandomData()
    {
        formsPlot1.Plot.Clear();

        double[] data1 = ScottPlot.RandomDataGenerator.Generate.RandomWalk(count: 51, mult: 1);
        var sig1 = formsPlot1.Plot.Add.Signal(data1);
        sig1.Axes.YAxis = YAxis1;
        YAxis1.Label.Text = "YAxis1";
        YAxis1.Label.ForeColor = sig1.LineStyle.Color;

        double[] data2 = ScottPlot.RandomDataGenerator.Generate.RandomWalk(count: 51, mult: 1000);
        var sig2 = formsPlot1.Plot.Add.Signal(data2);
        sig2.Axes.YAxis = YAxis2;
        YAxis2.Label.Text = "YAxis2";
        YAxis2.Label.ForeColor = sig2.LineStyle.Color;

        formsPlot1.Plot.AutoScale();
        formsPlot1.Plot.Zoom(.8, .7); // zoom out slightly
        formsPlot1.Refresh();
    }

    private void btnRandomize_Click(object sender, EventArgs e)
    {
        PlotRandomData();
    }

    private void btnManualScale_Click(object sender, EventArgs e)
    {
        formsPlot1.Plot.SetAxisLimits(0, 50, -20, 20, formsPlot1.Plot.BottomAxis, YAxis1);
        formsPlot1.Plot.SetAxisLimits(0, 50, -20_000, 20_000, formsPlot1.Plot.BottomAxis, YAxis2);
        formsPlot1.Refresh();
    }

    private void btnAutoScale_Click(object sender, EventArgs e)
    {
        formsPlot1.Plot.Margins();
        formsPlot1.Plot.AutoScale();
        formsPlot1.Refresh();
    }

    private void btnAutoScaleTight_Click(object sender, EventArgs e)
    {
        formsPlot1.Plot.Margins(0, 0);
        formsPlot1.Refresh();
    }

    private void btnAutoScaleWithPadding_Click(object sender, EventArgs e)
    {
        formsPlot1.Plot.Margins(1, 1);
        formsPlot1.Refresh();
    }
}
