/*
 * Original Author: Kenneth Haugland
 * Original Article: https://www.codeproject.com/Articles/747928/Spline-Interpolation-history-theory-and-implementa
 * Original License: CPOL (https://www.codeproject.com/info/cpol10.aspx)
 *
 * Code here was adapted by Scott W Harden 1/24/2022 and is released under MIT license.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Statistics.Interpolation
{
    public class Bezier
    {
        public static (double[] xs, double[] ys) InterpolateXY(double[] xs, double[] ys, double stepSize)
        {
            List<PointD> points = new();
            for (int i = 0; i < xs.Length; i++)
                points.Add(new PointD(xs[i], ys[i]));

            //var smooth = BezierFunction(points, stepSize);
            double[] weights = Enumerable.Range(0, points.Count).Select(x => 1.0 / points.Count).ToArray();
            var smooth = RationalBezierFunction(points, weights, stepSize);
            double[] xs2 = smooth.Select(s => s.X).ToArray();
            double[] ys2 = smooth.Select(s => s.Y).ToArray();
            return (xs2, ys2);
        }

        private static List<PointD> RationalBezierFunction(List<PointD> p, double[] Weight, double StepSize)
        {
            List<PointD> result = new();

            for (double k = 0; k <= 1; k += StepSize)
            {
                double[] B = RationalBasisFunction(p.Count, k, Weight);

                double CX = 0;
                double CY = 0;
                for (int j = 0; j <= p.Count - 1; j++)
                {
                    CX += B[j] * p[j].X;
                    CY += B[j] * p[j].Y;
                }

                result.Add(new PointD(CX, CY));
            }

            if (!result.Contains(p[p.Count - 1]))
                result.Add(p[p.Count - 1]);

            return result;
        }

        private static double[] RationalBasisFunction(int n, double u, double[] weight)
        {
            if (weight.Length != n)
                throw new ArgumentException("weight length must match n");

            double[] B = AllBernstein(n, u);

            double test = 0;
            for (int j = 0; j <= n - 1; j++)
                test += B[j] * weight[j];

            double[] result = new double[n];
            for (int i = 0; i <= n - 1; i++)
                result[i] = B[i] * weight[i] / test;

            return result;
        }

        private static double[] AllBernstein(int n, double u)
        {
            double u1 = 1 - u;

            double[] B = new double[n];
            B[0] = 1;

            for (int j = 1; j <= n - 1; j++)
            {
                double saved = 0;
                for (int k = 0; k <= j - 1; k++)
                {
                    double temp = B[k];
                    B[k] = saved + u1 * temp;
                    saved = u * temp;
                }
                B[j] = saved;
            }

            return B;
        }
    }
}
