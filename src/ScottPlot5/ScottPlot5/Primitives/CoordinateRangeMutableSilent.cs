namespace ScottPlot;

public class CoordinateRangeMutableSilent : CoordinateRangeMutable
{
    public bool WasChanges = false;

    public override double Min
    {
        get => base.Min;
        set
        {
            base.Min = value;
            WasChanges = true;
        }
    }
    public override double Max
    {
        get => base.Max;
        set
        {
            base.Max = value;
            WasChanges = true;
        }
    }

    public void SetSilent(double min, double max)
    {
        base.Min = min;
        base.Max = max;
    }

    public static new CoordinateRangeMutableSilent NotSet => new(double.PositiveInfinity, double.NegativeInfinity);

    public CoordinateRangeMutableSilent(double min, double max) : base(min, max)
    {
    }
}
