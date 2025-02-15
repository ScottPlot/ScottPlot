namespace ScottPlot;
using OpenTK.Graphics;
#if NETCOREAPP || NET
using OpenTK.Mathematics;
#endif

public static class GLExtensions
{
    public static Color4 ToTkColor(this ScottPlot.Color color)
    {
        return new Color4(color.Red, color.Green, color.Blue, color.Alpha);
    }
}
