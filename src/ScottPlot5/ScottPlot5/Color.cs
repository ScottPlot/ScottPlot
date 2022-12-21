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

    public Color(float red, float green, float blue, float alpha = 1)
    {
        Red = (byte)(red * 255);
        Green = (byte)(green * 255);
        Blue = (byte)(blue * 255);
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

    public (float h, float s, float l) ToHSL()
    {
        // adapted from Microsoft.Maui.Graphics/Color.cs (MIT license)

        float v = Math.Max(Red, Green);
        v = Math.Max(v, Blue);

        float m = Math.Min(Red, Green);
        m = Math.Min(m, Blue);

        float h, s, l;
        l = (m + v) / 2.0f;
        if (l <= 0.0)
        {
            return (0, 0, 0);
        }

        float vm = v - m;
        s = vm;
        if (s <= 0.0)
        {
            return (0, 0, l);
        }

        s /= l <= 0.5f ? v + m : 2.0f - v - m;

        float r2 = (v - Red) / vm;
        float g2 = (v - Green) / vm;
        float b2 = (v - Blue) / vm;

        if (Red == v)
        {
            h = Green == m ? 5.0f + b2 : 1.0f - g2;
        }
        else if (Green == v)
        {
            h = Blue == m ? 1.0f + r2 : 3.0f - b2;
        }
        else
        {
            h = Red == m ? 3.0f + g2 : 5.0f - r2;
        }

        h /= 6.0f;

        return (h, s, l);
    }

    public static Color FromHSL(float hue, float saturation, float luminosity)
    {
        // adapted from Microsoft.Maui.Graphics/Color.cs (MIT license)

        if (luminosity == 0)
        {
            return new Color(0, 0, 0);
        }

        if (saturation == 0)
        {
            return new Color(luminosity, luminosity, luminosity);
        }
        float temp2 = luminosity <= 0.5f ? luminosity * (1.0f + saturation) : luminosity + saturation - luminosity * saturation;
        float temp1 = 2.0f * luminosity - temp2;

        var t3 = new[] { hue + 1.0f / 3.0f, hue, hue - 1.0f / 3.0f };
        var clr = new float[] { 0, 0, 0 };
        for (var i = 0; i < 3; i++)
        {
            if (t3[i] < 0)
                t3[i] += 1.0f;
            if (t3[i] > 1)
                t3[i] -= 1.0f;
            if (6.0 * t3[i] < 1.0)
                clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0f;
            else if (2.0 * t3[i] < 1.0)
                clr[i] = temp2;
            else if (3.0 * t3[i] < 2.0)
                clr[i] = temp1 + (temp2 - temp1) * (2.0f / 3.0f - t3[i]) * 6.0f;
            else
                clr[i] = temp1;
        }

        return new Color(clr[0], clr[1], clr[2]);
    }

    public Color WithLightness(float lightness = .5f)
    {
        (float h, float s, float l) = ToHSL();
        return FromHSL(h, s, lightness);
    }

    public Color Lighten(float fraction = .5f)
    {
        (float h, float s, float l) = ToHSL();
        return FromHSL(h, s, l + (1 - l) * fraction);
    }

    public Color Darken(float fraction = .5f)
    {
        (float h, float s, float l) = ToHSL();
        return FromHSL(h, s, l * fraction);
    }
}
