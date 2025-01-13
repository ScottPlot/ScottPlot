namespace ScottPlot;

public interface IMultiplot
{
    void Reset(Plot plot);
    void Render(SKSurface surface);
    void Render(SKCanvas canvas, PixelRect figureRect);
    Image Render(int width, int height);
    Plot? GetPlotAtPixel(Pixel pixel);
}
