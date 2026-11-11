using SkiaSharp;
using ScottPlot.FontResolvers;

namespace ScottPlotTests.FontTests;

internal class FontNameResolutionTests
{
    private static string FindDemoFont(string fileName)
    {
        string dir = AppContext.BaseDirectory;
        while (dir is not null)
        {
            string candidate = Path.Combine(dir, "ScottPlot5 Demos", "Avalonia Demo", "Assets", "Fonts", "AlumniSans", fileName);
            if (File.Exists(candidate))
                return candidate;
            dir = Path.GetDirectoryName(dir)!;
        }

        Assert.Ignore($"Test font not found: {fileName}");
        return string.Empty;
    }

    [Test]
    public void Test_Font_Missing_FallsBackWithoutThrowing()
    {
        SKTypeface typeface = Fonts.GetTypeface("This Font Does Not Exist 12345", FontWeight.Normal, FontSlant.Upright, FontSpacing.Normal);
        typeface.Should().NotBeNull();
        typeface.FamilyName.Should().Be(SystemFontResolver.DefaultSystemFont());
    }

    [Test]
    public void Test_Font_InstalledFamily_StillResolves()
    {
        string family = Fonts.Default;
        SKTypeface typeface = Fonts.GetTypeface(family, bold: true, italic: false);
        typeface.FamilyName.Should().Be(family);
    }

    [Test]
    public void Test_Font_AddFontFile_ResolvesByFileNameWithoutStyleInfo()
    {
        string path = FindDemoFont("AlumniSans-Regular.ttf");
        NamedFileFontResolver resolver = new(path);

        // file name, with different casing/punctuation, and no weight/slant/width knowledge required
        resolver.CreateTypeface("alumnisans-regular", bold: false, italic: false).Should().NotBeNull();
        resolver.CreateTypeface("Alumni Sans Regular", FontWeight.Normal, FontSlant.Upright, FontSpacing.Condensed).Should().NotBeNull();
        resolver.CreateTypeface("Some Other Font", bold: false, italic: false).Should().BeNull();
    }

    [Test]
    public void Test_Font_AddFontFile_MissingFileThrows()
    {
        Action act = () => Fonts.AddFontFile("this-file-does-not-exist.ttf");
        act.Should().Throw<FileNotFoundException>();
    }

    [Test]
    public void Test_Font_SystemResolver_FindsFaceBySystemName()
    {
        // Every installed face should be found by its family and style name (e.g., "Segoe UI Semibold"),
        // and the face returned must be that one, not just any face from the family.
        SystemFontResolver resolver = new();
        HashSet<string> families = new(SKFontManager.Default.FontFamilies, StringComparer.InvariantCultureIgnoreCase);
        int tested = 0;

        foreach (string family in families.Take(50))
        {
            using SKFontStyleSet set = SKFontManager.Default.GetFontStyles(family);
            for (int i = 0; i < set.Count; i++)
            {
                string styleName = set.GetStyleName(i);
                string faceName = $"{family} {styleName}";
                if (string.IsNullOrWhiteSpace(styleName) || families.Contains(faceName))
                    continue; // names that are also family names resolve to that family

                // faces that share this style name (normally just one)
                List<(int, int, SKFontStyleSlant)> expectedStyles = [];
                for (int j = 0; j < set.Count; j++)
                {
                    if (set.GetStyleName(j) != styleName)
                        continue;
                    using SKTypeface? face = set.CreateTypeface(j);
                    if (face is not null)
                        expectedStyles.Add((face.FontWeight, face.FontWidth, face.FontSlant));
                }

                if (expectedStyles.Count == 0)
                    continue;

                using SKTypeface? resolved = resolver.CreateTypeface(faceName, FontWeight.Normal, FontSlant.Upright, FontSpacing.Normal);
                resolved.Should().NotBeNull($"'{faceName}' should resolve");
                expectedStyles.Should().Contain((resolved!.FontWeight, resolved.FontWidth, resolved.FontSlant),
                    $"'{faceName}' should resolve to the face with that name");
                tested++;
            }
        }

        if (tested == 0)
            Assert.Ignore("No installed fonts to test");
    }

