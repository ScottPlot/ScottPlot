using ScottPlot.Reporting.Components;

namespace ScottPlot.Reporting.Export;

public class HtmlReportExporter(Report report)
{
    private Report Report { get; } = report;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool SvgPlotImages { get; set; } = false;

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

    public string GetHtml()
    {
        StringBuilder sb = new();
        foreach (IComponent component in Report.ComponentList)
        {
            switch (component)
            {
                case HeadingComponent hc:
                    sb.AppendLine($"<h{hc.Level}>{hc.Text}</h{hc.Level}>");
                    break;
                case TextComponent tc:
                    sb.AppendLine($"<p>{tc.Text}</p>");
                    break;
                case PlotComponent pc:
                    string pcHtml = SvgPlotImages
                        ? pc.Plot.GetSvgHtml(pc.Width, pc.Height)
                        : pc.Plot.GetPngHtml(pc.Width, pc.Height).Replace("<img ", "<img style='max-width: 100%' ");
                    sb.AppendLine($"<div>{pcHtml}</div>");
                    break;
                case PageBreakComponent pb:
                    sb.AppendLine($"<hr class='my-5 invisible' style='page-break-after: always;'>");
                    break;
                default:
                    sb.AppendLine($"""
                                   <div class="alert alert-danger" role="alert">
                                     <h4 class="alert-heading">Unsupported Component</h4>
                                     <p class="mb-0"><code>{component}</code></p>
                                   </div>
                                   """);
                    break;
            }
        }

        return Template
            .Replace("{{ TITLE }}", Title)
            .Replace("{{ DESCRIPTION }}", Description)
            .Replace("{{ CONTENT }}", sb.ToString());
    }

    public void SaveAs(string filePath)
    {
        File.WriteAllText(filePath, GetHtml());
    }
}
