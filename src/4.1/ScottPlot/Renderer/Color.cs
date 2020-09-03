namespace ScottPlot.Renderer
{
    public class Color
    {
        public byte A { get; set; } = 255;
        public byte R { get; set; } = 0;
        public byte G { get; set; } = 0;
        public byte B { get; set; } = 0;

        public Color(byte r, byte g, byte b) => (A, R, G, B) = (255, r, g, b);
        public Color(byte a, byte r, byte g, byte b) => (A, R, G, B) = (a, r, g, b);
        
        public static Color FromARGB(byte a, byte r, byte g, byte b) => new Color(a, r, g, b);
        public static Color FromARGB(byte a, Color color) => new Color(a, color.R, color.G, color.B);
        public static Color Convert(System.Drawing.Color color) => new Color(color.A, color.R, color.G, color.B);
    }
}
