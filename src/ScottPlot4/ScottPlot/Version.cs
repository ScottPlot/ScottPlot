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

    /// <summary>
    /// Throws an exception if this version of ScottPlot does not match the expected major version
    /// </summary>
    public static void ShouldBe(int major)
    {
        if (major != AssemblyVersion.Major)
        {
            throw new InvalidOperationException($"ScottPlot was expected to be {major}.x.x but is actually {ShortString}");
        }
    }

    /// <summary>
    /// Throws an exception if this version of ScottPlot does not match the expected major and minor versions
    /// </summary>
    public static void ShouldBe(int major, int minor)
    {
        if ((major != AssemblyVersion.Major) || (minor != AssemblyVersion.Minor))
        {
            throw new InvalidOperationException($"ScottPlot was expected to be {major}.{minor}.x but is actually {ShortString}");
        }
    }

    /// <summary>
    /// Throws an exception if this version of ScottPlot does not match the exact one given
    /// </summary>
    public static void ShouldBe(int major, int minor, int build)
    {
        if ((major != AssemblyVersion.Major) || (minor != AssemblyVersion.Minor) || (build != AssemblyVersion.Build))
        {
            throw new InvalidOperationException($"ScottPlot was expected to be {major}.{minor}.{build} but is actually {ShortString}");
        }
    }
}
