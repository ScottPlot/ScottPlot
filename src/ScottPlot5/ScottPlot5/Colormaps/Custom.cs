namespace ScottPlot.Colormaps;

public class Custom : ColormapBase
{
    public override string Name { get; }

    private readonly Color[] Colors;

    public Custom(Color[] colors, string name = "custom")
    {
        Colors = colors;
        Name = name;
    }

    public override Color GetColor(double position)
    {
        int index = (int)((Colors.Length - 1) * position);
        return Colors[index];
    }
}
