namespace ScottPlot.Interactivity;

public interface IMouseButtonAction : IMouseAction
{
    MouseButton Button { get; }
    public bool IsPressed { get; }
}
