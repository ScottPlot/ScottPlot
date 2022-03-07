using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace Versioner
{
    class Program
    {
        /// <summary>
        /// This program increments version numbers in all ScottPlot NuGet package csproj files
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException("argument required: path to src folder");

            string srcPath = Path.GetFullPath(args[0]);
            string slnPath = Path.Combine(srcPath, "ScottPlot.sln");
            if (!File.Exists(slnPath))
                throw new ArgumentException($"bad path: {slnPath}");

            (Version version, string suffix) = GetCurrentVersion(Path.Combine(srcPath, "ScottPlot/ScottPlot.csproj"));
            IncrementVersion(Path.Combine(srcPath, "ScottPlot/ScottPlot.csproj"), version, suffix);
            IncrementVersion(Path.Combine(srcPath, "ScottPlot.WinForms/ScottPlot.WinForms.NUGET.csproj"), version, suffix);
            IncrementVersion(Path.Combine(srcPath, "ScottPlot.WPF/ScottPlot.WPF.NUGET.csproj"), version, suffix);
            IncrementVersion(Path.Combine(srcPath, "ScottPlot.Avalonia/ScottPlot.Avalonia.NUGET.csproj"), version, suffix);
            IncrementVersion(Path.Combine(srcPath, "ScottPlot.Eto/ScottPlot.Eto.NUGET.csproj"), version, suffix);

            Console.WriteLine("COMPLETE");
        }

        public static (Version version, string suffix) GetCurrentVersion(string csprojPath)
        {
            csprojPath = Path.GetFullPath(csprojPath);
            Console.WriteLine($"Reading version from {csprojPath}");

            XDocument doc = XDocument.Load(csprojPath);
            string versionString = doc.Element("Project").Element("PropertyGroup").Element("Version").Value;

            if (versionString.Contains('-'))
            {
                string[] versionParts = versionString.Split('-');
                Version version = new Version(versionParts[0]);
                string suffix = versionParts[1];
                Console.WriteLine($"Current version (pre-release): {version}-{suffix}");
                return (version, suffix);
            }
            else
            {
                Version version = new Version(versionString);
                Console.WriteLine($"Current version (release): {version}");
                return (version, null);
            }
        }

        public static void IncrementVersion(string csprojPath, Version oldVersion, string suffix)
        {
            // prepare new version strings
            Version newVersion = new Version(oldVersion.Major, oldVersion.Minor, oldVersion.Build + 1);
            string newVersionNumber = newVersion.ToString() + ".0";
            string newVersionName = newVersion.ToString();
            if (suffix != null)
                newVersionName += "-" + suffix;
            Console.WriteLine($"{oldVersion} -> {newVersion} {System.IO.Path.GetFileName(csprojPath)}");

            // modify only those lines in the csproj file
            string[] lines = System.IO.File.ReadAllLines(csprojPath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("    <Version>"))
                    lines[i] = $"    <Version>{newVersionName}</Version>";
                if (lines[i].StartsWith("    <AssemblyVersion>"))
                    lines[i] = $"    <AssemblyVersion>{newVersionNumber}</AssemblyVersion>";
                if (lines[i].StartsWith("    <FileVersion>"))
                    lines[i] = $"    <FileVersion>{newVersionNumber}</FileVersion>";
            }
            File.WriteAllLines(csprojPath, lines);
        }
    }
}