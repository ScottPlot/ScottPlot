namespace ScottPlot.Interactivity.UserActions;

public record struct KeyUp(Key Key) : IUserAction
{
    public readonly string Device => $"key {Key.name}";
    public readonly string Description => $"key {Key.name} released";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
