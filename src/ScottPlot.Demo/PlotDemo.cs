using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo
{
    public abstract class PlotDemo
    {
        public string classPath
        {
            get
            {
                return GetType().ToString();
            }
        }

        public string id
        {
            get
            {
                return $"{categoryMajor}_{categoryMinor}_{categoryClass}";
            }
        }

        public string categoryMajor
        {
            get
            {
                string category = classPath.Substring(15);
                string[] pathAndName = category.Split('+');
                string[] folderAndFile = pathAndName[0].Split('.');
                return folderAndFile[0];
            }
        }

        public string categoryMinor
        {
            get
            {
                string category = classPath.Substring(15);
                string[] pathAndName = category.Split('+');
                string[] folderAndFile = pathAndName[0].Split('.');
                return folderAndFile[1];
            }
        }

        public string categoryClass
        {
            get
            {
                string category = classPath.Substring(15);
                string[] pathAndName = category.Split('+');
                return pathAndName[1];
            }
        }

        public string GetSourceCode(string pathDemoFolder)
        {
            string sourceFilePath = $"{pathDemoFolder}/{categoryMajor}/{categoryMinor}.cs";
            sourceFilePath = System.IO.Path.GetFullPath(sourceFilePath);

            if (!System.IO.File.Exists(sourceFilePath))
                return $"source code not found in: {sourceFilePath}";

            string code = System.IO.File.ReadAllText(sourceFilePath);
            code = code.Replace("\r\n", "\n");

            StringBuilder sb = new StringBuilder();

            var lines = code.Split('\n');
            bool inRenderFunction = false;
            bool inCorrectClass = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.StartsWith("        public class"))
                {
                    inCorrectClass = (line.StartsWith($"        public class {categoryClass}"));
                }

                if (line.StartsWith("            public void Render(Plot plt)"))
                {
                    inRenderFunction = true;
                    i += 1;
                    continue;
                }

                if (line.StartsWith("            }"))
                {
                    inRenderFunction = false;
                    continue;
                }

                if (line.StartsWith("                "))
                    line = line.Substring(16);
                if (inRenderFunction && inCorrectClass)
                    sb.AppendLine(line);
            }

            return sb.ToString().Trim();
        }
    }
}
