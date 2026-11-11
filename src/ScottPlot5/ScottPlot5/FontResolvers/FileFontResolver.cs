namespace ScottPlot.FontResolvers;

/// <summary>
/// Font resolver that creates a typeface from a specified TTF file
/// </summary>
public class FileFontResolver(string name, string path, FontWeight weight, FontSlant slant, FontSpacing width) : IFontResolver
{

    [Obsolete("Explicitly declare SkFontStyles")]
    public FileFontResolver(string name, string path, bool bold, bool italic) : this(name, path,
        bold ? FontWeight.Bold : FontWeight.Normal,
        italic ? FontSlant.Italic : FontSlant.Upright, FontSpacing.Normal)
    { }

    private string FontName { get; } = name;
    private string FontPath { get; } = File.Exists(path)
        ? Path.GetFullPath(path)
        : throw new FileNotFoundException(path);
    private FontWeight Weight { get; } = weight;
    private FontSlant Slant { get; } = slant;
    private FontSpacing Width { get; } = width;

    /// <summary>
    /// Attempt to create the typeface using the given settings.
    /// Returns null if this resolver does not match the requested font.
    /// </summary>
    public SKTypeface? CreateTypeface(string fontName, FontWeight weight, FontSlant slant, FontSpacing width)
    {
        return (FontName == fontName) && (Weight == weight) && (Slant == slant) && (Width == width)
        ? SKTypeface.FromFile(FontPath)
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

    public override string ToString()
    {
        return $"File font resolver: name='{FontName}', weight={Weight}, slant={Slant}, width={Width}";
    }
}

/// <summary>
/// Font resolver that creates typefaces from font files and matches them by any of their names
/// (file name, Windows font name, PostScript name, family name) without requiring
/// the caller to know each font's weight, slant, or width.
/// Every face in a font collection (.ttc) is available.
/// When several files of the same family are added (e.g., regular and bold),
/// a request for the family name returns the face closest to the requested style.
/// </summary>
public class NamedFileFontResolver : IFontResolver
{
    private readonly object Lock = new();
    private readonly FontFaceIndex<(string Path, int FaceIndex)> Faces = new();
    private readonly HashSet<string> Paths = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> Names = [];
    private readonly List<string> Families = [];

    /// <summary>
    /// Names (as read from the files) that this resolver responds to
    /// </summary>
    public IReadOnlyCollection<string> NormalizedNames
    {
        get
        {
            lock (Lock)
                return Names.ToArray();
        }
    }

    /// <summary>
    /// Family names of the fonts that have been added
    /// </summary>
    public IReadOnlyCollection<string> FamilyNames
    {
        get
        {
            lock (Lock)
                return Families.ToArray();
        }
    }

    public NamedFileFontResolver()
    {
    }

    public NamedFileFontResolver(string path)
    {
        AddFile(path);
    }

    /// <summary>
    /// Make every face in the given font file available by any of its names
    /// </summary>
    public void AddFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(path);

        string fullPath = Path.GetFullPath(path);
        string fileName = Path.GetFileNameWithoutExtension(fullPath);

        lock (Lock)
        {
            if (Paths.Contains(fullPath))
                return;

            // Font collections hold several faces, so load faces until there are no more
            const int maxFaceCount = 1024;
            int faceCount = 0;
            for (int i = 0; i < maxFaceCount; i++)
            {
                using SKTypeface? typeface = SKTypeface.FromFile(fullPath, i);
                if (typeface is null)
                    break;

                HashSet<string> familyNames = FontNames.GetFamilyNames(typeface);
                HashSet<string> faceNames = FontNames.GetFaceNames(typeface);
                faceNames.Add(FontNames.Normalize(fileName));
                faceNames.Remove(string.Empty);

                using SKFontStyle style = typeface.FontStyle;
                Faces.Add((fullPath, i), familyNames, style, faceNames, familyNames);

                Names.UnionWith(familyNames);
                Names.UnionWith(faceNames);
                if (!string.IsNullOrWhiteSpace(typeface.FamilyName) && !Families.Contains(typeface.FamilyName))
                    Families.Add(typeface.FamilyName);

                faceCount++;
            }

            if (faceCount == 0)
                throw new InvalidOperationException($"Unable to load font file: {path}");

            Paths.Add(fullPath);
        }
    }

    public SKTypeface? CreateTypeface(string fontName, FontWeight weight, FontSlant slant, FontSpacing spacing)
    {
        (string Path, int FaceIndex) face;

        lock (Lock)
        {
            if (!Faces.TryFind(fontName, weight, slant, spacing, out face))
                return null;
        }

        return SKTypeface.FromFile(face.Path, face.FaceIndex);
    }

    public SKTypeface? CreateTypeface(string fontName, bool bold, bool italic)
    {
        return CreateTypeface(fontName,
            bold ? FontWeight.Bold : FontWeight.Normal,
            italic ? FontSlant.Italic : FontSlant.Upright,
            FontSpacing.Normal);
    }

    public override string ToString()
    {
        return $"Named file font resolver: families='{string.Join("', '", FamilyNames)}'";
    }
}
