using System.Linq;

namespace ScottPlot.Axis;

/// <summary>
/// This object stores the axis limits for multiple axes.
/// It is used for mouse interaction to translate pixel distances 
/// to coordinate distances based on previous renders.
/// </summary>
public class MultiAxisLimits
{
    private struct ImmutableRange
    {
        public double Min { get; }
        public double Max { get; }
        public ImmutableRange(double min, double max)
        {
            Min = min;
            Max = max;
        }
    }

    private readonly Dictionary<IXAxis, ImmutableRange> RememberedLimitsX = new();

    private readonly Dictionary<IYAxis, ImmutableRange> RememberedLimitsY = new();

    public MultiAxisLimits()
    {

    }

    public void RememberLimits(IXAxis xAxis, double min, double max)
    {
        RememberedLimitsX[xAxis] = new(min, max);
    }

    public void RememberLimits(IYAxis yAxis, double min, double max)
    {
        RememberedLimitsY[yAxis] = new(min, max);
    }

    public void RestoreLimits(IXAxis xAxis)
    {
        ImmutableRange original = RememberedLimitsX[xAxis];
        xAxis.Range.Min = original.Min;
        xAxis.Range.Max = original.Max;
    }

    public void RestoreLimits(IYAxis yAxis)
    {
        ImmutableRange original = RememberedLimitsY[yAxis];
        yAxis.Range.Min = original.Min;
        yAxis.Range.Max = original.Max;
    }

    public void RememberLimits(MultiAxisLimits limits)
    {
        ForgetAllLimits();
        limits.RememberedLimitsX.ToList().ForEach(x => RememberLimits(x.Key, x.Value.Min, x.Value.Max));
        limits.RememberedLimitsY.ToList().ForEach(y => RememberLimits(y.Key, y.Value.Min, y.Value.Max));
    }

    public void ForgetAllLimits()
    {
        RememberedLimitsX.Clear();
        RememberedLimitsY.Clear();
    }
}
