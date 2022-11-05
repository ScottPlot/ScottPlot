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

    private SKBitmap Stripe()
    {
        var bmp = new SKBitmap(20, 50);

        using var paint = new SKPaint() { Color = HatchColor.ToSKColor() };
        using var path = new SKPath();
        using var canvas = new SKCanvas(bmp);

        canvas.Clear(Color.ToSKColor());
        canvas.DrawRect(new SKRect(0, 0, 20, 20), paint);

        return bmp;
    }

    private SKBitmap Square()
    {
        var bmp = new SKBitmap(20, 20);
        using var paint = new SKPaint() { Color = HatchColor.ToSKColor() };
        using var path = new SKPath();
        using var canvas = new SKCanvas(bmp);

        canvas.Clear(Color.ToSKColor());
        canvas.DrawRect(new SKRect(0, 0, 10, 10), paint);

        return bmp;
    }

    private SKBitmap Checker()
    {
        var bmp = new SKBitmap(20, 20);
        using var paint = new SKPaint() { Color = HatchColor.ToSKColor() };
        using var path = new SKPath();
        using var canvas = new SKCanvas(bmp);
        
        canvas.Clear(Color.ToSKColor());
        canvas.DrawRect(new SKRect(0, 0, 10, 10), paint);
        canvas.DrawRect(new SKRect(10, 10, 20, 20), paint);

        return bmp;
    }

    private SKBitmap Dot()
    {
        var bmp = new SKBitmap(20, 20);
        using var paint = new SKPaint() { Color = HatchColor.ToSKColor() };
        using var path = new SKPath();
        using var canvas = new SKCanvas(bmp);

        paint.IsAntialias = true; // AA is especially important for circles, it seems to do little for the other shapes

        canvas.Clear(Color.ToSKColor());
        canvas.DrawCircle(5, 5, 5, paint);

        return bmp;
    }

    private SKBitmap Lattice()
    {
        var bmp = new SKBitmap(20, 20);
        using var paint = new SKPaint() { 
            Color = HatchColor.ToSKColor(),
            IsStroke = true,
            StrokeWidth = 3
        };
        using var path = new SKPath();
        using var canvas = new SKCanvas(bmp);

        canvas.Clear(Color.ToSKColor());
        canvas.DrawRect(0, 0, 20, 20, paint);

        return bmp;
    }

    public SKShader? GetShader()
    {
        return Pattern switch
        {
            HatchPattern.HorizontalLines => HorizontalLines(),
            HatchPattern.VerticalLines => VerticalLines(),
            HatchPattern.DiagonalUp => DiagonalUp(),
            HatchPattern.DiagonalDown => DiagonalDown(),
            HatchPattern.Squares => Squares(),
            HatchPattern.CheckerBoard => Checkerboard(),
            HatchPattern.Dots => Dots(),
            HatchPattern.Grid => Grid(),
            HatchPattern.None => null,
            _ => throw new NotImplementedException(nameof(Pattern))
        };
    }

    private SKShader HorizontalLines() =>
        SKShader.CreateBitmap(
            Stripe(),
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat,
            SKMatrix.CreateScale(0.1f, 0.15f));

    private SKShader VerticalLines() =>
        SKShader.CreateBitmap(
            Stripe(),
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat,
            SKMatrix.CreateScale(0.1f, 0.15f)
                .PostConcat(SKMatrix.CreateRotationDegrees(90)));
    private SKShader DiagonalUp() =>
        SKShader.CreateBitmap(
            Stripe(),
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat,
            SKMatrix.CreateScale(0.1f, 0.15f)
                .PostConcat(SKMatrix.CreateRotationDegrees(-45)));

    private SKShader DiagonalDown() =>
        SKShader.CreateBitmap(
            Stripe(),
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat,
            SKMatrix.CreateScale(0.1f, 0.15f)
                .PostConcat(SKMatrix.CreateRotationDegrees(45)));

    private SKShader Squares() =>
        SKShader.CreateBitmap(
            Square(),
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat,
            SKMatrix.CreateScale(0.5f, 0.5f));

    private SKShader Checkerboard() =>
        SKShader.CreateBitmap(
            Checker(),
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat,
            SKMatrix.CreateScale(0.5f, 0.5f));

    private SKShader Dots() =>
        SKShader.CreateBitmap(
            Dot(),
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat,
            SKMatrix.CreateScale(0.5f, 0.5f));

    private SKShader Grid() =>
        SKShader.CreateBitmap(
            Lattice(),
            SKShaderTileMode.Repeat,
            SKShaderTileMode.Repeat,
            SKMatrix.CreateScale(0.5f, 0.5f));
}
