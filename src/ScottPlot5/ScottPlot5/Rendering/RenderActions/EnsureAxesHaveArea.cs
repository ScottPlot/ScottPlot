namespace ScottPlot.Rendering.RenderActions;

/// <summary>
/// Identifies axes with zero span and expands them slightly
/// </summary>
public class EnsureAxesHaveArea : IRenderAction
{
    public void Render(RenderPack rp)
    {
        foreach (CoordinateRange range in rp.Plot.Axes.GetAxes().Where(x => x.Range.Span == 0).Select(x => x.Range))
        {
            range.Min -= 1;
            range.Max += 1;
        }
    }
}
