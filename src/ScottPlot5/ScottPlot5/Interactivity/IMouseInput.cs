namespace ScottPlot.Interactivity;

/// <summary>
/// A user input caused by the mouse with a cursor at a specific pixel
/// </summary>
public interface IMouseInput : IUserAction
{
    public Pixel Pixel { get; }
    public DateTime DateTime { get; }
}
