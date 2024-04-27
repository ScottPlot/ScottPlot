namespace ScottPlot;

/// <summary>
/// Contains settings and logic for how to draw an arrow
/// once the base and tip pixels have been determined
/// </summary>
public interface IArrow
{
    // styling options mimic names from matplotlib
    // https://matplotlib.org/stable/api/_as_gen/matplotlib.axes.Axes.quiver.html
    // https://github.com/ScottPlot/ScottPlot/issues/3697
    public float HeadAxisLength { get; set; }
    public float HeadLength { get; set; }
    public float Width { get; set; }
    public float HeadWidth { get; set; }

    public void Render(RenderPack rp, Pixel pxBase, Pixel pxTip, ArrowStyle arrowStyle);
}
