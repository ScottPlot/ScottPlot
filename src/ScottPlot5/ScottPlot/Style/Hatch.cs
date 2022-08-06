namespace ScottPlot.Style;

public struct Hatch
{
    public Color Color { get; set; } = Colors.Gray;
    public float Scale { get; set; } = 1;
    public HatchPattern Pattern { get; set; } = HatchPattern.DiagnalUp;

    public Hatch()
    {
    }
}
