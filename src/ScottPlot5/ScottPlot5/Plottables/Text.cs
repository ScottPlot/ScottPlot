namespace ScottPlot.Plottables;

public class Text : LabelStyleProperties, IPlottable
{
    public Coordinates Location { get; set; }
    public float OffsetX { get; set; }
    public float OffsetY { get; set; }

    public override Label LabelStyle { get; set; } = new() { FontSize = 14 };
    public Alignment Alignment { get => LabelAlignment; set => LabelAlignment = value; }

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    #region obsolete

    [Obsolete("Interact properties in this class (e.g., LabelFontColor) or properties of LabelStyle", true)]
    public readonly Label Label = null!;

    [Obsolete("Use LabelFontColor")]
    public Color Color { get => LabelFontColor; set => LabelFontColor = value; }

    [Obsolete("Use LabelFontColor")]
    public Color FontColor { get => LabelFontColor; set => LabelFontColor = value; }

    [Obsolete("Use LabelBackgroundColor")]
    public Color BackgroundColor { get => LabelBackgroundColor; set => LabelBackgroundColor = value; }

    [Obsolete("use LabelBackgroundColor")]
    public Color BackColor { get => LabelBackgroundColor; set => LabelBackgroundColor = value; }

    [Obsolete("use LabelBorderColor")]
    public Color BorderColor { get => LabelBorderColor; set => LabelBorderColor = value; }

    [Obsolete("use LabelBorderWidth")]
    public float BorderWidth { get => LabelBorderWidth; set => LabelBorderWidth = value; }

    [Obsolete("use LabelPadding or LabelPixelPadding")]
    public float Padding { set => LabelPadding = value; }

    [Obsolete("use LabelFontSize")]
    public float Size { get => LabelFontSize; set => LabelFontSize = value; }

    [Obsolete("use LabelFontSize")]
    public float FontSize { get => LabelFontSize; set => LabelFontSize = value; }

    [Obsolete("use LabelBold")]
    public bool Bold { get => LabelBold; set => LabelBold = value; }

    [Obsolete("use LabelRotation")]
    public float Rotation { get => LabelRotation; set => LabelRotation = value; }

    [Obsolete("use LabelFontName")]
    public string FontName { get => LabelFontName; set => LabelFontName = value; }

    [Obsolete("use LabelLineSpacing")]
    public float? LineSpacing { get => LabelLineSpacing; set => LabelLineSpacing = value; }

    #endregion

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(Location);
    }

    public virtual void Render(RenderPack rp)
    {
        Pixel pixelLocation = Axes.GetPixel(Location);
        pixelLocation.X += OffsetX;
        pixelLocation.Y += OffsetY;

        using SKPaint paint = new();
        LabelStyle.Render(rp.Canvas, pixelLocation, paint);
    }
}
