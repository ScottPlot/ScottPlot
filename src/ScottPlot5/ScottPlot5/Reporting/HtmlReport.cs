namespace ScottPlot.Reporting;

public class HtmlReport() : PlotCollection
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DefaultPlotWidth { get; set; } = 600;
    public int DefaultPlotHeight { get; set; } = 400;
    readonly StringBuilder SB = new();

    public string Template { get; set; } =
        """
        <!doctype html>
        <html lang="en">
          <head>
            <meta charset="utf-8">
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <title>{{ TITLE }}</title>
            <meta name="description" content="{{ DESCRIPTION }}">
            <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
          </head>
          <body>
            <main class='container'>{{ CONTENT }}</main>
          </body>
        </html>
        """;

    public new void Add(Plot plot, string? title = null, string? description = null)
    {
        AddPlotPng(plot, title ?? string.Empty, description ?? string.Empty);
    }

    public void AddHeading(string text, int level = 1)
    {
        SB.AppendLine($"<h{level}>{text}</h{level}>");
    }

    public void AddParagraph(string text)
    {
        SB.AppendLine($"<p>{text}</p>");
    }

    public void AddHtml(string html)
    {
        SB.AppendLine(html);
    }

    public void AddPlotPng(Plot plot, string title, string description)
    {
        AddPlotPng(plot, title, description, DefaultPlotWidth, DefaultPlotHeight);
    }

    public void AddPlotPng(Plot plot, string title, string description, int width, int height)
    {
        base.Add(plot, title, description);
        string plotHtml = plot.GetPngHtml(width, height, styleContent: "max-width: 100%");
        SB.AppendLine($"<h1>{title}</h1>");
        SB.AppendLine($"<p>{description}</p>");
        SB.AppendLine($"<div>{plotHtml}</div>");
    }

    public void AddPlotSvg(Plot plot, string title, string description)
    {
        AddPlotSvg(plot, title, description, DefaultPlotWidth, DefaultPlotHeight);
    }

    public void AddPlotSvg(Plot plot, string title, string description, int width, int height)
    {
        base.Add(plot, title, description);
        string plotHtml = plot.GetSvgHtml(width, height);
        SB.AppendLine($"<h1>{title}</h1>");
        SB.AppendLine($"<p>{description}</p>");
        SB.AppendLine($"<div>{plotHtml}</div>");
    }

    public void AddPageBreak()
    {
        SB.AppendLine($"<hr class='my-5 invisible' style='page-break-after: always;'>");
    }

    public string GetHtml()
    {
        return Template
            .Replace("{{ TITLE }}", Title)
            .Replace("{{ DESCRIPTION }}", Description)
            .Replace("{{ CONTENT }}", SB.ToString());
    }

    public void SaveAs(string filePath)
    {
        File.WriteAllText(filePath, GetHtml());
    }
}
