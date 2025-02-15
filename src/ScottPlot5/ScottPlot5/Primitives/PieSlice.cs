namespace ScottPlot;

public class PieSlice : LabelStyleProperties, IHasLegendText, IHasLabel
{
    public string Label { get => LabelStyle.Text; set => LabelStyle.Text = value; }
    public string LegendText { get; set; } = string.Empty;
    public double Value { get; set; }
    public FillStyle Fill { get; set; } = new();
    public override LabelStyle LabelStyle { get; set; } = new() { Alignment = Alignment.MiddleCenter };
    public Color FillColor { get => Fill.Color; set => Fill.Color = value; }

    public PieSlice() : this(default, default, string.Empty)
    {
    }

    public PieSlice(double value, Color color) : this(value, color, string.Empty)
    {
    }

    public PieSlice(double value, Color color, string label)
    {
        Value = value;
        Fill.Color = color;
        Label = label;
    }
}
