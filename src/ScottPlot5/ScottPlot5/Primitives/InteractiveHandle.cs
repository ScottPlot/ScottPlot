namespace ScottPlot;

public class InteractiveHandle(IHasInteractiveHandles parent, Cursor cursor, int index = 0)
{
    /// <summary>
    /// The plottable this handle belongs to.
    /// This is useful when events pass handles themselves.
    /// </summary>
    public IHasInteractiveHandles Parent { get; } = parent;

    /// <summary>
    /// The index that uniquely identifies this handle.
    /// This value is only useful for interactive objects with multiple handles.
    /// </summary>
    public int Index { get; } = index;

    /// <summary>
    /// Cursor the control should use when interacting with this handle
    /// </summary>
    public Cursor Cursor { get; } = cursor;

    public override string ToString() => $"{Parent.GetType()} handle {Index}";
}
