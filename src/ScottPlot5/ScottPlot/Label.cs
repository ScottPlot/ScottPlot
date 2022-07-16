namespace ScottPlot;

public struct Label
{
    public string Text { get; set; } = string.Empty;
    public float FontSize { get; set; } = 12;
    public string Font { get; set; } = "Consolas";
    public Color Color { get; set; } = Colors.Black;
    public bool Bold { get; set; } = false;
    public bool Italic { get; set; } = false;
    public bool AntiAlias { get; set; } = true;

    public SkiaSharp.SKPaint GetPaint()
    {
        return new SkiaSharp.SKPaint()
        {
            TextSize = FontSize,
            FakeBoldText = Bold,
            Color = Color.ToSKColor(),
            IsAntialias = AntiAlias,
        };
    }

    public SkiaSharp.SKFont GetFont()
    {
        return new SkiaSharp.SKFont(SkiaSharp.SKTypeface.FromFamilyName(Font), FontSize);
    }

    public Label()
    {
    }

    public Label(string text)
    {
        Text = text;
    }

    public Label(string text, float fontSize)
    {
        Text = text;
        FontSize = fontSize;
    }
}
