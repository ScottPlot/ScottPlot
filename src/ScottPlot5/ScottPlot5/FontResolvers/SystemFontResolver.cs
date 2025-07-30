
using ScottPlot.Primitives;

namespace ScottPlot.FontResolvers;

public class SystemFontResolver : IFontResolver
{
    private static HashSet<string> GetInstalledFonts()
    {
        return new(SKFontManager.Default.FontFamilies, StringComparer.InvariantCultureIgnoreCase);
    }

    internal static string InstalledSansFont()
    {
        // Prefer the system default because it is probably the best for international users
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

    internal static string InstalledMonospaceFont()
    {
        var installedFonts = GetInstalledFonts();

        string[] preferredFonts = ["Roboto Mono", "Consolas", "DejaVu Sans Mono", "Courier"];
        foreach (string preferredFont in preferredFonts)
        {
            if (installedFonts.Contains(preferredFont))
            {
                return SKTypeface.FromFamilyName(preferredFont).FamilyName;
            }
        }

        return SKTypeface.Default.FamilyName;
    }

    internal static string InstalledSerifFont()
    {
        var installedFonts = GetInstalledFonts();

        string[] preferredFonts = ["Times New Roman", "DejaVu Serif", "Times"];
        foreach (string preferredFont in preferredFonts)
        {
            if (installedFonts.Contains(preferredFont))
            {
                return SKTypeface.FromFamilyName(preferredFont).FamilyName;
            }
        }

        return SKTypeface.Default.FamilyName;
    }

    public static string DefaultSystemFont()
    {
        return SKTypeface.Default.FamilyName;
    }

    public static SKTypeface CreateDefaultTypeface()
    {
        string fontName = DefaultSystemFont();
        SKTypeface? typeface = SKTypeface.FromFamilyName(fontName);
        return typeface is null
            ? throw new InvalidOperationException($"Unable to create typeface using the default system font ({fontName})")
            : typeface;
    }

    public SKTypeface? CreateTypeface(string fontName, FontWeight weight, FontSlant slant, FontWidth width)
    {
        if (!GetInstalledFonts().Contains(fontName))
            return null;

        SKFontStyle style = new(weight.ToSKFontStyleWeight(), width.ToSKFontStyleWidth(), slant.ToSKFontStyleSlant());
        SKTypeface? typeface = SKTypeface.FromFamilyName(fontName, style);
        return typeface;
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic)
    {
        return CreateTypeface(fontName,
            bold ? FontWeight.Bold : FontWeight.Normal,
            italic ? FontSlant.Italic : FontSlant.Upright,
            FontWidth.Normal);
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic, FontWidth width = FontWidth.Normal)
    {
        return CreateTypeface(fontName,
            bold ? FontWeight.Bold : FontWeight.Normal,
            italic ? FontSlant.Italic : FontSlant.Upright,
            width);
    }
}
