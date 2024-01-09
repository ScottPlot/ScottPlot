namespace ScottPlot.DataSources;

public class SignalXYSourceDoubleArray
{
    // TODO: support XOffset and YOffset

    public double[] Xs;
    public double[] Ys;

    public int Length => Ys.Length;

    public SignalXYSourceDoubleArray(double[] xs, double[] ys)
    {
        Xs = xs;
        Ys = ys;
    }

    public AxisLimits GetAxisLimits()
    {
        MinMaxRangeQuery(0, Length - 1, out double yMin, out double yMax);
        return new AxisLimits(Xs[0], Xs[Length - 1], yMin, yMax);
    }

    private void MinMaxRangeQuery(int l, int r, out double lowestValue, out double highestValue)
    {
        lowestValue = Ys[l];
        highestValue = Ys[l];

        for (int i = l; i <= r; i++)
        {
            lowestValue = Math.Min(Ys[i], lowestValue);
            highestValue = Math.Max(Ys[i], highestValue);
        }
    }

    public IEnumerable<Pixel> GetPixelsForRange(int pixelColumnIndex, int from, int length, RenderPack rp, IAxes axes)
    {
        double unitsPerPixelX = axes.XAxis.Width / rp.DataRect.Width;
        double start = axes.XAxis.Min + unitsPerPixelX * pixelColumnIndex;
        double end = start + unitsPerPixelX;

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

        yield return new Pixel(pixelColumnIndex + rp.DataRect.Left, axes.GetPixelY(Ys[startIndex]));

        if (pointsInRange > 1)
        {
            yield return new Pixel(pixelColumnIndex + rp.DataRect.Left, axes.GetPixelY(min));
            yield return new Pixel(pixelColumnIndex + rp.DataRect.Left, axes.GetPixelY(max));
            yield return new Pixel(pixelColumnIndex + rp.DataRect.Left, axes.GetPixelY(Ys[endIndex - 1]));
        }
    }

    public Pixel[] GetPixelsToDraw(RenderPack rp, IAxes axes)
    {
        Pixel[] PointBefore;
        Pixel[] PointAfter;
        int dataIndexFirst;
        int dataIndexLast;

        // Calculate point before displayed points
        int pointBeforeIndex = Array.BinarySearch(Xs, axes.XAxis.Min);
        if (pointBeforeIndex < 0)
        {
            pointBeforeIndex = ~pointBeforeIndex;
        }

        if (pointBeforeIndex > 0)
        {
            float beforeX = axes.GetPixelX(Xs[pointBeforeIndex - 1]);
            float beforeY = axes.GetPixelY(Ys[pointBeforeIndex - 1]);
            Pixel beforePoint = new(beforeX, beforeY);
            PointBefore = new Pixel[] { beforePoint };
            dataIndexFirst = pointBeforeIndex;
        }
        else
        {
            PointBefore = new Pixel[] { };
            dataIndexFirst = 0;
        }

        // Calculate point after displayed points
        int pointAfterIndex = Array.BinarySearch(Xs, axes.XAxis.Max);
        if (pointAfterIndex < 0)
        {
            pointAfterIndex = ~pointAfterIndex;
        }

        if (pointAfterIndex <= Ys.Length - 1)
        {
            float afterX = axes.GetPixelX(Xs[pointAfterIndex]);
            float afterY = axes.GetPixelY(Ys[pointAfterIndex]);
            Pixel afterPoint = new(afterX, afterY);
            PointAfter = new Pixel[] { afterPoint };
            dataIndexLast = pointAfterIndex;
        }
        else
        {
            PointAfter = new Pixel[] { };
            dataIndexLast = Ys.Length - 1;
        }

        int dataIndexCount = dataIndexLast - dataIndexFirst + 1;

        IEnumerable<Pixel> VisiblePoints = Enumerable.Range(0, (int)Math.Ceiling(rp.DataRect.Width))
            .Select(pixelColumnIndex => GetPixelsForRange(pixelColumnIndex, dataIndexFirst, dataIndexCount, rp, axes))
            .SelectMany(x => x);

        Pixel[] PointsToDraw = PointBefore.Concat(VisiblePoints).Concat(PointAfter).ToArray();

        // Interpolate before displayed point to make it x = -1 (close to visible area)
        // this fix extreme zoom in bug
        if (PointBefore.Length > 0 && PointsToDraw.Length >= 2)
        {
            // only extrapolate if points are different (otherwise extrapolated point may be infinity)
            if (PointsToDraw[0].X != PointsToDraw[1].X)
            {
                float x0 = -1 + rp.DataRect.Left;
                float yDelta = PointsToDraw[0].Y - PointsToDraw[1].Y;
                float xDelta1 = x0 - PointsToDraw[1].X;
                float xDelta2 = PointsToDraw[0].X - PointsToDraw[1].X;
                float y0 = PointsToDraw[1].Y + yDelta * xDelta1 / xDelta2;
                PointsToDraw[0] = new Pixel(x0, y0);
            }
        }

        // Interpolate after displayed point to make it x = datasize.Width(close to visible area)
        // this fix extreme zoom in bug
        if (PointAfter.Length > 0 && PointsToDraw.Length >= 2)
        {
            Pixel lastPoint = PointsToDraw[PointsToDraw.Length - 2];
            Pixel afterPoint = PointsToDraw[PointsToDraw.Length - 1];

            // only extrapolate if points are different (otherwise extrapolated point may be infinity)
            if (afterPoint.X != lastPoint.X)
            {
                float x1 = rp.DataRect.Width + rp.DataRect.Left;
                float yDelta = afterPoint.Y - lastPoint.Y;
                float xDelta1 = x1 - lastPoint.X;
                float xDelta2 = afterPoint.X - lastPoint.X;
                float y1 = lastPoint.Y + yDelta * xDelta1 / xDelta2;
                PointsToDraw[PointsToDraw.Length - 1] = new Pixel(x1, y1);
            }
        }

        return PointsToDraw;
    }

}
