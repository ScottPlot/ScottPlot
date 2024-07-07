namespace ScottPlot.Interactivity;

public readonly record struct UserActionResult
{
    public string Summary { get; private init; }
    public bool RefreshRequired { get; private init; }
    public bool ResetAllActions { get; private init; }

    public override string ToString()
    {
        string message = Summary;
        if (RefreshRequired) message += " [REFRESH]";
        if (ResetAllActions) message += " [RESET]";
        return message;
    }

    public static UserActionResult NotRelevant() => new() { Summary = string.Empty };

    public static UserActionResult Handled(string summary)
    {
        return new()
        {
            Summary = summary,
        };
    }

    public static UserActionResult Refresh(string summary)
    {
        return new()
        {
            Summary = summary,
            RefreshRequired = true,
        };
    }

    public static UserActionResult RefreshAndReset(string summary)
    {
        return new()
        {
            Summary = summary,
            RefreshRequired = true,
            ResetAllActions = true,
        };
    }
};
