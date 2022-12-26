namespace ScottPlot;

/// <summary>
/// Represents a font which may or may not exist on the system
/// </summary>
public readonly struct Font
{
    public readonly string Name;
    public readonly float Size;
    public readonly float Weight;
    public readonly bool Italic;

    public Font(string name, float size = 12, float weight = 400, bool italic = false)
    {
        Name = name;
        Size = size;
        Weight = weight;
        Italic = italic;
    }

    public static Font Default => new(FontService.DefaultFontName);

    public Font WithSize(float size) => new(Name, size, Weight, Italic);
    public Font WithName(string name) => new(name, Size, Weight, Italic);
    public Font WithBold(bool bold = true) => new(Name, Size, bold ? (float)FontWeight.Bold : (float)FontWeight.Normal, Italic);
    public Font WithWeight(FontWeight weight) => new(Name, Size, (float)weight, Italic);
    public Font WithItalic(bool italic = true) => new(Name, Size, Weight, italic);
    public FontWeight GetNearestWeight()
    {
        foreach (FontWeight weight in Enum.GetValues(typeof(FontWeight)))
        {
            if (Weight >= (float)weight)
                return weight;
        }

        return Enum.GetValues(typeof(FontWeight)).Cast<FontWeight>().Last();
    }
}
