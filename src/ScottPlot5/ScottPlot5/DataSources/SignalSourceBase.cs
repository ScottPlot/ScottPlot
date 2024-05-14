namespace ScottPlot.DataSources;

public abstract class SignalSourceBase
{
    public double Period { get; set; }
    public abstract int Length { get; }

    public int MinimumIndex { get; set; } = 0;
    public int MaximumIndex { get; set; } = int.MaxValue;

    public int MinRenderIndex => Math.Max(0, MinimumIndex);
    public int MaxRenderIndex => Math.Min(Length - 1, MaximumIndex);

    public double XOffset { get; set; } = 0;
    public double YOffset { get; set; } = 0;
    public double YScale { get; set; } = 1;

    public int GetIndex(double x, bool visibleDataOnly)
    {
        int i = (int)((x - XOffset) / Period);

        if (visibleDataOnly)
        {
            i = Math.Max(i, MinRenderIndex);
            i = Math.Min(i, MaxRenderIndex);
        }

        return i;
    }

    public bool RangeContainsSignal(double xMin, double xMax)
    {
        int xMinIndex = GetIndex(xMin, false);
        int xMaxIndex = GetIndex(xMax, false);
        return xMaxIndex >= MinRenderIndex && xMinIndex <= MaxRenderIndex;
    }

    public double GetX(int index)
    {
        return index * Period + XOffset;
    }

    public CoordinateRange GetLimitsX()
    {
        CoordinateRect rect = GetLimits().Rect;
        return new CoordinateRange(rect.Left, rect.Left);
    }

    public CoordinateRange GetLimitsY()
    {
        CoordinateRect rect = GetLimits().Rect;
        return new CoordinateRange(rect.Bottom, rect.Bottom);
    }

    public abstract SignalRangeY GetLimitsY(int firstIndex, int lastIndex);

    public AxisLimits GetLimits()
    {
        SignalRangeY rangeY = GetLimitsY(MinRenderIndex, MaxRenderIndex);

        return new AxisLimits(
            left: XOffset + MinRenderIndex * Period,
            right: XOffset + MaxRenderIndex * Period,
            bottom: rangeY.Min * YScale + YOffset,
            top: rangeY.Max * YScale + YOffset);
    }
}
