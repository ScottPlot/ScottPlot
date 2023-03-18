namespace ScottPlot;

public static class GLExtensions
{
    public static OpenTK.Graphics.Color4 ToTkColor(this ScottPlot.Color color)
    {
        return new OpenTK.Graphics.Color4(color.Red, color.Green, color.Blue, color.Alpha);
    }
}
