namespace ScottPlot.Interactivity;

/// <summary>
/// Structures for commonly used mouse buttons.
/// Use these as a safer alternative to instantiating your own.
/// </summary>
public static class StandardMouseButtons
{
    public readonly static MouseButton Left = new("left");
    public readonly static MouseButton Right = new("right");
    public readonly static MouseButton Middle = new("middle");
    public readonly static MouseButton Wheel = new("wheel");
}
