namespace ScottPlot.FontResolvers;

/// <summary>
/// Font resolver that creates a typeface from a specified TTF file
/// </summary>
public class FileFontResolver(string name, string path, bool bold, bool italic) : IFontResolver
{
    private string FontName { get; } = name;
    private string FontPath { get; } = File.Exists(path)
        ? Path.GetFullPath(path)
        : throw new FileNotFoundException(path);
    private bool Bold { get; } = bold;
    private bool Italic { get; } = italic;

    public bool Exists(string fontName)
    {
        return FontName == fontName;
    }

    /// <summary>
    /// Attempt to create the typeface using the given settings.
    /// Returns null if this resolver does not match the requested font.
    /// </summary>
    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic)
    {
        if (FontName != fontName)
            return null;

        if (Bold != bold)
            return null;

        if (Italic != bold)
            return null;

        return SKTypeface.FromFile(FontPath);
    }
}
