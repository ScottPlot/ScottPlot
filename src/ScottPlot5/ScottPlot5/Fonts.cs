namespace ScottPlot;

/// <summary>
/// Cross-platform tools for working with fonts
/// </summary>
public static class Fonts
{
    /// <summary>
    /// This font is used for almost all text rendering.
    /// </summary>
    public static string Default { get; set; } = GetSans().FamilyName;

    /// <summary>
    /// Name of a sans-serif font present on the system
    /// </summary>
    public static string Sans { get; set; } = GetSans().FamilyName;

    /// <summary>
    /// Name of a serif font present on the system
    /// </summary>
    public static string Serif { get; set; } = GetSerif().FamilyName;

    /// <summary>
    /// Name of a monospace font present on the system
    /// </summary>
    public static string Monospace { get; set; } = GetMonospace().FamilyName;

    /// <summary>
    /// Return the name of the font which will best display the given character.
    /// This method helps identify the best fonts for displaying Chinese, Japanese, and Korean (CJK) characters.
    /// </summary>
    public static string Detect(char c)
    {
        // https://github.com/ScottPlot/ScottPlot/issues/2746
        return SKFontManager.Default.MatchCharacter(c).FamilyName;
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

    private static SKTypeface GetSans()
    {
        var installedFonts = GetInstalledFonts();

        string[] preferredFonts = { "Open Sans", "Segoe UI", "Lato", "DejaVu Sans", "Helvetica" };
        foreach (string preferredFont in preferredFonts)
        {
            if (installedFonts.Contains(preferredFont))
            {
                return SKTypeface.FromFamilyName(preferredFont);
            }
        }

        return SKTypeface.Default;
    }

    private static SKTypeface GetMonospace()
    {
        var installedFonts = GetInstalledFonts();

        string[] preferredFonts = { "Roboto Mono", "Consolas", "DejaVu Sans Mono", "Courier" };
        foreach (string preferredFont in preferredFonts)
        {
            if (installedFonts.Contains(preferredFont))
            {
                return SKTypeface.FromFamilyName(preferredFont);
            }
        }

        return SKTypeface.Default;
    }

    private static SKTypeface GetSerif()
    {
        var installedFonts = GetInstalledFonts();

        string[] preferredFonts = { "Times New Roman", "DejaVu Serif", "Times" };
        foreach (string preferredFont in preferredFonts)
        {
            if (installedFonts.Contains(preferredFont))
            {
                return SKTypeface.FromFamilyName(preferredFont);
            }
        }

        return SKTypeface.Default;
    }

    #endregion
}
