using System.Xml.Linq;

internal static class CsprojEditor
{
    public static void BumpVersion(string csprojPath, bool mock = false)
    {
        string name = Path.GetFileNameWithoutExtension(csprojPath);

        // get current version
        csprojPath = Path.GetFullPath(csprojPath);
        XDocument doc = XDocument.Load(csprojPath);
        string versionString = doc.Element("Project")!.Element("PropertyGroup")!.Element("Version")!.Value;
        versionString = versionString.Split("-")[0];
        Version oldVersion = new(versionString);

        // bump it
        Version newVersion = new(oldVersion.Major, oldVersion.Minor, oldVersion.Build + 1);
        Console.WriteLine($"{name,-20} {oldVersion} -> {newVersion}");
        string newVersionString = newVersion.ToString();
        string newVersionStringLong = newVersionString + ".0";

        if (mock)
            return;

        // save output
        string[] lines = File.ReadAllLines(csprojPath);
        for (int i = 0; i < lines.Length; i++)
        {
            ReplaceElement(lines, "Version", newVersionString);
            ReplaceElement(lines, "AssemblyVersion", newVersionStringLong);
            ReplaceElement(lines, "FileVersion", newVersionStringLong);
        }
        File.WriteAllLines(csprojPath, lines);

        static void ReplaceElement(string[] lines, string element, string value)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains($"<{element}>"))
                {
                    string spaces = new(' ', lines[i].IndexOf("<"));
                    lines[i] = $"{spaces}<{element}>{value}</{element}>";
                    return;
                }
            }
        }
    }
}
