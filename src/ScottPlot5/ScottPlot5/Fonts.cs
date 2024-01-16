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
    /// Returns the name of the installed font most likely to support this character set
    /// or null if an ideal font can be determined.
    /// </summary>
    public static string? Detect(char c)
    {
        return SKFontManager.Default.MatchCharacter(c)?.FamilyName ?? null;
    }

    /// <summary>
    /// Use the characters in the string to detetermine an installed system font 
    /// most likely to support this character set. 
    /// Returns the system <see cref="Default"/> font if an ideal font cannot be determined.
    /// </summary>
    public static string Detect(string s)
    {
        CountingCollection<string> counts = new();

        IEnumerable<string> names = s.ToCharArray()
            .Select(x => Detect(x) ?? string.Empty)
            .Where(x => !string.IsNullOrEmpty(x));

        counts.AddRange(names);

        return counts.Any() ? counts.SortedKeys.Last() : Default;
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
