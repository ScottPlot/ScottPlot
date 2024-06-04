namespace ScottPlot.Plottables;

public class Annotation : LabelStyleProperties, IPlottable, IHasLabel
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public override Label LabelStyle { get; set; } = new() { ShadowColor = Colors.Black.WithAlpha(.2) };

    [Obsolete("Interact properties in this class (e.g., LabelFontColor) or properties of LabelStyle")]
    public Label Label { get => LabelStyle; set => LabelStyle = value; }

    public string Text { get => LabelText; set => LabelText = value; }

    public Alignment Alignment { get; set; } = Alignment.UpperLeft;
    public float OffsetX { get; set; } = 10;
    public float OffsetY { get; set; } = 10;

    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        using SKPaint paint = new();

        Pixel px = LabelStyle.GetRenderLocation(rp.DataRect, Alignment, OffsetX, OffsetY, paint);

        LabelStyle.Render(rp.Canvas, px, paint);
    }
}
