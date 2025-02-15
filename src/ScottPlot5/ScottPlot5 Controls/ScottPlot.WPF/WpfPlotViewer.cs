namespace ScottPlot.WPF;

public static class WpfPlotViewer
{
    public static void Launch(Plot plot, string title = "", int width = 600, int height = 400)
    {
        IPlotControl? originalControl = plot.PlotControl;
        WpfPlot wpfPlot = new();
        wpfPlot.Reset(plot);
        System.Windows.Window win = new()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
            Width = width,
            Height = height,
            Title = title,
            Content = wpfPlot,
        };
        win.Closed += (s, e) => plot.PlotControl = originalControl;
        win.ShowDialog();
    }
}
