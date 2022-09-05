namespace ScottPlot;

public struct Color
{
    public readonly byte Red;
    public readonly byte Green;
    public readonly byte Blue;
    public readonly byte Alpha;

    public uint ARGB => (uint)Alpha << 24 | (uint)Red << 16 | (uint)Green << 8 | (uint)Blue << 0;

    public Color(byte red, byte green, byte blue, byte alpha = 255)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    public readonly Color WithRed(byte red) => new(red, Green, Blue, Alpha);
    public readonly Color WithGreen(byte green) => new(Red, green, Blue, Alpha);
    public readonly Color WithBlue(byte blue) => new(Red, Green, blue, Alpha);
    public readonly Color WithAlpha(byte alpha) => new(Red, Green, Blue, alpha);

    public static Color Gray(byte value) => new(value, value, value);

    public static Color FromARGB(uint argb)
    {
        byte alpha = (byte)(argb >> 24);
        byte red = (byte)(argb >> 16);
        byte green = (byte)(argb >> 8);
        byte blue = (byte)(argb >> 0);
        return new Color(red, green, blue, alpha);
    }

    public static Color FromHex(string hex)
    {
        if (hex[0] == '#')
        {
            return FromHex(hex.Substring(1));
        }

        if (hex.Length == 6)
        {
            hex += "FF";
        }

        if (!uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out uint rgba))
        {
            return new Color(0, 0, 0);
        }

        uint argb = ((rgba & 0xFF) << 24) | (rgba >> 8);
        return FromARGB(argb);
    }

    public static Color[] FromHex(string[] hex)
    {
        return hex.Select(x => FromHex(x)).ToArray();
    }

    public string ToStringRGB()
    {
        return "#" + Red.ToString("X2") + Green.ToString("X2") + Blue.ToString("X2");
    }

    public string ToStringRGBA()
    {
        return "#" + Red.ToString("X2") + Green.ToString("X2") + Blue.ToString("X2") + Alpha.ToString("X2");
    }

    public SkiaSharp.SKColor ToSKColor()
    {
        return new SkiaSharp.SKColor(ARGB);
    }
}
