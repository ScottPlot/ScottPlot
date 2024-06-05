namespace ScottPlot;

public class CoordinateRangeMutableSilent(double min, double max) : CoordinateRangeMutable(min, max)
{
    public bool WasChanged = false;

    public override double Min
    {
        get => base.Min;
        set
        {
            base.Min = value;
            WasChanged = true;
        }
    }
    public override double Max
    {
        get => base.Max;
        set
        {
            base.Max = value;
            WasChanged = true;
        }
    }

    public void SetSilent(double min, double max)
    {
        base.Min = min;
        base.Max = max;
    }

    public static new CoordinateRangeMutableSilent NotSet => new(double.PositiveInfinity, double.NegativeInfinity);
}
