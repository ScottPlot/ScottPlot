using Microsoft.Maui.Graphics;
using System;
using System.Linq;

namespace ScottPlot.Plottable;

public class Scatter<T> : IPlottable
{
    public readonly T[] Xs;
    public readonly T[] Ys;
    public float MarkerSize = 3;

    public Scatter(T[] xs, T[] ys)
    {
        Xs = xs;
        Ys = ys;
    }

    private PointF[] GetPixelPoints(PlotView view)
    {
        PointF[] points = Enumerable.Range(0, Xs.Length)
           .Select(x => new PointF(
               x: view.GetPixelX(Convert.ToDouble(Xs[x])),
               y: view.GetPixelY(Convert.ToDouble(Ys[x]))))
           .ToArray();
        return points;
    }

    public void Draw(ICanvas canvas, PlotView view)
    {
        PointF[] points = GetPixelPoints(view);

        for (int i = 0; i < points.Length - 1; i++)
        {
            canvas.DrawLine(points[i], points[i + 1]);
        }

        for (int i = 0; i < points.Length; i++)
        {
            canvas.DrawCircle(points[i], MarkerSize);
        }
    }
}