namespace ScottPlot;

/// <summary>
/// Contains settings and logic for how to draw an arrow
/// once the base and tip pixels have been determined
/// </summary>
public interface IArrowShape
{
    public void Render(RenderPack rp, PixelLine pxLine, ArrowStyle arrowStyle);
}
