namespace ScottPlot.FontResolvers;

/// <summary>
/// Font resolver that creates a typeface from a specified TTF file
/// </summary>
public class FileFontResolver(string name, string path, FontWeight weight, FontSlant slant, FontWidth width) : IFontResolver
{

    [Obsolete("Explicitly declare SkFontStyles")]
    public FileFontResolver(string name, string path, bool bold, bool italic) : this(name, path,
        bold ? FontWeight.Bold : FontWeight.Normal,
        italic ? FontSlant.Italic : FontSlant.Upright, FontWidth.Normal)
    { }

    private string FontName { get; } = name;
    private string FontPath { get; } = File.Exists(path)
        ? Path.GetFullPath(path)
        : throw new FileNotFoundException(path);
    private FontWeight Weight { get; } = weight;
    private FontSlant Slant { get; } = slant;
    private FontWidth Width { get; } = width;

    /// <summary>
    /// Attempt to create the typeface using the given settings.
    /// Returns null if this resolver does not match the requested font.
    /// </summary>
    public SKTypeface? CreateTypeface(string fontName, FontWeight weight, FontSlant slant, FontWidth width)
    {
        return (FontName == fontName) && (Weight == weight) && (Slant == slant) && (Width == width)
        ? SKTypeface.FromFile(FontPath)
        : null;
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic)
    {
        return CreateTypeface(fontName, 
            bold ? FontWeight.Bold : FontWeight.Normal,
            italic ? FontSlant.Italic : FontSlant.Upright, 
            FontWidth.Normal);
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic, FontWidth width = FontWidth.Normal)
    {
        return CreateTypeface(fontName,
            bold ? FontWeight.Bold : FontWeight.Normal,
            italic ? FontSlant.Italic : FontSlant.Upright,
            width);
    }

    public override string ToString()
    {
        return $"File font resolver: name='{FontName}', weight={Weight}, slant={Slant}, width={Width}";
    }
}
