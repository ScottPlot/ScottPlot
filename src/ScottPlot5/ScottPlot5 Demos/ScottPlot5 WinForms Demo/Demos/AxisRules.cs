using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class AxisRules : Form, IDemoWindow
{
    public string Title => "Axis Rules";

    public string Description => "Configure rules that limit how far the user " +
        "can zoom in or out or enforce equal axis scaling";

    public AxisRules()
    {
        InitializeComponent();
        UnlockButtons();

        var coordinates = Generate.RandomCoordinates(1000);
        var sp = formsPlot1.Plot.Add.Scatter(coordinates);
        sp.LineWidth = 0;
        sp.MarkerStyle.Size = 5;

        btnBoundaryMin.Click += (s, e) =>
        {
            ScottPlot.AxisRules.MinimumBoundary rule = new(
                xAxis: formsPlot1.Plot.Axes.Bottom,
                yAxis: formsPlot1.Plot.Axes.Left,
                limits: new AxisLimits(0, 1, 0, 1));

            formsPlot1.Plot.Axes.Rules.Clear();
            formsPlot1.Plot.Axes.Rules.Add(rule);
            formsPlot1.Plot.Title("Disable zooming in beyond [0, 1]");
            formsPlot1.Refresh();
            LockButtons();
        };

        btnBoundaryMax.Click += (s, e) =>
        {
            ScottPlot.AxisRules.MaximumBoundary rule = new(
                xAxis: formsPlot1.Plot.Axes.Bottom,
                yAxis: formsPlot1.Plot.Axes.Left,
                limits: new AxisLimits(0, 1, 0, 1));

            formsPlot1.Plot.Axes.Rules.Clear();
            formsPlot1.Plot.Axes.Rules.Add(rule);
            formsPlot1.Plot.Title("Disable zooming out beyond [0, 1]");
            formsPlot1.Refresh();
            LockButtons();
        };

        btnScalePreserveX.Click += (s, e) =>
        {
            ScottPlot.AxisRules.SquarePreserveX rule = new(
                xAxis: formsPlot1.Plot.Axes.Bottom,
                yAxis: formsPlot1.Plot.Axes.Left);

            formsPlot1.Plot.Axes.Rules.Clear();
            formsPlot1.Plot.Axes.Rules.Add(rule);
            formsPlot1.Plot.Title("Square axes zooming Y to preserve X");
            formsPlot1.Refresh();
            LockButtons();
        };

        btnScalePreserveY.Click += (s, e) =>
        {
            ScottPlot.AxisRules.SquarePreserveY rule = new(
                xAxis: formsPlot1.Plot.Axes.Bottom,
                yAxis: formsPlot1.Plot.Axes.Left);

            formsPlot1.Plot.Axes.Rules.Clear();
            formsPlot1.Plot.Axes.Rules.Add(rule);
            formsPlot1.Plot.Title("Square axes zooming X to preserve Y");
            formsPlot1.Refresh();
            LockButtons();
        };

        btnScaleZoom.Click += (s, e) =>
        {
            ScottPlot.AxisRules.SquareZoomOut rule = new(
                xAxis: formsPlot1.Plot.Axes.Bottom,
                yAxis: formsPlot1.Plot.Axes.Left);

            formsPlot1.Plot.Axes.Rules.Clear();
            formsPlot1.Plot.Axes.Rules.Add(rule);
            formsPlot1.Plot.Title("Square axes by zooming out the smaller axis");
            formsPlot1.Refresh();
            LockButtons();
        };

        btnSpanMin.Click += (s, e) =>
        {
            ScottPlot.AxisRules.MinimumSpan rule = new(
                xAxis: formsPlot1.Plot.Axes.Bottom,
                yAxis: formsPlot1.Plot.Axes.Left,
                xSpan: 1,
                ySpan: 1);

            formsPlot1.Plot.Axes.Rules.Clear();
            formsPlot1.Plot.Axes.Rules.Add(rule);
            formsPlot1.Plot.Title("Disabled zooming in beyond an axis span of 1");
            formsPlot1.Refresh();
            LockButtons();
        };

        btnSpanMax.Click += (s, e) =>
        {
            ScottPlot.AxisRules.MaximumSpan rule = new(
                xAxis: formsPlot1.Plot.Axes.Bottom,
                yAxis: formsPlot1.Plot.Axes.Left,
                xSpan: 1,
                ySpan: 1);

            formsPlot1.Plot.Axes.Rules.Clear();
            formsPlot1.Plot.Axes.Rules.Add(rule);
            formsPlot1.Plot.Title("Disabled zooming out beyond an axis span of 1");
            formsPlot1.Refresh();
            LockButtons();
        };

        btnReset.Click += (s, e) =>
        {
            formsPlot1.Plot.Axes.Rules.Clear();
            formsPlot1.Plot.Axes.AutoScale();
            formsPlot1.Plot.Title("No axis rules are in effect");
            formsPlot1.Refresh();
            UnlockButtons();
        };
    }

    private void LockButtons()
    {
        groupBox1.Enabled = false;
        groupBox2.Enabled = false;
        groupBox3.Enabled = false;
        btnReset.Enabled = true;
    }

    private void UnlockButtons()
    {
        groupBox1.Enabled = true;
        groupBox2.Enabled = true;
        groupBox3.Enabled = true;
        btnReset.Enabled = false;
    }
}
