namespace ScottPlot;

public interface IRenderLast
{
    /// <summary>
    /// Plottables that implement this interface have a second render method that runs 
    /// after the axes are drawn, allowing graphics to be placed on top of the axes.
    /// </summary>
    void RenderLast(RenderPack rp);
}
