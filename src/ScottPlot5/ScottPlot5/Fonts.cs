using ScottPlot.FontResolvers;
using System.Collections.Concurrent;

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
    private static readonly ConcurrentDictionary<(string, FontWeight, FontSlant, FontSpacing), (SKTypeface Typeface, bool Found)> TypefaceCache = [];

    /// <summary>
    /// Number of font resolvers when the typeface cache was last used.
    /// Cached typefaces (including the default typeface cached for fonts that were not found)
    /// are discarded when resolvers are added or removed so the new resolvers get a chance to respond.
    /// </summary>
    private static int CachedResolverCount = -1;

    /// <summary>
    /// Collection of font resolvers that return typefaces from font names and style information
    /// </summary>
    public static List<IFontResolver> FontResolvers { get; } = [new SystemFontResolver()];

    /// <summary>
    /// Add a font resolver that creates a typeface from a TTF file
    /// </summary>
    public static void AddFontFile(string name, string path, bool bold = false, bool italic = false)
    {
        FontWeight weight = bold ? FontWeight.Bold : FontWeight.Normal;
        FontSlant slant = italic ? FontSlant.Italic : FontSlant.Upright;
        FontSpacing width = FontSpacing.Normal;
        FontResolvers.FileFontResolver resolver = new(name, path, weight, slant, width);
        AddFileResolver(resolver);
        TypefaceCache.Clear();
    }

    /// <summary>
    /// Add a font from a file and make it available by any of its names
    /// (file name, Windows font name, PostScript name, or family name),
    /// without needing to know the font's weight, slant, or width.
    /// Every face in a font collection (.ttc) is added.
    /// When several files of the same family are added (e.g., regular and bold),
    /// requesting the family name returns the face closest to the requested style.
    /// </summary>
    public static void AddFontFile(string path)
    {
        NamedFileFontResolver? resolver = FontResolvers.OfType<NamedFileFontResolver>().FirstOrDefault();

        if (resolver is null)
        {
            AddFileResolver(new NamedFileFontResolver(path));
        }
        else
        {
            resolver.AddFile(path);
        }

        TypefaceCache.Clear();
    }

    /// <summary>
    /// Fonts loaded from files are placed ahead of the system font resolver
    /// so they are used even if a font with the same name is installed
    /// </summary>
    private static void AddFileResolver(IFontResolver resolver)
    {
        int systemResolverIndex = FontResolvers.FindIndex(x => x is SystemFontResolver);
        if (systemResolverIndex >= 0)
            FontResolvers.Insert(systemResolverIndex, resolver);
        else
            FontResolvers.Add(resolver);
    }

    public static FontWeight? DefaultWeight { get; set; }
    public static FontSlant? DefaultSlant { get; set; }
    public static FontSpacing? DefaultWidth { get; set; }
    public static SKTypeface? DefaultFontStyle { get; set; }

    /// <summary>
    /// This font is used for almost all text rendering.
    /// </summary>
    public static string Default { get; set; } = SystemFontResolver.InstalledSansFont();

    /// <summary>
    /// Name of a sans-serif font present on the system
    /// </summary>
    public static string Sans { get; set; } = SystemFontResolver.InstalledSansFont();

    /// <summary>
    /// Name of a serif font present on the system
    /// </summary>
    public static string Serif { get; set; } = SystemFontResolver.InstalledSerifFont();

    /// <summary>
    /// Name of a monospace font present on the system
    /// </summary>
    public static string Monospace { get; set; } = SystemFontResolver.InstalledMonospaceFont();

    /// <summary>
    /// Default system font name
    /// </summary>
    public static string System => SystemFontResolver.DefaultSystemFont();

    [Obsolete("To determine if a font exists, call FindTypeface() and check for null", true)]
    public static bool Exists(string fontName)
    {
        throw new NotFiniteNumberException();
    }

    [Obsolete("To determine if a font exists, call FindTypeface() and check for null", true)]
    public static bool Exists(string fontName, bool bold, bool italic)
    {
        throw new NotFiniteNumberException();
    }

    /// <summary>
    /// Returns a typeface for the requested font name and style.
    /// A cached typeface will be used if it exists,
    /// otherwise one will be created, cached, and returned.
    /// The default system typeface is returned if the font is not found.
    /// Use <see cref="FindTypeface"/> to determine whether a font can be found.
    /// </summary>
    public static SKTypeface GetTypeface(string fontName, FontWeight weight, FontSlant slant, FontSpacing spacing)
    {
        return ResolveTypeface(fontName, weight, slant, spacing).Typeface;
    }

    /// <summary>
    /// Returns a typeface for the requested font name and style,
    /// or null if the font is not installed and has not been loaded from a file.
    /// Unlike <see cref="GetTypeface(string, FontWeight, FontSlant, FontSpacing)"/>,
    /// the default system typeface is not substituted for fonts that are not found.
    /// </summary>
    public static SKTypeface? FindTypeface(string fontName,
        FontWeight weight = FontWeight.Normal,
        FontSlant slant = FontSlant.Upright,
        FontSpacing spacing = FontSpacing.Normal)
    {
        (SKTypeface typeface, bool found) = ResolveTypeface(fontName, weight, slant, spacing);
        return found ? typeface : null;
    }

    private static (SKTypeface Typeface, bool Found) ResolveTypeface(string fontName, FontWeight weight, FontSlant slant, FontSpacing spacing)
    {
        // A font that was not found earlier may be found now if resolvers were added
        int resolverCount = FontResolvers.Count;
        if (global::System.Threading.Interlocked.Exchange(ref CachedResolverCount, resolverCount) != resolverCount)
            TypefaceCache.Clear();

        var typefaceCacheKey = (fontName, weight, slant, spacing);

        if (TypefaceCache.TryGetValue(typefaceCacheKey, out (SKTypeface Typeface, bool Found) cached))
            return cached;

        foreach (IFontResolver resolver in FontResolvers)
        {
            SKTypeface? resolvedTypeface = resolver.CreateTypeface(fontName, weight, slant, spacing);
            if (resolvedTypeface is not null)
            {
                TypefaceCache.TryAdd(typefaceCacheKey, (resolvedTypeface, true));
                return (resolvedTypeface, true);
            }
        }

        SKTypeface defaultTypeface = SystemFontResolver.CreateDefaultTypeface();
        Debug.WriteLine($"Font '{fontName}' was not found; using the default font '{defaultTypeface.FamilyName}' instead. " +
            "Use the Windows font name or file name, or call Fonts.AddFontFile(path) to load it from a file.");
        TypefaceCache.TryAdd(typefaceCacheKey, (defaultTypeface, false));
        return (defaultTypeface, false);
    }

    /// <summary>
    /// Returns a typeface for the requested font name and style.
    /// A cached typeface will be used if it exists, 
    /// otherwise one will be created, cached, and returned.
    /// </summary>
    public static SKTypeface GetTypeface(string fontName, bool bold, bool italic)
    {
        FontWeight weight = bold ? FontWeight.Bold : FontWeight.Normal;
        FontSlant slant = italic ? FontSlant.Italic : FontSlant.Upright;
        FontSpacing width = FontSpacing.Normal;
        return GetTypeface(fontName, weight, slant, width);
    }

    public static SKTypeface GetTypeface(string fontName, bool bold, bool italic, FontSpacing width)
    {
        FontWeight weight = bold ? FontWeight.Bold : FontWeight.Normal;
        FontSlant slant = italic ? FontSlant.Italic : FontSlant.Upright;
        return GetTypeface(fontName, weight, slant, width);
    }

    #region Font Detection

    /// <summary>
    /// Use the characters in the string to determine an installed system font
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

        if (string.IsNullOrWhiteSpace(text))
            return Fonts.Default;

        string defaultFontFamily = GetDefaultFontFamily(); // Should use the ScottPlot default font instead
        List<int> standaloneCodePoints = GetStandaloneCodePoints(text);

        List<string> candidateFontNames = GetCandidateFontsForString(standaloneCodePoints);

        // Fonts loaded from files are unknown to the system font manager, so consider them too.
        // They follow the system fonts so an installed font is favored when both render the text equally well.
        foreach (NamedFileFontResolver fileResolver in FontResolvers.OfType<NamedFileFontResolver>())
        {
            foreach (string familyName in fileResolver.FamilyNames)
            {
                if (!candidateFontNames.Contains(familyName))
                    candidateFontNames.Add(familyName);
            }
        }

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
        }

        return candidateFontNames.ToList();
    }

    public static int CountMissingGlyphs(string fontName, List<int> standaloneCodePoints)
    {
        int missingGlyphCount = 0;

        // Resolve by family or face name so fonts grouped under another family name are not dropped.
        // A new typeface is created (rather than using the cache) so it can be disposed here.
        using SKTypeface? typeface = FontResolvers
            .Select(resolver => resolver.CreateTypeface(fontName, FontWeight.Normal, FontSlant.Upright, FontSpacing.Normal))
            .FirstOrDefault(x => x is not null);
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

    public static void Reset()
    {
        DefaultWeight = null;
        DefaultSlant = null;
        DefaultWidth = null;
        DefaultFontStyle = null;

        Default = SystemFontResolver.InstalledSansFont();
        Sans = SystemFontResolver.InstalledSansFont();
        Serif = SystemFontResolver.InstalledSerifFont();
        Monospace = SystemFontResolver.InstalledMonospaceFont();
    }
}
