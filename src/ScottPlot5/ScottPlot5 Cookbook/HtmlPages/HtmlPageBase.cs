namespace ScottPlotCookbook.HtmlPages;

internal abstract class HtmlPageBase
{
    protected StringBuilder SB = new();

    /// <summary>
    /// Write the HTML file to disk.
    /// If local file mode is enabled, pretty URLs like '/5.0/#graph' 
    /// will be replaced by ugly file URLs like '/5.0/index.html#graph'
    /// which allow browsing on a local filesystem.
    /// </summary>
    protected void Save(string folder, string title, string rootUrl = "", bool localFile = false, string filename = "index.html")
    {
        string html = File.ReadAllText("HtmlTemplates/Page.html")
            .Replace("{{VERSION}}", ScottPlot.Version.VersionString)
            .Replace("{{DATE}}", DateTime.Now.ToShortDateString())
            .Replace("{{TIME}}", DateTime.Now.ToShortTimeString())
            .Replace("{{TITLE}}", title)
            .Replace("{{CONTENT}}", SB.ToString())
            .Replace("{{HEADER_LINK}}", rootUrl)
            ;

        if (localFile)
        {
            html = html.Replace("/#", "/index.local.html#");
            filename = filename.Replace(".html", ".local.html");
        }

        string saveAs = Path.Combine(folder, filename);
        File.WriteAllText(saveAs, html);
        TestContext.WriteLine(saveAs);
    }
}
