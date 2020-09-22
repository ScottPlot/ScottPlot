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

        public string sourceFile
        {
            get
            {
                return $"/src/ScottPlot.Demo/{categoryMajor}/{categoryMinor}.cs";
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
            // show Render(int, int) of certain files
            bool showRenderIntInt = (categoryMinor.ToLower() == "multiplot");

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
                    inCorrectClass = (line.StartsWith($"        public class {categoryClass} :"));
                }

                if (line.StartsWith("            public void Render(Plot plt)") ||
                    line.StartsWith("            public System.Drawing.Bitmap Render(int width, int height)"))
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
                {
                    // skip the typical Render() method in MultiPlot examples
                    if (showRenderIntInt)
                    {
                        if (line.Contains("var plt = new ScottPlot.Plot(") ||
                            line.Contains("throw new InvalidOperationException") ||
                            line.Contains("return mp.GetBitmap()"))
                            continue;
                    }
                    sb.AppendLine(line);
                }
            }

            if (showRenderIntInt)
            {
                sb.AppendLine($"\r\nmp.SaveFig(\"{id}.png\");");
            }
            else
            {
                sb.Insert(0, "var plt = new ScottPlot.Plot(600, 400);\r\n\r\n");
                sb.AppendLine($"\r\nplt.SaveFig(\"{id}.png\");");
            }

            string codeText = sb.ToString().Trim();
            string threeBreaks = "\r\n\r\n\r\n";
            string twoBreaks = "\r\n\r\n";
            while (codeText.Contains(threeBreaks))
                codeText = codeText.Replace(threeBreaks, twoBreaks);

            return codeText;
        }
    }
}
