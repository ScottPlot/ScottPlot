using System.Text.RegularExpressions;

namespace ScottPlot;

internal class SvgImage : IDisposable
{
    private bool IsDisposed = false;
    public readonly int Width;
    public readonly int Height;
    private readonly MemoryStream Stream;
    public readonly SKCanvas Canvas;

    public SvgImage(int width, int height)
    {
        Width = width;
        Height = height;
        SKRect rect = new(0, 0, width, height);
        Stream = new MemoryStream();
        Canvas = SKSvgCanvas.Create(rect, Stream);
    }

    private static int CountSubstring(string text, string substring)
    {
#if NET5_0_OR_GREATER
        int count = Regex.Matches(text, substring).Count;

#else
        var expression = new Regex(substring);
        int count = expression.Matches(text).Count;
#endif
        return count;
    }

    private static string[] SvgTags = new string[] { "g", "svg" };
    public string GetXml()
    {
        var str = Encoding.UTF8.GetString(Stream.ToArray());
        // Add possible missed closing tags
        foreach (var tag in SvgTags)
        {
            if (CountSubstring(str, $"\\<{tag}(\\s+.*)*\\>") > CountSubstring(str, $"\\</{tag}\\>"))
                str += $"</{tag}>";
        }

        return str;
    }

    public void Dispose()
    {
        if (IsDisposed)
            return;

        Canvas.Dispose();
        IsDisposed = true;

        GC.SuppressFinalize(this);
    }
}
