namespace ScottPlot;

public struct Angle
{
    public double Degrees { get; set; }

    public double Radians
    {
        readonly get => Degrees * Math.PI / 180;
        set => Degrees = value * 180 / Math.PI;
    }

    public readonly Angle Inverted => FromDegrees(-Degrees);

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

    public static Angle FromFraction(double fraction, bool clockwise = false)
    {
        if (clockwise)
            fraction *= -1;
        return new Angle() { Degrees = 360 * fraction };
    }

    public static Angle FromFraction(double fraction, Angle start, bool clockwise = false)
    {
        if (clockwise)
            fraction *= -1;
        return new Angle() { Degrees = 360 * fraction + start.Degrees };
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

    public static Angle operator *(Angle a, double b)
    {
        return FromDegrees(a.Degrees * b);
    }

    public static Angle operator *(double a, Angle b)
    {
        return FromDegrees(a * b.Degrees);
    }

    public static Angle operator /(Angle a, double b)
    {
        return FromDegrees(a.Degrees / b);
    }

    public static Angle operator %(Angle a, double b)
    {
        return FromDegrees(a.Degrees % b);
    }

    public static bool operator ==(Angle a, Angle b)
    {
        return a.Degrees == b.Degrees;
    }

    public static bool operator !=(Angle a, Angle b)
    {
        return a.Degrees != b.Degrees;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is Angle other && Degrees == other.Degrees;
    }

    public readonly override int GetHashCode()
    {
        return Degrees.GetHashCode();
    }
}
