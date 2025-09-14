namespace ScottPlot;

/// <summary>
/// Allows sharing of information across multiple plots in a Multiplot.
/// </summary>
/// <remarks>
/// Multiplot maintains a list of IShareableManagers and calls Update() on each
/// prior to rendering each frame. This allows shareable managers to coordinate
/// state across multiple plots in the multiplot.
/// </remarks>
public interface IShareableManager
{
    public void Update();
}
