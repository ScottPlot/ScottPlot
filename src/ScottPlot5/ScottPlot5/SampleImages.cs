using System.Runtime.InteropServices;

namespace ScottPlot;

public class SampleImages
{
    public static Image MonaLisa()
    {
        double[,] data = SampleData.MonaLisa();
        int height = data.GetLength(0);
        int width = data.GetLength(1);
        Range range = Range.GetRange(data);

        Colormaps.Viridis colormap = new();

        uint[] argb = new uint[data.Length];
        for (int y = 0; y < height; y++)
        {
            int rowOffset = y * width;
            for (int x = 0; x < width; x++)
            {
                argb[rowOffset + x] = colormap.GetColor(data[y, x], range).ARGB;
            }
        }

        SKBitmap bmp = Drawing.BitmapFromArgbs(argb, width, height);
        return new Image(bmp);
    }

    public static Image ScottPlotLogo(int width = 256, int height = 256)
    {
        using SKSurface surface = Drawing.CreateSurface(width, height);
        using SKCanvas canvas = surface.Canvas;
        using SKPaint paint = new();
        PixelRect canvasRect = new(0, width, height, 0);

        FillStyle fillStyle = new();
        LineStyle lineStyle = new();

        // purple upper background
        fillStyle.Color = Color.FromHex("#67217a");
        Drawing.FillRectangle(canvas, canvasRect, paint, fillStyle);

        // pink lower background
        Pixel[] pointsLowerBackground =
        [
            new(0 * width, height),
            new(0 * width, .8 * height),
            new(.3 * width, .5 * height),
            new(.5 * width, .65 * height),
            new(1 * width, .16 * height),
            new(1 * width, height),
        ];

        fillStyle.Color = Color.FromHex("#9a4993");
        FillPolygon(canvas, canvasRect, paint, fillStyle, pointsLowerBackground);

        // white ziggy
        Pixel[] pointsZiggy =
        [
            new(0 * width, .7 * height),
            new(.3 * width, .4 * height),
            new(.5 * width, .55 * height),
            new(1 * width, .06 * height),
            new(1 * width, .27 * height),
            new(.5 * width, .75 * height),
            new(.3 * width, .6 * height),
            new(0 * width, .9 * height),
        ];

        fillStyle.Color = Colors.White;
        FillPolygon(canvas, canvasRect, paint, fillStyle, pointsZiggy);

        return new Image(surface);
    }

    private static void FillPolygon(SKCanvas canvas, PixelRect canvasRect, SKPaint paint, FillStyle fillStyle, Pixel[] pixels)
    {
        using SKPath path = new();
        path.MoveTo(pixels.First().ToSKPoint());
        foreach (Pixel point in pixels.Skip(1))
        {
            path.LineTo(point.ToSKPoint());
        }
        path.Close();

        fillStyle.ApplyToPaint(paint, canvasRect);
        canvas.DrawPath(path, paint);
    }

    public static Image NoiseGrayscale(int width, int height, int seed = 0)
    {
        RandomDataGenerator gen = new(seed);

        byte[,] noise = new byte[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noise[y, x] = gen.RandomByte();
            }
        }

        return new Image(noise);
    }

    public static Image NoisyText(string text, int width, int height)
    {
        PixelRect rect = new(0, width, height, 0);
        using SKSurface surface = Drawing.CreateSurface((int)rect.Width, (int)rect.Height);
        using SKCanvas canvas = surface.Canvas;
        using SKPaint paint = new();

        Image noiseImage = NoiseGrayscale(width, height);
        noiseImage.Render(canvas, rect, paint, false);

        paint.Color = Colors.LightBlue.WithAlpha(.5).ToSKColor();
        Drawing.DrawRectangle(canvas, rect, paint);
        Drawing.DrawDebugRectangle(canvas, rect);

        LabelStyle label = new()
        {
            Alignment = Alignment.MiddleCenter,
            ForeColor = Colors.Navy,
            FontSize = 36,
            Bold = true,
        };

        label.Render(canvas, rect.Center, paint, text);

        Image img = new(surface);
        return img;
    }
}
