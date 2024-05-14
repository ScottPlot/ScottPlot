namespace ScottPlot;

public interface IStarAxis
{
    LineStyle AxisStyle { get; set; }

    public void Render(RenderPack rp, IAxes axes, double maxSpokeLength, int numSpokes, float rotationDegrees = 0);
}
