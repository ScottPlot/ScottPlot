namespace ScottPlot.Plottables;

// TODO: make vertical and horizontal lines inherit an abstract AxisLine class
public class VerticalLine : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public Label Label { get; set; } = new();
    public LineStyle LineStyle { get; set; } = new();

    internal double X { get; set; } = 0;

    public IEnumerable<LegendItem> LegendItems
    {
        get
        {
            return LegendItem.Single(new LegendItem()
            {
                Label = Label.Text,
                Line = LineStyle,
            });
        }
    }

    public AxisLimits GetAxisLimits()
    {
        return AxisLimits.HorizontalOnly(X, X);
    }

    public void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        float y1 = rp.DataRect.Bottom;
        float y2 = rp.DataRect.Top;
        float x = Axes.GetPixelX(X);
        rp.Canvas.DrawLine(x, y1, x, y2, paint);
    }
}
