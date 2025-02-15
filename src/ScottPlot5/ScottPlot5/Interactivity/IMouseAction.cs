namespace ScottPlot.Interactivity;

/// <summary>
/// User actions that occur at a point in pixel space
/// </summary>
public interface IMouseAction : IUserAction
{
    public Pixel Pixel { get; }
}
