namespace ScottPlot;

/// <summary>
/// Allows sharing of information across multiple plots in an <see cref="IMultiplot"/>.
/// </summary>
public interface ISubplotPreRenderAction
{
    /// <summary>
    /// Called before each render for every shareable manager in an <see cref="IMultiplot"/>.
    /// This allows shareable managers to coordinate state across multiple plots in the multiplot.
    /// </summary>
    public void Invoke();
}
