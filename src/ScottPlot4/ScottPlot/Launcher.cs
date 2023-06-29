using System;
using System.Diagnostics;
using System.IO;

namespace ScottPlot;

#nullable enable

/// <summary>
/// This class consumes a <see cref="Plot"/> and has 
/// helper methods for displaying it in different ways.
/// </summary>
public class Launcher
{
    public Plot Plot { get; }

    public Launcher(Plot plot)
    {
        Plot = plot;
    }

    /// <summary>
    /// Launch a file using the system default file handler
    /// </summary>
    private void ExecuteFile(string filePath)
    {
        ProcessStartInfo psi = new(filePath) { UseShellExecute = true };
        Process.Start(psi);
    }

    /// <summary>
    /// Save the plot as an image file and open it with the default file launcher
    /// </summary>
    public void ImageFile(int width = 600, int height = 400, string? filename = null)
    {
        string saveAs = filename ?? $"plot-{DateTime.Now.Ticks}.png";
        Plot.SaveFig(saveAs, width, height);
        ExecuteFile(saveAs);
    }

    /// <summary>
    /// Save the plot as an image embedded in a HTML file and launch it with the default web browser
    /// </summary>
    public void ImageHTML(int width = 600, int height = 400, string saveAs = "plot.html")
    {
        Plot.Resize(width, height);
        string html = "<html>" +
            "<body style='text-align: center; margin-top: 2em;'>" +
            $"<code style='font-size: 1.5em;'>{DateTime.Now}</code>" +
            $"<div style='margin: 1em;'>{Plot.GetImageHTML()}</div>" +
            "</body>" +
            "</html>";
        File.WriteAllText(saveAs, html);
        ExecuteFile(saveAs);
    }
}
