namespace ScottPlot.Renderer
{
    public class Color
    {
        public byte A { get; set; } = 255;
        public byte R { get; set; } = 0;
        public byte G { get; set; } = 0;
        public byte B { get; set; } = 0;

        public Color(byte r, byte g, byte b)
        {
            (A, R, G, B) = (255, r, g, b);
        }
    }
}
