namespace ScottPlot.Plottables;

public class Annotation : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public Label Label { get; set; } = new();
    public Alignment Alignment { get; set; } = Alignment.UpperLeft;
    public float OffsetX { get; set; } = 5;
    public float OffsetY { get; set; } = 5;

    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        PixelSize textSize = Label.Measure();

        float _verticalOrigin = Alignment switch
        {
            Alignment.UpperLeft => rp.DataRect.Top + 0.5f * textSize.Height + OffsetY,
            Alignment.UpperCenter => rp.DataRect.Top + 0.5f * textSize.Height + OffsetY,
            Alignment.UpperRight => rp.DataRect.Top + 0.5f * textSize.Height + OffsetY,
            Alignment.MiddleLeft => rp.DataRect.LeftCenter.Y - 0.5f * textSize.Height,
            Alignment.MiddleCenter => rp.DataRect.LeftCenter.Y - 0.5f * textSize.Height,
            Alignment.MiddleRight => rp.DataRect.LeftCenter.Y - 0.5f * textSize.Height,
            Alignment.LowerLeft => rp.DataRect.Bottom - textSize.Height - 4 - OffsetY,
            Alignment.LowerCenter => rp.DataRect.Bottom - textSize.Height - 4 - OffsetY,
            Alignment.LowerRight => rp.DataRect.Bottom - textSize.Height - 4 - OffsetY,
            _ => throw new NotImplementedException()
        };

        float _horizontalOrigin = Alignment switch
        {
            Alignment.UpperLeft => rp.DataRect.Left + 4 + OffsetX,
            Alignment.UpperCenter => rp.DataRect.TopCenter.X - 0.5f * textSize.Width,
            Alignment.UpperRight => rp.DataRect.Right - textSize.Width - 4 - OffsetX,
            Alignment.MiddleLeft => rp.DataRect.Left + 4 + OffsetX,
            Alignment.MiddleCenter => rp.DataRect.BottomCenter.X - 0.5f * textSize.Width,
            Alignment.MiddleRight => rp.DataRect.Right - textSize.Width - 4 - OffsetX,
            Alignment.LowerLeft => rp.DataRect.Left + 4 + OffsetX,
            Alignment.LowerCenter => rp.DataRect.BottomCenter.X - 0.5f * textSize.Width,
            Alignment.LowerRight => rp.DataRect.Right - textSize.Width - 4 - OffsetX,
            _ => throw new NotImplementedException()
        };

        using SKPaint paint = new();
        Label.Render(
            canvas: rp.Canvas,
            x: _horizontalOrigin,
            y: _verticalOrigin,
            paint: paint);
    }
}
