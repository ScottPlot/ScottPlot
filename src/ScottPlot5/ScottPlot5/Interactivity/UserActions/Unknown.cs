namespace ScottPlot.Interactivity.UserActions;

public record struct Unknown(string device, string? description) : IUserAction
{
    public readonly string Device => device;
    public readonly string Description => description ?? string.Empty;
    public DateTime DateTime { get; set; } = DateTime.Now;
}
