/*
 * Original Author: Reinoud Veenhof (http://www.veetools.xyz)
 * Original Article: https://www.codeproject.com/Articles/1093960/D-Polyline-Vertex-Smoothing
 * Original License: CPOL (https://www.codeproject.com/info/cpol10.aspx)
 *
 * Code here was translated to C# from VB by Scott W Harden 1/24/2022 and is released under MIT license.
 */

namespace ScottPlot.Statistics.Interpolation
{
    internal class PointD
    {
        public readonly double X = 0;
        public readonly double Y = 0;

        public PointD() { }

        public PointD(double nx, double ny) { X = nx; Y = ny; }

        public PointD(PointD p) { X = p.X; Y = p.Y; }

        public static PointD operator +(PointD p1, PointD p2) => new(p1.X + p2.X, p1.Y + p2.Y);

        public static PointD operator +(PointD p, double d) => new(p.X + d, p.Y + d);

        public static PointD operator +(double d, PointD p) => p + d;

        public static PointD operator -(PointD p1, PointD p2) => new(p1.X - p2.X, p1.Y - p2.Y);

        public static PointD operator -(PointD p, double d) => new(p.X - d, p.Y - d);

        public static PointD operator -(double d, PointD p) => p - d;

        public static PointD operator *(PointD p1, PointD p2) => new(p1.X * p2.X, p1.Y * p2.Y);

        public static PointD operator *(PointD p, double d) => new(p.X * d, p.Y * d);

        public static PointD operator *(double d, PointD p) => p * d;

        public static PointD operator /(PointD p1, PointD p2) => new(p1.X / p2.X, p1.Y / p2.Y);

        public static PointD operator /(PointD p, double d) => new(p.X / d, p.Y / d);

        public static PointD operator /(double d, PointD p) => new(d / p.X, d / p.Y);
    }

}
