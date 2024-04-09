using System.Numerics;

namespace ScottPlot;

/// <summary>
/// Represents a vector at a point in coordinate space
/// </summary>
public struct RootedCoordinateVector(Coordinates point, Vector2 vector)
{
    public Coordinates Point { get; set; } = point;
    public Vector2 Vector { get; set; } = vector;

    /// <summary>
    /// Angle of the vector in radians
    /// </summary>
    public readonly float Angle => (float)Math.Atan2(Vector.Y, Vector.X); // Note that this is properly defined for x,y = 0, unlike Math.Atan(y / x)

    /// <summary>
    /// Length of the vector squared in coordinate units
    /// </summary>
    public readonly float MagnitudeSquared => Vector.LengthSquared();

    /// <summary>
    /// Length of the vector in coordinate units
    /// </summary>
    public readonly float Magnitude => Vector.Length();
}
