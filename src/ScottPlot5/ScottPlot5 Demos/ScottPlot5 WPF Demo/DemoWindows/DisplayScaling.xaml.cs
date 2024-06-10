using System.Windows;

namespace WPF_Demo.DemoWindows;

public partial class DisplayScaling : Window, IDemoWindow
{
    public string DemoTitle => "Display Scaling";
    public string Description => "Demonstrates how to track " +
        "mouse position on displays which use DPI scaling.";

    readonly ScottPlot.Plottables.Crosshair Crosshair;

    public DisplayScaling()
    {
        InitializeComponent();

        WpfPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin());
        WpfPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos());
        Crosshair = WpfPlot1.Plot.Add.Crosshair(0, 0);

        MouseMove += DisplayScaling_MouseMove;
    }

    private void DisplayScaling_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        Point p = e.GetPosition(WpfPlot1);
        ScottPlot.Pixel mousePixel = new(p.X * WpfPlot1.DisplayScale, p.Y * WpfPlot1.DisplayScale);
        ScottPlot.Coordinates coordinates = WpfPlot1.Plot.GetCoordinates(mousePixel);
        Crosshair.Position = coordinates;
        WpfPlot1.Refresh();
    }
}
