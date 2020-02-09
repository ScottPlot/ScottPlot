using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Cookbook
{
    public static class Report
    {
        public static void GenerateHTML(string outputPath)
        {
            string imageFolder = System.IO.Path.Combine(outputPath, "images");
            if (!System.IO.Directory.Exists(imageFolder))
                throw new ArgumentException("incorrect path to cookbook image folder");

            string sourcePath = System.IO.Path.GetFullPath(outputPath + "/../../../../Cookbook/Cookbook.cs");
            if (!System.IO.File.Exists(sourcePath))
                throw new ArgumentException("incorrect path to cookbook source code");
            string source = System.IO.File.ReadAllText(sourcePath);

            StringBuilder html = new StringBuilder("<h1>ScottPlot Cookbook</h1>");
            html.AppendLine($"<br><br>The ScottPlot cookbook is a collection of small code examples which demonstrate how to create various types of plots using ScottPlot.");
            html.AppendLine($"<br><br><i>This cookbook was automatically generated using ScottPlot {ScottPlot.Tools.GetVersionString()}</i>");

            StringBuilder md = new StringBuilder("# ScottPlot Cookbook");
            md.AppendLine($"\nThe ScottPlot cookbook is a collection of small code examples which demonstrate how to create various types of plots using ScottPlot.");
            md.AppendLine($"\n_This cookbook was [automatically generated](/tests/Cookbook.cs) using ScottPlot {ScottPlot.Tools.GetVersionString()}_");

            string[] imagePaths = System.IO.Directory.GetFiles(imageFolder, "*.png");
            foreach (string path in imagePaths)
            {
                string functionName = System.IO.Path.GetFileNameWithoutExtension(path);
                string functionSource = GetFunctionSource(functionName, source);
                string cleanName = functionName.Substring(functionName.IndexOf('_') + 1).Replace("_", " ");

                html.AppendLine($"<h2><br>{cleanName}</h2>");
                html.AppendLine($"<img src='images/{functionName}.png'>");
                html.AppendLine("<pre style='font-family: monospace; background-color: #DDD; padding: 10px;'>");
                html.AppendLine(functionSource);
                html.AppendLine("</pre>");

                md.AppendLine();
                md.AppendLine();
                md.AppendLine($"## {cleanName}");
                md.AppendLine();
                md.AppendLine($"```cs\n{functionSource}\n```");
                md.AppendLine();
                md.AppendLine($"![](images/{functionName}.png)");
                md.AppendLine();
                md.AppendLine();
            }

            html.Insert(0, "<body style='background-color: #EEE; margin: 30px;'>");
            html.AppendLine("</body>");

            html.Insert(0, "<html>");
            html.AppendLine("</html>");

            System.IO.File.WriteAllText(System.IO.Path.Combine(outputPath, "cookbook.html"), html.ToString());
            System.IO.File.WriteAllText(System.IO.Path.Combine(outputPath, "readme.md"), md.ToString());
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
                if (i == 1 || i == 2)
                    lines[i] = "";
                if (lines[i].Contains("Saved:"))
                    lines[i] = "";
                if (lines[i].Length > 12)
                    lines[i] = lines[i].Substring(12);
                if (lines[i].Contains("SaveFig"))
                    lines[i] = $"plt.SaveFig(600, 400, \"{functionName}.png\");";
            }
            code = string.Join("\n", lines).Trim();
            return code;
        }
    }
}
