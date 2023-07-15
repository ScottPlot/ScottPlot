namespace ScottPlot;

/// <summary>
/// Cross-platform tools for working with fonts
/// </summary>
public static class Fonts
{
    /// <summary>
    /// This font is used for almost all text rendering.
    /// </summary>
    public static string Default { get; set; } = InstalledSansFont();

    /// <summary>
    /// Name of a sans-serif font present on the system
    /// </summary>
    public static string Sans { get; set; } = InstalledSansFont();

    /// <summary>
    /// Name of a serif font present on the system
    /// </summary>
    public static string Serif { get; set; } = InstalledSerifFont();

    /// <summary>
    /// Name of a monospace font present on the system
    /// </summary>
    public static string Monospace { get; set; } = InstalledMonospaceFont();

    /// <summary>
    /// The default font on the system
    /// </summary>
    public static string System { get; } = SKTypeface.Default.FamilyName;

    /// <summary>
    /// Return the name of the font which will best display the given character.
    /// This method helps identify the best fonts for displaying Chinese, Japanese, and Korean (CJK) characters.
    /// </summary>
    public static string? Detect(char c)
    {
        // https://github.com/ScottPlot/ScottPlot/issues/2746
        var tf = SKFontManager.Default.MatchCharacter(c);

        if (tf is null)
            return null;

        return tf.FamilyName;
    }

    /// <summary>
    /// Returns true if the given font is present on the system
    /// </summary>
    public static bool Exists(string fontName)
    {
        return GetInstalledFonts().Contains(fontName);
    }

    #region PRIVATE

    private static HashSet<string> GetInstalledFonts()
    {
        return new(SKFontManager.Default.FontFamilies, StringComparer.InvariantCultureIgnoreCase);
    }

    private static string InstalledSansFont()
    {
        // Prefer the the system default because it is probably the best for international users
        // https://github.com/ScottPlot/ScottPlot/issues/2746
        string font = SKTypeface.Default.FamilyName;

        // Favor "Open Sans" over "Segoe UI" because better anti-aliasing
        var installedFonts = GetInstalledFonts();
        if (font == "Segoe UI" && installedFonts.Contains("Open Sans"))
        {
            font = "Open Sans";
        }

        return font;
    }

    private static string InstalledMonospaceFont()
    {
        var installedFonts = GetInstalledFonts();

        string[] preferredFonts = { "Roboto Mono", "Consolas", "DejaVu Sans Mono", "Courier" };
        foreach (string preferredFont in preferredFonts)
        {
            if (installedFonts.Contains(preferredFont))
            {
                return SKTypeface.FromFamilyName(preferredFont).FamilyName;
            }
        }

        return SKTypeface.Default.FamilyName;
    }

    private static string InstalledSerifFont()
    {
        var installedFonts = GetInstalledFonts();

        string[] preferredFonts = { "Times New Roman", "DejaVu Serif", "Times" };
        foreach (string preferredFont in preferredFonts)
        {
            if (installedFonts.Contains(preferredFont))
            {
                return SKTypeface.FromFamilyName(preferredFont).FamilyName;
            }
        }

        return SKTypeface.Default.FamilyName;
    }

    #endregion
}
