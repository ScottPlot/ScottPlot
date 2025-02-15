namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class FontStyle
{
    public SKTypeface Typeface => Fonts.GetTypeface(Name, Bold, Italic);

    public string Name { get; set; } = Fonts.Default;
    public bool Bold { get; set; } = false;
    public bool Italic { get; set; } = false;

    // TODO: consider whether color really belongs here...
    public Color Color { get; set; } = Colors.Black;
    public float Size { get; set; } = 12;
    public bool AntiAlias { get; set; } = true;

    public override string ToString()
    {
        return $"{Name}, Size {Size}, {Color}";
    }

    [Obsolete("This method is deprecated. Use Fonts.GetTypeface(font, bold, italic) instead.", true)]
    public static SKTypeface CreateTypefaceFromName(string font, bool bold, bool italic)
    {
        throw new NotImplementedException();
    }

    [Obsolete("This method is deprecated. Use Fonts.GetTypeface(font, bold, italic) instead.", true)]
    public static SKTypeface CreateTypefaceFromFile(string path)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Use the characters in <paramref name="text"/> to determine an installed 
    /// system font most likely to support this character set.
    /// </summary>
    public void SetBestFont(string text)
    {
        Name = Fonts.Detect(text);
    }

    public FontStyle Clone()
    {
        return new FontStyle()
        {
            Name = Name,
            Bold = Bold,
            Italic = Italic,
            Color = Color,
            Size = Size,
            AntiAlias = AntiAlias,
        };
    }

    public void ApplyToPaint(SKPaint paint)
    {
        paint.Shader = null;
        paint.IsStroke = false;
        paint.Typeface = Typeface;
        paint.TextSize = Size;
        paint.Color = Color.ToSKColor();
        paint.IsAntialias = AntiAlias;
        paint.FakeBoldText = Bold;
    }

}
