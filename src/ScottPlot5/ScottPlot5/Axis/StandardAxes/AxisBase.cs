namespace ScottPlot.Axis.StandardAxes;

public abstract class AxisBase
{
    public bool IsVisible { get; set; } = true;

    public abstract Edge Edge { get; }

    public virtual CoordinateRange Range { get; private set; } = CoordinateRange.NotSet;

    public double Min
    {
        get => Range.Min;
        set => Range.Min = value;
    }

    public double Max
    {
        get => Range.Max;
        set => Range.Max = value;
    }

    public ITickGenerator TickGenerator { get; set; } = null!;

    public Label Label { get; private set; } = new()
    {
        Text = string.Empty,
        Font = new() { Size = 16, Bold = true },
        Rotation = -90,
    };
    public bool ShowDebugInformation { get; set; } = false;

    public LineStyle FrameLineStyle { get; } = new();

    public FontStyle TickFont { get; set; } = new();

    public float MajorTickLength { get; set; } = 4;
    public float MajorTickWidth { get; set; } = 1;
    public Color MajorTickColor { get; set; } = Colors.Black;
    public TickStyle MajorTickStyle => new()
    {
        Length = MajorTickLength,
        Width = MajorTickWidth,
        Color = MajorTickColor
    };

    public float MinorTickLength { get; set; } = 2;
    public float MinorTickWidth { get; set; } = 1;
    public Color MinorTickColor { get; set; } = Colors.Black;
    public TickStyle MinorTickStyle => new()
    {
        Length = MinorTickLength,
        Width = MinorTickWidth,
        Color = MinorTickColor
    };
}
