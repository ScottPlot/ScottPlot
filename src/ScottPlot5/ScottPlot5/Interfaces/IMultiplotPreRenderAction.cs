namespace ScottPlot;

/// <summary>
/// An action that may be included in the collection of <see cref="IMultiplot.PreRenderActions"/>
/// which gets invoked at the start of a multiplot render, before any subplots are rendered.
/// Useful for coordinating information across subplots (e.g., shared axes or plottable positions).
/// </summary>
public interface IMultiplotPreRenderAction
{
    /// <summary>
    /// Called at the start of a multiplot render, before any of the subplots are rendered.
    /// </summary>
    public void Invoke();
}
