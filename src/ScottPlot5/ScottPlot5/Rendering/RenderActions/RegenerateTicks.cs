namespace ScottPlot.Rendering.RenderActions;

public class RegenerateTicks : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.Axes.Bottom.TickGenerator.Regenerate(
            range: rp.Plot.Axes.Bottom.Range.ToRectifiedCoordinateRange,
            edge: rp.Plot.Axes.Bottom.Edge,
            size: rp.DataRect.Width);

        rp.Plot.Axes.Left.TickGenerator.Regenerate(
            range: rp.Plot.Axes.Left.Range.ToRectifiedCoordinateRange,
            edge: rp.Plot.Axes.Left.Edge,
            size: rp.DataRect.Height);
    }
}
