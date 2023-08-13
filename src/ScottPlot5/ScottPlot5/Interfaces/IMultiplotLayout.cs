namespace ScottPlot;

/// <summary>
/// This interface describes a class that holds logic for arranging subplots in a figure.
/// </summary>
public interface IMultiplotLayout
{
    /// <summary>
    /// Given a collection of plots and the size of the figure,
    /// use the logic in this class to arrange plots accordingly,
    /// then return the plots paired with positioning information.
    /// </summary>
    List<PositionedPlot> GetPositionedPlots(List<Plot> plots, PixelRect figureRect);
}
