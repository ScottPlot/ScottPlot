namespace ScottPlot;

internal static class ColorByteExtensions
{
    static public byte Lighten(this byte color, double fraction)
    {
        var fColor = color + (255 - color) * fraction;
        return (byte)Math.Min(Math.Max(fColor, 0), 255);
    }

    static public byte Darken(this byte color, double fraction)
    {
        var fColor = color * fraction;
        return (byte)Math.Min(Math.Max(fColor, 0), 255);
    }
}

public readonly struct Color
{
    public readonly byte Red;
    public readonly byte Green;
    public readonly byte Blue;
    public readonly byte Alpha;

    // TODO: benchmark if referencing these is slower
    public byte R => Red;
    public byte G => Green;
    public byte B => Blue;
    public byte A => Alpha;

    public uint ARGB
    {
        get
        {
            return (uint)Alpha << 24 |
                (uint)Red << 16 |
                (uint)Green << 8 |
                (uint)Blue << 0;
        }
    }

    public override string ToString()
    {
        return $"Color {ToHex()} (R={R}, G={G}, B={B}, A={A})";
    }

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
        Alpha = (byte)(alpha * 255);
    }

    public static bool operator ==(Color a, Color b)
    {
        return a.ARGB == b.ARGB;
    }

    public static bool operator !=(Color a, Color b)
    {
        return a.ARGB != b.ARGB;
    }

    public override int GetHashCode()
    {
        return (int)ARGB;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is not Color)
            return false;

        return ((Color)obj).ARGB == ARGB;
    }

    public readonly Color WithRed(byte red) => new(red, Green, Blue, Alpha);
    public readonly Color WithGreen(byte green) => new(Red, green, Blue, Alpha);
    public readonly Color WithBlue(byte blue) => new(Red, Green, blue, Alpha);

    public readonly Color WithAlpha(byte alpha)
    {
        // If requesting a semitransparent black, make it slightly non-black
        // to prevent SVG export from rendering the color as opaque.
        // https://github.com/ScottPlot/ScottPlot/issues/3063
        if (Red == 0 && Green == 0 && Blue == 0 && alpha < 255)
        {
            return new Color(1, 1, 1, alpha);
        }

        return new(Red, Green, Blue, alpha);
    }

    public readonly Color WithAlpha(double alpha) => WithAlpha((byte)(alpha * 255));

    public readonly Color WithOpacity(double opacity = .5) => WithAlpha((byte)(opacity * 255));

    public static Color Gray(byte value) => new(value, value, value);

    public static Color FromARGB(int argb)
    {
        return FromARGB((uint)argb);
    }

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

    public static Color FromColor(System.Drawing.Color color)
    {
        return new Color(color.R, color.G, color.B, color.A);
    }

    public string ToHex()
    {
        return Alpha == 255
            ? $"#{R:X2}{G:X2}{B:X2}"
            : $"#{R:X2}{G:X2}{B:X2}{A:X2}";
    }

    public static Color FromSKColor(SKColor skcolor)
    {
        return new Color(skcolor.Red, skcolor.Green, skcolor.Blue, skcolor.Alpha);
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
        return new SKColor(Red, Green, Blue, Alpha);
    }

    public (float h, float s, float l) ToHSL()
    {
        // Converter adapted from http://en.wikipedia.org/wiki/HSL_color_space
        float r = Red / 255f;
        float g = Green / 255f;
        float b = Blue / 255f;

        float max = Math.Max(Math.Max(r, g), b);
        float min = Math.Min(Math.Min(r, g), b);

        float h, s, l;
        l = (min + max) / 2.0f;
        if (l <= 0.0)
        {
            return (0, 0, 0);
        }

        float delta = max - min;
        s = delta;
        if (s <= 0.0)
        {
            return (0, 0, l);
        }

        s = (l > 0.5f) ? delta / (2.0f - delta) : delta / (max + min);

        if (r > g && r > b)
            h = (g - b) / delta + (g < b ? 6.0f : 0.0f);

        else if (g > b)
            h = (b - r) / delta + 2.0f;

        else
            h = (r - g) / delta + 4.0f;

        h /= 6.0f;
        return (h, s, l);
    }

    public static Color FromHSL(float hue, float saturation, float luminosity, float alpha = 1)
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

        return new Color(clr[0], clr[1], clr[2], alpha);
    }

    public Color WithLightness(float lightness = .5f)
    {
        (float h, float s, float _) = ToHSL();
        return FromHSL(h, s, Math.Min(Math.Max(lightness, 0f), 1f));
    }

    /// <summary>
    /// Amount to lighten the color (from 0-1).
    /// Larger numbers produce lighter results.
    /// </summary>
    public Color Lighten(double fraction = .5f)
    {
        if (fraction < 0)
            return Darken(-fraction);
        fraction = Math.Min(1f, fraction);
        return new Color(R.Lighten(fraction), G.Lighten(fraction), B.Lighten(fraction), Alpha);
    }

    /// <summary>
    /// Amount to darken the color (from 0-1).
    /// Larger numbers produce darker results.
    /// </summary>
    public Color Darken(double fraction = .5f)
    {
        if (fraction < 0)
            return Lighten(-fraction);
        fraction = Math.Max(0f, 1f - fraction);
        return new Color(R.Darken(fraction), G.Darken(fraction), B.Darken(fraction), Alpha);
    }

    /// <summary>
    /// Return this color mixed with another color.
    /// </summary>
    /// <param name="otherColor">Color to mix with this color</param>
    /// <param name="fraction">Fraction of <paramref name="otherColor"/> to use</param>
    /// <returns></returns>
    public Color MixedWith(Color otherColor, double fraction)
    {
        return InterpolateRgb(otherColor, fraction);
    }

    public Color InterpolateRgb(Color c1, double factor)
    {
        return InterpolateRgb(this, c1, factor);
    }

    public Color[] InterpolateArrayRgb(Color c1, int steps)
    {
        return InterpolateRgbArray(this, c1, steps);
    }

    static byte InterpolateRgb(byte b1, byte b2, double factor)
    {
        if (b1 < b2)
            return Math.Min(Math.Max((byte)(b1 + (b2 - b1) * factor), (byte)0), (byte)255);
        else
            return Math.Min(Math.Max((byte)(b2 + (b1 - b2) * (1 - factor)), (byte)0), (byte)255);
    }

    static public Color InterpolateRgb(Color c1, Color c2, double factor)
    {
        return new Color(
            InterpolateRgb(c1.R, c2.R, factor),
            InterpolateRgb(c1.G, c2.G, factor),
            InterpolateRgb(c1.B, c2.B, factor),
            InterpolateRgb(c1.A, c2.A, factor)
            );
    }

    static public Color[] InterpolateRgbArray(Color c1, Color c2, int steps)
    {
        var stepFactor = 1.0 / (steps - 1);
        var array = new Color[steps];
        for (var i = 0; i < steps; ++i)
        {
            array[i] = InterpolateRgb(c1, c2, stepFactor * i);
        }
        return array;
    }

    public uint PremultipliedARGB
    {
        get
        {
            byte premultipliedRed = (byte)((Red * Alpha) / 255);
            byte premultipliedGreen = (byte)((Green * Alpha) / 255);
            byte premultipliedBlue = (byte)((Blue * Alpha) / 255);
            return
                ((uint)Alpha << 24) |
                ((uint)premultipliedRed << 16) |
                ((uint)premultipliedGreen << 8) |
                ((uint)premultipliedBlue << 0);
        }
    }

    public static Color RandomHue()
    {
        float hue = (float)Generate.RandomNumber();
        float saturation = 1;
        float luminosity = 0.5f;
        return Color.FromHSL(hue, saturation, luminosity);
    }
}
