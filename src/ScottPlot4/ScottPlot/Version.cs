namespace ScottPlot;

public static class Version
{
    public static readonly System.Version AssemblyVersion = GetVersion();

    /// <summary>
    /// ScottPlot version number in the format "1.2.3"
    /// </summary>
    public static readonly string ShortString = GetShortVersionString();

    /// <summary>
    /// Full ScottPlot version in the format "ScottPlot 1.2.3"
    /// </summary>
    public static readonly string LongString = GetLongVersionString();

    private static System.Version GetVersion()
    {
        return typeof(Plot).Assembly.GetName().Version;
    }

    private static string GetShortVersionString()
    {
        var v = AssemblyVersion;
        return $"{AssemblyVersion.Major}.{AssemblyVersion.Minor}.{AssemblyVersion.Build}";
    }

    private static string GetLongVersionString()
    {
        return $"ScottPlot " + GetShortVersionString();
    }
}
