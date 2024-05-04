namespace ScottPlot;

///<summary>
///Represents a range between any two finite values (inclusive)
///</summary>
public readonly struct Range // TODO: evaluate if this can be replaced with more task-specific primitives
{
    public double Min { get; }
    public double Max { get; }

    public Range(double min, double max)
    {
        if (double.IsInfinity(min) || double.IsNaN(min))
            throw new ArgumentException($"{nameof(min)} must be a real number");

        if (double.IsInfinity(max) || double.IsNaN(max))
            throw new ArgumentException($"{nameof(max)} must be a real number");

        if (min > max)
        {
            throw new ArgumentException($"Argument ${nameof(min)} must be less than or equal to ${nameof(max)}.");
        }

        Min = min;
        Max = max;
    }

    public override string ToString()
    {
        return $"Range: [{Min}, {Max}]";
    }

    /// <summary>
    /// Returns the given value as a fraction of the difference between Min and Max. This is a min-max feature scaling.
    /// </summary>
    /// <param name="value">The value to normalize</param>
    /// <param name="clamp">If true, values outside of the range will be clamped onto the interval [0, 1].</param>
    /// <returns>The normalized value</returns>
    public double Normalize(double value, bool clamp = false)
    {
        if (Max == Min)
        {
            throw new ArgumentException($"Cannot normalize to the range if {nameof(Min)} == {nameof(Max)}");
        }

        double normalized = (value - Min) / (Max - Min);

        Range unitRange = new(0, 1);
        return clamp ? unitRange.Clamp(normalized) : normalized;
    }

    ///<summary>
    ///Returns the given value clamped to the range (inclusive).
    ///</summary>
    public double Clamp(double value)
    {
        if (value < Min)
        {
            return Min;
        }
        if (value > Max)
        {
            return Max;
        }

        return value;
    }

    public static Range GetRange(double[,] input)
    {
        return GetRange(input.Cast<double>());
    }

    public static Range GetRange(IEnumerable<double> input)
    {
        double min = double.PositiveInfinity;
        double max = double.NegativeInfinity;
        foreach (var curr in input)
        {
            if (double.IsNaN(curr))
                continue;

            min = Math.Min(min, curr);
            max = Math.Max(max, curr);
        }

        return new(min, max);
    }
}
