namespace ScottPlotCookbook.Website;

internal class PalettesPage : PageBase
{
    public PalettesPage()
    {
    }

    private static void MoveFront(List<IPalette> palettes, string Name)
    {
        for (int i = 0; i < palettes.Count; i++)
        {
            if (palettes[i].Name == Name)
            {
                IPalette pal = palettes[i];
                palettes.RemoveAt(i);
                palettes.Insert(0, pal);
                return;
            }
        }
    }

    public static void Generate(string folder)
    {
        StringBuilder sb = new();

        List<IPalette> palettes = ScottPlot.Palette.GetPalettes().ToList();
        MoveFront(palettes, "Category 20");
        MoveFront(palettes, "Category 10");

        sb.AppendLine($"# ScottPlot 5.0 Color Palettes");
        sb.AppendLine();

        foreach (IPalette palette in palettes)
        {
            sb.AppendLine($"## {palette.Name}");
            sb.AppendLine();
            sb.AppendLine($"{palette.Description}");
            sb.AppendLine();
            sb.AppendLine($"```cs");
            sb.AppendLine($"IPalette palette = new {palette}();");
            sb.AppendLine($"```");
            sb.AppendLine();
            for (int i = 0; i < palette.Colors.Length; i++)
            {
                Color color = palette.Colors[i];
                string textColor = color.Luminance > .5 ? "black" : "white";
                sb.AppendLine($"<div class='px-3 py-2 fw-semibold' style='background-color: {color.ToHex()}; color: {textColor}'>" +
                    $"palette.GetColor({i}) returns {color.ToHex()}" +
                    $"</div>");
            }
            sb.AppendLine();
            sb.AppendLine();
        }

        string md = @"---
title: Color Palettes - ScottPlot 5.0
description: Color palettes available in ScottPlot version 5.0
url: /cookbook/5.0/palettes/
type: single
BreadcrumbNames: [""ScottPlot 5.0 Cookbook"", ""Palettes""]
BreadcrumbUrls: [""/cookbook/5.0/"", ""/cookbook/5.0/palettes/""]
date: {{ DATE }}
jsFiles: [""https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/highlight.min.js"", ""/js/cookbook-search-5.0.js""]
---

{{ HTML }}
"
.Replace("{{ DATE }}", $"{DateTime.UtcNow:yyyy-MM-dd}")
.Replace("{{ HTML }}", sb.ToString());

        string saveAs = Path.Combine(folder, "Palettes.md");
        File.WriteAllText(saveAs, md);
    }
}
