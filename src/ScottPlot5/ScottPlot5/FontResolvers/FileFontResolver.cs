namespace ScottPlot.FontResolvers;

/// <summary>
/// Font resolver that creates a typeface from a specified TTF file
/// </summary>
public class FileFontResolver(string name, string path, SKFontStyleWeight weight, SKFontStyleSlant slant, SKFontStyleWidth width) : IFontResolver
{

    [Obsolete("Explicitly declare SkFontStyles")]
    public FileFontResolver(string name, string path, bool bold, bool italic) : this(name, path,
        bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal,
        italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright, SKFontStyleWidth.Normal)
    { }

    private string FontName { get; } = name;
    private string FontPath { get; } = File.Exists(path)
        ? Path.GetFullPath(path)
        : throw new FileNotFoundException(path);
    private SKFontStyleWeight Weight { get; } = weight;
    private SKFontStyleSlant Slant { get; } = slant;
    private SKFontStyleWidth Width { get; } = width;

    /// <summary>
    /// Attempt to create the typeface using the given settings.
    /// Returns null if this resolver does not match the requested font.
    /// </summary>
    public SKTypeface? CreateTypeface(string fontName, SKFontStyleWeight weight, SKFontStyleSlant slant, SKFontStyleWidth width)
    {
        return (FontName == fontName) && (Weight == weight) && (Slant == slant) && (Width == width)
        ? SKTypeface.FromFile(FontPath)
        : null;
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic)
    {
        return CreateTypeface(fontName, 
            bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal,
            italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright, 
            SKFontStyleWidth.Normal);
    }

    public override string ToString()
    {
        return $"File font resolver: name='{FontName}', weight={Weight}, slant={Slant}, width={Width}";
    }
}
