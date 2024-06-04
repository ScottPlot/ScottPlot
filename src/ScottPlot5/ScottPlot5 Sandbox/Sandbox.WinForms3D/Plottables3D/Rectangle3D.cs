using Sandbox.WinForms3D.Primitives3D;

namespace Sandbox.WinForms3D.Plottables3D;

internal class Rectangle3D : IPlottable3D
{
    Surface3D[] Surfaces { get; }
    public Point3D Location { get; }
    public Size3D Size { get; }

    public Rectangle3D(Point3D location, Size3D size)
    {
        Location = location;
        Size = size;
        Surfaces =
        [
            GetBottom(),
            GetWest(),
            GetNorth(),
            GetEast(),
            GetSouth(),
            GetTop(),
        ];
    }

    private Surface3D GetBottom()
    {
        Point3D[] points =
        [
            Location,
            Location.WithPan(Size.Width, 0, 0),
            Location.WithPan(Size.Width, Size.Length, 0),
            Location.WithPan(0, Size.Length, 0),
            Location,
        ];

        return new Surface3D(points);
    }

    private Surface3D GetWest()
    {
        Point3D[] points =
        [
            Location,
            Location.WithPan(0, Size.Length, 0),
            Location.WithPan(0, Size.Length, Size.Height),
            Location.WithPan(0, 0, Size.Height),
            Location,
        ];

        return new Surface3D(points);
    }

    private Surface3D GetNorth()
    {
        Point3D[] points =
        [
            Location,
            Location.WithPan(Size.Width, 0, 0),
            Location.WithPan(Size.Width, 0, Size.Height),
            Location.WithPan(0, 0, Size.Height),
            Location,
        ];

        return new Surface3D(points);
    }

    private Surface3D GetEast()
    {
        Point3D[] points =
        [
            Location.WithPan(Size.Width, 0, 0),
            Location.WithPan(Size.Width, 0, Size.Height),
            Location.WithPan(Size.Width, Size.Length, Size.Height),
            Location.WithPan(Size.Width, Size.Length, 0),
            Location.WithPan(Size.Width, 0, 0),
        ];

        return new Surface3D(points);
    }

    private Surface3D GetSouth()
    {
        Point3D[] points =
        [
            Location.WithPan(0, Size.Length, 0),
            Location.WithPan(0, Size.Length, Size.Height),
            Location.WithPan(Size.Width, Size.Length, Size.Height),
            Location.WithPan(Size.Width, Size.Length, 0),
            Location.WithPan(0, Size.Length, 0),
        ];

        return new Surface3D(points);
    }

    private Surface3D GetTop()
    {
        Point3D[] points =
        [
            Location.WithPan(0, 0, Size.Height),
            Location.WithPan(Size.Width, 0, Size.Height),
            Location.WithPan(Size.Width, Size.Length, Size.Height),
            Location.WithPan(0, Size.Length, Size.Height),
            Location.WithPan(0, 0, Size.Height),
        ];

        return new Surface3D(points);
    }

    public void Render(RenderPack3D rp)
    {
        foreach (var p in Surfaces)
        {
            p.Render(rp);
        }
    }
}
