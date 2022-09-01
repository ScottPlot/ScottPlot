namespace ScottPlot.Colormaps
{
    public class GrayscaleReversed : ColormapBase 
    {
        public override string Name => "Grayscale Reversed";

        protected override Color GetColor(double normalizedIntensity)
        {
            byte value = (byte)(255 - (byte)(255 * normalizedIntensity));
            return Color.Gray(value);
        }
    }
}
