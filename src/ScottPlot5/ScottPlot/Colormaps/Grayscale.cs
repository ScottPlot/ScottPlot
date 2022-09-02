namespace ScottPlot.Colormaps
{
    public class Grayscale : ColormapBase
    {
        public override string Name => "Grayscale";

        protected override Color GetColor(double normalizedIntensity)
        {
            byte value = (byte)(255 * normalizedIntensity);
            return Color.Gray(value);
        }
    }
}
