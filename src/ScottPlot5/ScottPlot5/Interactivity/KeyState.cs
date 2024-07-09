namespace ScottPlot.Interactivity;

/// <summary>
/// Tracks which keyboard keys are pressed.
/// Although designed for keyboard keys, other user inputs 
/// could be tracked here as long as they implement <see cref="IKey"/>.
/// </summary>
public class KeyState
{
    private readonly HashSet<string> PressedKeyNames = [];

    public int PressedKeyCount => PressedKeyNames.Count;

    public void Add(IKey key)
    {
        PressedKeyNames.Add(key.Name);
    }

    public void Remove(IKey key)
    {
        PressedKeyNames.Remove(key.Name);
    }

    public bool IsPressed(IKey key)
    {
        return IsPressed(key.Name);
    }

    public bool IsPressed(string keyName)
    {
        return PressedKeyNames.Contains(keyName);
    }

    public string[] GetPressedKeyNames => PressedKeyNames.ToArray();

    public override string ToString()
    {
        return (PressedKeyNames.Count == 0)
            ? "KeyState with 0 pressed key"
            : $"KeyState with {PressedKeyNames.Count} pressed keys: " + string.Join(", ", PressedKeyNames.Select(x => x.ToString()));
    }
}
