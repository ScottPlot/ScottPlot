using System;

namespace ScottPlot
{
    /// <summary>
    /// Describes an X/Y position in pixel space
    /// </summary>
    public struct Pixel
    {
        public float X;
        public float Y;

        public Pixel(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"[X={X}, Y={Y}]";

        /// <summary>
        /// True as lone as neither coordinate is NaN or Infinity
        /// </summary>
        public bool IsFinite()
        {
            return
                !double.IsNaN(X) &&
                !double.IsNaN(Y) &&
                !double.IsInfinity(X) &&
                !double.IsInfinity(Y);
        }

        /// <summary>
        /// Return the distance to another pixel (in pixel units)
        /// </summary>
        public float Distance(Pixel other)
        {
            double dX = Math.Abs(other.X - X);
            double dY = Math.Abs(other.Y - Y);
            return (float)Math.Sqrt(dX * dX + dY * dY);
        }

        /// <summary>
        /// Shift the pixel location by the given deltas
        /// </summary>
        public void Translate(float deltaX, float deltaY)
        {
            X += deltaX;
            Y += deltaY;
        }
    }
}
