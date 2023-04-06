using System;
using System.Diagnostics;
using System.IO;

namespace ScottPlot;

public class Launcher
{
    public Plot Plot { get; }

    public Launcher(Plot plot) { Plot = plot; }

    /// <summary>
    /// Launch a file using the system default file handler
    /// </summary>
    public void Launch(string filePath)
    {
        filePath = Path.GetFullPath(filePath);
        Console.WriteLine($"Launching: {filePath}");
        ProcessStartInfo psi = new(filePath) { UseShellExecute = true };
        Process.Start(psi);
    }
}

public static class LauncherExtensions
{
    /// <summary>
    /// Save the plot as an image file and open it with the default file launcher
    /// </summary>
    public static void ImageFile(this Launcher launcher, string saveAs = "plot.png", int width = 600, int height = 400)
    {
        launcher.Plot.SaveFig(saveAs, width, height);
        launcher.Launch(saveAs);
    }

    /// <summary>
    /// Save the plot as an image embedded in a HTML file and launch it with the default web browser
    /// </summary>
    public static void Web(this Launcher launcher, string saveAs = "plot.html", int width = 600, int height = 400)
    {
        launcher.Plot.Resize(width, height);
        string html = "<html>" +
            "<body style='text-align: center; margin-top: 2em;'>" +
            $"<code style='font-size: 1.5em;'>{DateTime.Now}</code>" +
            $"<div style='margin: 1em;'>{launcher.Plot.GetImageHTML()}</div>" +
            "</body>" +
            "</html>";
        File.WriteAllText(saveAs, html);
        launcher.Launch(saveAs);
    }
}
