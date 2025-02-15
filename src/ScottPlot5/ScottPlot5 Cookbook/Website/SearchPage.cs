namespace ScottPlotCookbook.Website;

internal class SearchPage : PageBase
{
    public SearchPage()
    {
    }

    public static void Generate(string folder)
    {
        string saveAs = Path.Combine(folder, "Search.md");
        string md = @"---
title: Cookbook Search - ScottPlot 5.0
description: Search all cookbook recipes for ScottPlot version 5.0
url: /cookbook/5.0/search/
type: single
BreadcrumbNames: [""ScottPlot 5.0 Cookbook"", ""Search""]
BreadcrumbUrls: [""/cookbook/5.0/"", ""/cookbook/5.0/search/""]
date: {{ DATE }}
jsFiles: [""https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/highlight.min.js"", ""/js/cookbook-search-5.0.js""]
---

{{< cookbook-search >}}
".Replace("{{ DATE }}", $"{DateTime.UtcNow:yyyy-MM-dd}");

        File.WriteAllText(saveAs, md);
    }
}
