namespace ScottPlot;

public class Multiplot
{
    public List<Plot> Plots { get; } = [];
    public IMultiplotLayout Layout { get; set; } = new MultiplotLayouts.Rows();

    public Multiplot()
    {

    }

    public void AddPlot(Plot plot) => Plots.Add(plot);

    public Plot AddPlot()
    {
        Plot plot = new();
        Plots.Add(plot);
        return plot;
    }

    public void AddPlots(IEnumerable<Plot> plots) => Plots.AddRange(plots);

    public void Render(SKSurface surface)
    {
        Render(surface.Canvas, surface.Canvas.LocalClipBounds.ToPixelRect());
    }

    public void Render(SKCanvas canvas, PixelRect figureRect)
    {
        foreach ((FractionRect fracRect, Plot plot) in Layout.GetLayout(Plots))
        {
            PixelRect subPlotRect = fracRect.GetPixelRect((int)figureRect.Width, (int)figureRect.Height);
            plot.RenderManager.ClearCanvasBeforeEachRender = false;
            plot.Render(canvas, subPlotRect);
        }
    }

    public Image Render(int width, int height)
    {
        SKImageInfo imageInfo = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        SKSurface surface = SKSurface.Create(imageInfo);
        PixelRect rect = new(0, width, height, 0);
        Render(surface.Canvas, rect);
        return new(surface);
    }

    public SavedImageInfo SavePng(string filename, int width = 800, int height = 600)
    {
        return Render(width, height).SavePng(filename);
    }
}
