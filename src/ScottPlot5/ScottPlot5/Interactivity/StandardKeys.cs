namespace ScottPlot.Interactivity;

/// <summary>
/// Structures for commonly used keys.
/// Use these as a safer alternative to instantiating your own.
/// </summary>
public static class StandardKeys
{
    public static readonly Key Alt = new("alt");
    public static readonly Key Control = new("ctrl");
    public static readonly Key Shift = new("shift");

    public static readonly Key Down = new("down");
    public static readonly Key Up = new("up");
    public static readonly Key Left = new("left");
    public static readonly Key Right = new("right");

    public static readonly Key Unknown = new("unknown");

    public static readonly Key A = new("a");

    public static bool IsArrowKey(Key key)
    {
        if (key == Left) return true;
        else if (key == Right) return true;
        else if (key == Down) return true;
        else if (key == Up) return true;
        else return false;
    }
}
