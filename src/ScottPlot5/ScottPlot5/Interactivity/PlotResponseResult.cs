namespace ScottPlot.Interactivity;

public readonly record struct PlotResponseResult
{
    /// <summary>
    /// A description of what the response did.
    /// Used only for testing and debugging.
    /// Null indicates no action was taken.
    /// </summary>
    public string? Summary { get; init; } // TODO: remove this property

    /// <summary>
    /// Request a render after all responses have finished executing
    /// </summary>
    public bool RefreshRequired { get; init; }

    /// <summary>
    /// If true, all other responses will not be executed until the response
    /// that returned this result returns a new result with this flag false.
    /// </summary>
    public bool IsPrimaryResponse { get; init; }

    public override string ToString()
    {
        string message = Summary ?? "NO ACTION";
        if (RefreshRequired) message += " [REFRESH]";
        if (IsPrimaryResponse) message += " [PRIMARY]";
        return message;
    }

    public static PlotResponseResult NoActionTaken => new();
};
