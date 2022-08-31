namespace ScottPlot.Colormaps
{
    public class GrayscaleReversed : IColormap
    {
        public string Name => "Grayscale Reversed";

        public Color GetColor(double intensity, Range? domain)
        {
            if (double.IsNaN(intensity))
            {
                return Colors.Transparent;
            }

            domain ??= Range.UnitRange;

            double normalized = domain.Value.NormalizeAndClampToUnitRange(intensity);

            byte value = (byte)(255 - (byte)(255 * normalized));
            return Color.Gray(value);
        }
    }
}
