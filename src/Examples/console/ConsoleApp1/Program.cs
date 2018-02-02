

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

            // RUN TESTS
            Console.WriteLine("RUNNING ALL TESTS ...");
            Cookbook.demo_001();
            Cookbook.demo_002();
            Console.WriteLine("ALL TESTS COMPLETE!");
            
            // CREATE OUTPUT REPORT
            GenerateMarkdown();

            Console.WriteLine("\nOutput saved in:\n" + outPath);
            System.Diagnostics.Process.Start("explorer", outPath);
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
                codeBlocks[i] = funcFirstLine + codeBlocks[i];
                codeBlocks[i] = codeBlocks[i].Split(new string[] { funcLastLine }, StringSplitOptions.RemoveEmptyEntries)[0];
                codeBlocks[i] = codeBlocks[i] + funcLastLine;
                codeBlocks[i] = codeBlocks[i].Replace("\n        ", "\n");
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
            foreach (string pngFile in pngFiles)
            {
                string basename = System.IO.Path.GetFileName(pngFile);
                string functionName = basename.Replace(".png", "");
                md += $"\n\n## {functionName}\n";
                md += $"\n```C#\n{functions[functionName]}\n```\n";
                md += $"![]({basename})\n";
            }
            System.IO.File.WriteAllText(outPath + "readme.md", md);
            System.Console.WriteLine(outPath + "readme.md has been recreated.");
                
            }
    }
}