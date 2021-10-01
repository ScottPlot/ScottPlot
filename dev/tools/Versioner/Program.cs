using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace Versioner
{
    class Program
    {
        /// <summary>
        /// This program increments the version number in Directory.Packages.props
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException("argument required: path to src folder");

            string srcPath = Path.GetFullPath(args[0]);
            string directoryBuildPath = Path.Combine(srcPath, "Directory.Build.props");
            
            if (!File.Exists(directoryBuildPath))
                throw new ArgumentException($"bad path: {directoryBuildPath}");

            (Version version, string suffix) = GetCurrentVersion(Path.Combine(directoryBuildPath));
            IncrementVersion(directoryBuildPath, version, suffix);

            Console.WriteLine("COMPLETE");
        }

        public static (Version version, string suffix) GetCurrentVersion(string directoryBuildPath)
        {
            directoryBuildPath = Path.GetFullPath(directoryBuildPath);
            Console.WriteLine($"Reading version from {directoryBuildPath}");

            XDocument doc = XDocument.Load(directoryBuildPath);
            string versionString = doc.Element("Project").Element("PropertyGroup").Element("ScottPlotVersion").Value;

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

        public static void IncrementVersion(string directoryBuildPath, Version oldVersion, string suffix)
        {
            // prepare new version strings
            Version newVersion = new Version(oldVersion.Major, oldVersion.Minor, oldVersion.Build + 1);
            string newVersionName = newVersion.ToString();
            if (suffix != null)
                newVersionName += "-" + suffix;
            Console.WriteLine($"{oldVersion} -> {newVersion} {System.IO.Path.GetFileName(directoryBuildPath)}");

            // modify only those lines in the csproj file
            string[] lines = File.ReadAllLines(directoryBuildPath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("        <ScottPlotVersion>"))
                    lines[i] = $"        <ScottPlotVersion>{newVersionName}</ScottPlotVersion>";
            }
            File.WriteAllLines(directoryBuildPath, lines);
        }
    }
}
