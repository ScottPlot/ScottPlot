using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot;

public static class Version
{
    public readonly static string InformalVersion = Assembly.GetAssembly(typeof(Plot))!
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
        .InformationalVersion;

    public static string VersionString => $"ScottPlot {InformalVersion}";
}
