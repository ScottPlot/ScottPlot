using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests
{
    public static class Report
    {
        public static void GenerateHTML(string outputPath)
        {
            string sourcePath = System.IO.Path.GetFullPath(outputPath + "/../../../../Cookbook.cs");
            if (!System.IO.File.Exists(sourcePath))
                throw new ArgumentException("incorrect path to cookbook source code");
            string source = System.IO.File.ReadAllText(sourcePath);

            StringBuilder sb = new StringBuilder("<h1>ScottPlot Cookbook</h1>\n");
            string[] imagePaths = System.IO.Directory.GetFiles(outputPath, "*.png");
            foreach (string path in imagePaths)
            {
                string functionName = System.IO.Path.GetFileNameWithoutExtension(path);
                string functionSource = GetFunctionSource(functionName, source);
                sb.AppendLine($"<h2><br>{functionName}</h2>");
                sb.AppendLine($"<img src='{functionName}.png'>");
                sb.AppendLine("<pre style='font-family: monospace; background-color: #DDD; padding: 10px;'>");
                sb.AppendLine(functionSource);
                sb.AppendLine("</pre>");
            }

            sb.Insert(0, "<body style='background-color: #EEE; margin: 30px;'>");
            sb.AppendLine("</body>");

            sb.Insert(0, "<html>");
            sb.AppendLine("</html>");

            System.IO.File.WriteAllText(System.IO.Path.Join(outputPath, "cookbook.html"), sb.ToString());
        }

        static string GetFunctionSource(string functionName, string code)
        {
            int posStart = code.IndexOf($"public void Figure_{functionName}()");

            if (posStart < 0)
                throw new Exception($"function {functionName}() not found in source code");

            code = code.Substring(posStart);
            code = code.Substring(code.IndexOf("\n        {"));
            code = code.Substring(0, code.IndexOf("\n        }"));
            code = code.Trim();
            code = code.Trim(new char[] { '{', '}' });
            string[] lines = code.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > 12)
                    lines[i] = lines[i].Substring(12);
                //if (lines[i].Contains("Tools.SaveFig"))
                //lines[i] = $"figure.Save(600, 400, \"{functionName}.png\");";
            }
            code = string.Join("\n", lines).Trim();
            return code;
        }
    }
}
