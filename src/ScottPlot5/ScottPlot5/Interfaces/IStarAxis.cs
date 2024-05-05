namespace ScottPlot;

public interface IStarAxis
{
    LineStyle AxisStyle { get; set; }

    public void Render(RenderPack rp, IAxes axes, IReadOnlyList<double> values, float rotationDegrees = 0);
}
