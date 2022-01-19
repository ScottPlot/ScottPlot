/* Parabolic Spline Interpolation Module for C#
 * Original Author: Ryan Seghers
 * Original Copyright: Copyright (C) 2013-2014 Ryan Seghers
 * Original License: MIT https://opensource.org/licenses/MIT
 * Commentary: https://www.codeproject.com/Articles/560163/Csharp-Cubic-Spline-Interpolation
 * Adapted by Scott Harden for ScottPlot.NET on Jan 18, 2022
 *   - replaced System.Windows.Forms.DataVisualization.Charting with ScottPlot
 *   - upgraded from .NET Framework 4.5 to .NET Standard 2.0
 *   - replaced float logic with double logic (TODO: <T>?)
 *   - eliminated debug code, simplified logic, implemented modern language features
 */

using System;

namespace ScottPlot.Statistics.Interpolation
{
    public static class Cubic
    {
        public static (double[] xs, double[] ys) Interpolate(double[] xs, double[] ys, int multiple)
        {
            CubicSpline.FitParametric(xs, ys, xs.Length * multiple, out double[] xOut, out double[] yOut);
            return (xOut, yOut);
        }

        /// <summary>
        /// Cubic spline interpolation.
        /// Call Fit (or use the corrector constructor) to compute spline coefficients, 
        /// then Eval to evaluate the spline at other X coordinates.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is implemented based on the wikipedia article:
        /// http://en.wikipedia.org/wiki/Spline_interpolation
        /// I'm not sure I have the right to include a copy of the article so the equation numbers referenced in 
        /// comments will end up being wrong at some point.
        /// </para>
        /// <para>
        /// This is not optimized, and is not MT safe.
        /// This can extrapolate off the ends of the splines.
        /// You must provide points in X sort order.
        /// </para>
        /// </remarks>
        private class CubicSpline
        {
            private double[] a;
            private double[] b;

            private double[] xOrig;
            private double[] yOrig;

            private CubicSpline()
            {
            }

            /// <summary>
            /// Throws if Fit has not been called.
            /// </summary>
            private void CheckAlreadyFitted()
            {
                if (a == null) throw new Exception("Fit must be called before you can evaluate.");
            }

            private int _lastIndex = 0;

            /// <summary>
            /// Find where in xOrig the specified x falls, by simultaneous traverse.
            /// This allows xs to be less than x[0] and/or greater than x[n-1]. So allows extrapolation.
            /// This keeps state, so requires that x be sorted and xs called in ascending order, and is not multi-thread safe.
            /// </summary>
            private int GetNextXIndex(double x)
            {
                if (x < xOrig[_lastIndex])
                {
                    throw new ArgumentException("The X values to evaluate must be sorted.");
                }

                while ((_lastIndex < xOrig.Length - 2) && (x > xOrig[_lastIndex + 1]))
                {
                    _lastIndex++;
                }

                return _lastIndex;
            }

            /// <summary>
            /// Evaluate the specified x value using the specified spline.
            /// </summary>
            /// <param name="x">The x value.</param>
            /// <param name="j">Which spline to use.</param>
            /// <returns>The y value.</returns>
            private double EvalSpline(double x, int j)
            {
                double dx = xOrig[j + 1] - xOrig[j];
                double t = (x - xOrig[j]) / dx;
                double y = (1 - t) * yOrig[j] + t * yOrig[j + 1] + t * (1 - t) * (a[j] * (1 - t) + b[j] * t); // equation 9
                return y;
            }

            /// <summary>
            /// Fit x,y and then eval at points xs and return the corresponding y's.
            /// This does the "natural spline" style for ends.
            /// This can extrapolate off the ends of the splines.
            /// You must provide points in X sort order.
            /// </summary>
            /// <param name="x">Input. X coordinates to fit.</param>
            /// <param name="y">Input. Y coordinates to fit.</param>
            /// <param name="xs">Input. X coordinates to evaluate the fitted curve at.</param>
            /// <param name="startSlope">Optional slope constraint for the first point. Single.NaN means no constraint.</param>
            /// <param name="endSlope">Optional slope constraint for the final point. Single.NaN means no constraint.</param>
            /// <returns>The computed y values for each xs.</returns>
            public double[] FitAndEval(double[] x, double[] y, double[] xs, double startSlope = double.NaN, double endSlope = double.NaN)
            {
                Fit(x, y, startSlope, endSlope);
                return Eval(xs);
            }

