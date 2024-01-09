using ScottPlot.Drawing;
using System.Drawing;

namespace ScottPlot.Plottable;

/// <summary>
/// Experimental plottabled refactored for migration to ScottPlot 5
/// </summary>
public class SignalXY_EXPERIMENTAL : IPlottable
{
    public double[] Xs;
    public double[] Ys;

    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(Xs.First(), Xs.Last(), Ys.Min(), Ys.Max());
    }

    public LegendItem[] GetLegendItems() => LegendItem.None;

    public void MinMaxRangeQuery(int l, int r, out double lowestValue, out double highestValue)
    {
        lowestValue = Ys[l];
        highestValue = Ys[l];

        for (int i = l; i <= r; i++)
        {
            lowestValue = Math.Min(Ys[i], lowestValue);
            highestValue = Math.Max(Ys[i], highestValue);
        }
    }

    public IEnumerable<PointF> ProcessInterval(int x, int from, int length, PlotDimensions dims)
    {
        double start = dims.XMin + dims.XSpan / dims.DataWidth * x;
        double end = dims.XMin + dims.XSpan / dims.DataWidth * (x + 1);

        int startIndex = Array.BinarySearch(Xs, from, length, start);
        if (startIndex < 0)
        {
            startIndex = ~startIndex;
        }

        int endIndex = Array.BinarySearch(Xs, from, length, end);
        if (endIndex < 0)
        {
            endIndex = ~endIndex;
        }

        if (startIndex == endIndex)
        {
            yield break;
        }

        MinMaxRangeQuery(startIndex, endIndex - 1, out double min, out double max);

        int pointsInRange = endIndex - startIndex;

        yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(Ys[startIndex]));

        if (pointsInRange > 1)
        {
            yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(min));
            yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(max));
            yield return new PointF(x + dims.DataOffsetX, dims.GetPixelY(Ys[endIndex - 1]));
        }
    }

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        PointF[] PointBefore;
        PointF[] PointAfter;
        int searchFrom;
        int searchTo;

        // Calculate point before displayed points
        int pointBeforeIndex = Array.BinarySearch(Xs, dims.XMin);
        if (pointBeforeIndex < 0)
        {
            pointBeforeIndex = ~pointBeforeIndex;
        }

        if (pointBeforeIndex > 0)
        {
            float beforeX = dims.GetPixelX(Xs[pointBeforeIndex - 1]);
            float beforeY = dims.GetPixelY(Ys[pointBeforeIndex - 1]);
            PointF beforePoint = new(beforeX, beforeY);
            PointBefore = new PointF[] { beforePoint };
            searchFrom = pointBeforeIndex;
        }
        else
        {
            PointBefore = new PointF[] { };
            searchFrom = 0;
        }

        // Calculate point after displayed points
        int pointAfterIndex = Array.BinarySearch(Xs, dims.XMax);
        if (pointAfterIndex < 0)
        {
            pointAfterIndex = ~pointAfterIndex;
        }

        if (pointAfterIndex <= Ys.Length - 1)
        {
            float afterX = dims.GetPixelX(Xs[pointAfterIndex]);
            float afterY = dims.GetPixelY(Ys[pointAfterIndex]);
            PointF afterPoint = new(afterX, afterY);
            PointAfter = new PointF[] { afterPoint };
            searchTo = pointAfterIndex;
        }
        else
        {
            PointAfter = new PointF[] { };
            searchTo = Ys.Length - 1;
        }

        IEnumerable<PointF> VisiblePoints = Enumerable.Range(0, (int)Math.Round(dims.DataWidth))
            .Select(x => ProcessInterval(x, searchFrom, searchTo - searchFrom + 1, dims))
            .SelectMany(x => x);

        PointF[] PointsToDraw = PointBefore.Concat(VisiblePoints).Concat(PointAfter).ToArray();

        // Interpolate before displayed point to make it x = -1 (close to visible area)
        // this fix extreme zoom in bug
        if (PointBefore.Length > 0 && PointsToDraw.Length >= 2)
        {
            // only extrapolate if points are different (otherwise extrapolated point may be infinity)
            if (PointsToDraw[0].X != PointsToDraw[1].X)
            {
                float x0 = -1 + dims.DataOffsetX;
                float yDelta = PointsToDraw[0].Y - PointsToDraw[1].Y;
                float xDelta1 = x0 - PointsToDraw[1].X;
                float xDelta2 = PointsToDraw[0].X - PointsToDraw[1].X;
                float y0 = PointsToDraw[1].Y + yDelta * xDelta1 / xDelta2;
                PointsToDraw[0] = new PointF(x0, y0);
            }
        }

        // Interpolate after displayed point to make it x = datasize.Width(close to visible area)
        // this fix extreme zoom in bug
        if (PointAfter.Length > 0 && PointsToDraw.Length >= 2)
        {
            PointF lastPoint = PointsToDraw[PointsToDraw.Length - 2];
            PointF afterPoint = PointsToDraw[PointsToDraw.Length - 1];

            // only extrapolate if points are different (otherwise extrapolated point may be infinity)
            if (afterPoint.X != lastPoint.X)
            {
                float x1 = dims.DataWidth + dims.DataOffsetX;
                float yDelta = afterPoint.Y - lastPoint.Y;
                float xDelta1 = x1 - lastPoint.X;
                float xDelta2 = afterPoint.X - lastPoint.X;
                float y1 = lastPoint.Y + yDelta * xDelta1 / xDelta2;
                PointsToDraw[PointsToDraw.Length - 1] = new PointF(x1, y1);
            }
        }

        using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);

        using Pen penHD = GDI.Pen(Color.Black);

        gfx.DrawLines(penHD, PointsToDraw);
    }

    public void ValidateData(bool deep = false)
    {
    }
}
