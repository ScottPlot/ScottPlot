using System;
using System.Threading;
using System.Windows;

namespace ScottPlot;

public static class LauncherExtensions
{
    /// <summary>
    /// Launch the plot in an interactive WPF control
    /// </summary>
    public static void WpfPlot(this Launcher launcher, int width = 600, int height = 400, string title = "Interactive Plot")
    {
        Thread t = new(() =>
        {
            new WpfPlotViewer(launcher.Plot, width, height, title).ShowDialog();
        });
        t.SetApartmentState(ApartmentState.STA);
        t.Start();
    }
}
