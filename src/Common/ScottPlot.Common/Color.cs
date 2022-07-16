namespace ScottPlot;

// TODO: rename this to just Color
public struct SPColor
{
    public readonly byte Red;
    public readonly byte Green;
    public readonly byte Blue;
    public readonly byte Alpha;

    public uint ARGB => (uint)Alpha << 24 | (uint)Red << 16 | (uint)Green << 8 | (uint)Blue << 0;

    public SPColor(byte red, byte green, byte blue, byte alpha = 255)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    public readonly SPColor WithRed(byte red) => new(red, Green, Blue, Alpha);
    public readonly SPColor WithGreen(byte green) => new(Red, green, Blue, Alpha);
    public readonly SPColor WithBlue(byte blue) => new(Red, Green, blue, Alpha);
    public readonly SPColor WithAlpha(byte alpha) => new(Red, Green, Blue, alpha);

    public static SPColor FromARGB(uint argb)
    {
        byte alpha = (byte)(argb >> 24);
        byte red = (byte)(argb >> 16);
        byte green = (byte)(argb >> 8);
        byte blue = (byte)(argb >> 0);
        return new SPColor(red, green, blue, alpha);
    }

    public string ToStringRGB()
    {
        return "#" + Red.ToString("X2") + Green.ToString("X2") + Blue.ToString("X2");
    }

    public string ToStringRGBA()
    {
        return "#" + Red.ToString("X2") + Green.ToString("X2") + Blue.ToString("X2") + Alpha.ToString("X2");
    }
}
