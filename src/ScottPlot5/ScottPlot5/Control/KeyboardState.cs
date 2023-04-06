namespace ScottPlot.Control;

public class KeyboardState
{
    private readonly HashSet<Key> Pressed = new();

    public IReadOnlyCollection<Key> PressedKeys => Pressed.ToArray();

    public void Down(Key key)
    {
        if (key == Key.Unknown)
            return;

        Pressed.Add(key);
    }

    public void Up(Key key)
    {
        if (key == Key.Unknown)
            return;

        Pressed.Remove(key);
    }
}
