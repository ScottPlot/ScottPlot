namespace ScottPlot.Interactivity;

/// <summary>
/// Mouse actions that describe a button changing state
/// </summary>
public interface IMouseButtonAction : IMouseAction
{
    MouseButton Button { get; }
    public bool IsPressed { get; }
}
