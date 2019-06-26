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
            recipies.Figure_01e_Legend();
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

            recipies.Figure_50_StyleBlue1();
            recipies.Figure_51_StyleBlue2();
            recipies.Figure_52_StyleBlue3();
            recipies.Figure_53_StyleLight1();
            recipies.Figure_54_StyleLight2();
            recipies.Figure_55_StyleGray1();
            recipies.Figure_56_StyleGray2();
            recipies.Figure_57_StyleBlack();
            recipies.Figure_58_StyleDefault();
            recipies.Figure_59_StyleControl();

            recipies.Figure_60_Plotting_With_Errorbars();
            recipies.Figure_61_Plot_Bar_Data();
            recipies.Figure_62_Plot_Bar_Data_Fancy();
            recipies.Figure_63_Step_Plot();

            GenerateReport(outputFolderName);
            ValidateImageHashes(outputFolderName);
            Console.WriteLine("COMPLETE");
            Console.ReadLine();
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
            knownHashes += "F1BF24218B13F98AF1B52EE6D2FE1F81"; // 01a_Scatter_Sin.png
            knownHashes += "5398EEDD564CC6152F354502CDAE8F1C"; // 01b_Automatic_Margins.png
            knownHashes += "5A479D3AA181AF49F521B5A5E00A3BD6"; // 01d_Zoom_and_Pan.png
            knownHashes += "FAD229B3D6D9930857961F3F0EFDDEA8"; // 01e_Legend.png
            knownHashes += "B5AEA70494FC7E9C7E3E6FEAD123BEC6"; // 02_Styling_Scatter_Plots.png
            knownHashes += "5D29D3600F22FBC55614461206739252"; // 04_Plot_Lines_Only.png
            knownHashes += "C55A4672469D6112A42757C0AE14B029"; // 05_Plot_Points_Only.png
            knownHashes += "9492F419A62982ABA8EA7C95D823A078"; // 06_Styling_XY_Plots.png
            knownHashes += "5B6B5791ADB1CDE4898E370B0E07B0C8"; // 07_Plotting_Points.png
            knownHashes += "3CB596AF995E0A0463EBC3FA6AC918B3"; // 08_Plotting_Text.png
            knownHashes += "DE08A7F272BCB2D513DDCB92C8EAE61C"; // 09_Clearing_Plots.png
            knownHashes += "E89FFE5ED7A23469A849DBC7E882E701"; // 10_Modifying_Plotted_Data.png
            knownHashes += "CDE7A970B392899AF1D891A94495C297"; // 20_Small_Plot.png
            knownHashes += "FD8D51D822F47E8CD757369831F1479C"; // 22_Custom_Colors.png
            knownHashes += "1A384F0231B728C98DAD5F235939626A"; // 23_Frameless_Plot.png
            knownHashes += "F1AE329A8C901CD85A83E0A8D8126AE4"; // 24_Disable_the_Grid.png
            knownHashes += "1DD94924518186E386B8B6744362B0F2"; // 25_Corner_Axis_Frame.png
            knownHashes += "701D37B85C3316FCD865F4CFAA31AB26"; // 26_Horizontal_Ticks_Only.png
            knownHashes += "D2939FAC08DF7095D6C9E04AE0A92DD7"; // 32_Signal_Styling.png
            knownHashes += "917AEA31B8D1766984AA1513FF8F75D8"; // 40_Vertical_and_Horizontal_Lines.png
            knownHashes += "C45A631C4260ACC22E8CE768D7123803"; // 50_StyleBlue1.png
            knownHashes += "AA832CD0A0B6118203BB2A7AE4E5B0C8"; // 51_StyleBlue2.png
            knownHashes += "17472BB102312A0E5749265CAE6FA664"; // 52_StyleBlue3.png
            knownHashes += "18A08A98DEABBA8D07E2BAA6536C4BF4"; // 53_StyleLight1.png
            knownHashes += "8C694D2A044845B078FA159A4DE707DC"; // 54_StyleLight2.png
            knownHashes += "3CA4FF2051D836C9EC3D681E6F74EA0F"; // 55_StyleGray1.png
            knownHashes += "FB11487B08871FA5DC449A04CA1866B1"; // 56_StyleGray2.png
            knownHashes += "BA3F8414CF88A5B07EE2A2DC990B1E8A"; // 57_StyleBlack.png
            knownHashes += "E9AB4B8B21026C8A8F1ECCDDBD5889C3"; // 58_StyleDefault.png
            knownHashes += "FE164ACD044F0A0F6CA858C4E2891BC5"; // 59_StyleControl.png
            knownHashes += "C7150625F8158F607EBB840663967D22"; // 60_Plotting_With_Errorbars.png
            knownHashes += "0E70C73BEA58AE15467363592E839E26"; // 61_Plot_Bar_Data.png
            knownHashes += "342298588383BB3CF748FFA48D618438"; // 62_Plot_Bar_Data_Fancy.png
            knownHashes += "AF0CEE68A78E649357F7930B255B062D"; // 01c_Defined_Axis_Limits.png
            knownHashes += "BC96F9ABB8333D25CC120C8133EFD613"; // 03_Plot_XY_Data.png
            knownHashes += "9B46257F74B67B6B1217B8FBB6AD3ACE"; // 21a_Title_and_Axis_Labels.png
            knownHashes += "C74AFBB386C11DC426F6007C92F1CD7E"; // 21b_Extra_Padding.png
            knownHashes += "CDF51C783DFFF8D6BE83CD754946F85A"; // 63_Step_Plot.png

            var md5 = System.Security.Cryptography.MD5.Create();
            string[] images = System.IO.Directory.GetFiles($"./{outputFolderName}", "*.png");
            Array.Sort(images);
            string sourceCodeToAdd = "";
            foreach (string filePath in images)
            {
                string hashString = "";
                string fileName = System.IO.Path.GetFileName(filePath);

                // skip images with benchmarks (which change every time)
                if (fileName.StartsWith("30_Signal") || fileName.StartsWith("31_Signal"))
                    continue;

                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    for (int i = 0; i < hashBytes.Length; i++)
                        hashString += hashBytes[i].ToString("X2");
                }
                if (!knownHashes.Contains(hashString))
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

            string html = "<h1>ScottPlot Cookbook</h1><h3>This cookbook was automatically generated by cookbook.csproj</h3>";
            string md = "# ScottPlot Cookbook\n\n_This cookbook was automatically generated by [cookbook.csproj](/demos/ScottPlotCookbook)_\n\n";

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

            string pathMd = System.IO.Path.GetFullPath("README.md");
            System.IO.File.WriteAllText(pathMd, md);

            Debug.WriteLine(pathHtml);
        }
    }
}
