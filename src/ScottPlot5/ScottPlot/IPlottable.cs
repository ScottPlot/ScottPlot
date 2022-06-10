namespace ScottPlot;

public interface IPlottable
{
    public void Render(SkiaSharp.SKSurface surface, PixelRect dataRect, HorizontalAxis xAxis, VerticalAxis yAxis);
}