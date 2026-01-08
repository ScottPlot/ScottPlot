using System.Drawing;
using System.Numerics;
using ScottPlot.ScottPlot3D.Plottables3D;
using ScottPlot.ScottPlot3D.Primitives3D;

namespace ScottPlot.ScottPlot3D;

public record struct Scene
{
    private Point3D _position = new();
    private Rotation3D _rotation = new() { DegreesX = 110, DegreesY = 18, DegreesZ = 5 };
    private double _zoom = 200;

    public Point3D Position
    {
        get => _position;
        set
        {
            _position = value;
            UpdateTransformationMatrix();
        }
    }

    public Rotation3D Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            UpdateTransformationMatrix();
        }
    }

    public double Zoom
    {
        get => _zoom;
        set
        {
            _zoom = value;
            UpdateTransformationMatrix();
        }
    }
    
    private Matrix4x4 transformationMatrix;

    private static Matrix4x4 OrthographicMatrix => new Matrix4x4(
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 1
    );

    private void UpdateTransformationMatrix()
    {
        transformationMatrix = Matrix4x4.Identity;
        transformationMatrix = OrthographicMatrix * transformationMatrix;
        transformationMatrix = Matrix4x4.CreateScale((float)Zoom, (float)Zoom, (float)Zoom) * transformationMatrix;
        transformationMatrix = Matrix4x4.CreateTranslation((float)-Position.X, (float)Position.Y, (float)-Position.Z) * transformationMatrix;
        transformationMatrix = Matrix4x4.CreateRotationX((float)Rotation.RadiansX) * transformationMatrix;
        transformationMatrix = Matrix4x4.CreateRotationY((float)Rotation.RadiansY) * transformationMatrix;
        transformationMatrix = Matrix4x4.CreateRotationZ((float)Rotation.RadiansZ) * transformationMatrix;
    }
    
    public Scene()
    {
        UpdateTransformationMatrix();
    }

    public Pixel GetPoint2D(Point3D point)
    {
        Vector4 point4 = new((float)point.X, (float)point.Y, (float)point.Z, 1);
        Vector4 transformedPoint = Vector4.Transform(point4, transformationMatrix);
        return new Pixel(transformedPoint.X, transformedPoint.Y);
    }
}

public class Plot3D
{
    public Scene Scene = new();
    public readonly Axis3D Axis3D = new();
    public readonly List<IPlottable3D> Plottables = [];
    
    public Pixel GetPoint2D(Point3D point, Pixel imageCenter)
    {
        return Scene.GetPoint2D(point);
    }

    public void Render(SKSurface surface)
    {
        RenderPack3D rp = new(this, surface);

        FillStyle fs = new() { Color = Colors.White };
        Drawing.FillRectangle(rp.Canvas, rp.ImageRect, rp.Paint, fs);

        Axis3D.Render(rp);

        foreach (IPlottable3D plottable in Plottables)
        {
            plottable.Render(rp);
        }
    }
}
