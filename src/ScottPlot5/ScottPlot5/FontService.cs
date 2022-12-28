using SkiaSharp;

namespace ScottPlot;

/// <summary>
/// This class provides cross-platform tools for working with fonts
/// </summary>
public static class FontService
{
    public static SKTypeface SansTypeface { get; private set; } = GetDefaultSansFont();
    public static SKTypeface MonospaceTypeface { get; private set; } = GetDefaultMonospaceFont();
    public static SKTypeface SerifTypeface { get; private set; } = GetDefaultSerifFont();

    public static string SansFontName => SansTypeface.FamilyName;
    public static string MonospaceFontName => MonospaceTypeface.FamilyName;
    public static string SerifFontName => SerifTypeface.FamilyName;

    public static string[] GetInstalledFonts()
    {
        return SKFontManager.Default.FontFamilies.ToArray();
    }

    private static SKTypeface GetDefaultSansFont()
    {
        HashSet<string> installedFonts = new(SKFontManager.Default.FontFamilies);

        // NOTE: Lato has more bold options on Linux in GitHub Actions

        string[] preferredFonts = { "Segoe UI", "Lato", "DejaVu Sans", "Helvetica" };
        foreach (string preferredFont in preferredFonts)
        {
            if (installedFonts.Contains(preferredFont))
            {
                return SKTypeface.FromFamilyName(preferredFont);
            }
        }

        return SKTypeface.Default;
    }

    private static SKTypeface GetDefaultMonospaceFont()
    {
        HashSet<string> installedFonts = new(SKFontManager.Default.FontFamilies);

        string[] preferredFonts = { "Consolas", "DejaVu Sans Mono", "Courier" };
        foreach (string preferredFont in preferredFonts)
        {
            if (installedFonts.Contains(preferredFont))
            {
                return SKTypeface.FromFamilyName(preferredFont);
            }
        }

        return SKTypeface.Default;
    }

    private static SKTypeface GetDefaultSerifFont()
    {
        HashSet<string> installedFonts = new(SKFontManager.Default.FontFamilies);

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
}
