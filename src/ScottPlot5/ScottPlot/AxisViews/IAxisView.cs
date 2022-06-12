namespace ScottPlot.AxisViews;

public interface IAxisView
{
    public Axes.IAxis Axis { get; }
    public void Render(SkiaSharp.SKSurface surface, PixelRect dataRect);
}
