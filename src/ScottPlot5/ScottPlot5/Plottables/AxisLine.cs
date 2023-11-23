namespace ScottPlot.Plottables;

/// <summary>
/// An axis line is a straight vertical or horizontal line that spans the data area.
/// </summary>
public abstract class AxisLine : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public Label Label { get; set; } = new();
    public LineStyle LineStyle { get; set; } = new();

    public double Position { get; set; } = 0;
    public abstract PixelLine GetPixelLine(RenderPack rp);

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

    public abstract AxisLimits GetAxisLimits();

    public void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        PixelLine line = GetPixelLine(rp);
        line.Draw(rp.Canvas, paint);
    }
}