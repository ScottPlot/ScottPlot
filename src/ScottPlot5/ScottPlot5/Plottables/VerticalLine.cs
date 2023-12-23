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

    public VerticalLine()
    {
        Label.ForeColor = Colors.White;
        Label.FontSize = 14;
        Label.Bold = true;
        Label.Padding = 5;
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
        if (Label.IsVisible == false || string.IsNullOrEmpty(Label.Text))
            return;

        // determine location
        float x = Axes.GetPixelX(X);
        if (!rp.DataRect.ContainsX(x))
            return;

        // draw label
        rp.DisableClipping();
        using SKPaint paint = new();
        Label.Alignment = Alignment.UpperCenter;
        Label.BackColor = LineStyle.Color;
        Label.Render(rp.Canvas, x, rp.DataRect.Bottom + Label.Padding, paint);
    }
}
