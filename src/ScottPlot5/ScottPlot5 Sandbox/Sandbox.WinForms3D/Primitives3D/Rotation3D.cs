namespace Sandbox.WinForms3D.Primitives3D;

public record struct Rotation3D
{
    public double DegreesX;
    public double DegreesY;
    public double DegreesZ;
    public double CenterX;
    public double CenterY;
    public double CenterZ;
    public Point3D CenterPoint => new(CenterX, CenterY, CenterZ);
}
