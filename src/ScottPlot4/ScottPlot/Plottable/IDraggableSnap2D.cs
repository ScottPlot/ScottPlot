namespace ScottPlot.Plottable;

/// <summary>
/// This interface describes draggable plot types which snap to points in X/Y space
/// </summary>
public interface IDraggableSnap2D
{
    public SnapLogic.ISnap2D DragSnapXY { get; set; }
}
