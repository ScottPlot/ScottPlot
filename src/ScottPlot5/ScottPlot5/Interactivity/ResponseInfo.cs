namespace ScottPlot.Interactivity;

/// <summary>
/// Describes what may need to happen after a user action response has completed executing
/// </summary>
public readonly record struct ResponseInfo
{
    /// <summary>
    /// Request a render after all responses have finished executing
    /// </summary>
    public bool RefreshNeeded { get; init; }

    /// <summary>
    /// If true, all other responses will not be executed until the response
    /// that returned this result returns a new result with this flag false.
    /// </summary>
    public bool IsPrimary { get; init; }

    public override string ToString()
    {
        string message = "PlotResponseResult";
        if (RefreshNeeded) message += " [REFRESH]";
        if (IsPrimary) message += " [PRIMARY]";
        return message;
    }

    public static ResponseInfo NoActionRequired => new();

    public static ResponseInfo Refresh => new() { RefreshNeeded = true };
};
