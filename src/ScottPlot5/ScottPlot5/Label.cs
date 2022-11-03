using SkiaSharp;

namespace ScottPlot;

public class Label
{
    public string Text { get; set; } = string.Empty;
    public Style.Font Font { get; set; } = new()
    {
        Family = "Consolas",
        Size = 12,
    };
    public Color Color { get; set; } = Colors.Black;
    public bool AntiAlias { get; set; } = true;
    public float Rotation { get; set; } = 0;

    public SKPaint MakePaint()
    {
        return new SKPaint(MakeFont())
        {
            Color = Color.ToSKColor(),
            IsAntialias = AntiAlias,
            TextAlign = SKTextAlign.Center,
        };
    }

    public SKFont MakeFont()
    {
        return Font.GetFont();
    }

    public Label()
    {
    }

    public void Draw(SKSurface surface, Pixel point, SKPaint paint)
    {
        using SKFont font = MakeFont();
        surface.Canvas.Save();
        surface.Canvas.Translate(point.X, point.Y);
        surface.Canvas.RotateDegrees(Rotation);
        surface.Canvas.DrawText(Text, 0, font.Size, font, paint);
        surface.Canvas.Restore();
    }
}