            /// <summary>
            /// Compute spline coefficients for the specified x,y points.
            /// This does the "natural spline" style for ends.
            /// This can extrapolate off the ends of the splines.
            /// You must provide points in X sort order.
            /// </summary>
            /// <param name="x">Input. X coordinates to fit.</param>
            /// <param name="y">Input. Y coordinates to fit.</param>
            /// <param name="startSlope">Optional slope constraint for the first point. Single.NaN means no constraint.</param>
            /// <param name="endSlope">Optional slope constraint for the final point. Single.NaN means no constraint.</param>
            public void Fit(double[] x, double[] y, double startSlope = double.NaN, double endSlope = double.NaN)
            {
                if (double.IsInfinity(startSlope) || double.IsInfinity(endSlope))
                {
                    throw new Exception("startSlope and endSlope cannot be infinity.");
                }

                // Save x and y for eval
                this.xOrig = x;
                this.yOrig = y;

                int n = x.Length;
                double[] r = new double[n]; // the right hand side numbers: wikipedia page overloads b

                TriDiagonalMatrixF m = new TriDiagonalMatrixF(n);
                double dx1, dx2, dy1, dy2;

                // First row is different (equation 16 from the article)
                if (double.IsNaN(startSlope))
                {
                    dx1 = x[1] - x[0];
                    m.C[0] = 1.0f / dx1;
                    m.B[0] = 2.0f * m.C[0];
                    r[0] = 3 * (y[1] - y[0]) / (dx1 * dx1);
                }
                else
                {
                    m.B[0] = 1;
                    r[0] = startSlope;
                }

                // Body rows (equation 15 from the article)
                for (int i = 1; i < n - 1; i++)
                {
                    dx1 = x[i] - x[i - 1];
                    dx2 = x[i + 1] - x[i];

                    m.A[i] = 1.0f / dx1;
                    m.C[i] = 1.0f / dx2;
                    m.B[i] = 2.0f * (m.A[i] + m.C[i]);

                    dy1 = y[i] - y[i - 1];
                    dy2 = y[i + 1] - y[i];
                    r[i] = 3 * (dy1 / (dx1 * dx1) + dy2 / (dx2 * dx2));
                }

                // Last row also different (equation 17 from the article)
                if (double.IsNaN(endSlope))
                {
                    dx1 = x[n - 1] - x[n - 2];
                    dy1 = y[n - 1] - y[n - 2];
                    m.A[n - 1] = 1.0f / dx1;
                    m.B[n - 1] = 2.0f * m.A[n - 1];
                    r[n - 1] = 3 * (dy1 / (dx1 * dx1));
                }
                else
                {
                    m.B[n - 1] = 1;
                    r[n - 1] = endSlope;
                }

                // k is the solution to the matrix
                double[] k = m.Solve(r);

                // a and b are each spline's coefficients
                this.a = new double[n - 1];
                this.b = new double[n - 1];

                for (int i = 1; i < n; i++)
                {
                    dx1 = x[i] - x[i - 1];
                    dy1 = y[i] - y[i - 1];
                    a[i - 1] = k[i - 1] * dx1 - dy1; // equation 10 from the article
                    b[i - 1] = -k[i] * dx1 + dy1; // equation 11 from the article
                }
            }

            /// <summary>
            /// Evaluate the spline at the specified x coordinates.
            /// This can extrapolate off the ends of the splines.
            /// You must provide X's in ascending order.
            /// The spline must already be computed before calling this, meaning you must have already called Fit() or FitAndEval().
            /// </summary>
            /// <param name="x">Input. X coordinates to evaluate the fitted curve at.</param>
            /// <returns>The computed y values for each x.</returns>
            public double[] Eval(double[] x)
            {
                CheckAlreadyFitted();

                int n = x.Length;
                double[] y = new double[n];
                _lastIndex = 0; // Reset simultaneous traversal in case there are multiple calls

                for (int i = 0; i < n; i++)
                {
                    // Find which spline can be used to compute this x (by simultaneous traverse)
                    int j = GetNextXIndex(x[i]);

                    // Evaluate using j'th spline
                    y[i] = EvalSpline(x[i], j);
                }

                return y;
            }

            /// <summary>
            /// Fit the input x,y points using the parametric approach, so that y does not have to be an explicit
            /// function of x, meaning there does not need to be a single value of y for each x.
            /// </summary>
            /// <param name="x">Input x coordinates.</param>
            /// <param name="y">Input y coordinates.</param>
            /// <param name="nOutputPoints">How many output points to create.</param>
            /// <param name="xs">Output (interpolated) x values.</param>
            /// <param name="ys">Output (interpolated) y values.</param>
            /// <param name="firstDx">Optionally specifies the first point's slope in combination with firstDy. Together they
            /// are a vector describing the direction of the parametric spline of the starting point. The vector does
            /// not need to be normalized. If either is NaN then neither is used.</param>
            /// <param name="firstDy">See description of dx0.</param>
            /// <param name="lastDx">Optionally specifies the last point's slope in combination with lastDy. Together they
            /// are a vector describing the direction of the parametric spline of the last point. The vector does
            /// not need to be normalized. If either is NaN then neither is used.</param>
            /// <param name="lastDy">See description of dxN.</param>
            public static void FitParametric(double[] x, double[] y, int nOutputPoints, out double[] xs, out double[] ys,
                double firstDx = Single.NaN, double firstDy = Single.NaN, double lastDx = Single.NaN, double lastDy = Single.NaN)
            {
                // Compute distances
                int n = x.Length;
                double[] dists = new double[n]; // cumulative distance
                dists[0] = 0;
                double totalDist = 0;

                for (int i = 1; i < n; i++)
                {
                    double dx = x[i] - x[i - 1];
                    double dy = y[i] - y[i - 1];
                    double dist = (float)Math.Sqrt(dx * dx + dy * dy);
                    totalDist += dist;
                    dists[i] = totalDist;
                }

                // Create 'times' to interpolate to
                double dt = totalDist / (nOutputPoints - 1);
                double[] times = new double[nOutputPoints];
                times[0] = 0;

                for (int i = 1; i < nOutputPoints; i++)
                {
                    times[i] = times[i - 1] + dt;
                }

                // Normalize the slopes, if specified
                NormalizeVector(ref firstDx, ref firstDy);
                NormalizeVector(ref lastDx, ref lastDy);

                // Spline fit both x and y to times
                CubicSpline xSpline = new CubicSpline();
                xs = xSpline.FitAndEval(dists, x, times, firstDx / dt, lastDx / dt);

                CubicSpline ySpline = new CubicSpline();
                ys = ySpline.FitAndEval(dists, y, times, firstDy / dt, lastDy / dt);
            }

