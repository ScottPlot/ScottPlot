namespace ScottPlot;

/// <summary>
/// Provides functionality that converts a requested typeface into a physical font
/// </summary>
public interface IFontResolver
{
    /// <summary>
    /// Returns true if the given font is supported by FontResolver
    /// </summary>
    bool Exists(string fontName);

    /// <summary>
    /// Returns a new instance to a typeface that most closely matches the requested family name and style
    /// </summary>
    SKTypeface CreateTypeface(string fontName, bool bold, bool italic);
}
