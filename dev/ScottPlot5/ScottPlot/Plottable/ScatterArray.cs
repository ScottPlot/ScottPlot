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

    protected override PointF[] GetPoints(PlotView view)
    {
        return Enumerable.Range(0, Xs.Length)
           .Select(x => new PointF(
               x: view.GetPixelX(Convert.ToDouble(Xs[x])),
               y: view.GetPixelY(Convert.ToDouble(Ys[x]))))
           .ToArray();
    }
}