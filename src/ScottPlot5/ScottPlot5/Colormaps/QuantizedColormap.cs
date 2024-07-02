namespace ScottPlot.Colormaps;

internal class QuantizedColormap(IColormap backingColormap, int numBuckets) : ColormapBase
{
    private readonly IColormap colormap = backingColormap;
    private readonly int buckets = numBuckets;
    private readonly double[] bucketCutoffs = Enumerable.Range(0, numBuckets).Select(i => i / (numBuckets - 1.0)).ToArray();

    private double DecreaseToCutoff(double value)
    {
        for (int i = 0; i < bucketCutoffs.Length; i++)
        {
            if (bucketCutoffs[i] > value)
                return bucketCutoffs[i - 1];
        }

        return bucketCutoffs[^1];
    }

    public override string Name => $"Quantized {colormap.Name} with {buckets} buckets";

    public override Color GetColor(double position)
    {
        return colormap.GetColor(DecreaseToCutoff(position));
    }
}
