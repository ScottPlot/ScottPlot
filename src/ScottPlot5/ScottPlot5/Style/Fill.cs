using SkiaSharp;

namespace ScottPlot.Style;

public struct Fill
{
    public Color Color { get; set; } = Colors.CornflowerBlue;
    public Color HatchColor { get; set; } = Colors.Gray;
    public HatchPattern Pattern { get; set; } = HatchPattern.None;

    public Fill()
    {
    }

    public Fill(Color color)
    {
        Color = color;
    }

    private SKBitmap Stripes()
    {
        var bmp = new SKBitmap(2, 5);

        using var paint = new SKPaint() { Color = HatchColor.ToSKColor() };
        using var path = new SKPath();
        using var canvas = new SKCanvas(bmp);

        canvas.Clear(Color.ToSKColor());
        canvas.DrawRect(new SKRect(0, 0, 2, 2), paint);

        return bmp;
    }

    public SKShader? GetShader()
    {
        return Pattern switch
        {
            HatchPattern.HorizontalLines => HorizontalLines(),
            HatchPattern.VerticalLines=> VerticalLines(),
            HatchPattern.DiagonalUp => DiagonalUp(),
            HatchPattern.DiagonalDown => DiagonalDown(),
            HatchPattern.None => null,
            _ => throw new NotImplementedException(nameof(Pattern))
        };
    }

    private SKShader HorizontalLines() => 
        SKShader.CreateBitmap(
            Stripes(),
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat);

    private SKShader VerticalLines() =>
        SKShader.CreateBitmap(
            Stripes(),
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat,
            SKMatrix.CreateRotationDegrees(90));
    private SKShader DiagonalUp() =>
        SKShader.CreateBitmap(
            Stripes(),
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat,
            SKMatrix.CreateRotationDegrees(-45));
    
    private SKShader DiagonalDown() =>
    SKShader.CreateBitmap(
        Stripes(),
        SKShaderTileMode.Repeat,
        SKShaderTileMode.Repeat,
        SKMatrix.CreateRotationDegrees(45));
}
