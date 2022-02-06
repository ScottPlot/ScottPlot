using Microsoft.Maui.Graphics;
using System;

namespace ScottPlot.Plottable;

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

    protected override PointF[] GetPoints(PlotInfo info)
    {
        PointF[] points = new PointF[Xs.Length];
        for (int i = 0; i < points.Length; i++)
            points[i] = info.GetPixel(Xs[i], Ys[i]).PointF;
        return points;
    }
}