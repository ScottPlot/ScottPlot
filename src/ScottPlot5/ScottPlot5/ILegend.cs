namespace ScottPlot;

public interface ILegend
{
    void Render(SKCanvas canvas, PixelRect dataRect, LegendItem[] items);
}
