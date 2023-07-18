namespace ScottPlot.Rendering.RenderActions;

public class EnsureAxesHaveArea : IRenderAction
{
    public void Render(RenderPack rp)
    {
        foreach (CoordinateRange range in rp.Plot.GetAllAxes().Where(x => x.Range.Span == 0).Select(x => x.Range))
        {
            range.Min -= 1;
            range.Max += 1;
        }
    }
}
