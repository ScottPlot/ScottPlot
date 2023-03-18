namespace ScottPlot.OpenGL;

/// <summary>
/// Defines behavior to use when OpenGL is not available
/// </summary>
public enum GLFallbackRenderStrategy
{
    /// <summary>
    /// Do not render anything
    /// </summary>
    Skip,

    /// <summary>
    /// Use software rendering (may be slow)
    /// </summary>
    Software,
}
