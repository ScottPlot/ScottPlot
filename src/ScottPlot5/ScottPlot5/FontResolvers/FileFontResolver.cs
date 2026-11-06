namespace ScottPlot.FontResolvers;

/// <summary>
/// Font resolver that creates a typeface from a specified TTF file
/// </summary>
public class FileFontResolver(string name, string path, FontWeight weight, FontSlant slant, FontSpacing width) : IFontResolver
{

    [Obsolete("Explicitly declare SkFontStyles")]
    public FileFontResolver(string name, string path, bool bold, bool italic) : this(name, path,
        bold ? FontWeight.Bold : FontWeight.Normal,
        italic ? FontSlant.Italic : FontSlant.Upright, FontSpacing.Normal)
    { }

    private string FontName { get; } = name;
    private string FontPath { get; } = File.Exists(path)
        ? Path.GetFullPath(path)
        : throw new FileNotFoundException(path);
    private FontWeight Weight { get; } = weight;
    private FontSlant Slant { get; } = slant;
    private FontSpacing Width { get; } = width;

    /// <summary>
    /// Attempt to create the typeface using the given settings.
    /// Returns null if this resolver does not match the requested font.
    /// </summary>
    public SKTypeface? CreateTypeface(string fontName, FontWeight weight, FontSlant slant, FontSpacing width)
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
            FontSpacing.Normal);
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic, FontSpacing width = FontSpacing.Normal)
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

/// <summary>
/// Font resolver that creates a typeface from a font file and matches it by any of its names
/// (file name, Windows font name, PostScript name, family name) without requiring
/// the caller to know the font's weight, slant, or width.
/// </summary>
public class NamedFileFontResolver : IFontResolver
{
    private string FontPath { get; }
    private HashSet<string> Names { get; }

    /// <summary>
    /// Names (as read from the file) that this resolver responds to
    /// </summary>
    public IReadOnlyCollection<string> NormalizedNames => Names;

    public NamedFileFontResolver(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(path);

        FontPath = Path.GetFullPath(path);

        using SKTypeface typeface = SKTypeface.FromFile(FontPath)
            ?? throw new InvalidOperationException($"Unable to load font file: {path}");

        Names = FontNames.GetNames(typeface);
        string fileName = FontNames.Normalize(Path.GetFileNameWithoutExtension(FontPath));
        if (fileName.Length > 0)
            Names.Add(fileName);
    }

    public SKTypeface? CreateTypeface(string fontName, FontWeight weight, FontSlant slant, FontSpacing spacing)
    {
        return Names.Contains(FontNames.Normalize(fontName))
            ? SKTypeface.FromFile(FontPath)
            : null;
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic)
    {
        return CreateTypeface(fontName,
            bold ? FontWeight.Bold : FontWeight.Normal,
            italic ? FontSlant.Italic : FontSlant.Upright,
            FontSpacing.Normal);
    }

    public override string ToString()
    {
        return $"Named file font resolver: path='{FontPath}'";
    }
}
