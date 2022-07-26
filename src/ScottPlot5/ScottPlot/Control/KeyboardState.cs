namespace ScottPlot.Control;

internal class KeyboardState
{
    private readonly HashSet<Key> Pressed = new();

    public IReadOnlyCollection<Key> PressedKeys => Pressed.ToArray();

    public bool IsPressed(Key key) => Pressed.Contains(key);

    public void Down(Key key)
    {
        if (key == Key.UNKNOWN)
            return;
        Pressed.Add(key);
    }

    public void Up(Key key)
    {
        if (key == Key.UNKNOWN)
            return;
        Pressed.Remove(key);
    }
}
