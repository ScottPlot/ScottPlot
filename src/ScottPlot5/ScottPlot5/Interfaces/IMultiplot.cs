namespace ScottPlot;

public interface IMultiplot
{
    /// <summary>
    /// Number of subplots in this multiplot
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Add (or remove) plots until the given number of subplots is achieved
    /// </summary>
    Plot[] AddPlots(int total);

    /// <summary>
    /// Return the plot at the given index
    /// </summary>
    Plot GetPlot(int index);

    /// <summary>
    /// Return all plots in this multiplot
    /// </summary>
    Plot[] GetPlots();

    /// <summary>
    /// This logic is used to create the initial layout for subplots in the multiplot
    /// </summary>
    IMultiplotLayout? Layout { get; set; }

    /// <summary>
    /// Set the position of the subplot at the given index
    /// </summary>
    void SetPosition(int index, ISubplotPosition position);

    /// <summary>
    /// Link horizontal axis limits of the given collection of plots
    /// so when one changes they all change in unison
    /// </summary>
    void ShareX(IEnumerable<Plot> plots);

    /// <summary>
    /// Link vertical axis limits of the given collection of plots
    /// so when one changes they all change in unison
    /// </summary>
    void ShareY(IEnumerable<Plot> plots);

    /// <summary>
    /// Return the plot beneath the given pixel according to the last render.
    /// Returns null if no render occurred or the pixel is not over a plot.
    /// </summary>
    Plot? GetPlotAtPixel(Pixel pixel);

    void Reset(Plot plot);
    void Render(SKSurface surface);
    void Render(SKCanvas canvas, PixelRect figureRect);
    Image Render(int width, int height);
}
