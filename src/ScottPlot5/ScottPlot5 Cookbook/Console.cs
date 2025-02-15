namespace ScottPlotCookbook;

public static class Console
{
    public static bool SilenceOutput { get; set; } = IsGitHubActions;

    public static bool IsGitHubActions => Environment.GetEnvironmentVariable("GITHUB_ACTIONS") is not null;

    public static void WriteLine(string text)
    {
        if (SilenceOutput)
            return;

        System.Console.WriteLine(text);
    }

    public static void Write(string text)
    {
        if (SilenceOutput)
            return;

        System.Console.Write(text);
    }
}
