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

        return CreateTypefaceFromFaceName(fontName, weight, slant);
    }

    /// <summary>
    /// Faces installed on the system, keyed by every normalized name that can refer to them.
    /// Built only when a name does not match an installed family (the uncommon case).
    /// </summary>
    private static readonly Lazy<Dictionary<string, (string Family, int StyleIndex)>> FaceIndex = new(BuildFaceIndex);

    private static Dictionary<string, (string Family, int StyleIndex)> BuildFaceIndex()
    {
        Dictionary<string, (string, int)> index = [];

        foreach (string family in SKFontManager.Default.FontFamilies)
        {
            using SKFontStyleSet styleSet = SKFontManager.Default.GetFontStyles(family);
            for (int i = 0; i < styleSet.Count; i++)
            {
                using SKTypeface? face = styleSet.CreateTypeface(i);
                if (face is null)
                    continue;

                foreach (string name in FontNames.GetNames(face, family, styleSet.GetStyleName(i)))
                {
                    if (!index.ContainsKey(name))
                        index[name] = (family, i);
                }
            }
        }

        return index;
    }

    /// <summary>
    /// Create a typeface from a name that identifies a single font face
    /// (e.g., the Windows font name, PostScript name, or file name) rather than a family.
    /// The face is chosen by name, so the requested width and weight are not used to find it.
    /// If bold or italic is requested, a sibling face from the same family is used if one exists.
    /// </summary>
    private static SKTypeface? CreateTypefaceFromFaceName(string fontName, FontWeight weight, FontSlant slant)
    {
        string key = FontNames.Normalize(fontName);
        if (key.Length == 0 || !FaceIndex.Value.TryGetValue(key, out var match))
            return null;

        using SKFontStyleSet styleSet = SKFontManager.Default.GetFontStyles(match.Family);
        if (match.StyleIndex >= styleSet.Count)
            return null;

        int chosenIndex = match.StyleIndex;
        bool wantsWeight = weight != FontWeight.Normal;
        bool wantsSlant = slant != FontSlant.Upright;

        if (wantsWeight || wantsSlant)
        {
            SKFontStyle matched = styleSet[match.StyleIndex];
            int targetWeight = wantsWeight ? (int)weight : matched.Weight;
            bool matchedHasSlant = !wantsSlant || matched.Slant == slant.ToSKFontStyleSlant();
            int bestDistance = matchedHasSlant ? Math.Abs(matched.Weight - targetWeight) : int.MaxValue;

            for (int i = 0; i < styleSet.Count; i++)
            {
                SKFontStyle candidate = styleSet[i];
                if (candidate.Width != matched.Width)
                    continue;
                if (wantsSlant && candidate.Slant != slant.ToSKFontStyleSlant())
                    continue;

                int distance = Math.Abs(candidate.Weight - targetWeight);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    chosenIndex = i;
                }
            }
        }

        return styleSet.CreateTypeface(chosenIndex);
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
