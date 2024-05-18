namespace ScottPlot;

/// <summary>
/// Cross-platform tools for working with fonts
/// </summary>
public static class Fonts
{
    /* Typefaces are cached to improve performance.
     * https://github.com/ScottPlot/ScottPlot/issues/2833
     * https://github.com/ScottPlot/ScottPlot/pull/2848
     */
    private static readonly Dictionary<int, SKTypeface> TypefaceCache = new();

    /// <summary>
    /// Gets or sets the global font resolver
    /// </summary>
    private static IFontResolver? _fontResolver;
    public static IFontResolver? FontResolver
    {
        get => _fontResolver;
        set
        {
            // Cannot remove font resolver
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // Ignore multiple setting of same object
            if (ReferenceEquals(value, _fontResolver))
            {
                return;
            }

            _fontResolver = value;
        }
    }

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
    /// Returns true if the given font is present on the system
    /// </summary>
    public static bool Exists(string fontName)
    {
        return (FontResolver is { } && FontResolver.Exists(fontName)) || GetInstalledFonts().Contains(fontName);
    }

    /// <summary>
    /// Returns a cached typeface (if one exists in the cache) or
    /// a new instance to a typeface that most closely matches the requested family name and style
    /// </summary>
    public static SKTypeface GetTypeface(string fontName, bool bold, bool italic)
    {
        int hashCode = GetTypefaceHashCode(fontName, bold, italic);
        if (TypefaceCache.TryGetValue(hashCode, out var typeface))
        {
            return typeface;
        }

        if (FontResolver is null)
        {
            typeface = CreateInstalledTypeface(fontName, bold, italic);
        }
        else
        {
            typeface = FontResolver.CreateTypeface(fontName, bold, italic);
            if (typeface is null)
            {
                fontName = InstalledSansFont();
                hashCode = GetTypefaceHashCode(fontName, bold, italic);
                if (TypefaceCache.TryGetValue(hashCode, out typeface))
                {
                    return typeface;
                }
                typeface = CreateInstalledTypeface(fontName, bold, italic);
            }
        }

        //Debug.WriteLine($"{hashCode}: {fontName}, {bold}, {italic}");
        TypefaceCache.Add(hashCode, typeface);
        return typeface;
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

    /// <summary>
    /// https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode
    /// </summary>
    private static int GetTypefaceHashCode(string fontName, bool bold, bool italic)
    {
#if NET6_0_OR_GREATER
        return HashCode.Combine(fontName, bold, italic);
#else
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + fontName.GetHashCode();
            hash = hash * 23 + bold.GetHashCode();
            hash = hash * 23 + italic.GetHashCode();
            return hash;
        }
#endif
    }

    private static SKTypeface CreateInstalledTypeface(string fontName, bool bold, bool italic)
    {
        SKFontStyleWeight weight = bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal;
        SKFontStyleSlant slant = italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;
        SKFontStyleWidth width = SKFontStyleWidth.Normal;
        SKFontStyle style = new(weight, width, slant);
        return SKTypeface.FromFamilyName(fontName, style);
    }
    #endregion

    #region Font Detection

    /// <summary>
    /// Use the characters in the string to detetermine an installed system font 
    /// most likely to support this character set. 
    /// Returns the system <see cref="Default"/> font if an ideal font cannot be determined.
    /// </summary>
    public static string Detect(string text)
    {
        // TODO: Use the ScottPlot default font. Maybe give as an input parameter?
        // TODO: Replace unrenderable characters with "�"
        //         Should replace characters consisting of multiple code points, as well as missing glyphs.
        //
        // We ignore Unicode characters that are made up of multiple code points since it looks like they
        // cannot be rendered without more work, e.g. by using HarfBuzz or similar at higher levels.
        //
        // The function breaks down the input text string into "text elements" (=grapheme clusters).
        // A text element can consist of multiple Unicode code points.
        // Each code point can consist of 1 or 2 C# 'char's (=UTF-16 code units).

        string defaultFontFamily = GetDefaultFontFamily(); // Should use the ScottPlot default font instead
        List<int> standaloneCodePoints = GetStandaloneCodePoints(text);

        List<string> candidateFontNames = GetCandidateFontsForString(standaloneCodePoints);

        if (!candidateFontNames.Any())
            return string.Empty; // TODO: Signal an error somehow? Could return default font name.

        // We prefer the default font if it can render the string without missing glyphs
        if (candidateFontNames.Contains(defaultFontFamily) &&
            CountMissingGlyphs(defaultFontFamily, standaloneCodePoints) == 0)
            return defaultFontFamily;

        string bestFontName = candidateFontNames
            .Select(fontName => new { fontName, NumMissingGlyphs = CountMissingGlyphs(fontName, standaloneCodePoints) })
            .OrderBy(result => result.NumMissingGlyphs)
            .First()
            .fontName;

        return bestFontName;
    }

