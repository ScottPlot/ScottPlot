namespace ScottPlot;

// NOTE: Code here aims to facilitate the transition between
// SkiaSharp 2 (where paints have font styles) and SkiaSharp 3 (where they are separate)
// After the project builds again, carefully review for methods which don't need both,
// and consider separate Pant and Font classes.

public class Paint : IDisposable
{
    // TODO: make this private and disallow instantiating this throughout the code base
    public SKPaint SKPaint { get; set; } = new();

    // TODO: make this private and disallow instantiating this throughout the code base
    public SKFont SKFont { get; set; } = new();

    public float FontSpacing { get => SKFont.Spacing; }
    public SKTextAlign TextAlign { get; set; } = SKTextAlign.Left;
    public bool IsStroke { get => SKPaint.IsStroke; set => SKPaint.IsStroke = value; }
    public SKTypeface Typeface { get => SKFont.Typeface; set => SKFont.Typeface = value; }
    public float TextSize { get => SKFont.Size; set => SKFont.Size = value; }
    public SKColor Color { get => SKPaint.Color; set => SKPaint.Color = value; }
    public bool IsAntialias { get => SKPaint.IsAntialias; set => SKPaint.IsAntialias = value; }
    public bool SubpixelText { get => SKFont.Subpixel; set => SKFont.Subpixel = value; }
    public SKShader? Shader { get => SKPaint.Shader; set => SKPaint.Shader = value; }
    public float StrokeWidth { get => SKPaint.StrokeWidth; set => SKPaint.StrokeWidth = value; }
    public SKPathEffect PathEffect { get => SKPaint.PathEffect; set => SKPaint.PathEffect = value; }
    public SKStrokeCap StrokeCap { get => SKPaint.StrokeCap; set => SKPaint.StrokeCap = value; }
    public SKStrokeJoin StrokeJoin { get => SKPaint.StrokeJoin; set => SKPaint.StrokeJoin = value; }
    public float StrokeMiter { get => SKPaint.StrokeMiter; set => SKPaint.StrokeMiter = value; }
    public bool FakeBoldText { get => SKFont.Embolden; set => SKFont.Embolden = value; }

    public SKSamplingOptions SamplingOptions { get; set; }

    public ResizeFilter ResizeFilter
    {
        set
        {
            SamplingOptions = value switch
            {
                ResizeFilter.NearestNeighbor => new SKSamplingOptions(SKFilterMode.Nearest, SKMipmapMode.None),
                ResizeFilter.Bilinear => new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear),
                ResizeFilter.Bicubic => new SKSamplingOptions(SKCubicResampler.Mitchell),
                _ => throw new ArgumentOutOfRangeException(nameof(value), $"Unknown filter quality: '{value}'"),
            };
        }
    }

    public SKPaintStyle Style { get => SKPaint.Style; set => SKPaint.Style = value; }

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

    public void Dispose()
    {
        SKPaint.Dispose();
        SKFont.Dispose();
        GC.SuppressFinalize(this);
    }
}
