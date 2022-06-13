namespace ScottPlot;

public interface ITickGenerator
{
    Tick[] GenerateTicks(double min, double max, float edgeSize);
}
