namespace Sandbox.WinForms3D.Primitives3D;

public record struct Point3D(double x, double y, double z)
{
    public double X = x;
    public double Y = y;
    public double Z = z;

    public readonly static Point3D Origin = new(0, 0, 0);

    public readonly Point3D WithZoom(double zoom) => new(X * zoom, Y * zoom, Z * zoom);

    public readonly Point3D WithPan(double x, double y, double z) => new(X + x, Y + y, Z + z);

    public Point3D Translated(Point3D oldOrigin, Point3D newOrigin)
    {
        double dX = newOrigin.X - oldOrigin.X;
        double dY = newOrigin.Y - oldOrigin.Y;
        double dZ = newOrigin.Z - oldOrigin.Z;
        return new(X + dX, Y + dY, Z + dZ);
    }

    public Point3D RotatedX(double degrees)
    {
        double radians = Math.PI * degrees / 180.0f;
        double cos = Math.Cos(radians);
        double sin = Math.Sin(radians);
        double y = Y * cos + Z * sin;
        double z = Y * -sin + Z * cos;
        return new Point3D(X, y, z);
    }

    public Point3D RotatedY(double degrees)
    {
        double radians = Math.PI * degrees / 180.0;
        double cos = Math.Cos(radians);
        double sin = Math.Sin(radians);
        double x = X * cos + Z * sin;
        double z = X * -sin + Z * cos;
        return new Point3D(x, Y, z);
    }

    public Point3D RotatedZ(double degrees)
    {
        double radians = Math.PI * degrees / 180.0;
        double cos = Math.Cos(radians);
        double sin = Math.Sin(radians);
        double x = X * cos + Y * sin;
        double y = X * -sin + Y * cos;
        return new Point3D(x, y, Z);
    }
}
