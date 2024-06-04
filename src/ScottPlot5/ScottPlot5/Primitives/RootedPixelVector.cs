using System.Numerics;

namespace ScottPlot;

/// <summary>
/// Represents a vector at a point in pixel space
/// </summary>
public struct RootedPixelVector(Pixel point, Vector2 vector)
{
    public Pixel Point { get; set; } = point;
    public Vector2 Vector { get; set; } = vector;

    /// <summary>
    /// Angle of the vector in radians
    /// </summary>
    public readonly float Angle => (float)Math.Atan2(Vector.Y, Vector.X);

    /// <summary>
    /// Length of the vector squared in pixel units
    /// </summary>
    public readonly float MagnitudeSquared => Vector.LengthSquared();

    /// <summary>
    /// Length of the vector in pixel units
    /// </summary>
    public readonly float Magnitude => Vector.Length();
}
