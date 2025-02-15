namespace ScottPlot.Plottables;

public class Annotation : LabelStyleProperties, IPlottable, IHasLabel
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public override LabelStyle LabelStyle { get; set; } = new() { ShadowColor = Colors.Black.WithAlpha(.2) };

    [Obsolete("Interact properties in this class (e.g., LabelFontColor) or properties of LabelStyle")]
    public LabelStyle Label { get => LabelStyle; set => LabelStyle = value; }

    public string Text { get => LabelText; set => LabelText = value; }

    public Alignment Alignment { get; set; } = Alignment.UpperLeft;

    public PixelOffset PixelOffset { get; set; } = new(10, 10);

    public float OffsetX
    {
        get => PixelOffset.X;
        set => PixelOffset = PixelOffset.WithX(value);
    }

    public float OffsetY
    {
        get => PixelOffset.Y;
        set => PixelOffset = PixelOffset.WithY(value);
    }

    /// <summary>
    /// The annotation will be placed inside this fractional portion of the data area
    /// according to <see cref="Alignment"/> and <see cref="PixelOffset"/>
    /// </summary>
    public FractionRect FractionRect { get; set; } = FractionRect.Full;

    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        using SKPaint paint = new();

        PixelRect rect = rp.DataRect.Fraction(FractionRect);
        Pixel px = LabelStyle.GetRenderLocation(rect, Alignment, OffsetX, OffsetY, paint);

        LabelStyle.Render(rp.Canvas, px, paint);
    }
}
