using ScottPlot.Interfaces;

namespace ScottPlot.Plottables;

/// <summary>
/// Displays 1 or more boxes all styled the same
/// </summary>
public class BoxPlot : IPlottable, IHoldLineStyle
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public string Label { get; set; } = string.Empty;
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, Boxes.First().Fill);

    public readonly List<Box> Boxes = new();

    // helper methods to quickly style all boxes with common traits
    public Color FillColor { set => Boxes.ForEach(x => x.Fill.Color = value); }
    public Color StrokeColor { set => Boxes.ForEach(x => x.Stroke.Color = value); }
    public float StrokeWidth { set => Boxes.ForEach(x => x.Stroke.Width = value); }
    public LineStyle LineStyle { get; } = new();

    public AxisLimits GetAxisLimits()
    {
        ExpandingAxisLimits limits = new();

        foreach (Box box in Boxes)
        {
            limits.Expand(box.GetAxisLimits());
        }

        return limits.AxisLimits;
    }

    public void Render(RenderPack rp)
    {
        using SKPaint paint = new();

        foreach (Box box in Boxes)
        {
            // style the box.  This would be better located outside the render loop...
            box.LineStyle.Pattern = LineStyle.Pattern;
            box.Render(rp, paint, Axes);
        }
    }
}
