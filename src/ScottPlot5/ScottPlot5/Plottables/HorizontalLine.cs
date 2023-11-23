namespace ScottPlot.Plottables;

/// <summary>
/// A line at a defined Y position that spans the entire horizontal space of the data area
/// </summary>
public class HorizontalLine : AxisLine
{
    public double Y
    {
        get => Position;
        set => Position = value;
    }

    public override AxisLimits GetAxisLimits()
    {
        return AxisLimits.VerticalOnly(Y, Y);
    }

    public override void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        rp.DisableClipping();

        using SKPaint paint = new();

        // determine location
        float x1 = rp.DataRect.Left;
        float x2 = rp.DataRect.Right;
        float y = Axes.GetPixelY(Y);

        if (!rp.DataRect.ContainsY(y))
            return;

        // draw line
        LineStyle.ApplyToPaint(paint);
        rp.Canvas.DrawLine(x1, y, x2, y, paint);

        // draw label
        Label.Rotation = -90;
        Label.Alignment = Alignment.LowerCenter;
        Label.BackgroundColor = LineStyle.Color;
        Label.Font.Size = 14;
        Label.Font.Bold = true;
        Label.Font.Color = Colors.White;
        Label.Padding = 5;
        Label.Draw(rp.Canvas, x1, y, paint);
    }
}
