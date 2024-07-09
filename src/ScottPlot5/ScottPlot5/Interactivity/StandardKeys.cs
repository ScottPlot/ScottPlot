using ScottPlot.Interactivity.Keys;

namespace ScottPlot.Interactivity;

public static class StandardKeys
{
    public static readonly IKey Alt = new AltKey();
    public static readonly IKey Control = new ControlKey();
    public static readonly IKey Shift = new ShiftKey();

    public static readonly IKey Down = new DownKey();
    public static readonly IKey Up = new UpKey();
    public static readonly IKey Left = new LeftKey();
    public static readonly IKey Right = new RightKey();
}
