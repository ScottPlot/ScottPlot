/*
 * Original Author: Reinoud Veenhof (http://www.veetools.xyz)
 * Original Article: https://www.codeproject.com/Articles/1093960/D-Polyline-Vertex-Smoothing
 * Original License: CPOL (https://www.codeproject.com/info/cpol10.aspx)
 *
 * Code here was translated to C# from VB by Scott W Harden 1/24/2022 and is released under MIT license.
 * About Chaikin curves: https://www.cs.unc.edu/~dm/UNC/COMP258/LECTURES/Chaikins-Algorithm.pdf
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Statistics.Interpolation
{
    public static class Chaikin
    {
        public static (double[] xs, double[] ys) InterpolateXY(double[] xs, double[] ys, int multiple, double tension = .5)
        {
            List<PointD> points = new();
            for (int i = 0; i < xs.Length; i++)
                points.Add(new PointD(xs[i], ys[i]));

            var smooth = GetCurveSmoothingChaikin(points, tension, multiple);
            double[] xs2 = smooth.Select(s => s.X).ToArray();
            double[] ys2 = smooth.Select(s => s.Y).ToArray();
            return (xs2, ys2);
        }

        private static List<PointD> GetCurveSmoothingChaikin(List<PointD> points, double tension, int nrOfIterations)
        {
            if (points == null || points.Count < 3)
                return null;

            if (nrOfIterations < 1)
                nrOfIterations = 1;

            if (tension < 0)
                tension = 0;
            else if (tension > 1)
                tension = 1;

            // the tension factor defines a scale between corner cutting distance in segment half length,
            // i.e. between 0.05 and 0.45. The opposite corner will be cut by the inverse
            // (i.e. 1-cutting distance) to keep symmetry.
            // with a tension value of 0.5 this amounts to 0.25 = 1/4 and 0.75 = 3/4,
            // the original Chaikin values
            double cutdist = 0.05 + (tension * 0.4);

            // make a copy of the pointlist and iterate it
            List<PointD> nl = new();
            for (int i = 0; i <= points.Count - 1; i++)
                nl.Add(new PointD(points[i]));

            for (int i = 1; i <= nrOfIterations; i++)
                nl = GetSmootherChaikin(nl, cutdist);

            return nl;
        }

        private static List<PointD> GetSmootherChaikin(List<PointD> points, double cuttingDist)
        {
            List<PointD> nl = new();

            nl.Add(new PointD(points[0]));

            PointD q, r;

            for (int i = 0; i <= points.Count - 2; i++)
            {
                q = (1 - cuttingDist) * points[i] + cuttingDist * points[i + 1];
                r = cuttingDist * points[i] + (1 - cuttingDist) * points[i + 1];
                nl.Add(q);
                nl.Add(r);
            }

            nl.Add(new PointD(points[points.Count - 1]));

            return nl;
        }
    }
}
