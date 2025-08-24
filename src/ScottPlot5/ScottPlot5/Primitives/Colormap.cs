using ScottPlot.Colormaps;

namespace ScottPlot;

public static class Colormap
{
    public static IColormap Default { get; set; } = new MellowRainbow();

    /// <summary>
    /// Return an array containing every available colormap
    /// </summary>
    public static IColormap[] GetColormaps()
    {
        return System.Reflection.Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass)
            .Where(x => !x.IsAbstract)
            .Where(x => x.GetInterfaces().Contains(typeof(IColormap)))
            .Where(x => x.GetConstructors().Where(x => x.GetParameters().Length == 0).Any())
            .Select(x => (IColormap)Activator.CreateInstance(x)!)
            .ToArray();
    }

    public static Image GetImage(IColormap colormap, int width, int height)
    {
        using SKBitmap bmp = new(width, height);
        using SKCanvas canvas = new(bmp);

        using Paint paint = Paint.NewDisposablePaint();
        paint.IsAntialias = false;
        paint.IsStroke = true;

        for (int i = 0; i < width; i++)
        {
            paint.Color = colormap.GetColor(i / (width - 1.0));
            PixelLine line = new(i, 0, i, height);
            Drawing.DrawLine(canvas, paint, line);
        }

        using MemoryStream ms = new();
        bmp.Encode(ms, SKEncodedImageFormat.Jpeg, quality: 85);
        byte[] bytes = ms.ToArray();
        return new ScottPlot.Image(bytes);
    }

    public static IColormap FromColors(Color[] colors, bool smooth = true)
    {
        return smooth
            ? new CustomInterpolated(colors)
            : new CustomPalette(colors);
    }
}
