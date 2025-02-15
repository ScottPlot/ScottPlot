namespace ScottPlot.Plottables;

/// <summary>
/// This plottable contains logic to modify tick labels to display them
/// using scientific notation with a multiplier displaed as text 
/// placed just outside the corner of the data area.
/// </summary>
public class TickModifierLabel : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;
    public LabelStyle LabelStyle { get; } = new();

    /// <summary>
    /// The axis this tick modifier will modify tick labels for
    /// </summary>
    public IAxis Axis { get; }

    /// <summary>
    /// This logic determines where the text will be placed relative to the data area
    /// </summary>
    public Func<PixelRect, Pixel> GetTextPixel { get; set; }

    /// <summary>
    /// This logic determines how to display the exponent as a string
    /// </summary>
    public Func<int, string> LabelFormatter { get; set; } = (x) => $"1e{x}";

    private int Exponent = 0;

    public TickModifierLabel(IAxis axis)
    {
        Axis = axis;

        if (axis is AxisPanels.LeftAxis)
        {
            LabelStyle.Alignment = Alignment.LowerLeft;
            GetTextPixel = (PixelRect dataRect) => dataRect.TopLeft;
        }
        else if (axis is AxisPanels.BottomAxis)
        {
            LabelStyle.Alignment = Alignment.UpperRight;
            GetTextPixel = (PixelRect dataRect) => dataRect.BottomRight.WithOffset(0, 20);
        }
        else
        {
            throw new NotImplementedException($"{axis.GetType()} is not yet supported");
        }
    }

    public virtual void UpdateTickLabels()
    {
        var tickPositions = Axis.TickGenerator.Ticks.Select(x => x.Position);
        double tickSpan = tickPositions.Max() - tickPositions.Min();

        Exponent = (int)(Math.Log10(tickSpan));

        double multiplier = Math.Pow(10, Exponent);
        for (int i = 0; i < Axis.TickGenerator.Ticks.Length; i++)
        {
            if (!Axis.TickGenerator.Ticks[i].IsMajor)
                continue;

            double position = Axis.TickGenerator.Ticks[i].Position;
            double divided = position / multiplier;
            string label = $"{divided:0.00}";
            Axis.TickGenerator.Ticks[i] = Tick.Major(position, label);
        }
    }

    public virtual void Render(RenderPack rp)
    {
        Pixel px = GetTextPixel.Invoke(rp.DataRect);
        string label = LabelFormatter.Invoke(Exponent);

        rp.CanvasState.DisableClipping();
        using SKPaint paint = new();
        LabelStyle.Render(rp.Canvas, px, paint, label);
    }
}
