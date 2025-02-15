namespace ScottPlotCookbook.Website;

internal class ColorsPage : PageBase
{
    public static void Generate(string folder)
    {
        StringBuilder sb = new();

        sb.AppendLine($"# ScottPlot 5.0 Colors");
        sb.AppendLine();

        sb.AppendLine("<ul style='list-style-type: none;'>");
        foreach ((string name, Color color) in Colors.GetNamedColors())
        {
            sb.AppendLine($"<li class='font-monospace'><div style=\"display: inline-block; width: 4rem; height: 1rem; background-color: {color.ToHex()}; outline: 1px solid black;\"></div>" +
                $" {color.ToHex()} {name}</li>");
        }
        sb.AppendLine("</ul>");

        string md = @"---
title: Colors - ScottPlot 5.0
description: Colors available in ScottPlot version 5.0
url: /cookbook/5.0/colors/
type: single
BreadcrumbNames: [""ScottPlot 5.0 Cookbook"", ""Colors""]
BreadcrumbUrls: [""/cookbook/5.0/"", ""/cookbook/5.0/colors/""]
date: {{ DATE }}
jsFiles: [""https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/highlight.min.js"", ""/js/cookbook-search-5.0.js""]
---

{{ HTML }}
"
.Replace("{{ DATE }}", $"{DateTime.UtcNow:yyyy-MM-dd}")
.Replace("{{ HTML }}", sb.ToString());

        string saveAs = Path.Combine(folder, "Colors.md");
        File.WriteAllText(saveAs, md);
    }
}
