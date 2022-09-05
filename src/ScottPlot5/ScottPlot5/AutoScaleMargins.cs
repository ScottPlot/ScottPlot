namespace ScottPlot;

/// <summary>
/// Controls the fraction of empty space added to the data area when calling AutoScale()
/// </summary>
public class AutoScaleMargins
{
    private double X = .05;
    private double Y = .1;

    /// <summary>
    /// Zoom out using this fraction to apply the current margin
    /// </summary>
    public double ZoomFracX => 1 - Horizontal;

    /// <summary>
    /// Zoom out using this fraction to apply the current margin
    /// </summary>
    public double ZoomFracY => 1 - Vertical;

    /// <summary>
    /// This fraction of empty space is added to the data area when calling AutoScale()
    /// </summary>
    public double Horizontal
    {
        get => X;
        set
        {
            if (value >= 0 && value <= 1)
                X = value;
            else
                throw new ArgumentException($"Margins must be within the range [0, 1]");
        }
    }

    /// <summary>
    /// This fraction of empty space is added to the data area when calling AutoScale()
    /// </summary>
    public double Vertical
    {
        get => Y;
        set
        {
            if (value >= 0 && value <= 1)
                Y = value;
            else
                throw new ArgumentException($"Margins must be within the range [0, 1]");
        }
    }
}
