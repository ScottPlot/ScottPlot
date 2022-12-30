using System.Reflection;

namespace ScottPlot;

public static class Version
{
    public static int Major => int.Parse(VersionString.Split('-')[0].Split('.')[0]);
    public static int Minor => int.Parse(VersionString.Split('-')[0].Split('.')[1]);
    public static int Build => int.Parse(VersionString.Split('-')[0].Split('.')[2]);

    public static string VersionString { get; private set; } = Assembly.GetAssembly(typeof(Plot))!
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
        .InformationalVersion;
}
