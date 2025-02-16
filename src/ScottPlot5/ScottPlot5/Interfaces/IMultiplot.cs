namespace ScottPlot;

public interface IMultiplot
{
    SubplotCollection Subplots { get; }

    /// <summary>
    /// This logic is used at render time to place subplots 
    /// within the rectangle containing the entire multiplot figure.
    /// </summary>
    IMultiplotLayout Layout { get; set; }

    /// <summary>
    /// Stores state about previous renders that can be used
    /// to determine plots at specific pixel positions.
    /// </summary>
    MultiplotLayoutSnapshot LastRender { get; }

    /// <summary>
    /// Logic for linking subplot axis limits together.
    /// </summary>
    public MultiplotSharedAxisManager SharedAxes { get; }

    /// <summary>
    /// Render this multiplot onto the given canvas using a layout
    /// created to fit inside the given rectangle.
    /// </summary>
    void Render(SKCanvas canvas, PixelRect figureRect);
}

public static class IMultiplotExtensions
{
    /// <summary>
    /// Reset this multiplot so it only contains the given plot
    /// </summary>
    public static void Reset(this IMultiplot multiplot, Plot plot)
    {
        multiplot.LastRender.Reset();
        multiplot.AddPlots(0);
        multiplot.AddPlot(plot);
    }

    /// <summary>
    /// Reset this multiplot so it only contains the first plot
    /// </summary>
    public static void Reset(this IMultiplot multiplot)
    {
        multiplot.AddPlots(1);
    }

    /// <summary>
    /// Create a new image, render the multiplot onto it, and return it
    /// </summary>
    public static Image Render(this IMultiplot multiplot, int width, int height)
    {
        SKImageInfo imageInfo = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        SKSurface surface = SKSurface.Create(imageInfo);
        PixelRect rect = new(0, width, height, 0);
        multiplot.Render(surface.Canvas, rect);
        return new(surface);
    }

    /// <summary>
    /// Render the multiplot into the clip boundary of the given surface.
    /// </summary>
    public static void Render(this IMultiplot multiplot, SKSurface surface)
    {
        multiplot.Render(surface.Canvas, surface.Canvas.LocalClipBounds.ToPixelRect());
    }

    /// <summary>
    /// Save the multiplot as a PNG image file
    /// </summary>
    public static SavedImageInfo SavePng(this IMultiplot multiplot, string filename, int width = 800, int height = 600)
    {
        return multiplot.Render(width, height).SavePng(filename);
    }

    /// <summary>
    /// Create a new plot, add it to the collection of subplots, and return it
    /// </summary>
    public static Plot AddPlot(this IMultiplot multiplot)
    {
        Plot plot = new();
        multiplot.AddPlot(plot);
        return plot;
    }

    /// <summary>
    /// Add (or remove) plots until the target number of subplots is achieved
    /// </summary>
    public static void AddPlots(this IMultiplot multiplot, int total)
    {
        while (multiplot.Count() < total)
        {
            multiplot.AddPlot();
        }

        while (multiplot.Count() > total)
        {
            multiplot.RemovePlot(multiplot.GetPlots().Last());
        }
    }

    /// <summary>
    /// Return the plot beneath the given pixel according to the last render.
    /// Returns null if no render occurred or the pixel is not over a plot.
    /// </summary>
    public static Plot? GetPlotAtPixel(this IMultiplot multiplot, Pixel pixel)
    {
        return multiplot.LastRender.GetPlotAtPixel(pixel);
    }

    /// <summary>
    /// Number of subplots in this multiplot
    /// </summary>
    public static int Count(this IMultiplot multiplot)
    {
        return multiplot.Subplots.Count;
    }

    /// <summary>
    /// Add the given plot to the collection of subplots
    /// </summary>
    public static void AddPlot(this IMultiplot multiplot, Plot plot)
    {
        multiplot.Subplots.Add(plot);
    }

    /// <summary>
    /// Remove the given plot from the collection of subplots
    /// </summary>
    public static void RemovePlot(this IMultiplot multiplot, Plot plot)
    {
        multiplot.Subplots.Remove(plot);
    }

    /// <summary>
    /// Return the plot at the given index
    /// </summary>
    public static Plot GetPlot(this IMultiplot multiplot, int index)
    {
        return multiplot.Subplots.GetPlot(index);
    }

    /// <summary>
    /// Return all plots in this multiplot
    /// </summary>
    public static Plot[] GetPlots(this IMultiplot multiplot)
    {
        return multiplot.Subplots.GetPlots();
    }

    /// <summary>
    /// Set bottom and top axis size to zero between each subplot
    /// so plots can be stacked vertically without any space between them.
    /// </summary>
    public static void CollapseVertically(this IMultiplot multiplot)
    {
        Plot[] plots = multiplot.Subplots.GetPlots();

        foreach (Plot plot in plots)
        {
            bool collapseAbove = plot != plots.First();
            bool collapseBelow = plot != plots.Last();

            if (collapseAbove)
            {
                plot.Axes.GetAxes().Where(x => x.Edge == Edge.Top).ToList().ForEach(x => x.Collapse());
            }

            if (collapseBelow)
            {
                plot.Axes.GetAxes().Where(x => x.Edge == Edge.Bottom).ToList().ForEach(x => x.Collapse());
            }
        }
    }
}
