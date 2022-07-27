namespace ScottPlot.Control;

public interface IPlotControl
{
    /// <summary>
    /// The <see cref="Plot"/> displayed by this interactive control
    /// </summary>
    Plot Plot { get; }

    /// <summary>
    /// Request a re-render of the <see cref="Plot"/>
    /// </summary>
    void Refresh();

    /// <summary>
    /// Configuration object used to configure how mouse and keyboard events affect the plot
    /// </summary>
    Backend Backend { get; set; }
}
