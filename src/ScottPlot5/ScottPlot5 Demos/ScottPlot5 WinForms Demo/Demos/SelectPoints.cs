using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class SelectPoints : Form, IDemoWindow
{
    readonly Coordinates[] DataPoints;
    Coordinates MouseDownCoordinates;
    Coordinates MouseNowCoordinates;
    CoordinateRect MouseSlectionRect => new(MouseDownCoordinates, MouseNowCoordinates);
    bool MouseIsDown = false;
    readonly ScottPlot.Plottables.Rectangle RectanglePlot;

    public string Title => "Select Data Points";

    public string Description => "Demonstrates how to use mouse events " +
        "to draw a rectangle around data points to select them";

    public SelectPoints()
    {
        InitializeComponent();

        // add sample data to the plot and keep track of the points
        DataPoints = Generate.RandomCoordinates(100);
        var sp = formsPlot1.Plot.Add.Scatter(DataPoints);
        sp.LineWidth = 0;

        // add a rectangle we can use as a selection indicator
        RectanglePlot = formsPlot1.Plot.Add.Rectangle(0, 0, 0, 0);
        RectanglePlot.FillStyle.Color = Colors.Red.WithAlpha(.2);

        // add events to trigger in response to mouse actions
        formsPlot1.MouseMove += FormsPlot1_MouseMove;
        formsPlot1.MouseDown += FormsPlot1_MouseDown;
        formsPlot1.MouseUp += FormsPlot1_MouseUp;
    }

    private void FormsPlot1_MouseDown(object? sender, MouseEventArgs e)
    {
        if (!checkBox1.Checked)
            return;

        MouseIsDown = true;
        RectanglePlot.IsVisible = true;
        MouseDownCoordinates = formsPlot1.Plot.GetCoordinates(e.X, e.Y);
        formsPlot1.Interaction.Disable(); // disable the default click-drag-pan behavior
    }

    private void FormsPlot1_MouseUp(object? sender, MouseEventArgs e)
    {
        if (!checkBox1.Checked)
            return;

        MouseIsDown = false;
        RectanglePlot.IsVisible = false;

        // clear old markers
        formsPlot1.Plot.Remove<ScottPlot.Plottables.Marker>();

        // identify selectedPoints
        var selectedPoints = DataPoints.Where(x => MouseSlectionRect.Contains(x));

        // add markers to outline selected points
        foreach (Coordinates selectedPoint in selectedPoints)
        {
            var newMarker = formsPlot1.Plot.Add.Marker(selectedPoint);
            newMarker.MarkerStyle.Shape = MarkerShape.OpenCircle;
            newMarker.MarkerStyle.Size = 10;
            newMarker.MarkerStyle.FillColor = Colors.Red.WithAlpha(.2);
            newMarker.MarkerStyle.LineColor = Colors.Red;
            newMarker.MarkerStyle.LineWidth = 1;
        }

        // reset the mouse positions
        MouseDownCoordinates = Coordinates.NaN;
        MouseNowCoordinates = Coordinates.NaN;

        // update the plot
        formsPlot1.Refresh();
        formsPlot1.Interaction.Enable(); // re-enable the default click-drag-pan behavior
    }

    private void FormsPlot1_MouseMove(object? sender, MouseEventArgs e)
    {
        if (!MouseIsDown || !checkBox1.Checked)
            return;

        MouseNowCoordinates = formsPlot1.Plot.GetCoordinates(e.X, e.Y);
        RectanglePlot.CoordinateRect = MouseSlectionRect;
        formsPlot1.Refresh();
    }
}
