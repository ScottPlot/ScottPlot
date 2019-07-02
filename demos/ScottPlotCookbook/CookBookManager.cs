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

            ValidateImageHashes(hashes.ToArray());

            Console.WriteLine("\nCOOKBOOK GENERATION COMPLETE");
            Console.WriteLine("\nOptionally update the cookbook with:");
            string cookBookCopierPath = System.IO.Path.GetFullPath("../../");
            Console.WriteLine($"cd \"{cookBookCopierPath}\" & \"COPY-COOKBOOK.bat\"");
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

        public void ValidateImageHashes(string[] hashLines)
        {
            string dontHashFunctions = "";
            dontHashFunctions += "30_Signal()\n"; // changes due to benchmark label
            dontHashFunctions += "31_Signal_With_Antialiasing_Off()\n"; // changes due to benchmark label

            string knownHahes = "";
            knownHahes += "F19AAD6C7E8831AE6950D7466C27CC09 01a_Scatter_Sin()\n";
            knownHahes += "2ACE246498ED9751FEDF869C6F0602E8 01b_Automatic_Margins()\n";
            knownHahes += "79A4E9CA633B3A95AA69C2CF28351BF2 01c_Defined_Axis_Limits()\n";
            knownHahes += "E3187703C560CAC6C13C86FF91279B22 01d_Zoom_and_Pan()\n";
            knownHahes += "27BF96B88858EC007B4BBA92D8252DBF 01e_Legend()\n";
            knownHahes += "6CC2D90F79EEAE0A0534522292C09901 02_Styling_Scatter_Plots()\n";
            knownHahes += "29FCEEC7E7B8517C59877B6C8EBD03A1 03_Plot_XY_Data()\n";
            knownHahes += "BD4BD41AB890F6B3A96E3DF118AFC5BC 04_Plot_Lines_Only()\n";
            knownHahes += "D232B7DC34989D5128B45BAA3A2AFBD5 05_Plot_Points_Only()\n";
            knownHahes += "2A1E94173C51B411F0AB69F0E715DC3A 06_Styling_XY_Plots()\n";
            knownHahes += "49EBD7AD30439E771ECD7CB5E7A32E2C 07_Plotting_Points()\n";
            knownHahes += "00F8E55344AA4B997AC750EF4433A538 08_Plotting_Text()\n";
            knownHahes += "CE36993D993E01F96EDF544E4404C03C 09_Clearing_Plots()\n";
            knownHahes += "63483359F18FC63792D75817C0B26B83 10_Modifying_Plotted_Data()\n";
            knownHahes += "93F00155ABC24CE2F3EC377FB8CC991C 20_Small_Plot()\n";
            knownHahes += "C1C14875B085144B5CF6BCBF62BD379E 21a_Title_and_Axis_Labels()\n";
            knownHahes += "1D44E0EC1E97211BD05E96405B346A7A 21b_Extra_Padding()\n";
            knownHahes += "46E3B3CCE955204526A4AE2BD4CD8B40 22_Custom_Colors()\n";
            knownHahes += "97128192B975404E99C093952871BE64 23_Frameless_Plot()\n";
            knownHahes += "2AE685629FE075F399C04718468D6D4D 24_Disable_the_Grid()\n";
            knownHahes += "8F04D5A37EE745EAA20A7D2B0A2C37D6 25_Corner_Axis_Frame()\n";
            knownHahes += "FA7F5D79DD7881A8E683CC905B31A115 26_Horizontal_Ticks_Only()\n";
            knownHahes += "027C1965628073DC1C3C63FFD6CFC2A4 30_Signal()\n";
            knownHahes += "A195BF94C538C5249E4B56505EF12D3A 31_Signal_With_Antialiasing_Off()\n";
            knownHahes += "B662FD3A0D5BA1DA8C8DBDFA2529F16A 32_Signal_Styling()\n";
            knownHahes += "8257EACCFDB5AA8717187CCB4CFFE383 40_Vertical_and_Horizontal_Lines()\n";
            knownHahes += "05EFB448C0C216AA100835739A9A2DE2 50_StyleBlue1()\n";
            knownHahes += "F52223B082A0F977751E69EA355887CC 51_StyleBlue2()\n";
            knownHahes += "7D2D35B91727B67E8D81C28C03490527 52_StyleBlue3()\n";
            knownHahes += "51685FE67C19F9BB5D667EB057533E0D 53_StyleLight1()\n";
            knownHahes += "FE1A97953EF3B0A4FC8FED6053D3A815 54_StyleLight2()\n";
            knownHahes += "E5D30E489832856E2D480F23032A0862 55_StyleGray1()\n";
            knownHahes += "D280C85A67025A467BADA3E28B43E3D7 56_StyleGray2()\n";
            knownHahes += "869C57A78ABF7120D58DA737C041D19B 57_StyleBlack()\n";
            knownHahes += "C02D5F2DC9A164EEDB2D04B22A6093F1 58_StyleDefault()\n";
            knownHahes += "148788132B3AA9A3C81EA066F8EFD1EE 59_StyleControl()\n";
            knownHahes += "DA9828A14CDDFA2CAC31EA214C57F3FE 60_Plotting_With_Errorbars()\n";
            knownHahes += "5568C8AD48CE9D734B1A27E1A8639E22 61_Plot_Bar_Data()\n";
            knownHahes += "9AE51BF77753951F7E71BE38CE4BC276 62_Plot_Bar_Data_Fancy()\n";
            knownHahes += "63462062CE1EC40B74F6919174C46737 63_Step_Plot()\n";
            knownHahes += "71C7AD954E03BB5580D6253379FAA368 64_Manual_Grid_Spacing()\n";
            knownHahes += "9D1542421E6B8ACC5428BF9A07479AA7 65_Histogram()\n";
            knownHahes += "BEA38BDABDA26A61C1F46802F225D68E 66_CPH()\n";

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
