namespace ScottPlot.Control.EventProcess
{
    /// <summary>
    /// There is one event type for every event class in the Events namespace.
    /// Events are described in the XML documentation for the class (not this enum).
    /// </summary>
    public enum EventType
    {
        BenchmarkToggle,
        ApplyZoomRectangle,
        MouseAutoAxis,
        MouseMovedToZoomRectangle,
        MousePan,
        MouseScroll,
        MouseupClearRender,
        MouseZoom,
        PlottableDrag,
    }
}
