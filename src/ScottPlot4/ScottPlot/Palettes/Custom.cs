namespace ScottPlot.Palettes;

internal class Custom : IPalette
{
    public System.Drawing.Color[] Colors { get; }

    public string Name { get; }

    public string Description { get; }

    public Custom(System.Drawing.Color[] colors, string name, string description)
    {
        Colors = colors;
        Name = name;
        Description = description;
    }

    public Custom(string[] hex, string name, string description)
    {
        Colors = hex.Select(System.Drawing.ColorTranslator.FromHtml).ToArray();
        Name = name;
        Description = description;
    }
}
