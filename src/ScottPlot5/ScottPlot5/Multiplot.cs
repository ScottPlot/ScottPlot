namespace ScottPlot;

public class PositionedSubplot(Plot plot, ISubplotPosition position)
{
    public Plot Plot { get; set; } = plot;
    public PixelRect LastRenderRect { get; set; } = PixelRect.NaN;
    public ISubplotPosition Position { get; set; } = position;
}

public class Multiplot
{
    public int Count => PositionedPlots.Count;
    public IEnumerable<Plot> Plots => PositionedPlots.Select(x => x.Plot);
    public List<PositionedSubplot> PositionedPlots { get; } = [];
    bool StyleNewPlotsAutomatically { get; set; } = true;

    /// <summary>
    /// This engine is used to resize all plots automatically every time new ones are added
    /// </summary>
    public IMultiplotLayout? Layout { get; set; } = new ScottPlot.MultiplotLayouts.Rows();

    public Multiplot()
    {

    }

    public Multiplot(Plot plot)
    {
        AddPlot(plot);
    }

    public void Reset(Plot plot)
    {
        PositionedPlots.Clear();
        AddPlot(plot);
    }

    public Plot AddPlot()
    {
        Plot plot = new();
        AddPlot(plot);
        return plot;
    }

    public void AddPlot(Plot plot)
    {
        if (StyleNewPlotsAutomatically && PositionedPlots.Count > 0)
        {
            Plot lastPlot = PositionedPlots.Last().Plot;
            plot.FigureBackground.Color = lastPlot.FigureBackground.Color;
            plot.DataBackground.Color = lastPlot.DataBackground.Color;
        }

        PositionedSubplot positionedPlot = new(plot, new SubplotPositions.Full());
        PositionedPlots.Add(positionedPlot);
        Layout?.ResetAllPositions(this);
    }

    public void Render(SKSurface surface)
    {
        Render(surface.Canvas, surface.Canvas.LocalClipBounds.ToPixelRect());
    }

    public void Render(SKCanvas canvas, PixelRect figureRect)
    {
        foreach (var positionedPlot in PositionedPlots)
        {
            PixelRect subPlotRect = positionedPlot.Position.GetRect(figureRect);
            positionedPlot.LastRenderRect = subPlotRect;
            positionedPlot.Plot.RenderManager.ClearCanvasBeforeEachRender = false;
            positionedPlot.Plot.Render(canvas, subPlotRect);
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
        foreach (var positionedPlot in PositionedPlots)
        {
            if (positionedPlot.LastRenderRect.Contains(pixel))
                return positionedPlot.Plot;
        }

        return null;
    }
}
