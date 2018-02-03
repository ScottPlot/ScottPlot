

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    class Program
    {

        public static string outPath = "./output/";
        public static string sourceFile = "../../Cookbook.cs";

        static void Main(string[] args)
        {
            outPath = System.IO.Path.GetFullPath(outPath);
            sourceFile = System.IO.Path.GetFullPath(sourceFile);
            System.IO.Directory.CreateDirectory(outPath);

            // BASIC DEMOS
            Console.WriteLine("RUNNING ALL TESTS ...");
            Cookbook.demo_001();
            Cookbook.demo_002();
            Cookbook.demo_003();
            Cookbook.demo_004();
            Cookbook.demo_005();
            Cookbook.demo_006();
            Cookbook.demo_007();
            Cookbook.demo_008();
            Cookbook.demo_009();

            // ADVANCED DEMOS
            Cookbook.demo_101();

            Console.WriteLine("ALL TESTS COMPLETE!");
            
            // CREATE OUTPUT REPORT
            GenerateMarkdown();

            Console.WriteLine("\nOutput saved in:\n" + outPath);
            System.Diagnostics.Process.Start("explorer", outPath+"cookbook.html");
            //Console.Write("\npress ENTER to exit ... ");
            //System.Console.ReadKey();
        }

        public static void GenerateMarkdown()
        {

            var functions = new Dictionary<string, string>();

            // read the content of Cookbook.cs
            string code = System.IO.File.ReadAllText(sourceFile);
            string funcFirstLine = "/// <summary>";
            string funcLastLine =  "        }";
            string[] codeBlocks = code.Split(new string[] { funcFirstLine }, StringSplitOptions.RemoveEmptyEntries);
            codeBlocks = codeBlocks.Skip(1).ToArray(); // remove the first one

            for (int i = 0; i < codeBlocks.Length; i++)
            {
                if (!codeBlocks[i].Contains("public static void")) continue;
                codeBlocks[i] = funcFirstLine + codeBlocks[i];
                //codeBlocks[i] = codeBlocks[i].Split(new string[] { funcLastLine }, StringSplitOptions.RemoveEmptyEntries)[0];
                //codeBlocks[i] = codeBlocks[i] + funcLastLine;
                codeBlocks[i] = codeBlocks[i].Replace("\n        ", "\n");
                codeBlocks[i] = codeBlocks[i].Replace("/// <summary>", "");
                codeBlocks[i] = codeBlocks[i].Replace("/// </summary>", "");
                codeBlocks[i] = codeBlocks[i].Trim();
                string functionName;
                functionName = codeBlocks[i].Split(new string[] { "public static void" }, StringSplitOptions.RemoveEmptyEntries)[1];
                functionName = functionName.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[0];
                functionName = functionName.Trim();
                functions[functionName] = codeBlocks[i];
            }
            
            string[] pngFiles = System.IO.Directory.GetFiles(outPath, "*.png");
            Array.Sort(pngFiles);
            string md = "# ScottPlot Cookbook\n\n";
            md += "This document was generated automatically by a [program](/src/Examples/console/ConsoleApp1/Program.cs) ";
            md += " which pulls examples from [Cookbook.cs](/src/Examples/console/ConsoleApp1/Cookbook.cs). ";
            md += "Since it generates graphs using many different features, it doubles as both a ";
            md += "test script and a cookbook to demonstrate various uses of the ScottPlot API.";
            string html = "<html><body><h1>ScottPlot Cookbook</h1>";
            html += "<script src='https://cdn.rawgit.com/google/code-prettify/master/loader/run_prettify.js?skin=desert'></script>";
            html += "<table width='100%'>";
            foreach (string pngFile in pngFiles)
            {
                string basename = System.IO.Path.GetFileName(pngFile);
                string functionName = basename.Replace(".png", "");
                string htmlSafeCode = functions[functionName];
                htmlSafeCode = htmlSafeCode.Replace("<", "&lt;").Replace(">", "&gt;");

                // title
                md += $"\n\n## {functionName}\n";
                html += $"<tr><td colspan='2' style='border-top: 5px solid #EEE;'> </tr>";
                html += $"<tr valign='top'><td width='100%'><h2>{functionName}</h2>";

                // code
                md += $"\n```C#\n{functions[functionName]}\n```\n";
                html += $"<pre style ='border: none; padding: 10px; border-radius: 15px;' class='prettyprint cs'>{htmlSafeCode}</pre></td>";

                // screenshot
                md += $"![]({basename})\n";
                html += $"<td><img src='{basename}'></td>";
            }
            html += "</tr></table></html></body>";
            System.IO.File.WriteAllText(outPath + "readme.md", md);
            System.IO.File.WriteAllText(outPath + "cookbook.html", html);
        }
    }
}