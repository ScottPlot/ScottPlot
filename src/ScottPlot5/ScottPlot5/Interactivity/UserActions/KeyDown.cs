namespace ScottPlot.Interactivity.UserActions;

public record struct KeyDown(Key Key) : IUserAction
{
    public readonly string Device => $"key {Key.Name}";
    public readonly string Description => $"key {Key.Name} pressed";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
