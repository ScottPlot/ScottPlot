namespace ScottPlot.Plottables;

/// <summary>
/// A line at a defined X position that spans the entire vertical space of the data area
/// </summary>
public class VerticalLine : AxisLine
{
    public double X
    {
        get => Position;
        set => Position = value;
    }

    public override AxisLimits GetAxisLimits()
    {
        return AxisLimits.HorizontalOnly(X, X);
    }

    public override void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        // determine location
        float y1 = rp.DataRect.Bottom;
        float y2 = rp.DataRect.Top;
        float x = Axes.GetPixelX(X);
        if (!rp.DataRect.ContainsX(x))
            return;

        // draw line
        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);
        rp.Canvas.DrawLine(x, y1, x, y2, paint);

    }

    public override void RenderLast(RenderPack rp)
    {
        // determine location
        float x = Axes.GetPixelX(X);
        if (!rp.DataRect.ContainsX(x))
            return;

        // draw label
        rp.DisableClipping();
        using SKPaint paint = new();
        Label.Alignment = Alignment.UpperCenter;
        Label.BackgroundColor = LineStyle.Color;
        Label.Font.Size = 14;
        Label.Font.Bold = true;
        Label.Font.Color = Colors.White;
        Label.Padding = 5;
        Label.Draw(rp.Canvas, x, rp.DataRect.Bottom, paint);
    }
}
