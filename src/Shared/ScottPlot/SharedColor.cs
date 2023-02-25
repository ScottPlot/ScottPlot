namespace ScottPlot;

/// <summary>
/// Platform-independent representation of a color.
/// Projects should use extension methods to supply platforms-specific operations.
/// </summary>
public struct SharedColor
{
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    public readonly byte A;

    public SharedColor(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public SharedColor(string hex)
    {
        if (hex[0] == '#')
            hex = hex.Substring(1);

        if (hex.Length == 6)
            hex += "FF";

        bool success = uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out uint rgba);
        if (!success)
            throw new ArgumentException($"invalid color hex string: {hex}");

        R = (byte)(rgba >> 24);
        G = (byte)(rgba >> 16);
        B = (byte)(rgba >> 8);
        A = (byte)(rgba >> 0);
    }
}
