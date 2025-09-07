namespace ScottPlot;

public interface IHasInteractiveHandles
{
    /// <summary>
    /// Cursor displayed when the cursor hovers over a handle
    /// </summary>
    public Cursor HoverCursor { get; set; }

    /// <summary>
    /// Return the interactive handle inside the rectangle
    /// </summary>
    public InteractiveHandle? GetHandle(CoordinateRect rect);

    /// <summary>
    /// Called when the given handle has been pressed (typically the start of a drag)
    /// </summary>
    public void PressHandle(InteractiveHandle handle);

    /// <summary>
    /// Called when the given handle has been moved (typically the result of a click-drag)
    /// </summary>
    public void MoveHandle(InteractiveHandle handle, Coordinates point);

    /// <summary>
    /// Called when the given handle has been released (typically the end of a drag)
    /// </summary>
    public void ReleaseHandle(InteractiveHandle handle);
}
