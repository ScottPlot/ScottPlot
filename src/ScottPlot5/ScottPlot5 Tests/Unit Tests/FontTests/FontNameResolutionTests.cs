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
    public void Test_Font_SystemResolver_FindsFaceByFullOrPostScriptName()
    {
        // Pick any installed face whose full name is not itself a family name,
        // and make sure it can be found by that name alone.
        SystemFontResolver resolver = new();
        foreach (string family in SKFontManager.Default.FontFamilies.Take(50))
        {
            using SKFontStyleSet set = SKFontManager.Default.GetFontStyles(family);
            for (int i = 0; i < set.Count; i++)
            {
                using SKTypeface? face = set.CreateTypeface(i);
                if (face is null)
                    continue;

                string faceName = $"{family} {set.GetStyleName(i)}";
                SKTypeface? resolved = resolver.CreateTypeface(faceName, FontWeight.Normal, FontSlant.Upright, FontSpacing.Normal);
                resolved.Should().NotBeNull($"'{faceName}' should resolve");
                return;
            }
        }

        Assert.Ignore("No installed fonts to test");
    }
}
