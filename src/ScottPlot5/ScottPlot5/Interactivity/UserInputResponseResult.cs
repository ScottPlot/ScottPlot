namespace ScottPlot.Interactivity;

public readonly record struct UserInputResponseResult
{
    /// <summary>
    /// A description of what the response did.
    /// Used only for testing and debugging.
    /// Null indicates no action was taken.
    /// </summary>
    public string Summary { get; init; } // TODO: remove this property

    /// <summary>
    /// Set this to request a render after all responses are processed
    /// </summary>
    public bool RefreshRequired { get; init; }

    /// <summary>
    /// Enable this to prevent all other drag responses from being processed
    /// </summary>
    public bool IsPrimaryDragResponse { get; init; }

    public override string ToString()
    {
        string message = Summary ?? "NO ACTION";
        if (RefreshRequired) message += " [REFRESH]";
        if (IsPrimaryDragResponse) message += " [PRIMARY]";
        return message;
    }

    public static UserInputResponseResult NoActionTaken => new();
};
