using SkiaSharp;

namespace ScottPlotCookbook.TestImages;

internal class Fonts
{
    /// <summary>
    /// This test function creates a large image demonstrating every installed font.
    /// It is only enabled when developers need to evaluate which fonts are available in the CI system.
    /// </summary>
    public void Test_CreateImageShowingEveryFont()
    {
        using SKPaint paint = new() { IsAntialias = true, TextSize = 16 };

        string[] fonts = SKFontManager.Default.FontFamilies.ToArray();
        SKTypeface[] typefaces = fonts.Select(x => SKTypeface.FromFamilyName(x)).OrderBy(x => x.FamilyName).ToArray();

        int totalHeight = (int)((fonts.Length + .5) * paint.TextSize * 1.5);
        SKBitmap bmp = new(400, totalHeight);
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColors.White);

        float offsetY = 0;
        for (int i = 0; i < typefaces.Length; i++)
        {
            offsetY += paint.TextSize * 1.5f;
            paint.Typeface = typefaces[i];
            canvas.DrawText(typefaces[i].FamilyName, 5, offsetY, paint);
        }

        SaveTestImage(bmp);
    }

    public void SaveTestImage(SKBitmap bmp, string subName = "")
    {
        var stackTrace = new System.Diagnostics.StackTrace();
        string callingMethod = stackTrace.GetFrame(1)!.GetMethod()!.Name;
        string callingClass = stackTrace.GetFrame(1)!.GetMethod()!.DeclaringType!.ToString();
        string prefix = callingClass + "." + callingMethod;
        if (subName != "")
            subName = "_" + subName;
        string fileName = prefix + subName + ".png";

        string targetFolder = Path.Combine(Cookbook.OutputFolder, "TestImages");
        Directory.CreateDirectory(targetFolder);

        string saveAs = Path.Combine(targetFolder, fileName);
        using SKFileWStream fs = new(saveAs);
        bmp.Encode(fs, SKEncodedImageFormat.Png, quality: 100);
        Console.WriteLine(saveAs);
    }
}
