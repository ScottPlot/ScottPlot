namespace ScottPlot.Drawing.Colormaps;

public class Palette : IColormap
{
    public string Name { get; } = "Palette";

    byte[] R;
    byte[] G;
    byte[] B;

    public Palette(System.Drawing.Color[] colors)
    {
        R = new byte[256];
        G = new byte[256];
        B = new byte[256];

        for (int i = 0; i < 256; i++)
        {
            double fraction = (double)i / 256;
            int colorIndex = (int)(colors.Length * fraction);
            System.Drawing.Color color = colors[colorIndex];
            R[i] = color.R;
            G[i] = color.G;
            B[i] = color.B;
        }
    }

    public (byte r, byte g, byte b) GetRGB(byte value) => (R[value], G[value], B[value]);
}
