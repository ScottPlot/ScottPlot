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
        float y = GetY(textSize.Height, rp.DataRect);
        float x = GetX(textSize.Width, rp.DataRect);

        using SKPaint paint = new();
        Label.Render(rp.Canvas, x, y, paint);
    }

    private float GetX(float textWidth, PixelRect rect)
    {
        return Alignment switch
        {
            Alignment.UpperLeft => rect.Left + 4 + OffsetX,
            Alignment.UpperCenter => rect.TopCenter.X - 0.5f * textWidth,
            Alignment.UpperRight => rect.Right - textWidth - 4 - OffsetX,
            Alignment.MiddleLeft => rect.Left + 4 + OffsetX,
            Alignment.MiddleCenter => rect.BottomCenter.X - 0.5f * textWidth,
            Alignment.MiddleRight => rect.Right - textWidth - 4 - OffsetX,
            Alignment.LowerLeft => rect.Left + 4 + OffsetX,
            Alignment.LowerCenter => rect.BottomCenter.X - 0.5f * textWidth,
            Alignment.LowerRight => rect.Right - textWidth - 4 - OffsetX,
            _ => throw new NotImplementedException()
        };
    }

    private float GetY(float textHeight, PixelRect rect)
    {
        return Alignment switch
        {
            Alignment.UpperLeft => rect.Top + 0.5f * textHeight + OffsetY,
            Alignment.UpperCenter => rect.Top + 0.5f * textHeight + OffsetY,
            Alignment.UpperRight => rect.Top + 0.5f * textHeight + OffsetY,
            Alignment.MiddleLeft => rect.LeftCenter.Y - 0.5f * textHeight,
            Alignment.MiddleCenter => rect.LeftCenter.Y - 0.5f * textHeight,
            Alignment.MiddleRight => rect.LeftCenter.Y - 0.5f * textHeight,
            Alignment.LowerLeft => rect.Bottom - textHeight - 4 - OffsetY,
            Alignment.LowerCenter => rect.Bottom - textHeight - 4 - OffsetY,
            Alignment.LowerRight => rect.Bottom - textHeight - 4 - OffsetY,
            _ => throw new NotImplementedException()
        };
    }
}
