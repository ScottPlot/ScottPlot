using System;
using System.Diagnostics;
using System.IO;

namespace ScottPlot;

public class Launcher
{
    public Plot Plot { get; }

    public Launcher(Plot plot) { Plot = plot; }
}

public static class LauncherExtensions
{
    /// <summary>
    /// Save the plot as an image file and open it with the default file launcher
    /// </summary>
    public static void ImageFile(this Launcher launcher, string saveAs = "plot.png", int width = 600, int height = 400)
    {
        saveAs = Path.GetFullPath(saveAs);
        launcher.Plot.SaveFig(saveAs, width, height);
        Console.WriteLine($"Launching: {saveAs}");

        ProcessStartInfo psi = new(saveAs) { UseShellExecute = true };
        Process.Start(psi);
    }
}