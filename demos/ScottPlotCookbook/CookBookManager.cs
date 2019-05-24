using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotCookbook
{
    class CookBookManager
    {

        private const int figureWidth = 600;
        private const int figureHeight = 400;

        public CookBookManager()
        {
            var plt = new ScottPlot.Plot(); // do this to pre-load modules
        }

        public void GenerateAllFigures(int width, int height, string outputFolderName = "images")
        {
            Console.WriteLine("Generating all figures...");
            CleanOutputFolder(outputFolderName);
            var recipies = new Recipes(outputFolderName, figureWidth, figureHeight);

            recipies.Figure_01a_Scatter_Sin();
            recipies.Figure_01b_Automatic_Margins();
            recipies.Figure_01c_Defined_Axis_Limits();
            recipies.Figure_01d_Zoom_and_Pan();
            recipies.Figure_02_Styling_Scatter_Plots();
            recipies.Figure_03_Plot_XY_Data();
            recipies.Figure_04_Plot_Lines_Only();
            recipies.Figure_05_Plot_Points_Only();
            recipies.Figure_06_Styling_XY_Plots();
            recipies.Figure_07_Plotting_Points();
            recipies.Figure_08_Plotting_Text();
            recipies.Figure_09_Clearing_Plots();
            recipies.Figure_10_Modifying_Plotted_Data();

            recipies.Figure_20_Small_Plot();
            recipies.Figure_21a_Title_and_Axis_Labels();
            recipies.Figure_21b_Extra_Padding();
            recipies.Figure_22_Custom_Colors();
            recipies.Figure_23_Frameless_Plot();
            recipies.Figure_24_Disable_the_Grid();
            recipies.Figure_25_Corner_Axis_Frame();
            recipies.Figure_26_Horizontal_Ticks_Only();

            recipies.Figure_30_Signal();
            recipies.Figure_31_Signal_With_Antialiasing_Off();
            recipies.Figure_32_Signal_Styling();

            recipies.Figure_40_Vertical_and_Horizontal_Lines();

            GenerateReport(outputFolderName);
            //ValidateImageHashes(outputFolderName);
            Console.WriteLine("COMPLETE");
        }

        public void CleanOutputFolder(string outputFolderName)
        {
            string outputFolder = System.IO.Path.GetFullPath($"./{outputFolderName}/");
            Console.WriteLine($"preparing output folder: {outputFolder}");
            if (!System.IO.Directory.Exists(outputFolder))
                System.IO.Directory.CreateDirectory(outputFolder);
            foreach (string fileName in System.IO.Directory.GetFiles(outputFolder, "*.*"))
                System.IO.File.Delete(System.IO.Path.Combine(outputFolder, fileName));
        }

        public string ReadCodeFromFile(string functionName, string fileName = "Recipes.cs")
        {
            string code;

            // read contents of file
            string sourceFilePath = "../../" + fileName;
            if (System.IO.File.Exists(sourceFilePath))
            {
                sourceFilePath = System.IO.Path.GetFullPath(sourceFilePath);
                code = System.IO.File.ReadAllText(sourceFilePath);
            }
            else
            {
                string message = $"source code file does not exist: {sourceFilePath}";
                Debug.WriteLine(message);
                return message;
            }

            // isolate the function
            functionName = "Figure_" + functionName;
            int posStart = code.IndexOf($"public void {functionName}()");
            if (posStart < 0)
            {
                string message = $"function {functionName}() not found in {sourceFilePath}";
                Debug.WriteLine(message);
                return message;
            }

            // format the code to be a pretty string
            int linesToSkip = 2;
            code = code.Substring(posStart);
            code = code.Substring(code.IndexOf("\n        {"));
            code = code.Substring(0, code.IndexOf("\n        }"));
            code = code.Trim();
            string[] lines = code.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > 12)
                    lines[i] = lines[i].Substring(12);
                if (i <= linesToSkip)
                    lines[i] = "";
                if (lines[i].StartsWith("Debug") || lines[i].StartsWith("Console") || lines[i].EndsWith("// hide"))
                    lines[i] = "";
            }
            code = string.Join("\n", lines).Trim();
            code = code.Replace("ScottPlot.Plot(width, height);", $"ScottPlot.Plot({figureWidth}, {figureHeight});");
            return code;
        }

        public void ValidateImageHashes(string outputFolderName)
        {
            string knownHashes = "";

            var md5 = System.Security.Cryptography.MD5.Create();
            string[] images = System.IO.Directory.GetFiles($"./{outputFolderName}", "*.png");
            Array.Sort(images);
            string sourceCodeToAdd = "";
            foreach (string filePath in images)
            {
                string hashString = "";
                string fileName = System.IO.Path.GetFileName(filePath);
                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    for (int i = 0; i < hashBytes.Length; i++)
                        hashString += hashBytes[i].ToString("X2");
                }
                if (knownHashes.Contains(hashString))
                {
                    Console.WriteLine($"[{hashString}] verified - {fileName}");
                }
                else
                {
                    Console.WriteLine($"[{hashString}] UNKNOWN - {fileName}");
                    sourceCodeToAdd += $"knownHashes += \"{hashString}\"; // {fileName}\n";
                }
            }
            if (sourceCodeToAdd.Length > 0)
            {
                Console.WriteLine($"\n\nYOU MAY WANT TO ADD THIS SOURCE CODE:\n");
                Console.WriteLine(sourceCodeToAdd);
                Console.WriteLine("\npress ENTER to continue...");
                Console.ReadLine();
            }
        }

        public void GenerateReport(string outputFolderName)
        {
            Console.WriteLine("Generating reports...");

            string html = "";
            string md = "";

            string[] images = System.IO.Directory.GetFiles($"./{outputFolderName}", "*.png");
            Array.Sort(images);

            foreach (string image in images)
            {
                string url = image.Replace("\\", "/");
                string name = System.IO.Path.GetFileNameWithoutExtension(url);
                string nameFriendly = name.Replace("_", " ").Substring(3);
                string code = ReadCodeFromFile(name);
                string codeHtml = System.Net.WebUtility.HtmlEncode(code);
                codeHtml = codeHtml.Replace("\n", "<br>");
                codeHtml = codeHtml.Replace(" ", "&nbsp;");

                html += $"<div style='font-size: 150%; font-family: sans-serif; margin-top: 50px; font-weight: bold;'>{nameFriendly}</div>";
                html += $"<div style='background-color: #EEE; padding: 5px; margin: 10px; font-family: Consolas;'>{codeHtml}</div>";
                html += $"<div><a href='{url}'><img src='{url}' /></a></div>";

                md += $"## {nameFriendly}\n\n```cs\n{code}\n```\n\n![]({url})\n\n";
            }

            html = $"<html><body>{html}</body></html>";

            string pathHtml = System.IO.Path.GetFullPath("cookbook.html");
            System.IO.File.WriteAllText(pathHtml, html);

            string pathMd = System.IO.Path.GetFullPath("cookbook.md");
            System.IO.File.WriteAllText(pathMd, md);

            Debug.WriteLine(pathHtml);
        }
    }
}
