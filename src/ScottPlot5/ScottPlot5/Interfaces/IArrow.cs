namespace ScottPlot;

public interface IArrow
{
    // styling options mimic names from matplotlib
    // https://matplotlib.org/stable/api/_as_gen/matplotlib.axes.Axes.quiver.html
    // https://github.com/ScottPlot/ScottPlot/issues/3697
    public float HeadAxisLength { get; set; }
    public float HeadLength { get; set; }
    public float Width { get; set; }
    public float HeadWidth { get; set; }
    public float Scale { get; set; }
    public float MinimumLength { get; set; }
    public float MaximumLength { get; set; }

    public void Render(RenderPack rp, Pixel pxBase, Pixel pxTip, ArrowStyle arrowStyle);
}
