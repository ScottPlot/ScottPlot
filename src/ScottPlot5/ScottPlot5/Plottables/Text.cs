namespace ScottPlot.Plottables;

public class Text : IPlottable
{
    public readonly Label Label = new();
    public Coordinates Location { get; set; }
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public Color Color
    {
        get => Label.ForeColor;
        set => Label.ForeColor = value;
    }

    public float Size
    {
        get => Label.FontSize;
        set => Label.FontSize = value;
    }

    public bool Bold
    {
        get => Label.Bold;
        set => Label.Bold = value;
    }

    public float Rotation
    {
        get => Label.Rotation;
        set => Label.Rotation = value;
    }

    public Text()
    {

    }

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(Location);
    }

    public void Render(RenderPack rp)
    {
        Pixel pixelLocation = Axes.GetPixel(Location);
        Label.Render(rp.Canvas, pixelLocation);
    }
}
