namespace ScottPlot;

public class Multiplot
{
    /// <summary>
    /// Number of subplots in this multiplot
    /// </summary>
    public int Count => Subplots.Count;

    /// <summary>
    /// Copy styling options (e.g., background color) to new plots as they are added
    /// </summary>
    bool StyleNewPlotsAutomatically { get; set; } = true;

    /// <summary>
    /// If enabled, canvases passed into Render() methods will be cleared before plots are drawn on top of them.
    /// This is helpful for interactive multiplots with layouts containing blank spaces to ensure drawings from previous 
    /// renders do not persist through multiple renders where figure dimensions change.
    /// </summary>
    bool ClearCanvasBeforeRender { get; set; } = true;

    /// <summary>
    /// This list contains plots, logic for positioning them, and records of where they were last rendered
    /// </summary>
    private readonly List<PositionedSubplot> Subplots = [];
    private class PositionedSubplot(Plot plot, ISubplotPosition position)
    {
        public Plot Plot { get; set; } = plot;
        public PixelRect LastRenderRect { get; set; } = PixelRect.NaN;
        public AxisLimits LastRenderAxisLimits { get; set; } = AxisLimits.Unset; // TODO: support multi-axis
        public ISubplotPosition Position { get; set; } = position;
    }


    // TODO: improve support for plots with non-standard axis limits
    private readonly List<PositionedSubplot> PlotsWithSharedX = [];
    private readonly List<PositionedSubplot> PlotsWithSharedY = [];

    private IMultiplotLayout? _Layout = new MultiplotLayouts.Rows();

    /// <summary>
    /// This logic is used to create the initial layout for subplots in the multiplot
    /// </summary>
    public IMultiplotLayout? Layout
    {
        get => _Layout;
        set
        {
            _Layout = value;
            _Layout?.ResetAllPositions(this);
        }
    }

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
        if (StyleNewPlotsAutomatically && Subplots.Count > 0)
        {
            Plot lastPlot = Subplots.Last().Plot;
            plot.FigureBackground.Color = lastPlot.FigureBackground.Color;
            plot.DataBackground.Color = lastPlot.DataBackground.Color;
        }

