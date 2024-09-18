namespace ScottPlot;

// NOTE: names are consistent with matplotlib linestyles
// https://matplotlib.org/stable/gallery/lines_bars_and_markers/linestyles.html

// TODO: add more preset LinePatterns

public readonly struct LinePattern(float[] intervals, float phase)
{
    // default(LinePattern) is actually "Solid"
    public static LinePattern Solid { get; } = default;
    public static LinePattern Dashed { get; } = new([10, 10], 0);
    public static LinePattern DenselyDashed { get; } = new([6, 6], 0);
    public static LinePattern Dotted { get; } = new([3, 5], 0);

    public float[] Intervals { get; } = intervals;

    public float Phase { get; } = phase;

    // default(LinePattern) does not set _hasBeenSet to true.
    private readonly bool _hasBeenSet = true;

    public SKPathEffect? GetPathEffect()
    {
        return !_hasBeenSet || Intervals is null
            ? null
            : SKPathEffect.CreateDash(Intervals, Phase);
    }
}
