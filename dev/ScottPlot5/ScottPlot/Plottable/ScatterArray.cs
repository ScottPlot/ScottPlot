using Microsoft.Maui.Graphics;
using System;
using System.Linq;

namespace ScottPlot.Plottable;

public class ScatterArray<T> : ScatterBase, IPlottable
{
    public readonly T[] Xs;
    public readonly T[] Ys;

    public ScatterArray(T[] xs, T[] ys)
    {
        Xs = xs;
        Ys = ys;
    }

    protected override PointF[] GetPoints(PlotInfo layout)
    {
        return Enumerable.Range(0, Xs.Length)
           .Select(i => layout.GetPixel(Xs[i], Ys[i]).PointF)
           .ToArray();
    }
}