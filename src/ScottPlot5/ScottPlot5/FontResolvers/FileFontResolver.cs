namespace ScottPlot.FontResolvers;

/// <summary>
/// Font resolver that looks for ttf files in the given folder.
/// </summary>
/// <param name="name">Name of the typeface which may include spaces and special characters</param>
/// <param name="pathBase">Path to .ttf files minus the style name and file extension (e.g., '-Regular.ttf')</param>
public class FileFontResolver(string name, string pathBase) : IFontResolver
{
    private string FontName { get; } = name;
    private string FontPath { get; } = pathBase;

    public bool Exists(string fontName)
    {
        // TODO: confirm the path exists too?
        return FontName == fontName;
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic)
    {
        if (!Exists(fontName))
        {
            return null;
        }

        string fileName = (bold, italic) switch
        {
            (true, false) => $"{FontPath}-Bold.ttf",
            (false, true) => $"{FontPath}-Italic.ttf",
            (true, true) => $"{FontPath}-BoldItalic.ttf",
            _ => $"{FontPath}-Regular.ttf"
        };

        return SKTypeface.FromFile(Path.GetFullPath(fileName));
    }
}