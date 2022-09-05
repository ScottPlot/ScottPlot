namespace ScottPlot.Colormaps
{
    public class GrayscaleReversed : ColormapBase
    {
        public override string Name => "Grayscale Reversed";

        public override Color GetColor(double normalizedIntensity)
        {
            byte value = (byte)(255 - (byte)(255 * normalizedIntensity));
            return Color.Gray(value);
        }
    }
}
