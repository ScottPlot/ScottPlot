namespace ScottPlot;

public interface ITickGenerator
{
    Tick[] GenerateTicks(CoordinateRange range, PixelLength size);
}