        PositionedSubplot positionedPlot = new(plot, new SubplotPositions.Full());
        Subplots.Add(positionedPlot);
        Layout?.ResetAllPositions(this);
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
        return Subplots[index].Plot;
    }

    /// <summary>
    /// Return all plots in this multiplot
    /// </summary>
    public Plot[] GetPlots()
    {
        return Subplots.Select(x => x.Plot).ToArray();
    }

    /// <summary>
    /// Return the positioned subplot associated with the given plot
    /// </summary>
    private PositionedSubplot GetPositionedSubplot(Plot plot)
    {
        foreach (var subplot in Subplots)
        {
            if (subplot.Plot == plot)
            {
                return subplot;
            }
        }
        throw new KeyNotFoundException();
    }

    /// <summary>
    /// Set the position of the given subplot
    /// </summary>
    public void SetPosition(Plot plot, ISubplotPosition position)
    {
        for (int i = 0; i < Subplots.Count; i++)
        {
            if (Subplots[i].Plot == plot)
            {
                Subplots[i].Position = position;
            }
        }
    }

    /// <summary>
    /// Set the position of the given subplot index
    /// </summary>
    public void SetPosition(int plotIndex, ISubplotPosition position)
    {
        Subplots[plotIndex].Position = position;
    }

    /// <summary>
    /// Get the pixel rectangle where the given subplot was last rendered.
    /// Returns PixelRect.NaN if a render has not yet occurred.
    /// </summary>
    public PixelRect GetLastRenderRectangle(Plot plot)
    {
        foreach (var pos in Subplots)
        {
            if (pos.Plot == plot)
            {
                return pos.LastRenderRect;
            }
        }

        throw new KeyNotFoundException();
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
        UpdateSharedPlotAxisLimits();

        if (ClearCanvasBeforeRender)
        {
            canvas.Clear();
        }

        foreach (var positionedPlot in Subplots)
        {
            PixelRect subPlotRect = positionedPlot.Position.GetRect(figureRect);
            positionedPlot.LastRenderRect = subPlotRect;
            positionedPlot.LastRenderAxisLimits = positionedPlot.Plot.Axes.GetLimits();
            positionedPlot.Plot.RenderManager.ClearCanvasBeforeEachRender = false;
            positionedPlot.Plot.Render(canvas, subPlotRect);
        }
    }

    /// <summary>
    /// Create a new image, render the multiplot onto it, and return it
    /// </summary>
    public Image Render(int width, int height)
    {
        SKImageInfo imageInfo = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        SKSurface surface = SKSurface.Create(imageInfo);
        PixelRect rect = new(0, width, height, 0);
        Render(surface.Canvas, rect);
        return new(surface);
    }

    /// <summary>
    /// Save the multiplot as a PNG image file
    /// </summary>
    public SavedImageInfo SavePng(string filename, int width = 800, int height = 600)
    {
        return Render(width, height).SavePng(filename);
    }

    /// <summary>
    /// Return the plot beneath the given pixel according to the last render.
    /// Returns null if no render occurred or the pixel is not over a plot.
    /// </summary>
    public Plot? GetPlotAtPixel(Pixel pixel)
    {
        foreach (var positionedPlot in Subplots)
        {
            if (positionedPlot.LastRenderRect.Contains(pixel))
                return positionedPlot.Plot;
        }

        return null;
    }

    private void UpdateSharedPlotAxisLimits()
    {
        Plot? parentPlotX = GetFirstPlotWithChangedLimitsX();
        if (parentPlotX is not null)
        {
            AxisLimits parentLimits = parentPlotX.Axes.GetLimits();
            PlotsWithSharedX.ForEach(x => x.Plot.Axes.SetLimitsX(parentLimits));
        }

        Plot? parentPlotY = GetFirstPlotWithChangedLimitsY();
        if (parentPlotY is not null)
        {
            AxisLimits parentLimits = parentPlotY.Axes.GetLimits();
            PlotsWithSharedY.ForEach(x => x.Plot.Axes.SetLimitsY(parentLimits));
        }
    }

    private Plot? GetFirstPlotWithChangedLimitsX()
    {
        foreach (var positionedPlot in PlotsWithSharedX)
        {
            var oldRange = positionedPlot.Plot.Axes.GetLimits().HorizontalRange;
            var newRange = positionedPlot.LastRenderAxisLimits.HorizontalRange;
            if (oldRange != newRange)
            {
                return positionedPlot.Plot;
            }
        }
        return null;
    }

    private Plot? GetFirstPlotWithChangedLimitsY()
    {
        foreach (var positionedPlot in PlotsWithSharedY)
        {
            var oldRange = positionedPlot.Plot.Axes.GetLimits().VerticalRange;
            var newRange = positionedPlot.LastRenderAxisLimits.VerticalRange;
            if (oldRange != newRange)
            {
                return positionedPlot.Plot;
            }
        }
        return null;
    }

    /// <summary>
    /// Link horizontal axis limits of the given collection of plots
    /// so when one changes they all change in unison
    /// </summary>
    public void ShareX(IEnumerable<Plot> plots)
    {
        PlotsWithSharedX.Clear();
        PlotsWithSharedX.AddRange(plots.Select(GetPositionedSubplot));

        // reset remembered axis limits to force realignment on the next render
        foreach (Plot plot in plots)
        {
            GetPositionedSubplot(plot).LastRenderAxisLimits = AxisLimits.Unset;
        }
    }

    /// <summary>
    /// Link vertical axis limits of the given collection of plots
    /// so when one changes they all change in unison
    /// </summary>
    public void ShareY(IEnumerable<Plot> plots)
    {
        PlotsWithSharedY.Clear();
        PlotsWithSharedY.AddRange(plots.Select(GetPositionedSubplot));

        // reset remembered axis limits to force realignment on the next render
        foreach (Plot plot in plots)
        {
            GetPositionedSubplot(plot).LastRenderAxisLimits = AxisLimits.Unset;
        }
    }
}
