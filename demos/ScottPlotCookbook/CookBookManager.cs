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
            knownHashes += "336FA49A099FFE6881D2C5D0B2B5AC9B"; // 01a_Scatter_Sin.png
            knownHashes += "53D7F12FED78FCBEE642C211D6A073BA"; // 01b_Automatic_Margins.png
            knownHashes += "AF0CEE68A78E649357F7930B255B062D"; // 01c_Defined_Axis_Limits.png
            knownHashes += "EC2FC84AADD0128A8E61A7DAD40BAE40"; // 01d_Zoom_and_Pan.png
            knownHashes += "8EE195CFB60AC8EADD1034E65A405A78"; // 01e_Legend.png
            knownHashes += "A3736FFCA35E48EEDFE82DE0FC60CE13"; // 02_Styling_Scatter_Plots.png
            knownHashes += "BC96F9ABB8333D25CC120C8133EFD613"; // 03_Plot_XY_Data.png
            knownHashes += "F9AE6E651679EB1127C3FEEB0E12F930"; // 04_Plot_Lines_Only.png
            knownHashes += "A4CE8C1004273D73725946FD54D22D9D"; // 05_Plot_Points_Only.png
            knownHashes += "327F34CB8838B01D63849D191847F141"; // 06_Styling_XY_Plots.png
            knownHashes += "36E42889689F15CDCD808ED19632DD81"; // 07_Plotting_Points.png
            knownHashes += "0C7C583C47960CFAD299DFED88875B84"; // 08_Plotting_Text.png
            knownHashes += "E89CE1FB91214A3BFD36DCA3409DCACB"; // 09_Clearing_Plots.png
            knownHashes += "4149DDDC359EBD018CB16FE4D96F3BA0"; // 10_Modifying_Plotted_Data.png
            knownHashes += "BA0B0F78BBF76E41F50B9250AE88D1B2"; // 20_Small_Plot.png
            knownHashes += "9B46257F74B67B6B1217B8FBB6AD3ACE"; // 21a_Title_and_Axis_Labels.png
            knownHashes += "C74AFBB386C11DC426F6007C92F1CD7E"; // 21b_Extra_Padding.png
            knownHashes += "1115C46702B261DE0B5F1480A2103545"; // 22_Custom_Colors.png
            knownHashes += "F25C121A32FBB4B7E39105E6B6229F2A"; // 23_Frameless_Plot.png
            knownHashes += "B7219E17329DBA6B3F7CDC6BC3FE16D7"; // 24_Disable_the_Grid.png
            knownHashes += "F18B4EE14FB45DD1974FAF7DE1613687"; // 25_Corner_Axis_Frame.png
            knownHashes += "44749276140469A42022E19E6813241D"; // 26_Horizontal_Ticks_Only.png
            knownHashes += "F2B8269F5703BA6706A2E95E3F128292"; // 32_Signal_Styling.png
            knownHashes += "2F336992A4E9B18ACC434097827E67D1"; // 40_Vertical_and_Horizontal_Lines.png
            knownHashes += "3C060D6B0148F681B33F72EE6BEBC5E3"; // 50_StyleBlue1.png
            knownHashes += "297DB26AD844889C39ADC2AB7D16CCD0"; // 51_StyleBlue2.png
            knownHashes += "53BE566ACA0E24F27835F8C6F2917428"; // 52_StyleBlue3.png
            knownHashes += "A2AD6143A980A45883650ABEBF9C8547"; // 53_StyleLight1.png
            knownHashes += "D134211A2F8A61D4C76A3CC71E7EE7ED"; // 54_StyleLight2.png
            knownHashes += "9C2006DC977C6431BCBE3142399E58BF"; // 55_StyleGray1.png
            knownHashes += "817740ADD481F93C3C76E0722EA6EFB8"; // 56_StyleGray2.png
            knownHashes += "CF423BE40B79587587EF20EF7F833049"; // 57_StyleBlack.png
            knownHashes += "8E83044E828DFE1E12633ADC42EB1D5B"; // 58_StyleDefault.png
            knownHashes += "A419B4DBC69B08663A6C570F8C8C708D"; // 59_StyleControl.png

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
