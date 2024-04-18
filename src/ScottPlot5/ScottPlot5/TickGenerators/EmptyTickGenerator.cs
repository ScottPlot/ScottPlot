namespace ScottPlot.TickGenerators;

public class EmptyTickGenerator : ITickGenerator
{
    public Tick[] Ticks { get; set; } = Array.Empty<Tick>();

    public int MaxTickCount { get; set; } = 50;

    public EmptyTickGenerator()
    {
    }

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, Label labelStyle)
    {
    }
}