            private static void NormalizeVector(ref double dx, ref double dy)
            {
                if (!double.IsNaN(dx) && !double.IsNaN(dy))
                {
                    double d = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (d > Single.Epsilon) // probably not conservative enough, but catches the (0,0) case at least
                    {
                        dx /= d;
                        dy /= d;
                    }
                    else
                    {
                        throw new ArgumentException("The input vector is too small to be normalized.");
                    }
                }
                else
                {
                    // In case one is NaN and not the other
                    dx = dy = Single.NaN;
                }
            }
        }

        /// <summary>
        /// A tri-diagonal matrix has non-zero entries only on the main diagonal, the diagonal above the main (super), and the
        /// diagonal below the main (sub).
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is based on the wikipedia article: http://en.wikipedia.org/wiki/Tridiagonal_matrix_algorithm
        /// </para>
        /// <para>
        /// The entries in the matrix on a particular row are A[i], B[i], and C[i] where i is the row index.
        /// B is the main diagonal, and so for an NxN matrix B is length N and all elements are used.
        /// So for row 0, the first two values are B[0] and C[0].
        /// And for row N-1, the last two values are A[N-1] and B[N-1].
        /// That means that A[0] is not actually on the matrix and is therefore never used, and same with C[N-1].
        /// </para>
        /// </remarks>
        private class TriDiagonalMatrixF
        {
            /// <summary>
            /// The values for the sub-diagonal. A[0] is never used.
            /// </summary>
            public readonly double[] A;

            /// <summary>
            /// The values for the main diagonal.
            /// </summary>
            public readonly double[] B;

            /// <summary>
            /// The values for the super-diagonal. C[C.Length-1] is never used.
            /// </summary>
            public readonly double[] C;

            /// <summary>
            /// The width and height of this matrix.
            /// </summary>
            public int N => A != null ? A.Length : 0;

            /// <summary>
            /// Indexer. Setter throws an exception if you try to set any not on the super, main, or sub diagonals.
            /// </summary>
            public double this[int row, int col]
            {
                get
                {
                    int di = row - col;

                    if (di == 0)
                        return B[row];
                    else if (di == -1)
                        return C[row];
                    else if (di == 1)
                        return A[row];
                    else return 0;
                }
                set
                {
                    int di = row - col;

                    if (di == 0)
                        B[row] = value;
                    else if (di == -1)
                        C[row] = value;
                    else if (di == 1)
                        A[row] = value;
                    else
                        throw new ArgumentException("Only the main, super, and sub diagonals can be set.");
                }
            }

            /// <summary>
            /// Construct an NxN matrix.
            /// </summary>
            public TriDiagonalMatrixF(int n)
            {
                A = new double[n];
                B = new double[n];
                C = new double[n];
            }

            /// <summary>
            /// Solve the system of equations this*x=d given the specified d.
            /// </summary>
            /// <remarks>
            /// Uses the Thomas algorithm described in the wikipedia article: http://en.wikipedia.org/wiki/Tridiagonal_matrix_algorithm
            /// Not optimized. Not destructive.
            /// </remarks>
            /// <param name="d">Right side of the equation.</param>
            public double[] Solve(double[] d)
            {
                int n = N;

                if (d.Length != n)
                    throw new ArgumentException("The input d is not the same size as this matrix.");

                // cPrime
                double[] cPrime = new double[n];
                cPrime[0] = C[0] / B[0];

                for (int i = 1; i < n; i++)
                    cPrime[i] = C[i] / (B[i] - cPrime[i - 1] * A[i]);

                // dPrime
                double[] dPrime = new double[n];
                dPrime[0] = d[0] / B[0];

                for (int i = 1; i < n; i++)
                    dPrime[i] = (d[i] - dPrime[i - 1] * A[i]) / (B[i] - cPrime[i - 1] * A[i]);

                // Back substitution
                double[] x = new double[n];
                x[n - 1] = dPrime[n - 1];

                for (int i = n - 2; i >= 0; i--)
                    x[i] = dPrime[i] - cPrime[i] * x[i + 1];

                return x;
            }
        }
    }
}
