using System.Numerics;

namespace ScottPlot.Primitives;

public struct RootedCoordinateVector(Coordinates tail, Vector2 vector)
{
    public Coordinates Tail { get; set; } = tail;
    public Vector2 Vector { get; set; } = vector;

    public readonly float Direction => (float)Math.Atan2(Vector.Y, Vector.X); // Note that this is properly defined for x,y = 0, unlike Math.Atan(y / x)
    public readonly float DistanceSquared => Vector.LengthSquared();
    public readonly float Distance => Vector.Length();
}
