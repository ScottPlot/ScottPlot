namespace ScottPlot;

public class PieSlice
{
    public string Label { get => LabelStyle.Text; set => LabelStyle.Text = value; }
    public double Value { get; set; }
    public FillStyle Fill { get; set; } = new();
    public Label LabelStyle { get; set; } = new() { Alignment = Alignment.MiddleCenter };
    public Color FillColor { get => Fill.Color; set => Fill.Color = value; }

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
