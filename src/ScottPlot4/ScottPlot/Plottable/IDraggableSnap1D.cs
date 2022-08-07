namespace ScottPlot.Plottable;

/// <summary>
/// This interface describes draggable plot types which can snap to points a single axis
/// </summary>
public interface IDraggableSnap1D
{
    public SnapLogic.ISnap DragSnapX { get; set; }
    public SnapLogic.ISnap DragSnapY { get; set; }
}
