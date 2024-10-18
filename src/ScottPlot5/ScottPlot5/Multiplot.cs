namespace ScottPlot;

public class Multiplot
{
    public List<Plot> Plots { get; } = [];
    public IMultiplotLayout Layout { get; set; } = new MultiplotLayouts.Rows();

    public Multiplot()
    {

    }

    public Multiplot(IEnumerable<Plot> plots)
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

    public Image Render(int width, int height)
    {
        SKImageInfo imageInfo = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        SKSurface surface = SKSurface.Create(imageInfo);

        foreach ((FractionRect rect, Plot plot) in Layout.GetLayout(Plots))
        {
            PixelRect pxRect = rect.GetPixelRect(width, height);
            plot.RenderManager.ClearCanvasBeforeEachRender = false;
            plot.Render(surface.Canvas, pxRect);
        }

        return new(surface);
    }

    public SavedImageInfo SavePng(string filename, int width = 800, int height = 600)
    {
        return Render(width, height).SavePng(filename);
    }
}
