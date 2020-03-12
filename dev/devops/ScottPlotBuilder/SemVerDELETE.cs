using System;
using System.Linq;

namespace ScottPlotBuilder
{
    /// <summary>
    /// Semantic Version
    /// </summary>
    class SemVer
    {
        public int MAJOR { get; private set; }
        public int MINOR { get; private set; }
        public int PATCH { get; private set; }

        public SemVer()
        {

        }

        public SemVer(int major = 0, int minor = 0, int patch = 0)
        {
            MAJOR = major;
            MINOR = minor;
            PATCH = patch;
        }

        public SemVer(string version)
        {
            // ensure we have a string with 3 substrings separated by decimal points
            int numberOfDecimals = version.Count(c => c == '.');
            if (numberOfDecimals == 0)
                version += ".0.0";
            else if (numberOfDecimals == 1)
                version += ".0";

            try
            {
                string[] parts = version.Split('.');
                MAJOR = int.Parse(parts[0]);
                MINOR = int.Parse(parts[1]);
                PATCH = int.Parse(parts[2]);
            }
            catch
            {
                throw new ArgumentException("invalid version string: " + version);
            }

        }

        public override string ToString()
        {
            return $"{MAJOR}.{MINOR}.{PATCH}";
        }

        public void ChangeBy(int delta = 1, bool major = false, bool minor = false, bool patch = true)
        {
            MAJOR = (major) ? MAJOR + delta : MAJOR;
            MINOR = (minor) ? MINOR + delta : MINOR;
            PATCH = (patch) ? PATCH + delta : PATCH;
        }

        public void Incriment()
        {
            ChangeBy(1);
        }
    }
}
