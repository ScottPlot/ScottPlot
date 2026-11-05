namespace ScottPlot.Hatches;

/// <summary>
/// Sparkles and faint vertical streaks in the hatch color over the background color.
/// The pattern is generated once with a fixed seed, so it looks the same every time a plot is rendered.
/// A transparent background lays the glitter over whatever is drawn beneath it.
/// </summary>
public class Glitter : IHatch
{
    private const int TileSize = 192;

    private static readonly SKBitmap Bitmap = CreateBitmap();

    private static SKBitmap CreateBitmap()
    {
        // white on black: the mask color filter maps white to the hatch color and black to the background color
        SKBitmap bmp = new(TileSize, TileSize);
        using SKCanvas canvas = new(bmp);
        using SKPaint paint = new() { IsAntialias = true };
        canvas.Clear(SKColors.Black);
        Random rand = new(0);

        // faint streaks as tall as the tile so they continue into the tiles above and below
        paint.Style = SKPaintStyle.Stroke;
        for (int i = 0; i < 24; i++)
        {
            float x = (float)rand.NextDouble() * TileSize;
            paint.StrokeWidth = 0.5f + 2.5f * (float)rand.NextDouble();
            paint.Color = SKColors.White.WithAlpha((byte)rand.Next(20, 90));
            canvas.DrawLine(x, 0, x, TileSize, paint);
        }

        // sparkles: mostly small dots, some with a soft glow, the largest with a four-pointed star
        for (int i = 0; i < 70; i++)
        {
            float x = (float)rand.NextDouble() * TileSize;
            float y = (float)rand.NextDouble() * TileSize;
            float size = 0.6f + 1.2f * (float)rand.NextDouble() + 2.4f * (float)(rand.NextDouble() * rand.NextDouble());
            bool glow = rand.NextDouble() < 0.3;
            byte alpha = (byte)rand.Next(glow ? 110 : 170, 256);

            // drawn on each side of the tile too, so a sparkle crossing an edge continues in the next tile
            for (int dx = -TileSize; dx <= TileSize; dx += TileSize)
            {
                for (int dy = -TileSize; dy <= TileSize; dy += TileSize)
                {
                    DrawSparkle(canvas, paint, x + dx, y + dy, size, glow, alpha);
                }
            }
        }

        return bmp;
    }

    private static void DrawSparkle(SKCanvas canvas, SKPaint paint, float x, float y, float size, bool glow, byte alpha)
    {
        paint.Style = SKPaintStyle.Fill;
        paint.Color = SKColors.White.WithAlpha(alpha);
        using (SKMaskFilter? blur = glow ? SKMaskFilter.CreateBlur(SKBlurStyle.Normal, size) : null)
        {
            paint.MaskFilter = blur;
            canvas.DrawCircle(x, y, glow ? 1.8f * size : size, paint);
            paint.MaskFilter = null;
        }

        if (size < 2.6f)
            return;

        paint.Style = SKPaintStyle.Stroke;
        paint.StrokeWidth = Math.Max(0.5f, 0.25f * size);
        paint.Color = SKColors.White.WithAlpha(200);
        canvas.DrawLine(x - 3 * size, y, x + 3 * size, y, paint);
        canvas.DrawLine(x, y - 3 * size, x, y + 3 * size, paint);
    }

    public SKShader GetShader(Color backgroundColor, Color hatchColor, PixelRect rect)
    {
        return SKShader.CreateBitmap(
            Bitmap,
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat)
                .WithColorFilter(Drawing.GetMaskColorFilter(hatchColor, backgroundColor));
    }
}
