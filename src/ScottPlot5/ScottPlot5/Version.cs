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

    private static string GetVersionString()
    {
        string v = Assembly.GetAssembly(typeof(Plot))!
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion;

        return v.Contains("+") ? v.Split('+')[0] : v;
    }
}
