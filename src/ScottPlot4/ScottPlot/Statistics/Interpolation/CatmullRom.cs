/*
 * Original Author: Reinoud Veenhof (http://www.veetools.xyz)
 * Original Article: https://www.codeproject.com/Articles/1093960/D-Polyline-Vertex-Smoothing
 * Original License: CPOL (https://www.codeproject.com/info/cpol10.aspx)
 *
 * Code here was translated to C# from VB by Scott W Harden 1/24/2022 and is released under MIT license.
 * About Catmull-Rom splines: https://www.cs.cmu.edu/~fp/courses/graphics/asst5/catmullRom.pdf
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Statistics.Interpolation
{
    public static class CatmullRom
    {
        public static (double[] xs, double[] ys) InterpolateXY(double[] xs, double[] ys, int multiple)
        {
            List<PointD> points = new();
            for (int i = 0; i < xs.Length; i++)
                points.Add(new PointD(xs[i], ys[i]));

            var smooth = GetSplineInterpolationCatmullRom(points, multiple);
            double[] xs2 = smooth.Select(s => s.X).ToArray();
            double[] ys2 = smooth.Select(s => s.Y).ToArray();
            return (xs2, ys2);
        }

        private static List<PointD> GetSplineInterpolationCatmullRom(List<PointD> points, int nrOfInterpolatedPoints)
        {
            if (points.Count < 3)
                throw new Exception("Catmull-Rom Spline requires at least 3 points");

            nrOfInterpolatedPoints = Math.Max(1, nrOfInterpolatedPoints);

            List<PointD> spoints = new();
            foreach (PointD p in points)
                spoints.Add(new PointD(p));

            double dx = spoints[1].X - spoints[0].X;
            double dy = spoints[1].Y - spoints[0].Y;
            spoints.Insert(0, new PointD(spoints[0].X - dx, spoints[0].Y - dy));
            dx = spoints[spoints.Count - 1].X - spoints[spoints.Count - 2].X;
            dy = spoints[spoints.Count - 1].Y - spoints[spoints.Count - 2].Y;
            spoints.Insert(spoints.Count, new PointD(spoints[spoints.Count - 1].X + dx, spoints[spoints.Count - 1].Y + dy));

            List<PointD> spline = new();
            for (int i = 0; i <= spoints.Count - 4; i++)
            {
                for (int intp = 0; intp <= nrOfInterpolatedPoints - 1; intp++)
                {
                    double t = 1 / (double)nrOfInterpolatedPoints * intp;
                    PointD spoint = new();
                    spoint = 2 * spoints[i + 1] + (-1 * spoints[i] + spoints[i + 2]) * t +
                        (2 * spoints[i] - 5 * spoints[i + 1] + 4 * spoints[i + 2] - spoints[i + 3]) * Math.Pow(t, 2) +
                        (-1 * spoints[i] + 3 * spoints[i + 1] - 3 * spoints[i + 2] + spoints[i + 3]) * Math.Pow(t, 3);
                    spline.Add(new PointD(spoint * .5));
                }
            }

            spline.Add(spoints[spoints.Count - 2]);
            return spline;
        }
    }
}
