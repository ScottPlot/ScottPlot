namespace ScottPlot.Drawing.Colormaps
{
    public class Grayscale : IColormap
    {
        public string Name => "Grayscale";

        public (byte r, byte g, byte b) GetRGB(byte value)
        {
            return (value, value, value);
        }
    }
}
