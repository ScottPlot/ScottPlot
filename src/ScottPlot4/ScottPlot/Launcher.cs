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

    private readonly string ImageHtmlTemplate = """
        <!doctype html>
        <html lang="en">

        <head>
            <meta charset="utf-8">
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <link rel="icon" href="https://scottplot.net/favicon.ico" sizes="any">
            <title>ScottPlot {{ FILENAME }}</title>
            <style>
                body {
                    padding: 0;
                    margin: 0;
                    text-align: center;
                    background-color: #f0f0f0;
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                }

                img {
                    background-color: white;
                    max-width: 80%;
                    margin: 5em auto 3em auto;
                    display: block;
                    box-shadow: 0px 0px 20px 10px rgba(0, 0, 0, 0.2);
                }

                a {
                    text-decoration: none;
                    color: black;
                }

                a:hover {
                    text-decoration: underline;
                }
            </style>

            <script language="javascript">
                function toggle(){
                    const url = isLive() ? '?' : '?refresh={{ REFRESH_MSEC }}';
                    window.location.href = url;
                }
        
                function isLive(){
                    const urlParams = new URLSearchParams(window.location.search);
                    const refreshMsec = urlParams.get('refresh');
                    return refreshMsec > 0;
                }
            </script>

        </head>

        <body>
            <a href='{{ FILE }}'><img src="{{ FILE }}" width="{{ WIDTH }}" height="{{ HEIGHT }}"></a>
            <div style='line-height: 150%;'>
                <div>{{ FILENAME }} ({{ WIDTH }}x{{ HEIGHT }}) <script>document.write(new Date().toLocaleTimeString());</script></div>
                <div><input id="liveCheckbox" type="checkbox" onclick="toggle();"> auto-refresh</div>
                <div style='opacity: .5;'><a href='https://scottplot.net'>{{ VERSION }}</a></div>
            </div>
        </body>

        <script language="javascript">
            document.getElementById('liveCheckbox').checked = isLive();
            const urlParams = new URLSearchParams(window.location.search);
            const refreshMsec = urlParams.get('refresh');
            if (refreshMsec > 0){
                setTimeout(function(){
                    window.location.reload(1);
                }, refreshMsec);
            }
        </script>

        </html>
        """;

    private string GetImageHtml(int width, int height, string imageFile, int refreshMsec = 2000)
    {
        string html = ImageHtmlTemplate;
        html = html.Replace("{{ REFRESH_MSEC }}", refreshMsec.ToString());
        html = html.Replace("{{ WIDTH }}", width.ToString());
        html = html.Replace("{{ HEIGHT }}", height.ToString());
        html = html.Replace("{{ VERSION }}", ScottPlot.Version.LongString);
        html = html.Replace("{{ FILE }}", imageFile);
        html = html.Replace("{{ FILENAME }}", Path.GetFileName(imageFile));
        return html;
    }

    /// <summary>
    /// Save the plot as an image embedded in a HTML file and launch it with the default web browser
    /// </summary>
    public void ImageHTML(int width = 600, int height = 400, string imageFile = "plot.png", string htmlFile = "plot.html")
    {
        imageFile = Plot.SaveFig(imageFile, width, height);
        htmlFile = Path.GetFullPath(htmlFile);
        string html = GetImageHtml(width, height, imageFile);
        File.WriteAllText(htmlFile, html);
        ExecuteFile(htmlFile);
    }
}