    [Test]
    public void Test_Font_NamedFile_FamilyNameSelectsFaceByStyle()
    {
        // Added bold first to be sure the first file added is not used for every style
        NamedFileFontResolver resolver = new(FindDemoFont("AlumniSans-Bold.ttf"));
        resolver.AddFile(FindDemoFont("AlumniSans-BoldItalic.ttf"));
        resolver.AddFile(FindDemoFont("AlumniSans-Italic.ttf"));
        resolver.AddFile(FindDemoFont("AlumniSans-Regular.ttf"));

        void ShouldResolve(string name, bool bold, bool italic, bool expectBold, bool expectItalic)
        {
            using SKTypeface? typeface = resolver.CreateTypeface(name, bold, italic);
            typeface.Should().NotBeNull($"'{name}' should resolve");
            string because = $"'{name}' was requested with bold={bold} and italic={italic}";
            (typeface!.FontWeight >= 600).Should().Be(expectBold, because);
            (typeface.FontSlant != SKFontStyleSlant.Upright).Should().Be(expectItalic, because);
        }

        // the family name returns the face matching the requested style
        ShouldResolve("Alumni Sans", bold: false, italic: false, expectBold: false, expectItalic: false);
        ShouldResolve("Alumni Sans", bold: true, italic: false, expectBold: true, expectItalic: false);
        ShouldResolve("Alumni Sans", bold: false, italic: true, expectBold: false, expectItalic: true);
        ShouldResolve("Alumni Sans", bold: true, italic: true, expectBold: true, expectItalic: true);

        // a name for a single face returns that face without knowing its style...
        ShouldResolve("AlumniSans-Bold", bold: false, italic: false, expectBold: true, expectItalic: false);
        ShouldResolve("AlumniSans-Italic", bold: false, italic: false, expectBold: false, expectItalic: true);

        // ...and a sibling if bold or italic is requested
        ShouldResolve("AlumniSans-Regular", bold: true, italic: false, expectBold: true, expectItalic: false);
        ShouldResolve("AlumniSans-Bold", bold: false, italic: true, expectBold: true, expectItalic: true);
    }

    [Test]
    public void Test_Font_NamedFile_AddingSameFileTwiceIsHarmless()
    {
        string path = FindDemoFont("AlumniSans-Regular.ttf");
        NamedFileFontResolver resolver = new(path);
        int nameCount = resolver.NormalizedNames.Count;

        resolver.AddFile(path);

        resolver.NormalizedNames.Should().HaveCount(nameCount);
        resolver.FamilyNames.Should().Equal("Alumni Sans");
    }

    [Test]
    public void Test_Font_NamedFile_FontCollectionExposesEveryFace()
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "msgothic.ttc");
        if (!File.Exists(path))
            Assert.Ignore("Font collection not found: msgothic.ttc");

        NamedFileFontResolver resolver = new(path);

        foreach (string name in new[] { "MS Gothic", "MS UI Gothic", "MS PGothic" })
        {
            using SKTypeface? typeface = resolver.CreateTypeface(name, bold: false, italic: false);
            typeface.Should().NotBeNull($"'{name}' is in the collection");
            typeface!.FamilyName.Should().Be(name);
        }
    }

    [Test]
    public void Test_Font_AddFontFile_FontIsFoundAfterFailedLookup()
    {
        string path = FindDemoFont("AlumniSans-Regular.ttf");
        const string name = "Alumni Sans";

        if (Fonts.FindTypeface(name) is not null)
            Assert.Ignore($"{name} is already available");

        try
        {
            // the default typeface is cached for the font that was not found
            Fonts.GetTypeface(name, bold: false, italic: false).FamilyName.Should().NotBe(name);

            Fonts.AddFontFile(path);

            Fonts.GetTypeface(name, bold: false, italic: false).FamilyName.Should().Be(name);
            Fonts.FindTypeface(name).Should().NotBeNull();

            // fonts loaded from files take priority over installed fonts
            int fileResolverIndex = Fonts.FontResolvers.FindIndex(x => x is NamedFileFontResolver);
            int systemResolverIndex = Fonts.FontResolvers.FindIndex(x => x is SystemFontResolver);
            fileResolverIndex.Should().BeLessThan(systemResolverIndex);
        }
        finally
        {
            Fonts.FontResolvers.RemoveAll(x => x is NamedFileFontResolver);
        }

        // typefaces cached from resolvers that have been removed are not used
        Fonts.FindTypeface(name).Should().BeNull();
    }

    [Test]
    public void Test_Font_FindTypeface_ReturnsNullOnlyIfFontIsMissing()
    {
        Fonts.FindTypeface("This Font Does Not Exist 12345").Should().BeNull();
        Fonts.FindTypeface(Fonts.Default).Should().NotBeNull();
    }
}
