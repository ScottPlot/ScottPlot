using System;
using System.Globalization;

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

        public static Color Random() => Random(new Random());
        public static Color Random(Random rand) => new Color((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));
        public static Color Convert(System.Drawing.Color color) => new Color(color.A, color.R, color.G, color.B);
        public static Color FromARGB(byte a, byte r, byte g, byte b) => new Color(a, r, g, b);
        public static Color FromARGB(byte a, Color color) => new Color(a, color.R, color.G, color.B);
        public static Color FromHex(string hex)
        {
            hex = hex.TrimStart('#');

            if (hex.Length == 6)
            {
                return FromARGB(255,
                                (byte)int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber),
                                (byte)int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
                                (byte)int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber));
            }
            else if (hex.Length == 8)
            {
                return FromARGB((byte)int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber),
                                (byte)int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
                                (byte)int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber),
                                (byte)int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber));
            }
            else
            {
                throw new ArgumentException("hex values must have 6 or 8 data characters");
            }
        }
    }
}
