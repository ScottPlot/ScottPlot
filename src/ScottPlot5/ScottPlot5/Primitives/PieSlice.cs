namespace ScottPlot;

public class PieSlice : LabelStyleProperties, IHasLegendText, IHasLabel
{
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get => LabelStyle.Text; set => LabelStyle.Text = value; }
    public double Value { get; set; }
    public FillStyle Fill { get; set; } = new();
    public override Label LabelStyle { get; set; } = new() { Alignment = Alignment.MiddleCenter };
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
        LegendText = label;
        Fill.Color = color;
    }
}
