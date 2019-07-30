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

            List<string> hashes = new List<string>();

            hashes.Add(recipies.Figure_01a_Scatter_Sin());
            hashes.Add(recipies.Figure_01b_Automatic_Margins());
            hashes.Add(recipies.Figure_01c_Defined_Axis_Limits());
            hashes.Add(recipies.Figure_01d_Zoom_and_Pan());
            hashes.Add(recipies.Figure_01e_Legend());
            hashes.Add(recipies.Figure_01f_Custom_Marker_Shapes());
            hashes.Add(recipies.Figure_02_Styling_Scatter_Plots());
            hashes.Add(recipies.Figure_03_Plot_XY_Data());
            hashes.Add(recipies.Figure_04_Plot_Lines_Only());
            hashes.Add(recipies.Figure_05_Plot_Points_Only());
            hashes.Add(recipies.Figure_06_Styling_XY_Plots());
            hashes.Add(recipies.Figure_07_Plotting_Points());
            hashes.Add(recipies.Figure_08_Plotting_Text());
            hashes.Add(recipies.Figure_09_Clearing_Plots());
            hashes.Add(recipies.Figure_10_Modifying_Plotted_Data());

            hashes.Add(recipies.Figure_20_Small_Plot());
            hashes.Add(recipies.Figure_21a_Title_and_Axis_Labels());
            hashes.Add(recipies.Figure_21b_Extra_Padding());
            hashes.Add(recipies.Figure_22_Custom_Colors());
            hashes.Add(recipies.Figure_23_Frameless_Plot());
            hashes.Add(recipies.Figure_24_Disable_the_Grid());
            hashes.Add(recipies.Figure_25_Corner_Axis_Frame());
            hashes.Add(recipies.Figure_26_Horizontal_Ticks_Only());
            hashes.Add(recipies.Figure_27_Very_Large_Numbers());
            hashes.Add(recipies.Figure_28_Very_Small_Numbers());

            hashes.Add(recipies.Figure_30_Signal());
            hashes.Add(recipies.Figure_31_Signal_With_Antialiasing_Off());
            hashes.Add(recipies.Figure_32_Signal_Styling());

            hashes.Add(recipies.Figure_40_Vertical_and_Horizontal_Lines());

            hashes.Add(recipies.Figure_50_StyleBlue1());
            hashes.Add(recipies.Figure_51_StyleBlue2());
            hashes.Add(recipies.Figure_52_StyleBlue3());
            hashes.Add(recipies.Figure_53_StyleLight1());
            hashes.Add(recipies.Figure_54_StyleLight2());
            hashes.Add(recipies.Figure_55_StyleGray1());
            hashes.Add(recipies.Figure_56_StyleGray2());
            hashes.Add(recipies.Figure_57_StyleBlack());
            hashes.Add(recipies.Figure_58_StyleDefault());
            hashes.Add(recipies.Figure_59_StyleControl());

            hashes.Add(recipies.Figure_60_Plotting_With_Errorbars());
            hashes.Add(recipies.Figure_61_Plot_Bar_Data());
            hashes.Add(recipies.Figure_62_Plot_Bar_Data_Fancy());
            hashes.Add(recipies.Figure_63_Step_Plot());
            hashes.Add(recipies.Figure_64_Manual_Grid_Spacing());
            hashes.Add(recipies.Figure_65_Histogram());
            hashes.Add(recipies.Figure_66_CPH());
            hashes.Add(recipies.Figure_67_Candlestick());
            hashes.Add(recipies.Figure_68_OHLC());

            ValidateImageHashes(hashes.ToArray());

            GenerateReport(outputFolderName);

            Console.WriteLine("\nCOOKBOOK GENERATION COMPLETE");
            Console.WriteLine("\nOptionally update the cookbook with:");
            string cookBookCopierPath = System.IO.Path.GetFullPath("../../");
            Console.WriteLine($"cd \"{cookBookCopierPath}\" & python UPDATE_COOKBOOK.py");
            Console.WriteLine();
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
            int posStart = code.IndexOf($"public string {functionName}()");
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
                if (lines[i].Contains("BitmapHash"))
                    lines[i] = "";
            }
            code = string.Join("\n", lines).Trim();
            code = code.Replace("ScottPlot.Plot(width, height);", $"ScottPlot.Plot({figureWidth}, {figureHeight});");
            return code;
        }

        public void ValidateImageHashes(string[] hashLines)
        {
            string dontHashFunctions = "";
            dontHashFunctions += "30_Signal()\n"; // changes due to benchmark label
            dontHashFunctions += "31_Signal_With_Antialiasing_Off()\n"; // changes due to benchmark label

            string knownHahes = "";
            knownHahes += "EE0E0FE11BA6798F935202484FAE6468 01a_Scatter_Sin()\n";
            knownHahes += "6F6C7E6E49CF13B10C967295FF648CBC 01b_Automatic_Margins()\n";
            knownHahes += "454780CFDDDAEB38052FD1A593A71CC1 01c_Defined_Axis_Limits()\n";
            knownHahes += "DEF40F0126BFEE435A2B53A2B18BA220 01d_Zoom_and_Pan()\n";
            knownHahes += "E34D5DD99D5AA923A95F387623B207F0 01e_Legend()\n";
            knownHahes += "4B10F6006DBCF90208F3AB48E51C7115 01f_Custom_Marker_Shapes()\n";
            knownHahes += "E3B5D54E18CBDAD3297D4ABD2247516C 02_Styling_Scatter_Plots()\n";
            knownHahes += "632F1577ACBBD9FE3C9492C4163C8007 03_Plot_XY_Data()\n";
            knownHahes += "E4B2D57E2B6028736D11F833FF6E2581 04_Plot_Lines_Only()\n";
            knownHahes += "7CE6645727BB1586529359E7249DC52F 05_Plot_Points_Only()\n";
            knownHahes += "6381D03077328056ACAFB09F28748901 06_Styling_XY_Plots()\n";
            knownHahes += "2929B33AD295A13474D7177C4DC935B3 07_Plotting_Points()\n";
            knownHahes += "D77F5E0CDAB932D6A1C7B1C4F0A85E04 08_Plotting_Text()\n";
            knownHahes += "6E4412A926944B990CA1757D6D13F4CA 09_Clearing_Plots()\n";
            knownHahes += "7A1DBB692D556BFE3B500C26F2591E74 10_Modifying_Plotted_Data()\n";
            knownHahes += "A17537244B684AFF4D9673340D7E3430 20_Small_Plot()\n";
            knownHahes += "C6D2F4049F9188E7CA3525A7C15BAFE3 21a_Title_and_Axis_Labels()\n";
            knownHahes += "98FA227839340FFC4B1B583026C8CE93 21b_Extra_Padding()\n";
            knownHahes += "6D81AC05448CB4FD90B6AC108A1E2132 22_Custom_Colors()\n";
            knownHahes += "217ADAA362B041BCF3F738CAA85A76AF 23_Frameless_Plot()\n";
            knownHahes += "57C53700AA71BCBF266AF831F95976EE 24_Disable_the_Grid()\n";
            knownHahes += "FC5A1A1EB36B590A95DCD4B86A8C9F7A 25_Corner_Axis_Frame()\n";
            knownHahes += "8F91B7CB261867A101B3A813F25EF94B 26_Horizontal_Ticks_Only()\n";
            knownHahes += "5C2FAB44A6998EC049CC4D1CB5A69B7B 32_Signal_Styling()\n";
            knownHahes += "23047BDA67E91829AD28C7213A9AB486 40_Vertical_and_Horizontal_Lines()\n";
            knownHahes += "9BDABA2C1975890D30DBD8BC93DD5B7E 50_StyleBlue1()\n";
            knownHahes += "2D85006F9C53EB2FBB7FD4FBEB0E1356 51_StyleBlue2()\n";
            knownHahes += "86DD2FDD548216EE9152ED13DF9D4E21 52_StyleBlue3()\n";
            knownHahes += "AA0FA6C1E4507B5960A33A3C807EAF3E 53_StyleLight1()\n";
            knownHahes += "F0A95CE3972B1C761EBC8A22378AA157 54_StyleLight2()\n";
            knownHahes += "C27FB3F5D46C431699C5FA494AA2A7E6 55_StyleGray1()\n";
            knownHahes += "AB7329864526AD5C7A042409DDC50773 56_StyleGray2()\n";
            knownHahes += "BA4D8562FC7979C312F4CA7DB774BB30 57_StyleBlack()\n";
            knownHahes += "F2ED9DF9A03ACA85135ABF6CAB5A558F 58_StyleDefault()\n";
            knownHahes += "B40AE6E964ECA1FCA261047942FAEFF6 59_StyleControl()\n";
            knownHahes += "68A55A0B235E29B30D48F92543179E20 60_Plotting_With_Errorbars()\n";
            knownHahes += "2DD90A0D407A0C8325DEEDB8086EB1B5 61_Plot_Bar_Data()\n";
            knownHahes += "BE67D12106AB9C1E0446E0FC6E64D137 62_Plot_Bar_Data_Fancy()\n";
            knownHahes += "C0A3E66F1BAF9DF44EB68AE144A1DC8A 63_Step_Plot()\n";
            knownHahes += "DCC984120C142F91AF13438641BC15FE 64_Manual_Grid_Spacing()\n";
            knownHahes += "7789995EFF9D69CB6132AEEDC8083687 65_Histogram()\n";
            knownHahes += "F3A1F0FC4E0027E0DDE4B6950DF3B534 66_CPH()\n";
            knownHahes += "79698A6E3CC91FDD36810332C05C58D9 67_Candlestick()\n";
            knownHahes += "8DA76CAA67E3AB76C70E63F48B7ABCE6 68_OHLC()\n";

            string hashCodeNeeded = "";

            foreach (string hashLine in hashLines)
            {

                string[] hashParts = hashLine.Split(':');
                string functionName = hashParts[0] + "()";
                string hash = hashParts[1];

                if (dontHashFunctions.Contains(functionName))
                    continue;

                if (!knownHahes.Contains(hash))
                    hashCodeNeeded += $"knownHahes += \"{hash} {functionName}\\n\";\n";
                else
                    Console.WriteLine($"Validated hash ({hash}) for {functionName}");
            }

            if (hashCodeNeeded.Length == 0)
            {
                Console.WriteLine("\nAll hashes verified unchanged.");
            }
            else
            {
                Console.WriteLine("\nWARNING: HASH CODES CHANGED!");
                Console.WriteLine(hashCodeNeeded);
            }
        }

        public void GenerateReport(string outputFolderName)
        {
            Console.WriteLine("Generating reports...");

            string html = "<h1>ScottPlot Cookbook</h1><h3>This cookbook was automatically generated by /cookbook/src</h3>";
            string md = "# ScottPlot Cookbook\n\n_This cookbook was automatically generated by [/cookbook/src](/cookbook/src)_\n\n";

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
