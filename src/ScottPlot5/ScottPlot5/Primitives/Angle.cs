namespace ScottPlot;

public static class Angle
{
    /// <summary>
    /// Convert degrees to radians
    /// </summary>
    public static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    /// <summary>
    /// Convert radians to degrees
    /// </summary>
    public static double ToDegrees(double radians)
    {
        return radians * 180 / Math.PI;
    }

    /// <summary>
    /// Radians normalized to [0, 2π]
    /// </summary>
    public static double NormalizeRadians(double radians)
    {
        double normalized = radians % (2 * Math.PI);
        return normalized < 0 ? normalized + 2 * Math.PI : normalized;
    }

    /// <summary>
    /// Degrees normalized to [0, 360]
    /// </summary>
    public static double NormalizeDegrees(double degrees)
    {
        double normalized = degrees % 360;
        return normalized < 0 ? normalized + 360 : normalized;
    }
}
