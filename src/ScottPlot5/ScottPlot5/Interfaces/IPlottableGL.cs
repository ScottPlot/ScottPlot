namespace ScottPlot;

/// <summary>
/// This interface is applied to plottables which can be rendered directly on the GPU using an OpenGL shader
/// </summary>
public interface IPlottableGL : IPlottable
{
    /// <summary>
    /// The control used to display this plottable.
    /// It is used to access the <see cref="GRContext"/> at render time.
    /// </summary>
    IPlotControl PlotControl { get; }

    /// <summary>
    /// Used to manually synchronize rendering
    /// </summary>
    void GLFinish();
    /// <summary>
    /// Store OpenGL atributes
    /// </summary>
    void StoreGLState();
    /// <summary>
    /// Restore previously saved OpenGL atributes
    /// </summary>
    void RestoreGLState();
}
