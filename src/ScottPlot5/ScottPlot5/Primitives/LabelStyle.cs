using SkiaSharp.HarfBuzz;

namespace ScottPlot;

[Obsolete("Label has been renamed to LabelStyle", true)]
public class Label : LabelStyle { }

public class LabelStyle
{
    /// <summary>
    /// Set this to globally enable support for right-to-left (RTL) languages
    /// </summary>
    public static bool RTLSupport { get; set; } = false;
    public bool IsVisible { get; set; } = true;

    // TODO: deprecate this and pass text into the render method
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
    public bool SubpixelText { get; set; } = true;

    public SKTypeface? Font { get; set; } = Fonts.DefaultFontStyle;
    public string FontName { get; set; } = Fonts.Default;
    public float FontSize { get; set; } = 12;
    public bool Bold { get; set; } = false;
    public bool Underline { get; set; } = false;
    private bool RenderUnderline => Underline & UnderlineWidth != 0;
    public double UnderlineWidth { get; set; } = 1;
    public double UnderlineOffset { get; set; } = 2;

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

    // TODO: should add padding and margin
    public PixelPadding PixelPadding { get; set; } = new(0, 0, 0, 0);

    public float PointSize = 0;
    public bool PointFilled = false;
    public Color PointColor = Colors.Magenta;

    public float OffsetX = 0; // TODO: automatic padding support for arbitrary rotations
    public float OffsetY = 0; // TODO: automatic padding support for arbitrary rotations

    public float BorderRadiusX = 0;
    public float BorderRadiusY = 0;
    public float BorderRadius
    {
        get => BorderRadiusX;
        set
        {
            BorderRadiusX = value;
            BorderRadiusY = value;
        }
    }

    /// <summary>
    /// If supplied, this label will be displayed as an image and its text and styling properties will be ignored
    /// </summary>
    public Image? Image { get; set; } = null;

    public static LabelStyle Default => new() { IsVisible = true, ForeColor = Colors.Black };

    /// <summary>
    /// Use the characters in <see cref="Text"/> to determine an installed
    /// system font most likely to support this character set.
    /// </summary>
    public void SetBestFont()
    {
        FontName = Fonts.Detect(Text);
    }

    private void ApplyBorderPaint(Paint paint)
    {
        paint.IsStroke = true;
        paint.StrokeWidth = BorderWidth;
        paint.Color = BorderColor;
        paint.IsAntialias = AntiAliasBackground;
        paint.SKShader = null;
    }

    private void ApplyShadowPaint(Paint paint)
    {
        paint.IsStroke = false;
        paint.Color = ShadowColor;
        paint.IsAntialias = AntiAliasBackground;
        paint.SKShader = null;
    }

    private void ApplyBackgroundPaint(Paint paint)
    {
        paint.IsStroke = false;
        paint.Color = BackgroundColor;
        paint.IsAntialias = AntiAliasBackground;
        paint.SKShader = null;
    }

    private readonly SKPathEffect SolidPathEffect = LinePattern.Solid.GetPathEffect();

    private void ApplyTextPaint(Paint paint)
    {
        paint.TextAlign = HorizontalAlignment.Left;
        paint.IsStroke = false;
        paint.SKTypeface = Font ?? Fonts.GetTypeface(FontName, Bold, Italic);
        paint.TextSize = FontSize;
        paint.Color = ForeColor;
        paint.IsAntialias = AntiAliasText;
        paint.SubpixelText = SubpixelText;
        paint.SKShader = null;
        paint.SKPathEffect = SolidPathEffect;
    }

    public void ApplyToPaint(Paint paint)
    {
        ApplyTextPaint(paint);
    }

    /// <summary>
    /// Return size information for the contents of the <see cref="Text"/> property
    /// </summary>
    public MeasuredText Measure(Paint paint) // NOTE: This should never be called internally
    {
        return Measure(Text, paint);
    }

