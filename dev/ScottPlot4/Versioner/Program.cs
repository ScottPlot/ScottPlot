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
            Console.WriteLine($"{oldVersion} -> {newVersion} {System.IO.Path.GetFileName(csprojPath)}");

            // modify only those lines in the csproj file
            string[] lines = System.IO.File.ReadAllLines(csprojPath);
            for (int i = 0; i < lines.Length; i++)
            {
                ReplaceElement(lines, "Version", newVersion.ToString());
                ReplaceElement(lines, "AssemblyVersion", newVersion.ToString() + ".0");
                ReplaceElement(lines, "FileVersion", newVersion.ToString() + ".0");
            }
            File.WriteAllLines(csprojPath, lines);
        }

        private static void ReplaceElement(string[] lines, string element, string value)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains($"<{element}>"))
                {
                    string spaces = new string(' ', lines[i].IndexOf("<"));
                    lines[i] = $"{spaces}<{element}>{value}</{element}>";
                    return;
                }
            }

            throw new InvalidOperationException($"Element not found: {element}");
        }
    }
}