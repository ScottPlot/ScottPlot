using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot;

public static class Version
{
    public static int Major => int.Parse(InformalVersion.Split('-')[0].Split('.')[0]);
    public static int Minor => int.Parse(InformalVersion.Split('-')[0].Split('.')[1]);
    public static int Build => int.Parse(InformalVersion.Split('-')[0].Split('.')[2]);

    public readonly static string InformalVersion = Assembly.GetAssembly(typeof(Plot))!
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
        .InformationalVersion;

    public static string VersionString => $"ScottPlot {InformalVersion}";
}
