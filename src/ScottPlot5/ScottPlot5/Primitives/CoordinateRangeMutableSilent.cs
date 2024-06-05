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

    public void Set(double min, double max, bool silent = true)
    {
        base.Min = min;
        base.Max = max;

        if (silent == false)
        {
            WasChanged = true;
        }
    }

    public void Set(IAxis otherAxis, bool silent = true)
    {
        Set(otherAxis.Min, otherAxis.Max, silent);
    }

    public static new CoordinateRangeMutableSilent NotSet => new(double.PositiveInfinity, double.NegativeInfinity);
}
