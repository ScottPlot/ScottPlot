using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Plottable;

public class ScatterList<T> : ScatterBase, IPlottable
{
    public readonly List<T> Xs = new();
    public readonly List<T> Ys = new();

    public ScatterList()
    {

    }

    public ScatterList(List<T> xs, List<T> ys)
    {
        Xs = xs;
        Ys = ys;
    }

    protected override PointF[] GetPoints(PlotInfo layout)
    {
        return Enumerable.Range(0, Xs.Count)
           .Select(i => layout.GetPixel(Xs[i], Ys[i]).PointF)
           .ToArray();
    }
}
