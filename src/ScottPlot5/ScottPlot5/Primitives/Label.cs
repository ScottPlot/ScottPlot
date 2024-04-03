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

    public Color BackColor { get; set; } = Colors.Transparent;

    public Color BorderColor { get; set; } = Colors.Transparent;

    public float BorderWidth { get; set; } = 1;

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

    public float? LineSpacing { get; set; } = 0;

    public bool Italic = false;
    public bool AntiAlias = true;
    public float Padding = 0;

    public float PointSize = 0;
    public bool PointFilled = false;
    public Color PointColor = Colors.Magenta;

    public float OffsetX = 0; // TODO: automatic padding support for arbitrary rotations
    public float OffsetY = 0; // TODO: automatic padding support for arbitrary rotations

    /// <summary>
    /// Use the characters in <see cref="Text"/> to detetermine an installed 
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
        paint.IsAntialias = AntiAlias;
    }

    private void ApplyBorderPaint(SKPaint paint)
    {
        paint.IsStroke = true;
        paint.StrokeWidth = BorderWidth;
        paint.Color = BorderColor.ToSKColor();
        paint.IsAntialias = AntiAlias;
    }

    private void ApplyBackgroundPaint(SKPaint paint)
    {
        paint.IsStroke = false;
        paint.Color = BackColor.ToSKColor();
        paint.IsAntialias = AntiAlias;
    }

    private void ApplyTextPaint(SKPaint paint)
    {
        paint.TextAlign = SKTextAlign.Left;
        paint.IsStroke = false;
        paint.Typeface = Typeface;
        paint.TextSize = FontSize;
        paint.Color = ForeColor.ToSKColor();
        paint.IsAntialias = AntiAlias;
    }

    public void ApplyToPaint(SKPaint paint)
    {
        ApplyTextPaint(paint);
    }

    // TODO: obsolete this (require a paint)
    public void Render(SKCanvas canvas, Pixel pixel)
    {
        Render(canvas, pixel.X, pixel.Y);
    }

    // TODO: obsolete this (require a paint)
    public void Render(SKCanvas canvas, float x, float y)
    {
        using SKPaint paint = new();
        Render(canvas, x, y, paint);
    }

    public void Render(SKCanvas canvas, Pixel pixel, SKPaint paint)
    {
        Render(canvas, pixel.X, pixel.Y, paint);
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

    private PixelSize MeasureText(SKPaint paint, string text)
    {
        ApplyTextPaint(paint);
        SKRect textBounds = new();
        ///INFO: MeasureText(string str, ref SKRect rect) works as follow:
        /// - returned value is the length of the text with leading and trailing white spaces
        /// - rect.Left contains the width of leading white spaces
        /// - rect.width contains the length of the text __without__ leading or trailing white spaces
        var fullTextWidth = paint.MeasureText(text, ref textBounds);
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

    public void Render(SKCanvas canvas, float x, float y, SKPaint paint)
    {
        PixelSize size = Measure(paint);

        float xOffset = size.Width * Alignment.HorizontalFraction();
        float yOffset = size.Height * Alignment.VerticalFraction();
        PixelRect textRect = new(0, size.Width, size.Height, 0);
        textRect = textRect.WithDelta(-xOffset, yOffset - size.Height);
        PixelRect backgroundRect = textRect.Expand(Padding);

        canvas.Save(); // WARNING: Save() must be paired 1:1 with Restore()
        canvas.Translate(x + OffsetX, y + OffsetY); // compensate for padding
        canvas.RotateDegrees(Rotation);
        ApplyBackgroundPaint(paint);
        canvas.DrawRect(backgroundRect.ToSKRect(), paint);
        ApplyTextPaint(paint);

        if (Text.Contains('\n'))
        {
            // TODO: multiline support could be significantly improved
            string[] lines = Text.Split('\n');
            float lineHeight = MeasureText(paint, Text).Height;
            float yPosition = lineHeight;
            for (int i = 0; i < lines.Length; i++)
            {
                canvas.DrawText(lines[i], textRect.Left, yPosition, paint);
                yPosition += lineHeight + LineSpacing ?? paint.FontSpacing;
            }
        }
        else
        {
            canvas.DrawText(Text, textRect.Left, textRect.Bottom, paint);
        }

        if (BorderWidth > 0)
        {
            ApplyBorderPaint(paint);
            canvas.DrawRect(backgroundRect.ToSKRect(), paint);
        }
        canvas.Restore(); // WARNING: Save() must be paired 1:1 with Restore()

        canvas.Save(); // WARNING: Save() must be paired 1:1 with Restore()
        canvas.Translate(x, y); // do not compensate for padding
        canvas.RotateDegrees(Rotation);

        if (PointSize > 0)
        {
            ApplyPointPaint(paint);
            canvas.DrawCircle(0, 0, PointSize, paint);
        }

        canvas.DrawLine(-PointSize, 0, PointSize, 0, paint);
        canvas.DrawLine(0, -PointSize, 0, PointSize, paint);
        canvas.Restore(); // WARNING: Save() must be paired 1:1 with Restore()
    }
}
