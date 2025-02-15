namespace ScottPlot.Interactivity;

/// <summary>
/// Tracks which keyboard keys are currently pressed.
/// </summary>
public class KeyboardState
{
    readonly HashSet<string> PressedKeyNames = [];

    public int PressedKeyCount => PressedKeyNames.Count;

    public void Reset()
    {
        PressedKeyNames.Clear();
    }

    public void Add(Key key)
    {
        PressedKeyNames.Add(key.Name);
    }

    public void Remove(Key key)
    {
        PressedKeyNames.Remove(key.Name);
    }

    public bool IsPressed(Key key)
    {
        return IsPressed(key.Name);
    }

    public bool IsPressed(IEnumerable<Key> keys)
    {
        foreach (var key in keys)
        {
            if (IsPressed(key.Name))
                return true;
        }

        return false;
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
