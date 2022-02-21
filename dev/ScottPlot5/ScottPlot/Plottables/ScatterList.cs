using Microsoft.Maui.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Plottables;

/// <summary>
/// Display a scatter plot from generic data stored as X/Y pairs.
/// Data can be modified at any time by calling Add(), RemoveAt(), Clear(), etc. 
/// </summary>
internal class ScatterList<T> : ScatterBase
{
    private readonly List<(T, T)> Points = new();
    protected override int Count => Points.Count;

    public ScatterList()
    {

    }

    protected override PointF[] GetPoints(PlotConfig plotInfo)
    {
        PointF[] points = new PointF[Points.Count];
        for (int i = 0; i < points.Length; i++)
        {
            double x = Convert.ToDouble(Points[i].Item1);
            double y = Convert.ToDouble(Points[i].Item2);
            points[i] = plotInfo.GetPixel(x, y).PointF;
        }
        return points;
    }

    public void Add(T x, T y) => Points.Add((x, y));

    public void AddRange(T[] xs, T[] ys)
    {
        if (xs is null)
            throw new ArgumentNullException(nameof(xs));

        if (ys is null)
            throw new ArgumentNullException(nameof(ys));

        if (xs.Length != ys.Length)
            throw new ArgumentOutOfRangeException($"{nameof(xs)} and {nameof(ys)} must be the same length");

        for (int i = 0; i < xs.Length; i++)
            Points.Add((xs[i], ys[i]));
    }

    public void RemoveAt(int index) => Points.RemoveAt(index);

    public void Clear() => Points.Clear();

    public override CoordinateRect GetDataLimits()
    {
        if (Count == 0)
            return CoordinateRect.AllNan();

        var xs = Points.Select(x => Convert.ToDouble(x.Item1));
        var ys = Points.Select(x => Convert.ToDouble(x.Item2));

        return new CoordinateRect(xs.Min(), xs.Max(), ys.Min(), ys.Max());
    }
}