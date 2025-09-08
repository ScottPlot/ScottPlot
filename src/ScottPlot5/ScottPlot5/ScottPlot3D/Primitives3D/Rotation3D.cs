namespace ScottPlot.ScottPlot3D.Primitives3D;

public record struct Rotation3D
{
    public double DegreesX;
    public double DegreesY;
    public double DegreesZ;
    public double CenterX;
    public double CenterY;
    public double CenterZ;
    
    public double RadiansX => Math.PI * DegreesX / 180.0;
    public double RadiansY => Math.PI * DegreesY / 180.0;
    public double RadiansZ => Math.PI * DegreesZ / 180.0;
    
    public Point3D CenterPoint => new(CenterX, CenterY, CenterZ);
}