    public MeasuredText Measure(string text, Paint paint)
    {
        if (Image is not null)
        {
            return new MeasuredText()
            {
                Size = Image.Size,
                LineHeight = Image.Height,
                LineWidths = [Image.Width],
                VerticalOffset = 0,
                Bottom = 0,
            };
        }

        string[] lines = string.IsNullOrEmpty(text) ? [] : text.Split('\n');
        ApplyToPaint(paint);
        (float lineHeight, SKFontMetrics metrics) = paint.GetFontMetrics();
        float[] lineWidths = lines.Select(x => paint.MeasureText(x).Width).ToArray();
        float maxWidth = lineWidths.Length == 0 ? 0 : lineWidths.Max();
        PixelSize size = new(maxWidth, lineHeight * lines.Length);

        // https://github.com/ScottPlot/ScottPlot/issues/3700
        float verticalOffset = metrics.Top + metrics.CapHeight - metrics.Bottom / 2;

        return new MeasuredText()
        {
            Size = size,
            LineHeight = lineHeight,
            LineWidths = lineWidths,
            VerticalOffset = verticalOffset,
            Bottom = metrics.Bottom,
        };
    }

    /// <summary>
    /// Use the Label's size and <see cref="Alignment"/> to determine where it should be drawn
    /// relative to the given rectangle (aligned to the rectangle according to <paramref name="rectAlignment"/>).
    /// </summary>
    public Pixel GetRenderLocation(PixelRect rect, Alignment rectAlignment, float offsetX, float offsetY, Paint paint)
    {
        PixelSize textSize = Measure(Text, paint).Size;
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

    public void Render(SKCanvas canvas, Pixel px, Paint paint, string text, bool bottom = true)
    {
        Text = text;
        Render(canvas, px, paint, bottom);
    }

    // TODO: deprecate this and require a string to be passed in
    public void Render(SKCanvas canvas, Pixel px, Paint paint, bool bottom = true)
    {
        if (!IsVisible)
            return;

        if (string.IsNullOrEmpty(Text) && Image is null)
            return;

        ApplyToPaint(paint);
        MeasuredText measured = Measure(Text, paint);
        PixelRect textRect = measured.Rect(Alignment);

        CanvasState canvasState = new(canvas);
        canvasState.Save();

        canvas.Translate(px.X + OffsetX, px.Y + OffsetY);
        canvas.RotateDegrees(Rotation);

        if (Image is null)
        {
            DrawBackground(canvas, px, paint, textRect);
            DrawText(canvas, measured, paint, textRect, bottom);
            DrawBorder(canvas, px, paint, textRect);
            DrawPoint(canvas, px, paint);
        }
        else
        {
            Image.Render(canvas, textRect, paint, false);
        }

        canvasState.Restore();
    }

    private void DrawBorder(SKCanvas canvas, Pixel px, Paint paint, PixelRect textRect)
    {
        if (BorderWidth == 0)
            return;

        PixelRect backgroundRect = textRect.Expand(PixelPadding);
        ApplyBorderPaint(paint);
        Drawing.DrawRoundRectangle(canvas, backgroundRect, paint, BorderRadiusX, BorderRadiusY);
    }

    private void DrawText(SKCanvas canvas, MeasuredText measured, Paint paint, PixelRect textRect, bool bottom)
    {
        if (Text is null)
            return;

        ApplyTextPaint(paint);

        float dY = bottom ? -measured.Bottom : measured.VerticalOffset;
        if (Text.Contains('\n'))
        {
            string[] lines = Text.Split('\n');
            float lineHeight = LineSpacing ?? paint.FontSpacing;

            for (int i = 0; i < lines.Length; i++)
            {
                float dX = Alignment.HorizontalFraction() switch
                {
                    0 => 0,
                    0.5f => textRect.Width / 2 - measured.LineWidths[i] / 2,
                    1 => textRect.Width - measured.LineWidths[i],
                    _ => throw new NotImplementedException(),
                };

                float xPx = textRect.Left + dX;
                float yPx = textRect.Top + (1 + i) * lineHeight + dY;
                Pixel px = new(xPx, yPx);
                if (LabelStyle.RTLSupport)
                {
                    using var shaper = new SKShaper(paint.SKTypeface);
                    float shapedWidth = shaper.Shape(lines[i], paint.SKFont).Width;
                    Drawing.DrawShapedText(canvas, shaper, lines[i], px, paint);

                    if (RenderUnderline)
                    {
                        float underlineY = yPx + (float)UnderlineOffset;

                        paint.Color = paint.Color;
                        paint.StrokeWidth = (float)UnderlineWidth;
                        paint.IsStroke = true;
                        paint.IsAntialias = paint.IsAntialias;

                        PixelLine line = new(xPx, underlineY, xPx + shapedWidth, underlineY);
                        Drawing.DrawLine(canvas, paint, line);
                    }
                }
                else
                {
                    Drawing.DrawText(canvas, lines[i], px, paint);

                    if (RenderUnderline)
                    {
                        float underlineY = yPx + (float)UnderlineOffset;
                        float textWidth = paint.MeasureText(lines[i]).Width;

                        paint.Color = paint.Color;
                        paint.StrokeWidth = (float)UnderlineWidth;
                        paint.IsStroke = true;
                        paint.IsAntialias = paint.IsAntialias;

                        PixelLine line = new(xPx, underlineY, xPx + textWidth, underlineY);
                        Drawing.DrawLine(canvas, paint, line);
                    }
                }
            }
        }
        else
        {
            float xPx = textRect.Left;
            float yPx = textRect.Bottom + dY;
            Pixel px = new(xPx, yPx);
            if (LabelStyle.RTLSupport)
            {
                using var shaper = new SKShaper(paint.SKTypeface);
                float shapedWidth = shaper.Shape(Text, paint.SKFont).Width;
                Drawing.DrawShapedText(canvas, shaper, Text, px, paint);

                if (RenderUnderline)
                {
                    float underlineY = yPx + (float)UnderlineOffset;

                    paint.Color = paint.Color;
                    paint.StrokeWidth = (float)UnderlineWidth;
                    paint.IsStroke = true;
                    paint.IsAntialias = paint.IsAntialias;

                    PixelLine line = new(xPx, underlineY, xPx + shapedWidth, underlineY);
                    Drawing.DrawLine(canvas, paint, line);
                }
            }
            else
            {
                Drawing.DrawText(canvas, Text, px, paint);

                if (RenderUnderline)
                {
                    float underlineY = yPx + (float)UnderlineOffset;
                    float textWidth = paint.MeasureText(Text).Width;

                    paint.Color = paint.Color;
                    paint.StrokeWidth = (float)UnderlineWidth;
                    paint.IsStroke = true;
                    paint.IsAntialias = paint.IsAntialias;

                    PixelLine line = new(xPx, underlineY, xPx + textWidth, underlineY);
                    Drawing.DrawLine(canvas, paint, line);
                }
            }
        }
    }

    private void DrawBackground(SKCanvas canvas, Pixel px, Paint paint, PixelRect textRect)
    {
        PixelRect backgroundRect = textRect.Expand(PixelPadding);
        PixelRect shadowRect = backgroundRect.WithOffset(ShadowOffset);

        if (ShadowColor != Colors.Transparent)
        {
            ApplyShadowPaint(paint);
            Drawing.DrawRoundRectangle(canvas, shadowRect, paint, BorderRadiusX, BorderRadiusY);
        }

        if (BackgroundColor != Colors.Transparent)
        {
            ApplyBackgroundPaint(paint);
            Drawing.DrawRoundRectangle(canvas, backgroundRect, paint, BorderRadiusX, BorderRadiusY);
        }

        // TODO: support rotation
        // https://github.com/ScottPlot/ScottPlot/issues/3519
        LastRenderPixelRect = backgroundRect.Expand(shadowRect).WithDelta(px.X + OffsetX, px.Y + OffsetY);
    }

    private void DrawPoint(SKCanvas canvas, Pixel px, Paint paint)
    {
        if (PointSize > 0)
        {
            if (PointFilled)
            {
                FillStyle ls = new()
                {
                    Color = PointColor,
                    AntiAlias = AntiAliasBackground,
                };

                Drawing.FillCircle(canvas, px, PointSize, ls, paint);
            }
            else
            {
                LineStyle ls = new()
                {
                    Color = PointColor,
                    AntiAlias = AntiAliasBackground,
                };

                Drawing.DrawCircle(canvas, px, PointSize, ls, paint);
            }
        }
    }

    public (string text, float height) MeasureHighestString(string[] strings, Paint paint)
    {
        float maxHeight = 0;
        string maxText = string.Empty;

        for (int i = 0; i < strings.Length; i++)
        {
            PixelSize size = Measure(strings[i], paint).Size;
            if (size.Height > maxHeight)
            {
                maxHeight = size.Height;
                maxText = strings[i];
            }
        }

        return (maxText, maxHeight);
    }

    public (string text, PixelLength width) MeasureWidestString(string[] strings, Paint paint)
    {
        float maxWidth = 0;
        string maxText = string.Empty;

        for (int i = 0; i < strings.Length; i++)
        {
            PixelSize size = Measure(strings[i], paint).Size;
            if (size.Width > maxWidth)
            {
                maxWidth = size.Width;
                maxText = strings[i];
            }
        }

        return (maxText, maxWidth);
    }
}
