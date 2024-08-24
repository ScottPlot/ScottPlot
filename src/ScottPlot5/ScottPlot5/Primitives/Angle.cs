namespace ScottPlot;

public struct Angle
{
    public double Degrees { get; set; }

    public double Radians
    {
        get => Degrees * Math.PI / 180;
        set => Degrees = value * 180 / Math.PI;
    }

    public readonly Angle Normalized
    {
        get
        {
            double normalized = Degrees % 360;
            double degrees = normalized < 0 ? normalized + 360 : normalized;
            return FromDegrees(degrees);
        }
    }

    public static Angle FromDegrees(double degrees)
    {
        return new Angle() { Degrees = degrees };
    }

    public static Angle FromRadians(double radians)
    {
        return new Angle() { Radians = radians };
    }

    public override readonly string ToString()
    {
        return $"Angle = {Degrees} degrees";
    }

    public static Angle operator +(Angle a)
    {
        return FromDegrees(+a.Degrees);
    }

    public static Angle operator -(Angle a)
    {
        return FromDegrees(-a.Degrees);
    }

    public static Angle operator +(Angle a, Angle b)
    {
        return FromDegrees(a.Degrees + b.Degrees);
    }

    public static Angle operator -(Angle a, Angle b)
    {
        return FromDegrees(a.Degrees - b.Degrees);
    }
}
