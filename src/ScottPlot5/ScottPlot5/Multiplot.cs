using System.Reflection.Emit;

namespace ScottPlot;

public class Multiplot
{
    public List<Plot> Plots { get; } = [];
    public IMultiplotLayout Layout { get; set; } = new MultiplotLayouts.Rows();
    private readonly List<(PixelRect, Plot)> LastRenderedRectangles = []; // TODO: replace with a custom manager

    public Multiplot()
    {

    }

    public Multiplot(Plot initialPlot)
    {
        Plots.Add(initialPlot);
    }

    public Multiplot(IEnumerable<Plot> initialPlots)
    {
        foreach (Plot plot in initialPlots)
        {
            Plots.Add(plot);
        }
    }

    public void Reset(Plot plot)
    {
        Plots.Clear();
        Plots.Add(plot);
    }

    public void AddPlot(Plot plot) => Plots.Add(plot);

    public Plot AddPlot(bool matchStyle = true)
    {
        Plot plot = new();

        if (matchStyle && Plots.Count > 0)
        {
            Plot previous = Plots.Last();
            plot.DataBackground.Color = previous.DataBackground.Color;
            plot.FigureBackground.Color = previous.FigureBackground.Color;
        }

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
        LastRenderedRectangles.Clear();

        foreach ((FractionRect fracRect, Plot plot) in Layout.GetLayout(Plots))
        {
            PixelRect subPlotRect = fracRect.GetPixelRect((int)figureRect.Width, (int)figureRect.Height);
            LastRenderedRectangles.Add((subPlotRect, plot));
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

    public Plot? GetPlotAtPixel(Pixel pixel)
    {
        foreach ((PixelRect rect, Plot plot) in LastRenderedRectangles)
        {
            if (rect.Contains(pixel))
                return plot;
        }

        return null;
    }
}
