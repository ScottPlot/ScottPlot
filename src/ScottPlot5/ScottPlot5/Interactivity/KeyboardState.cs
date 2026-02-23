using System.Runtime.InteropServices;

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
        //keys are only captured if the control has focus.  Which means if you shift-click from another application then the shift is lost.
        //also, if you ALT-tab out of your application then ALT is forever pressed and it is applied whenever you click back in (even though you're no longer holding it)
        //If .net standard 2.0 was dropped then we could do:  Keyboard.Modifiers.HasFlag(ModifierKeys.Alt)
        //User32 was used as a workaround to support net standard 2.0
        if (keyName == StandardKeys.Alt.Name)
            return (GetKeyState(VK_MENU) & 0x8000) != 0; // Keyboard.Modifiers.HasFlag(ModifierKeys.Alt);
        if (keyName == StandardKeys.Control.Name)
            return (GetKeyState(VK_CONTROL) & 0x8000) != 0; //Keyboard.Modifiers.HasFlag(ModifierKeys.Control);
        if (keyName == StandardKeys.Shift.Name)
            return (GetKeyState(VK_SHIFT) & 0x8000) != 0; //Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);

        return PressedKeyNames.Contains(keyName);
    }

    public string[] GetPressedKeyNames => PressedKeyNames.ToArray();

    public override string ToString()
    {
        return (PressedKeyNames.Count == 0)
            ? "KeyState with 0 pressed key"
            : $"KeyState with {PressedKeyNames.Count} pressed keys: " + string.Join(", ", PressedKeyNames.Select(x => x.ToString()));
    }

    [DllImport("user32.dll")]
    private static extern short GetKeyState(int nVirtKey);
    private const int VK_MENU = 0x12; // Alt
    private const int VK_CONTROL = 0x11; // Ctrl
    private const int VK_SHIFT = 0x10; // Shift
}
