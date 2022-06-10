namespace ScottPlot;

public interface IPlottable
{
    public bool IsVisible { get; set; }

    public void Render(SkiaSharp.SKSurface surface, PixelRect dataRect, HorizontalAxis xAxis, VerticalAxis yAxis);
}