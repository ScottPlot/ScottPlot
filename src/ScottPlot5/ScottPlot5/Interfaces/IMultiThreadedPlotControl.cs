namespace ScottPlot;

public interface IMultiThreadedPlotControl
{
    /// <summary>
    /// Returns only when the plot is successfully locked and rendering has stopped.
    /// </summary>
    void Lock();

    /// <summary>
    /// Releases the plot lock and permits rendering again.
    /// </summary>
    void UnLock();

    /// <summary>
    /// Attempt to lock the plot and return whether a lock was achieved.
    /// If true is returned, the plot is locked and rendering has stopped.
    /// If false is returned, the plot was not successfully locked and rendering is permitted.
    /// </summary>
    bool TryLock();

    /// <summary>
    /// Returns whether true if the plot currently locked.
    /// </summary>
    void IsLocked();

    /// <summary>
    /// Returns whether true if the plot is locked by the calling thread.
    /// </summary>
    void IsEntered();
}
