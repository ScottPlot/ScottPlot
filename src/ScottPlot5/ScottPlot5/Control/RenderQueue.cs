namespace ScottPlot.Control;

public class RenderQueue
{
    private readonly Queue<IPlotControl> ControlsNeedingRefresh = new();

    /// <summary>
    /// Render the <paramref name="other"/> plot after this control is rendered next.
    /// This method can invoked from inside events where calling <see cref="Refresh"/> 
    /// would cause infinite loops or artifacts from two controls rendering at the same time.
    /// </summary>
    public void Enqueue(IPlotControl other)
    {
        if (!ControlsNeedingRefresh.Contains(other))
        {
            ControlsNeedingRefresh.Enqueue(other);
        }
    }

    /// <summary>
    /// Refresh and remove all controls in the queue.
    /// </summary>
    public void RefreshAll()
    {
        while (ControlsNeedingRefresh.Any())
        {
            ControlsNeedingRefresh.Dequeue().Refresh();
        }
    }
}
