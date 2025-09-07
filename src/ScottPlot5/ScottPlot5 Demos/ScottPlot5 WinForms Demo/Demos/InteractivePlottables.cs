namespace WinForms_Demo.Demos;

public partial class InteractivePlottables : Form, IDemoWindow
{
    public string Title => "Interactive Plottables";
    public string Description => "Some plot types are immediately interactive, " +
        "without requiring the user to wire any mouse tracking.";

    public InteractivePlottables()
    {
        InitializeComponent(); formsPlot1.Plot.HandlePressed += (s, e) => Text = $"{e} pressed";
        formsPlot1.Plot.HandleMoved += (s, e) => Text = $"{e} moved";
        formsPlot1.Plot.HandleReleased += (s, e) => Text = $"{e} dropped";
        formsPlot1.Plot.HandleHoverChanged += (s, e) => Text = e is null ? "" : $"{e} hovered";

        for (int i = 0; i < 5; i++)
        {
            ScottPlot.CoordinateLine line = ScottPlot.Generate.RandomCoordinateLine();
            formsPlot1.Plot.Add.InteractiveLineSegment(line);
        }
    }
}
