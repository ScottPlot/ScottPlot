namespace ScottPlot;

public class PieSlice
{
    public string? Label { get; set; }
    public double Value { get; set; }
    public FillStyle Fill { get; set; } = new();
    public Color FillColor { get => Fill.Color; set => Fill.Color = value; }

    // TODO: deprecate constructors

    public PieSlice() { }

    public PieSlice(double value, Color color)
    {
        Value = value;
        Fill.Color = color;
    }

    public PieSlice(double value, Color color, string label)
    {
        Value = value;
        Label = label;
        Fill.Color = color;
    }
}
