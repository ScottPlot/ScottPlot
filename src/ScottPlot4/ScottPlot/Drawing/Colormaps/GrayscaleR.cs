namespace ScottPlot.Drawing.Colormaps
{
    public class GrayscaleR : IColormap
    {
        public string Name => "GrayscaleR";

        public (byte r, byte g, byte b) GetRGB(byte value)
        {
            value = (byte)(255 - value);
            return (value, value, value);
        }
    }
}
