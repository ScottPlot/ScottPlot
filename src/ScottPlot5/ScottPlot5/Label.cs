using SkiaSharp;

namespace ScottPlot;

public class Label
{
    public string Text { get; set; } = string.Empty;
    public float FontSize { get; set; } = 12;
    public string Font { get; set; } = "Consolas";
    public Color Color { get; set; } = Colors.Black;
    public bool Bold { get; set; } = false;
    public bool Italic { get; set; } = false;
    public bool AntiAlias { get; set; } = true;
    public float Rotation { get; set; } = 0;

    public SKPaint MakePaint()
    {
        return new SKPaint()
        {
            TextSize = FontSize,
            FakeBoldText = Bold,
            Color = Color.ToSKColor(),
            IsAntialias = AntiAlias,
            TextAlign = SKTextAlign.Center,
        };
    }

    public SKFont MakeFont()
    {
        return new SKFont(SKTypeface.FromFamilyName(Font), FontSize);
    }

    public Label()
    {
    }

    public void Draw(SKSurface surface, Pixel point, SKPaint paint)
    {
        using SKFont font = MakeFont();
        font.Embolden = Bold;
        surface.Canvas.Save();
        surface.Canvas.Translate(point.X, point.Y);
        surface.Canvas.RotateDegrees(Rotation);
        surface.Canvas.DrawText(Text, 0, FontSize, font, paint);
        surface.Canvas.Restore();
    }
}
