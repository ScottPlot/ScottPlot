namespace ScottPlot;

public class InteractiveHandle(IHasInteractiveHandles parent, int index, Cursor cursor)
{
    /// <summary>
    /// The plottable this handle belongs to.
    /// This is useful when events pass handles themselves.
    /// </summary>
    public IHasInteractiveHandles Parent { get; } = parent;

    /// <summary>
    /// The index of this handle (useful for plots with multiple handles)
    /// </summary>
    public int Index { get; } = index;

    /// <summary>
    /// Cursor the control should use when interacting with this handle
    /// </summary>
    public Cursor Cursor { get; } = cursor;

    public override string ToString() => $"{Parent.GetType()} handle {Index}";
}
