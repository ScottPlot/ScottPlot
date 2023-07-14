namespace ScottPlot;

/// <summary>
/// Cross-platform tools for working with fonts
/// </summary>
public static class FontService
{
    /// <summary>
    /// This default font is used for almost all text rendering
    /// </summary>
    public static string DefaultFontName { get; private set; } = GetDefaultSansFont().FamilyName;

    public static string SansFontName { get; private set; } = GetDefaultSansFont().FamilyName;
    public static string SerifFontName { get; private set; } = GetDefaultSerifFont().FamilyName;
    public static string MonospaceFontName { get; private set; } = GetDefaultMonospaceFont().FamilyName;

    private static HashSet<string> GetInstalledFonts()
    {
        return new(SKFontManager.Default.FontFamilies, StringComparer.InvariantCultureIgnoreCase);
    }

    public static SKTypeface GetDefaultSansFont()
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

    public static SKTypeface GetDefaultMonospaceFont()
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

    public static SKTypeface GetDefaultSerifFont()
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

    /// <summary>
    /// Return the name of the font which will best display the given character.
    /// This method helps identify the best fonts for displaying Chinese, Japanese, and Korean (CJK) characters.
    /// </summary>
    public static string DetectFont(char c)
    {
        // https://github.com/ScottPlot/ScottPlot/issues/2746
        return SKFontManager.Default.MatchCharacter(c).FamilyName;
    }

    /// <summary>
    /// Set the default font to the best one for displaying the given character.
    /// This method helps display Chinese, Japanese, and Korean (CJK) characters.
    /// </summary>
    public static void SetDefaultFont(char c)
    {
        // https://github.com/ScottPlot/ScottPlot/issues/2746
        DefaultFontName = DetectFont(c);
    }

    /// <summary>
    /// Set the default font to the best one for displaying the given character.
    /// This method helps display Chinese, Japanese, and Korean (CJK) characters.
    /// </summary>
    public static void SetDefaultFont(string fontName, bool throwIfNotFound = false)
    {
        var installedFonts = GetInstalledFonts();

        if (!installedFonts.Contains(fontName))
        {
            DefaultFontName = fontName;
            return;
        }

        if (throwIfNotFound)
        {
            throw new InvalidOperationException($"Font not found: {fontName}");
        }
    }
}
