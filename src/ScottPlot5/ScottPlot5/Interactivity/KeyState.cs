namespace ScottPlot.Interactivity;

/// <summary>
/// Tracks which keyboard keys are pressed.
/// Although designed for keyboard keys, other user inputs 
/// could be tracked here as long as they implement <see cref="IKey"/>.
/// </summary>
public class KeyState
{
    private readonly HashSet<IKey> PressedKeys = [];

    public int PressedKeyCount => PressedKeys.Count;

    public void Press(IKey key)
    {
        PressedKeys.Add(key);
    }

    public void Release(IKey key)
    {
        PressedKeys.Remove(key);
    }

    public bool IsPressed(IKey key)
    {
        return PressedKeys.Contains(key);
    }

    public override string ToString()
    {
        return (PressedKeys.Count == 0)
            ? "KeyState with 0 pressed key"
            : $"KeyState with {PressedKeys.Count} pressed keys: " + string.Join(", ", PressedKeys.Select(x => x.ToString()));
    }
}
