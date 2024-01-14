namespace ScottPlotCookbook.Website;

internal abstract class PageBase
{
    protected StringBuilder SB = new();

    public void AddVersionInformation()
    {
        /*
        string alertHtml = "\n\n" +
            "<div class='alert alert-warning' role='alert'>" +
            "<h4 class='alert-heading py-0 my-0'>" +
            $"⚠️ ScottPlot {ScottPlot.Version.VersionString} is a preview package" +
            "</h4>" +
            "<hr />" +
            "<p class='mb-0'>" +
            "<span class='fw-semibold'>This page describes a beta release of ScottPlot.</span> " +
            "It is available on NuGet as a preview package, but its API is not stable " +
            "and it is not recommended for production use. " +
            "See the <a href='https://scottplot.net/versions/'>ScottPlot Versions</a> page for more information. " +
            "</p>" +
            "</div>" +
            "\n\n";
        SB.AppendLine(alertHtml);
        */

        /*
        SB.AppendLine();
        SB.AppendLine("{{< banner-sp5 >}}");
        SB.AppendLine();
        */
    }

    public void Save(string folder, string title, string description, string filename, string url, string[]? frontmatter)
    {
        Directory.CreateDirectory(folder);

        if (!url.EndsWith("/"))
            url += "/";

        StringBuilder sbfm = new();
        sbfm.AppendLine("---");
        sbfm.AppendLine($"Title: {title}");
        sbfm.AppendLine($"Description: {description}");
        sbfm.AppendLine($"URL: {url}");
        if (frontmatter is not null)
        {
            foreach (string line in frontmatter)
            {
                sbfm.AppendLine(line);
            }
        }
        sbfm.AppendLine($"Date: {DateTime.UtcNow:yyyy-MM-dd}");
        sbfm.AppendLine($"Version: {ScottPlot.Version.LongString}");
        sbfm.AppendLine($"Version: {ScottPlot.Version.LongString}");
        sbfm.AppendLine($"SearchUrl: \"/cookbook/5.0/search/\"");
        sbfm.AppendLine($"ShowEditLink: false");
        sbfm.AppendLine("---");
        sbfm.AppendLine();

        string md = sbfm.ToString() + SB.ToString();

        string saveAs = Path.Combine(folder, filename);
        File.WriteAllText(saveAs, md);
    }

    public void SaveSearchPage(string folder)
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
