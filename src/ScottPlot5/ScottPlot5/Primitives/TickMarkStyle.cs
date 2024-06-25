namespace ScottPlot;

public class TickMarkStyle
{
    public float Length;
    public float Width;
    public Color Color;
    public bool AntiAlias;

    /// <summary>
    /// When true and the Width is 0, this will instruct skia to render a 1px width line,
    /// no matter what the scaling is on the target machine. 
    /// </summary>
    public bool Hairline;
}
