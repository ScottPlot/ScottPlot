namespace ScottPlot;

public class Label
{
    public bool IsVisible { get; set; } = true;
    public string Text { get; set; } = string.Empty;

    public Alignment Alignment { get; set; } = Alignment.UpperLeft;

    /// <summary>
    /// Rotation in degrees clockwise from 0 (horizontal text)
    /// </summary>
    public float Rotation { get; set; } = 0;

    public Color ForeColor { get; set; } = Colors.Black;

    [Obsolete("use BackgroundColor", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public Color BackColor { get; set; }
    public Color BackgroundColor { get; set; } = Colors.Transparent;

    public Color BorderColor { get; set; } = Colors.Transparent;

    public float BorderWidth { get; set; } = 1;

    public PixelRect LastRenderPixelRect { get; private set; }

    public Color ShadowColor { get; set; } = Colors.Transparent;

    public PixelOffset ShadowOffset { get; set; } = new(3, 3);

    public bool AntiAliasBackground { get; set; } = true;
    public bool AntiAliasText { get; set; } = true;

    // TODO: use a class for cached typeface management

    public bool UseCachedTypefaces = true;
    private SKTypeface? CachedTypeface = null;
    private SKTypeface Typeface
    {
        get
        {
            if (UseCachedTypefaces)
                return CachedTypeface ??= FontStyle.CreateTypeface(FontName, Bold, Italic);
            return FontStyle.CreateTypeface(FontName, Bold, Italic);
        }
    }

    private string _FontName = Fonts.Default;
    public string FontName
    {
        get => _FontName;
        set { _FontName = value; ClearCachedTypeface(); }
    }

    private float _FontSize = 12;
    public float FontSize
    {
        get => _FontSize;
        set { _FontSize = value; ClearCachedTypeface(); }
    }

    private bool _Bold = false;
    public bool Bold
    {
        get => _Bold;
        set { _Bold = value; ClearCachedTypeface(); }
    }

    /// <summary>
    /// Manually defined line height in pixels.
    /// If not defined, the default line spacing will be used (according to the typeface, size, etc.)
    /// </summary>
    public float? LineSpacing { get; set; } = null;

    public bool Italic = false;

    [Obsolete("use AntiAliasBackground and AntiAliasText", true)]
    public bool AntiAlias = true;

    public float Padding
    {
        [Obsolete("Get PixelPadding instead", true)]
        get => 0;
        set => PixelPadding = new(value);
    }

    public PixelPadding PixelPadding { get; set; } = new(0, 0, 0, 0);

    public float PointSize = 0;
    public bool PointFilled = false;
    public Color PointColor = Colors.Magenta;

    public float OffsetX = 0; // TODO: automatic padding support for arbitrary rotations
    public float OffsetY = 0; // TODO: automatic padding support for arbitrary rotations

    public float BorderRadius { get => BorderRadiusX; set { BorderRadiusX = value; BorderRadiusY = value; } }
    public float BorderRadiusX = 0;
    public float BorderRadiusY = 0;

    /// <summary>
    /// Use the characters in <see cref="Text"/> to determine an installed 
    /// system font most likely to support this character set.
    /// </summary>
    public void SetBestFont()
    {
        FontName = Fonts.Detect(Text);
    }

    public void ClearCachedTypeface()
    {
        CachedTypeface = null;
    }

    private void ApplyPointPaint(SKPaint paint)
    {
        paint.IsStroke = !PointFilled;
        paint.StrokeWidth = 1;
        paint.Color = PointColor.ToSKColor();
        paint.IsAntialias = AntiAliasBackground;
    }

    private void ApplyBorderPaint(SKPaint paint)
    {
        paint.IsStroke = true;
        paint.StrokeWidth = BorderWidth;
        paint.Color = BorderColor.ToSKColor();
        paint.IsAntialias = AntiAliasBackground;
    }

    private void ApplyShadowPaint(SKPaint paint)
    {
        paint.IsStroke = false;
        paint.Color = ShadowColor.ToSKColor();
        paint.IsAntialias = AntiAliasBackground;
    }

    private void ApplyBackgroundPaint(SKPaint paint)
    {
        paint.IsStroke = false;
        paint.Color = BackgroundColor.ToSKColor();
        paint.IsAntialias = AntiAliasBackground;
    }

    private void ApplyTextPaint(SKPaint paint)
    {
        paint.TextAlign = SKTextAlign.Left;
        paint.IsStroke = false;
        paint.Typeface = Typeface;
        paint.TextSize = FontSize;
        paint.Color = ForeColor.ToSKColor();
        paint.IsAntialias = AntiAliasText;
    }

    public void ApplyToPaint(SKPaint paint)
    {
        ApplyTextPaint(paint);
    }

    // TODO: obsolete this (require a paint)
    public void Render(SKCanvas canvas, Pixel pixel)
    {
        using SKPaint paint = new();
        Render(canvas, pixel, paint);
    }

    // TODO: obsolete this (require a paint)
    public void Render(SKCanvas canvas, float x, float y)
    {
        using SKPaint paint = new();
        Render(canvas, new Pixel(x, y), paint);
    }

    // TODO: figure which measure methods to obsolete

    // TODO: always pass paints in

    public PixelSize Measure2(string text, SKPaint paint)
    {
        string[] lines = text.Split('\n');
        ApplyToPaint(paint);
        float lineHeight = paint.GetFontMetrics(out SKFontMetrics metrics);
        float maxWidth = lines.Select(paint.MeasureText).Max();
        return new PixelSize(maxWidth, lineHeight * lines.Length);
    }

    // TODO: obsolete all other measurement tests

    public SKFontMetrics GetFontMetrics(SKPaint paint)
    {
        paint.GetFontMetrics(out SKFontMetrics metrics);
        return metrics;
    }

    public PixelSize MeasureMultiline()
    {
        return MeasureMultiline(Text);
    }

    public PixelSize MeasureMultiline(string text)
    {
        using SKPaint paint = new();
        return MeasureMultiLines(paint, text);
    }

    public PixelSize Measure()
    {
        using SKPaint paint = new();
        return Measure(paint);
    }

    public PixelSize Measure(string text)
    {
        using SKPaint paint = new();

        if (Text.Contains('\n'))
            return MeasureMultiLines(paint, text);

        return MeasureText(paint, text);
    }

    public PixelSize Measure(SKPaint paint)
    {
        if (Text.Contains('\n'))
            return MeasureMultiLines(paint, Text);

        return MeasureText(paint, Text);
    }

    public PixelSize MeasureMultiLines(SKPaint paint, string text)
    {
        ApplyTextPaint(paint);
        string[] lines = text.Split('\n');
        int lineNumber = lines.Length;

        // height measure
        float height = MeasureText(paint, text).Height;
        height = (height * lineNumber) + (LineSpacing ?? paint.FontSpacing) * (lineNumber - 1);

        // width measure
        string? longestLine = lines.OrderByDescending(line => line.Length).FirstOrDefault();
        float width = MeasureText(paint, longestLine ?? lines[0]).Width;

        return new PixelSize(width, height);
    }

    public PixelSize MeasureText(SKPaint paint, string text)
    {
        ApplyTextPaint(paint);
        SKRect textBounds = new();
        ///INFO: MeasureText(string str, ref SKRect rect) works as follow:
        /// - returned value is the length of the text with leading and trailing white spaces
        /// - rect.Left contains the width of leading white spaces
        /// - rect.width contains the length of the text __without__ leading or trailing white spaces
        float fullTextWidth = paint.MeasureText(text, ref textBounds);
        return new PixelSize(fullTextWidth, textBounds.Height);
    }

    /// <summary>
    /// Use the Label's size and <see cref="Alignment"/> to determine where it should be drawn
    /// relative to the given rectangle (aligned to the rectangle according to <paramref name="rectAlignment"/>).
    /// </summary>
    public Pixel GetRenderLocation(PixelRect rect, Alignment rectAlignment, float offsetX, float offsetY)
    {
        PixelSize textSize = Measure();
        float textWidth = textSize.Width;
        float textHeight = textSize.Height;

        if (Alignment != Alignment.UpperLeft)
            throw new NotImplementedException("This method only works for labels with upper-left aligned text");

        float x = rectAlignment switch
        {
            Alignment.UpperLeft => rect.Left + offsetX,
            Alignment.UpperCenter => rect.TopCenter.X - 0.5f * textWidth,
            Alignment.UpperRight => rect.Right - textWidth - offsetX,
            Alignment.MiddleLeft => rect.Left + offsetX,
            Alignment.MiddleCenter => rect.BottomCenter.X - 0.5f * textWidth,
            Alignment.MiddleRight => rect.Right - textWidth - offsetX,
            Alignment.LowerLeft => rect.Left + offsetX,
            Alignment.LowerCenter => rect.BottomCenter.X - 0.5f * textWidth,
            Alignment.LowerRight => rect.Right - textWidth - offsetX,
            _ => throw new NotImplementedException()
        };

        float y = rectAlignment switch
        {
            Alignment.UpperLeft => rect.Top + offsetY,
            Alignment.UpperCenter => rect.Top + offsetY,
            Alignment.UpperRight => rect.Top + offsetY,
            Alignment.MiddleLeft => rect.LeftCenter.Y - 0.5f * textHeight,
            Alignment.MiddleCenter => rect.LeftCenter.Y - 0.5f * textHeight,
            Alignment.MiddleRight => rect.LeftCenter.Y - 0.5f * textHeight,
            Alignment.LowerLeft => rect.Bottom - textHeight - offsetY,
            Alignment.LowerCenter => rect.Bottom - textHeight - offsetY,
            Alignment.LowerRight => rect.Bottom - textHeight - offsetY,
            _ => throw new NotImplementedException()
        };

        return new Pixel(x, y);
    }

    public void Render(SKCanvas canvas, Pixel px, SKPaint paint)
    {
        if (!IsVisible)
            return;

        PixelSize size = Measure2(Text, paint);

        float xOffset = size.Width * Alignment.HorizontalFraction();
        float yOffset = size.Height * Alignment.VerticalFraction();
        PixelRect textRect = new(0, size.Width, size.Height, 0);
        textRect = textRect.WithDelta(-xOffset, yOffset - size.Height);

        CanvasState canvasState = new(canvas);
        canvasState.Save();

        canvas.Translate(px.X + OffsetX, px.Y + OffsetY);
        canvas.RotateDegrees(Rotation);

        DrawBackground(canvas, px, paint, textRect);
        DrawText(canvas, px, paint, textRect);
        DrawBorder(canvas, px, paint, textRect);
        DrawPoint(canvas, px, paint);

        canvasState.Restore();
    }

    private void DrawBorder(SKCanvas canvas, Pixel px, SKPaint paint, PixelRect textRect)
    {
        if (BorderWidth == 0)
            return;

        PixelRect backgroundRect = textRect.Expand(PixelPadding);
        ApplyBorderPaint(paint);
        canvas.DrawRoundRect(backgroundRect.ToSKRect(), BorderRadiusX, BorderRadiusY, paint);
    }

    private void DrawText(SKCanvas canvas, SKPaint paint, PixelRect textRect)
    {
        ApplyTextPaint(paint);
        if (Text.Contains('\n'))
        {
            string[] lines = Text.Split('\n');
            float lineHeight = LineSpacing ?? paint.FontSpacing;

            for (int i = 0; i < lines.Length; i++)
            {
                float yPx = textRect.Top + (1 + i) * lineHeight;
                float xPx = textRect.Left;
                canvas.DrawText(lines[i], xPx, yPx, paint);
            }
        }
        else
        {
            canvas.DrawText(Text, textRect.Left + OffsetX, textRect.Bottom + OffsetY, paint);
        }
    }

    private void DrawBackground(SKCanvas canvas, Pixel px, SKPaint paint, PixelRect textRect)
    {
        PixelRect backgroundRect = textRect.Expand(PixelPadding);
        PixelRect shadowRect = backgroundRect.WithOffset(ShadowOffset);

        if (ShadowColor != Colors.Transparent)
        {
            ApplyShadowPaint(paint);
            canvas.DrawRoundRect(shadowRect.ToSKRect(), BorderRadiusX, BorderRadiusY, paint);
        }

        if (BackgroundColor != Colors.Transparent)
        {
            ApplyBackgroundPaint(paint);
            canvas.DrawRoundRect(backgroundRect.ToSKRect(), BorderRadiusX, BorderRadiusY, paint);
        }

        // TODO: support rotation
        // https://github.com/ScottPlot/ScottPlot/issues/3519
        LastRenderPixelRect = backgroundRect.Expand(shadowRect).WithDelta(px.X + OffsetX, px.Y + OffsetY);
    }

    private void DrawPoint(SKCanvas canvas, Pixel px, SKPaint paint)
    {
        if (PointSize > 0)
        {
            ApplyPointPaint(paint);
            canvas.DrawCircle(0, 0, PointSize, paint);
        }
    }

    public (string text, float height) MeasureHighestString(string[] strings, SKPaint paint)
    {
        float maxHeight = 0;
        string maxText = string.Empty;

        for (int i = 0; i < strings.Length; i++)
        {
            PixelSize size = MeasureText(paint, strings[i]);
            if (size.Height > maxHeight)
            {
                maxHeight = size.Height;
                maxText = strings[i];
            }
        }

        return (maxText, maxHeight);
    }

    public (string text, PixelLength width) MeasureWidestString(string[] strings, SKPaint paint)
    {
        float maxWidth = 0;
        string maxText = string.Empty;

        for (int i = 0; i < strings.Length; i++)
        {
            PixelSize size = MeasureText(paint, strings[i]);
            if (size.Width > maxWidth)
            {
                maxWidth = size.Width;
                maxText = strings[i];
            }
        }

        return (maxText, maxWidth);
    }
}
