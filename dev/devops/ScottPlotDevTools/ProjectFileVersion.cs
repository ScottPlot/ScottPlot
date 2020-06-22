using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlotDevTools
{
    public class ProjectFileVersion
    {
        public readonly string path;
        public Version version;
        public readonly string name;

        public ProjectFileVersion(string projectFilePath)
        {
            if (!projectFilePath.EndsWith(".csproj"))
                throw new ArgumentException("Project files must end with .csproj");

            projectFilePath = System.IO.Path.GetFullPath(projectFilePath);
            if (!System.IO.File.Exists(projectFilePath))
                throw new ArgumentException("Project file does not exist: " + projectFilePath);

            path = projectFilePath;
            name = System.IO.Path.GetFileNameWithoutExtension(projectFilePath);
            version = Read();
        }

        public override string ToString()
        {
            return $"{name} {version.Major}.{version.Minor}.{version.Build}";
        }

        private Version Read()
        {
            string[] sourceLines = System.IO.File.ReadAllLines(path);
            foreach (string line in sourceLines)
            {
                string[] parts = line.Split("Version>");
                if (parts.Length == 3)
                {
                    string versionString = parts[1].Split("<")[0];
                    //versionString += ".0";
                    return new Version(versionString);
                }
            }

            throw new ArgumentException("file does not contain version information: " + path);
        }

        public void Incriment()
        {
            version = new Version(version.Major, version.Minor, version.Build + 1);
        }

        public void Save()
        {
            Version fileVersion = Read();
            string fileVersionString = $"{fileVersion.Major}.{fileVersion.Minor}.{fileVersion.Build}";
            string newVersionShort = $"{version.Major}.{version.Minor}.{version.Build}";
            string newVersionLong = $"{newVersionShort}.0";

            string[] lines = System.IO.File.ReadAllText(path).Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Contains($">{fileVersionString}<"))
                {
                    if (line.Contains("<Version>"))
                        lines[i] = line.Replace(fileVersionString, newVersionShort);
                    else
                        lines[i] = line.Replace(fileVersionString, newVersionLong);
                }
            }
            System.IO.File.WriteAllText(path, string.Join("\n", lines));
        }
    }
}
