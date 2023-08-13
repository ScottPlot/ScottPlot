namespace ScottPlot;

/// <summary>
/// Holds multiple <see cref="Plot"/> objects and contains logic for arranging them on a single canvas
/// </summary>
public class Multiplot
{
    private readonly List<Plot> Plots = new();
    public int PlotCount => Plots.Count;
    public Plot[] GetPlots() => Plots.ToArray();
    public Color BackgroundColor { get; set; } = Colors.White;

    /// <summary>
    /// If set, the layout of this plot will be applied to all other plots
    /// </summary>
    public Plot? SharedLayoutSourcePlot { get; set; } = null;

    /// <summary>
    /// This object holds logic for arranging multiple plots on a canvas
    /// </summary>
    public IMultiplotLayout Layout { get; set; } = new MultiplotLayouts.TileHorizontally();

    /// <summary>
    /// Add the given plot to the multiplot
    /// </summary>
    public void Add(Plot plot)
    {
        plot.RenderManager.ClearCanvasBeforeRendering = false;
        Plots.Add(plot);
    }

    /// <summary>
    /// Create a new plot, and it to the multiplot, and return it
    /// </summary>
    public Plot Add()
    {
        Plot plot = new();
        Add(plot);
        return plot;
    }

    /// <summary>
    /// Arrange subplots according to <see cref="Layout"/> and render them all
    /// </summary>
    public void Render(SKCanvas canvas, PixelRect figureRect)
    {
        var positionedPlots = Layout.GetPositionedPlots(Plots, figureRect);

        if (SharedLayoutSourcePlot is not null)
        {
            var parent = positionedPlots.Where(x => x.Plot == SharedLayoutSourcePlot).Single();
            parent.Render(canvas);
            PixelPadding padding = parent.Plot.RenderManager.LastRender.Padding;

            foreach (var child in positionedPlots)
            {
                if (child.Plot == SharedLayoutSourcePlot)
                    continue;

                child.Plot.FixedLayout(padding);
            }
        }

        canvas.Clear(BackgroundColor.ToSKColor());

        positionedPlots.ForEach(x => x.Render(canvas));
    }
}
