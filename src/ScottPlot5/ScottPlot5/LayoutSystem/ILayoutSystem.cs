namespace ScottPlot.LayoutSystem;

/// <summary>
/// This interface describes a class that decides how to lay-out a collection of panels around the
/// edges of a figure and create a final layout containing size and position of all panels
/// and also the size and position of the data area.
/// </summary>
public interface ILayoutSystem
{
    public FinalLayout GetLayout(PixelRect figureRect, IEnumerable<IPanel> panels);
}
