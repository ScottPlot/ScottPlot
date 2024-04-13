using System.Reflection;

namespace ScottPlot;

public static class Version
{
    public static int Major => int.Parse(VersionString.Split('-')[0].Split('.')[0]);
    public static int Minor => int.Parse(VersionString.Split('-')[0].Split('.')[1]);
    public static int Build => int.Parse(VersionString.Split('-')[0].Split('.')[2]);

    /// <summary>
    /// Version formatted like "5.0.0-beta"
    /// </summary>
    public static string VersionString { get; private set; } = GetVersionString();

    /// <summary>
    /// Version formatted like "ScottPlot 5.0.0-beta"
    /// </summary>
    public static string LongString { get; private set; } = "ScottPlot " + GetVersionString();

    private static string GetVersionString()
    {
        string v = Assembly.GetAssembly(typeof(Plot))!
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion;

        return v.Contains("+") ? v.Split('+')[0] : v;
    }

    /// <summary>
    /// Throws an exception if this version of ScottPlot does not match the expected major version
    /// </summary>
    public static void ShouldBe(int major)
    {
        if (major != Major)
        {
            throw new InvalidOperationException($"ScottPlot was expected to be {major}.x.x but is actually {VersionString}");
        }
    }

    /// <summary>
    /// Throws an exception if this version of ScottPlot does not match the expected major and minor versions
    /// </summary>
    public static void ShouldBe(int major, int minor)
    {
        if ((major != Major) || (minor != Minor))
        {
            throw new InvalidOperationException($"ScottPlot was expected to be {major}.{minor}.x but is actually {VersionString}");
        }
    }

    /// <summary>
    /// Throws an exception if this version of ScottPlot does not match the exact one given
    /// </summary>
    public static void ShouldBe(int major, int minor, int build)
    {
        if ((major != Major) || (minor != Minor) || (build != Build))
        {
            throw new InvalidOperationException($"ScottPlot was expected to be {major}.{minor}.{build} but is actually {VersionString}");
        }
    }
}
