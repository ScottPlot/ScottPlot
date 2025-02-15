namespace ScottPlot.Interactivity.UserActions;

public record struct KeyUp(Key Key) : IUserAction
{
    public readonly string Device => $"key {Key.Name}";
    public readonly string Description => $"key {Key.Name} released";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
