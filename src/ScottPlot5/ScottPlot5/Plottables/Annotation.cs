namespace ScottPlot.Plottables;

public class Annotation : StyleProperties.LabelStyleProperties, IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public override Label LabelStyle { get; } = new() { ShadowColor = Colors.Black.WithAlpha(.2) };

    [Obsolete("Interact properties in this class (e.g., LabelFontColor) or properties of LabelStyle", true)]
    public Label Label { get; set; } = null!;

    public string Text { get => LabelText; set => LabelText = value; }

    public Alignment Alignment { get; set; } = Alignment.UpperLeft;
    public float OffsetX { get; set; } = 10;
    public float OffsetY { get; set; } = 10;

    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        Pixel px = LabelStyle.GetRenderLocation(rp.DataRect, Alignment, OffsetX, OffsetY);

        using SKPaint paint = new();
        LabelStyle.Render(rp.Canvas, px, paint);
    }
}
