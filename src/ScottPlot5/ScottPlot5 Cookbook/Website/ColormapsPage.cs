namespace ScottPlotCookbook.Website;

internal class ColormapsPage : PageBase
{
    private static void MoveFront(List<IColormap> colormaps, string Name)
    {
        for (int i = 0; i < colormaps.Count; i++)
        {
            if (colormaps[i].Name == Name)
            {
                IColormap colormap = colormaps[i];
                colormaps.RemoveAt(i);
                colormaps.Insert(0, colormap);
                return;
            }
        }
    }

    public static void Generate(string folder)
    {
        StringBuilder sb = new();

        List<IColormap> colormaps = ScottPlot.Colormap.GetColormaps().ToList();
        MoveFront(colormaps, "Turbo");
        MoveFront(colormaps, "Viridis");

        sb.AppendLine($"# ScottPlot 5.0 Colormaps");
        sb.AppendLine();

        foreach (IColormap colormap in colormaps)
        {
            sb.AppendLine($"### {colormap.Name}");
            sb.AppendLine();
            sb.AppendLine($"```cs");
            sb.AppendLine($"IColormap colormap = new {colormap}();");
            sb.AppendLine($"```");
            sb.AppendLine();
            int steps = 50;
            double[] fractions = Enumerable.Range(0, steps + 1).Select(x => x / (double)steps).ToArray();
            for (int i = 0; i < fractions.Length; i++)
            {
                // TODO: show colormaps as images
                Color color = colormap.GetColor(fractions[i]);
                sb.Append($"<span style='background-color: {color.ToHex()};'>&nbsp;&nbsp;</span>");
            }
            sb.AppendLine();
            sb.AppendLine();
        }

        string md = @"---
title: Colormaps - ScottPlot 5.0
description: Colormaps available in ScottPlot version 5.0
url: /cookbook/5.0/colormaps/
type: single
BreadcrumbNames: [""ScottPlot 5.0 Cookbook"", ""Colormaps""]
BreadcrumbUrls: [""/cookbook/5.0/"", ""/cookbook/5.0/colormaps/""]
date: {{ DATE }}
jsFiles: [""https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/highlight.min.js"", ""/js/cookbook-search-5.0.js""]
---

{{ HTML }}
"
.Replace("{{ DATE }}", $"{DateTime.UtcNow:yyyy-MM-dd}")
.Replace("{{ HTML }}", sb.ToString());

        string saveAs = Path.Combine(folder, "Colormaps.md");
        File.WriteAllText(saveAs, md);
    }
}