    public static string GetDefaultFontFamily()
    {
        using SKTypeface typeface = SKFontManager.Default.MatchCharacter(' ');

        if (typeface != null)
            return typeface.FamilyName;
        else
            return string.Empty;
    }

    public static List<string> GetCandidateFontsForString(List<int> standaloneCodePoints)
    {
        HashSet<string> candidateFontNames = []; // Using HashSet to avoid duplicates

        foreach (int standaloneCodePoint in standaloneCodePoints)
        {
            using SKTypeface? typeface = SKFontManager.Default.MatchCharacter(standaloneCodePoint);
            if (typeface != null)
                candidateFontNames.Add(typeface.FamilyName);

            var ch = char.ConvertFromUtf32(standaloneCodePoint);
            Debug.WriteLine($"Input codepoint '{standaloneCodePoint}', char '{ch}': Typeface = {typeface?.FamilyName}");
        }

        return candidateFontNames.ToList();
    }

    public static int CountMissingGlyphs(string fontName, List<int> standaloneCodePoints)
    {
        int missingGlyphCount = 0;

        using var typeface = SKTypeface.FromFamilyName(fontName);
        if (typeface != null)
        {
            foreach (int standaloneCodePoint in standaloneCodePoints)
            {
                ReadOnlySpan<int> codePoints = [standaloneCodePoint];
                if (!typeface.ContainsGlyphs(codePoints))
                    missingGlyphCount++;
            }
            var s = string.Join("", standaloneCodePoints.Select(char.ConvertFromUtf32));
            Debug.WriteLine($"Input '{s}': Font {fontName} has {missingGlyphCount} items with missing glyphs");
        }
        else
        {
            missingGlyphCount = int.MaxValue;
        }

        return missingGlyphCount;
    }

    public static List<int> GetStandaloneCodePoints(string inputText)
    {
        List<string> textElements = ConvertStringToTextElements(inputText);
        IEnumerable<List<int>> codePoints = textElements.Select(ConvertTextElementToUtf32CodePoints);
        List<int> standaloneCodePoints = GetStandaloneCodePoints(codePoints);

        return standaloneCodePoints;
    }

    public static List<string> ConvertStringToTextElements(string textString)
    {
        List<string> resultList = [];

        TextElementEnumerator chars = StringInfo.GetTextElementEnumerator(textString);
        while (chars.MoveNext())
        {
            string textElement = chars.GetTextElement();

            resultList.Add(textElement);
        }

        return resultList;
    }


    /// <summary>
    /// Take a single text element ("grapheme cluster") as input,
    /// and return one or more Unicode code points.
    /// The code points are represented with signed ints since that is idiomatic for C#,
    /// even though they are always unsigned values.
    /// </summary>
    public static List<int> ConvertTextElementToUtf32CodePoints(string textElement)
    {
        List<int> resultList = [];

        int i = 0;
        while (i < textElement.Length)
        {
            // ArgumentOutOfRangeException is possible for malformed input strings,
            // we let that propagate up to the user
            int codePoint = char.ConvertToUtf32(textElement, i);
            resultList.Add(codePoint);

            // Did we consume one or two chars?
            if (char.IsHighSurrogate(textElement, i))
                i += 2;
            else
                i += 1;
        }

        return resultList;
    }

    public static List<int> GetStandaloneCodePoints(IEnumerable<List<int>> codePointLists)
    {
        /// Take a list of code point lists as input.
        /// Each code point list must correspond to a single text element ("grapheme cluster").
        /// Filter the list to retain the code point lists made up of single code points,
        /// i.e. remove combining characters, etc.
        /// Return as a flattened list of the remaining standalone code points.

        List<int> codePoints = [];

        foreach (List<int> codePointList in codePointLists)
        {
            if (codePointList.Count == 1)
                codePoints.Add(codePointList[0]);
        }

        return codePoints;
    }

    #endregion
}
