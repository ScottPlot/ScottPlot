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
            var recipies = new ScottPlotTests.Recipes(outputFolderName, figureWidth, figureHeight);

            List<string> hashes = new List<string>();

            recipies.Figure_01a_Scatter_Sin();
            recipies.Figure_01b_Automatic_Margins();
            recipies.Figure_01c_Defined_Axis_Limits();
            recipies.Figure_01d_Zoom_and_Pan();
            recipies.Figure_01e_Legend();
            recipies.Figure_01f_Custom_Marker_Shapes();
            recipies.Figure_01g_All_Marker_Shapes();
            recipies.Figure_02_Styling_Scatter_Plots();
            recipies.Figure_03_Plot_XY_Data();
            recipies.Figure_04_Plot_Lines_Only();
            recipies.Figure_05_Plot_Points_Only();
            recipies.Figure_06_Styling_XY_Plots();
            recipies.Figure_06b_Custom_LineStyles();
            recipies.Figure_07_Plotting_Points();
            recipies.Figure_08_Plotting_Text();
            recipies.Figure_09_Clearing_Plots();
            recipies.Figure_10_Modifying_Plotted_Data();
            recipies.Figure_11_Modify_Styles_After_Plotting();
            recipies.Figure_12_Date_Axis();
            recipies.Figure_13_Ruler_Mode();

            recipies.Figure_20_Small_Plot();
            recipies.Figure_21a_Title_and_Axis_Labels();
            recipies.Figure_21b_Custom_Padding();
            recipies.Figure_21c_Automatic_Left_Padding();
            recipies.Figure_21d_Single_Axis_With_No_Padding();
            recipies.Figure_22_Custom_Colors();
            recipies.Figure_23_Frameless_Plot();
            recipies.Figure_24_Disable_the_Grid();
            recipies.Figure_25_Corner_Axis_Frame();
            recipies.Figure_26_Horizontal_Ticks_Only();
            recipies.Figure_27_Very_Large_Numbers();
            recipies.Figure_28_Axis_Exponent_And_Offset();
            recipies.Figure_28b_Multiplier_Notation_Default();
            recipies.Figure_28c_Multiplier_Notation_Disabled();
            recipies.Figure_29_Very_Large_Images();

            recipies.Figure_30a_Signal();
            //recipies.Figure_30b_Signal_With_Parallel_Processing();
            recipies.Figure_30c_SignalConst();
            //recipies.Figure_30d_SignalConst_One_Billion_Points(); // SLOW!
            recipies.Figure_32_Signal_Styling();

            recipies.Figure_40_Vertical_and_Horizontal_Lines();
            recipies.Figure_41_Axis_Spans();

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
            recipies.Figure_64_Manual_Grid_Spacing();
            recipies.Figure_65_Histogram();
            recipies.Figure_66_CPH();
            recipies.Figure_67_Candlestick();
            recipies.Figure_68_OHLC();

            recipies.Figure_70_Save_Scatter_Data();
            recipies.Figure_71_Save_Signal_Data();
            recipies.Figure_72_Custom_Fonts();

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

        public string ReadCodeFromFile(string functionName)
        {
            string code;

            // read contents of file
            string sourceFilePath = "../../../../tests/ScottPlotTests/Recipes.cs";
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
                if (lines[i].Contains("BitmapHash"))
                    lines[i] = "";
                if (lines[i].Contains("SaveFig"))
                    lines[i] = $"plt.SaveFig(\"{functionName}.png\");";
            }
            code = string.Join("\n", lines).Trim();
            code = code.Replace("ScottPlot.Plot(width, height);", $"ScottPlot.Plot({figureWidth}, {figureHeight});");
            return code;
        }

        public void GenerateReport(string outputFolderName)
        {
            Console.WriteLine("Generating reports...");

            string html = $"<h1>ScottPlot Cookbook</h1>";
            html += $"<h3>This cookbook was automatically generated by /cookbook/src using ScottPlot {ScottPlot.Tools.GetVersionString()}</h3>";
            html += $"<p>The ScottPlot cookbook is a collection of small code examples which demonstrates how to create various types of plots using ScottPlot.</p>";

            string md = $"# ScottPlot Cookbook\n\n";
            md += $"The ScottPlot cookbook is a collection of small code examples which demonstrates how to create various types of plots using ScottPlot.\n\n";
            md += $"_This cookbook was automatically generated by [/cookbook/src](/cookbook/src) using ScottPlot {ScottPlot.Tools.GetVersionString()}_\n\n";

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
