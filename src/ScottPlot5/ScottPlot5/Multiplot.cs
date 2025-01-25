namespace ScottPlot;

public class Multiplot : IMultiplot
{
    /// <summary>
    /// Number of subplots in this multiplot
    /// </summary>
    public int Count => Subplots.Count;

    public MultiplotSharedAxisManager SharedAxes { get; } = new();

    /// <summary>
    /// This list contains plots, logic for positioning them, and records of where they were last rendered
    /// </summary>
    private readonly List<Plot> Subplots = [];

    /// <summary>
    /// This logic is used to create the initial layout for subplots in the multiplot
    /// </summary>
    public IMultiplotLayout Layout { get; set; } = new MultiplotLayouts.Rows();

    /// <summary>
    /// Create a multiplot with no initial subplots
    /// </summary>
    public Multiplot()
    {

    }

    /// <summary>
    /// Create a multiplot with a single subplot
    /// </summary>
    public Multiplot(Plot plot)
    {
        AddPlot(plot);
    }

    public void RemovePlot(Plot plot)
    {
        Subplots.Remove(plot);
        ForgetLastRender(plot);
    }

    #region last render state

    // TODO: wrap this in a class
    private readonly Dictionary<Plot, PixelRect> LastRenderRect = [];
    private readonly Dictionary<Plot, AxisLimits> LastRenderAxisLimits = []; // TODO: support multi-axis

    public PixelRect? GetLastRenderRectangle(Plot plot) => LastRenderRect[plot];

    private void ForgetLastRender(Plot plot)
    {
        LastRenderRect.Remove(plot);
        LastRenderAxisLimits.Remove(plot);
    }

    #endregion

    /// <summary>
    /// Reset this multiplot so it only contains the given plot
    /// </summary>
    public void Reset(Plot plot)
    {
        Subplots.Clear();
        AddPlot(plot);
    }

    /// <summary>
    /// Create a new plot, add it as a subplot, and return it
    /// </summary>
    public Plot AddPlot()
    {
        Plot plot = new();
        AddPlot(plot);
        return plot;
    }

    /// <summary>
    /// Add the given plot as a subplot into this multiplot
    /// </summary>
    public void AddPlot(Plot plot)
    {
        if (Subplots.Count > 0)
        {
            plot.PlotControl = Subplots.First().PlotControl;
        }

        if (Subplots.Count > 0)
        {
            Plot lastPlot = Subplots.Last();
            plot.FigureBackground.Color = lastPlot.FigureBackground.Color;
            plot.DataBackground.Color = lastPlot.DataBackground.Color;
        }

        Subplots.Add(plot);
    }

    /// <summary>
    /// Add (or remove) plots until the given number of subplots is achieved
    /// </summary>
    public Plot[] AddPlots(int total)
    {
        while (Count > total)
        {
            Subplots.RemoveAt(Subplots.Count - 1);
        }

        while (Count < total)
        {
            AddPlot();
        }

        return GetPlots();
    }

    /// <summary>
    /// Return the plot at the given index
    /// </summary>
    public Plot GetPlot(int index)
    {
        return Subplots[index];
    }

    /// <summary>
    /// Return all plots in this multiplot
    /// </summary>
    public Plot[] GetPlots()
    {
        return Subplots.ToArray();
    }

    /// <summary>
    /// Render the multiplot into the clip boundary of the given surface.
    /// </summary>
    public void Render(SKSurface surface)
    {
        Render(surface.Canvas, surface.Canvas.LocalClipBounds.ToPixelRect());
    }

    /// <summary>
    /// Render the multiplot on a canvas inside the given rectangle.
    /// </summary>
    public void Render(SKCanvas canvas, PixelRect figureRect)
    {
        SharedAxes.UpdateSharedPlotAxisLimits();

        canvas.Clear();

        PixelRect[] subplotRectangles = Layout.GetSubplotRectangles(this, figureRect);
        if (subplotRectangles.Length != Subplots.Count)
        {
            throw new InvalidOperationException($"Layout returned {subplotRectangles.Length} rectangles for {Subplots.Count} subplots");
        }

        for (int i = 0; i < Subplots.Count; i++)
        {
            Plot plot = Subplots[i];
            plot.RenderManager.ClearCanvasBeforeEachRender = false;
            plot.Render(canvas, subplotRectangles[i]);

            LastRenderRect[plot] = subplotRectangles[i];
            LastRenderAxisLimits[plot] = plot.Axes.GetLimits();
        }
    }

    /// <summary>
    /// Return the plot beneath the given pixel according to the last render.
    /// Returns null if no render occurred or the pixel is not over a plot.
    /// </summary>
    public Plot? GetPlotAtPixel(Pixel pixel)
    {
        foreach (var entry in LastRenderRect)
        {
            if (entry.Value.Contains(pixel))
            {
                return entry.Key;
            }
        }

        return null;
    }
}
