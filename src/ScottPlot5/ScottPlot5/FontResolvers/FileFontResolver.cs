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

    /// <summary>
    /// Attempt to create the typeface using the given settings.
    /// Returns null if this resolver does not match the requested font.
    /// </summary>
    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic)
    {
        return (FontName == fontName) && (Bold == bold) && (Italic == italic)
            ? SKTypeface.FromFile(FontPath)
            : null;
    }

    public override string ToString()
    {
        return $"File font resolver: name='{FontName}', bold={Bold}, italic={Italic}";
    }
}
