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

    public SKTypeface? CreateTypeface(string fontName, FontWeight weight, FontSlant slant, FontSpacing width)
    {
        if (GetInstalledFonts().Contains(fontName))
        {
            SKFontStyle style = new(weight.ToSKFontStyleWeight(), width.ToSKFontStyleWidth(), slant.ToSKFontStyleSlant());
            SKTypeface? typeface = SKTypeface.FromFamilyName(fontName, style);
            if (typeface is not null)
                return typeface;
        }

        return CreateTypefaceFromFaceName(fontName, weight, slant, width);
    }

    /// <summary>
    /// Faces installed on the system, keyed by every normalized name that can refer to them.
    /// Built only when a name does not match an installed family (the uncommon case).
    /// </summary>
    private static readonly Lazy<FontFaceIndex<(string Family, int StyleIndex)>> FaceIndex = new(BuildFaceIndex);

    /// <summary>
    /// Index every installed face. This never throws (an exception would be cached
    /// and break every later lookup), so faces that cannot be read are skipped.
    /// </summary>
    private static FontFaceIndex<(string Family, int StyleIndex)> BuildFaceIndex()
    {
        FontFaceIndex<(string, int)> index = new();

        string[] families;
        try
        {
            families = SKFontManager.Default.FontFamilies.ToArray();
        }
        catch
        {
            return index;
        }

        foreach (string family in families)
        {
            try
            {
                using SKFontStyleSet styleSet = SKFontManager.Default.GetFontStyles(family);
                for (int i = 0; i < styleSet.Count; i++)
                {
                    try
                    {
                        using SKTypeface? face = styleSet.CreateTypeface(i);
                        if (face is null)
                            continue;

                        // The name the system gives a face is the only one sure to identify it alone:
                        // every instance of a variable font has the same name table, so names read
                        // from the table (even full names) may be shared by many faces.
                        using SKFontStyle style = styleSet[i];
                        string systemName = $"{family} {styleSet.GetStyleName(i)}";
                        index.Add((family, i), [family], style, [systemName], FontNames.GetNames(face));
                    }
                    catch
                    {
                        // skip faces that cannot be read
                    }
                }
            }
            catch
            {
                // skip families that cannot be read
            }
        }

        return index;
    }

    /// <summary>
    /// Create a typeface from a name that does not match an installed family,
    /// such as one that identifies a single font face (e.g., the Windows font name or PostScript name)
    /// or a family name the system lists differently (e.g., a localized name).
    /// If the name refers to several faces, the one closest to the requested style is used.
    /// If the name refers to a single face it is used regardless of the requested width,
    /// but if bold or italic is requested a sibling face from the same family is used if one exists.
    /// </summary>
    private static SKTypeface? CreateTypefaceFromFaceName(string fontName, FontWeight weight, FontSlant slant, FontSpacing width)
    {
        if (!FaceIndex.Value.TryFind(fontName, weight, slant, width, out (string Family, int StyleIndex) match))
            return null;

        using SKFontStyleSet styleSet = SKFontManager.Default.GetFontStyles(match.Family);
        return match.StyleIndex < styleSet.Count
            ? styleSet.CreateTypeface(match.StyleIndex)
            : null;
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic)
    {
        return CreateTypeface(fontName,
            bold ? FontWeight.Bold : FontWeight.Normal,
            italic ? FontSlant.Italic : FontSlant.Upright,
            FontSpacing.Normal);
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic, FontSpacing width = FontSpacing.Normal)
    {
        return CreateTypeface(fontName,
            bold ? FontWeight.Bold : FontWeight.Normal,
            italic ? FontSlant.Italic : FontSlant.Upright,
            width);
    }
}
