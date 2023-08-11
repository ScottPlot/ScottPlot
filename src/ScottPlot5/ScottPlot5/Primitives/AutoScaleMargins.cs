namespace ScottPlot;

/// <summary>
/// Controls the fraction of empty space added to the data area when calling AutoScale().
/// Margins of 0 produce "tight" axes which fit the data exactly.
/// </summary>
public readonly struct AutoScaleMargins
{
    public readonly double Horizontal;
    public readonly double Vertical;

    public AutoScaleMargins(double horizontal = 0.1, double vertical = 0.15)
    {
        if (horizontal < 0 || horizontal > 1)
            throw new ArgumentException($"Margins must be within the range [0, 1]");

        if (vertical < 0 || vertical > 1)
            throw new ArgumentException($"Margins must be within the range [0, 1]");

        Horizontal = horizontal;
        Vertical = vertical;
    }
}
