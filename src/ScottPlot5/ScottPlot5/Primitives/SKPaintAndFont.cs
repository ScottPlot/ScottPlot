namespace ScottPlot;

// NOTE: Code here aims to facilitate the transition between
// SkiaSharp 2 (where paints have font styles) and SkiaSharp 3 (where they are separate)
// After the project builds again, carefully review for methods which don't need both,
// and consider separate Pant and Font classes.

public class SKPaintAndFont : IDisposable
{
    // TODO: make this private and disallow instantiating this throughout the code base
    public SKPaint Paint { get; set; } = new();

    // TODO: make this private and disallow instantiating this throughout the code base
    public SKFont Font { get; set; } = new();

    public float FontSpacing { get => Font.Spacing; }
    public SKTextAlign TextAlign { get; set; } = SKTextAlign.Left;
    public bool IsStroke { get => Paint.IsStroke; set => Paint.IsStroke = value; }
    public SKTypeface Typeface { get => Font.Typeface; set => Font.Typeface = value; }
    public float TextSize { get => Font.Size; set => Font.Size = value; }
    public SKColor Color { get => Paint.Color; set => Paint.Color = value; }
    public bool IsAntialias { get => Paint.IsAntialias; set => Paint.IsAntialias = value; }
    public bool SubpixelText { get => Font.Subpixel; set => Font.Subpixel = value; }
    public SKShader? Shader { get => Paint.Shader; set => Paint.Shader = value; }
    public float StrokeWidth { get => Paint.StrokeWidth; set => Paint.StrokeWidth = value; }
    public SKPathEffect PathEffect { get => Paint.PathEffect; set => Paint.PathEffect = value; }
    public SKStrokeCap StrokeCap { get => Paint.StrokeCap; set => Paint.StrokeCap = value; }
    public SKStrokeJoin StrokeJoin { get => Paint.StrokeJoin; set => Paint.StrokeJoin = value; }
    public float StrokeMiter { get => Paint.StrokeMiter; set => Paint.StrokeMiter = value; }
    public bool FakeBoldText { get => Font.Embolden; set => Font.Embolden = value; }

    public SKFilterQuality FilterQuality { get => Paint.FilterQuality; set => Paint.FilterQuality = value; }
    public SKPaintStyle Style { get => Paint.Style; set => Paint.Style = value; }

    public float MeasureText(string s) => Font.MeasureText(s);

    public (float lineHeight, SKFontMetrics metrics) GetFontMetrics()
    {
        float lineHeight = Font.GetFontMetrics(out SKFontMetrics metrics);
        return (lineHeight, metrics);
    }

    public void Dispose()
    {
        Paint.Dispose();
        Font.Dispose();
        GC.SuppressFinalize(this);
    }
}
