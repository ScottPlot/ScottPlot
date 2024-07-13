namespace ScottPlot.Interactivity.UserActions;

public record struct KeyDown(Key Key) : IUserAction
{
    public readonly string Device => $"key {Key.name}";
    public readonly string Description => $"key {Key.name} pressed";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
