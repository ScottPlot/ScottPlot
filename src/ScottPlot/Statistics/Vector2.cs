using System;

namespace ScottPlot.Statistics
{
    // A simpler double-precision floating point alternative to the System.Numerics.Vectors package
    // https://github.com/microsoft/referencesource/blob/master/System.Numerics/System/Numerics/Vector2.cs

    public struct Vector2
    {
        public double X;
        public double Y;

        public Vector2(double x, double y)
        {
            (X, Y) = (x, y);
        }

        public double LengthSquared()
        {
            return X * X + Y * Y;
        }

        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public static Vector2 Multiply(Vector2 left, double right)
        {
            return new Vector2(left.X * right, left.Y * right);
        }
    }
}
