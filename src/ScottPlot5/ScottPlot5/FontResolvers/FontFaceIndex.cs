namespace ScottPlot.FontResolvers;

/// <summary>
/// Font faces keyed by every normalized name that can refer to them,
/// with style-aware selection when a name is shared by several faces.
/// This type is not thread safe, so callers must synchronize access if faces are added after lookups begin.
/// </summary>
/// <typeparam name="T">Information the caller needs to create the typeface for a face</typeparam>
internal class FontFaceIndex<T>
{
    private sealed class Entry(T face, string[] families, int weight, int width, SKFontStyleSlant slant)
    {
        public T Face { get; } = face;
        public string[] Families { get; } = families;
        public int Weight { get; } = weight;
        public int Width { get; } = width;
        public SKFontStyleSlant Slant { get; } = slant;
    }

    /// <summary>
    /// Names that are meant to identify a single face (e.g., "Arial Narrow Bold")
    /// </summary>
    private readonly Dictionary<string, List<Entry>> SpecificNames = [];

    /// <summary>
    /// Names that may be shared by many faces (e.g., "Arial Narrow")
    /// </summary>
    private readonly Dictionary<string, List<Entry>> SharedNames = [];

    /// <summary>
    /// Faces grouped by family so bold and italic siblings of a face can be found
    /// </summary>
    private readonly Dictionary<string, List<Entry>> Families = [];

    /// <summary>
    /// Add a face to the index.
    /// </summary>
    /// <param name="face">Information needed to create the typeface later</param>
    /// <param name="families">Names of the families this face belongs to</param>
    /// <param name="style">Style of the face</param>
    /// <param name="specificNames">Names meant to identify this face alone. These take priority over shared names.</param>
    /// <param name="sharedNames">Names this face is expected to share with other faces</param>
    public void Add(T face, IEnumerable<string> families, SKFontStyle style, IEnumerable<string> specificNames, IEnumerable<string> sharedNames)
    {
        string[] familyKeys = NormalizeAll(families);
        Entry entry = new(face, familyKeys, style.Weight, style.Width, style.Slant);

        AddToEach(Families, familyKeys, entry);
        AddToEach(SpecificNames, NormalizeAll(specificNames), entry);
        AddToEach(SharedNames, NormalizeAll(sharedNames), entry);
    }

    private static string[] NormalizeAll(IEnumerable<string> names)
    {
        return names.Select(FontNames.Normalize).Where(x => x.Length > 0).Distinct().ToArray();
    }

    private static void AddToEach(Dictionary<string, List<Entry>> dictionary, string[] keys, Entry entry)
    {
        foreach (string key in keys)
        {
            if (!dictionary.TryGetValue(key, out List<Entry>? entries))
                dictionary[key] = entries = [];
            entries.Add(entry);
        }
    }

    /// <summary>
    /// Find the face that best matches the given name and style.
    /// The face with that name closest to the requested style is found first
    /// (e.g., "Arial Narrow" may refer to regular, bold, and italic faces).
    /// If bold or italic is requested and the name does not refer to such a face
    /// (e.g., "Segoe UI Semibold" in italic), a sibling from the same family
    /// with the same width is returned if one exists.
    /// </summary>
    public bool TryFind(string fontName, FontWeight weight, FontSlant slant, FontSpacing width, out T face)
    {
        face = default!;

        string key = FontNames.Normalize(fontName);
        if (key.Length == 0)
            return false;

        if (!SpecificNames.TryGetValue(key, out List<Entry>? named) &&
            !SharedNames.TryGetValue(key, out named))
            return false;

        Entry match = Closest(named, (int)weight, (int)width.ToSKFontStyleWidth(), slant.ToSKFontStyleSlant());

        bool wantsWeight = weight != FontWeight.Normal;
        bool wantsSlant = slant != FontSlant.Upright;
        if (wantsWeight || wantsSlant)
        {
            // The name decides the width and anything that was not explicitly requested
            List<Entry> siblings = [match];
            foreach (string family in match.Families)
            {
                if (Families.TryGetValue(family, out List<Entry>? members))
                    siblings.AddRange(members.Where(x => x.Width == match.Width));
            }

            match = Closest(siblings,
                wantsWeight ? (int)weight : match.Weight,
                match.Width,
                wantsSlant ? slant.ToSKFontStyleSlant() : match.Slant);
        }

        face = match.Face;
        return true;
    }

    /// <summary>
    /// The first face in the list with the best slant match, then the nearest width, then the nearest weight
    /// </summary>
    private static Entry Closest(List<Entry> candidates, int weight, int width, SKFontStyleSlant slant)
    {
        Entry best = candidates[0];
        (int, int, int) bestScore = Score(best);

        foreach (Entry candidate in candidates)
        {
            (int, int, int) score = Score(candidate);
            if (score.CompareTo(bestScore) < 0)
            {
                best = candidate;
                bestScore = score;
            }
        }

        return best;

        (int, int, int) Score(Entry entry) => (
            SlantDistance(entry.Slant, slant),
            Math.Abs(entry.Width - width),
            Math.Abs(entry.Weight - weight));
    }

    /// <summary>
    /// Italic and oblique are near matches for each other, but neither is close to upright
    /// </summary>
    private static int SlantDistance(SKFontStyleSlant a, SKFontStyleSlant b)
    {
        if (a == b)
            return 0;

        return a != SKFontStyleSlant.Upright && b != SKFontStyleSlant.Upright ? 1 : 2;
    }
}
