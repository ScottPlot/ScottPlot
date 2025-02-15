namespace ScottPlot.Interactivity;

/// <summary>
/// Represents a single key on a keyboard that may be pressed and held.
/// Keys are tracked by <see cref="KeyboardState"/> and <see cref="IUserActionResponse"/>
/// classes can see which keys are pressed when they are executed.
/// </summary>
public readonly record struct Key
{
    /// <summary>
    /// A name that uniquely identifies a specific key
    /// </summary>
    public string Name { get; }

    public Key(string name)
    {
        Name = name.ToLower();
    }
}
