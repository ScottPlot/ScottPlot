namespace ScottPlot.Colormaps;

public class LinearSegmented : ColormapBase, IColormap
{
    public override string Name => "Linear Segmented";

    private readonly Color[] Colors;

    public LinearSegmented(Color[] colors)
    {
        if (colors.Length < 2)
            throw new ArgumentException($"{nameof(colors)} must contain at least 2 colors");

        Colors = colors;
    }

    public override Color GetColor(double position)
    {
        if (position <= 0)
            return Colors[0];

        if (position >= 1)
            return Colors[^1];

        int rangeCount = Colors.Length - 1;
        double rangeSize = 1.0 / rangeCount;

        int firstColorIndex = (int)(position / rangeSize);
        double positionInRange = (position - (rangeSize * firstColorIndex)) / rangeSize;

        Color color1 = Colors[firstColorIndex];
        Color color2 = Colors[firstColorIndex + 1];

        return color1.MixedWith(color2, positionInRange);
    }
}
