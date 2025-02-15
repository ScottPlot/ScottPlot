namespace ScottPlot;

/// <summary>
/// Provides functionality that converts a requested typeface into a physical font
/// </summary>
public interface IFontResolver
{
    /// <summary>
    /// Returns a new instance to a typeface that most closely matches the requested family name and style
    /// </summary>
    SKTypeface? CreateTypeface(string fontName, bool bold, bool italic);
}
