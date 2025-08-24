namespace ScottPlot;

// TODO: carefully control how this is new'd

// TODO: cut references to SKPaint wherever possible

// NOTE: Code here aims to facilitate the transition between
// SkiaSharp 2 (where paints have font styles) and SkiaSharp 3 (where they are separate)

public class Paint : IDisposable
{
    // options for customizing paint
    public Color Color { get => Color.FromSKColor(SKPaint.Color); set => SKPaint.Color = value.ToSKColor(); }
    public bool IsStroke { get => SKPaint.IsStroke; set => SKPaint.IsStroke = value; }
    public float StrokeWidth { get => SKPaint.StrokeWidth; set => SKPaint.StrokeWidth = value; }
    public bool IsAntialias { get => SKPaint.IsAntialias; set => SKPaint.IsAntialias = value; }
    public float StrokeMiter { get => SKPaint.StrokeMiter; set => SKPaint.StrokeMiter = value; }

    // options for customizing font
    public float TextSize { get => SKFont.Size; set => SKFont.Size = value; }
    public float FontSpacing { get => SKFont.Spacing; }
    public HorizontalAlignment TextAlign { set { SKTextAlign = value.ToSKTextAlign(); } }
    public bool SubpixelText { get => SKFont.Subpixel; set => SKFont.Subpixel = value; }
    public bool Bold { get => SKFont.Embolden; set => SKFont.Embolden = value; }

    // options for images
    public ResizeFilter ResizeFilter
    {
        set
        {
            SKSamplingOptions = value switch
            {
                ResizeFilter.NearestNeighbor => new SKSamplingOptions(SKFilterMode.Nearest, SKMipmapMode.None),
                ResizeFilter.Bilinear => new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear),
                ResizeFilter.Bicubic => new SKSamplingOptions(SKCubicResampler.Mitchell),
                _ => throw new ArgumentOutOfRangeException(nameof(value), $"Unknown filter quality: '{value}'"),
            };
        }
    }


    // NOTE: Callers really shouldn't interact with SkiaSharp primitives.
    // The exception is all the stuff that happens in the Drawing class.
    public SKPaint SKPaint { get; set; } = new();
    public SKFont SKFont { get; set; } = new();
    public SKColor SKColor { get => SKPaint.Color; set => SKPaint.Color = value; }
    public SKTextAlign SKTextAlign { get; set; }
    public SKTypeface SKTypeface { get => SKFont.Typeface; set => SKFont.Typeface = value; }
    public SKShader? SKShader { get => SKPaint.Shader; set => SKPaint.Shader = value; }
    public SKPathEffect SKPathEffect { get => SKPaint.PathEffect; set => SKPaint.PathEffect = value; }
    public SKStrokeCap SKStrokeCap { get => SKPaint.StrokeCap; set => SKPaint.StrokeCap = value; }
    public SKStrokeJoin SKStrokeJoin { get => SKPaint.StrokeJoin; set => SKPaint.StrokeJoin = value; }
    public SKSamplingOptions SKSamplingOptions { get; set; }
    public SKPaintStyle SKPaintStyle { get => SKPaint.Style; set => SKPaint.Style = value; }

    public void Dispose()
    {
        SKPaint.Dispose();
        SKFont.Dispose();
        GC.SuppressFinalize(this);
    }

    public PixelRect MeasureText(string str)
    {
        float width = SKFont.MeasureText(str, out SKRect textBounds);
        return new PixelRect(width, textBounds.Height);
    }

    public (float lineHeight, SKFontMetrics metrics) GetFontMetrics()
    {
        float lineHeight = SKFont.GetFontMetrics(out SKFontMetrics metrics);
        return (lineHeight, metrics);
    }
}
