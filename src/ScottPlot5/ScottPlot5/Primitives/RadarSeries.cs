namespace ScottPlot;

/// <summary>
/// Defines values and styling information for displaying a single shape on a radar chart
/// </summary>
public class RadarSeries : LabelStyleProperties, IHasLabel, IHasLegendText, IHasFill, IHasLine
{
    public IReadOnlyList<double> Values { get; set; } = [];

    public override LabelStyle LabelStyle { get; set; } = new() { Alignment = Alignment.MiddleCenter };
    public string LegendText { get => LabelStyle.Text; set => LabelStyle.Text = value; }

    public FillStyle FillStyle { get; set; } = new()
    {
        IsVisible = true,
        Color = Colors.Black.WithAlpha(.2),
    };
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }

    public LineStyle LineStyle { get; set; } = new()
    {
        IsVisible = true,
        Width = 1,
        Pattern = LinePattern.Solid,
        Color = Colors.Black,
    };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }
}
