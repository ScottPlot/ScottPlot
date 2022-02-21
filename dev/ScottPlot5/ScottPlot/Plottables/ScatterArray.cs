using Microsoft.Maui.Graphics;
using System;
using System.Linq;

namespace ScottPlot.Plottables;

/// <summary>
/// Scatter plot that displays generic data types stored in fixed-length arrays.
/// Data in arrays may continue to be manipulated after passing it into this plot.
/// </summary>
public class ScatterArray<T> : ScatterBase, IPlottable
{
    private readonly T[] Xs;
    private readonly T[] Ys;
    protected override int Count => Xs.Length;

    public ScatterArray(T[] xs, T[] ys)
    {
        if (xs is null || ys is null)
            throw new ArgumentException($"{nameof(xs)} and {nameof(xs)} must not be null");

        if (xs.Length != ys.Length)
            throw new ArgumentException($"{nameof(xs)} and {nameof(xs)} must have the same length");

        Xs = xs;
        Ys = ys;
    }

    protected override PointF[] GetPoints(PlotConfig info)
    {
        PointF[] points = new PointF[Xs.Length];
        for (int i = 0; i < points.Length; i++)
            points[i] = info.GetPixel(Xs[i], Ys[i]).PointF;
        return points;
    }

    public override CoordinateRect GetDataLimits()
    {
        if (Count == 0)
            return CoordinateRect.AllNan();

        var xs = Xs.Select(x => Convert.ToDouble(x));
        var ys = Ys.Select(y => Convert.ToDouble(y));

        return new CoordinateRect(xs.Min(), xs.Max(), ys.Min(), ys.Max());
    }
}