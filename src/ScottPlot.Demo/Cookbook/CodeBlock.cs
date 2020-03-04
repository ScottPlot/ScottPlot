using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Cookbook
{
    public class CodeBlock
    {
        public readonly string name;
        public readonly string id;
        public readonly string filePath;
        public readonly string fileName;
        public readonly string source;

        public bool isValid { get { return !(name == "?"); } }

        public CodeBlock(string textBlock, string filePath)
        {
            this.filePath = filePath;
            fileName = System.IO.Path.GetFileName(filePath);

            textBlock = textBlock.Replace("\r", "");
            name = GetName(textBlock);

            foreach (string lineRaw in textBlock.Split('\n'))
            {
                string line = lineRaw.Trim();
                if (line.EndsWith(": PlotDemo, IPlotDemo"))
                {
                    string className = line.Split(' ')[2];
                    id = filePath.Replace(".cs","").Replace("/","_").Replace("\\", "_") + "_" + className;
                    break;
                }
            }

            source = textBlock;
        }

        public override string ToString()
        {
            return $"Code block [{id}] from \"{name}\" in file {filePath}";
        }

        private string GetName(string code)
        {
            string[] lines = code.Split('\n');
            foreach (string line in lines)
            {
                if (line.Contains("name { get; } ="))
                {
                    var name = line.Split('=')[1];
                    name = name.Trim();
                    name = name.Trim(';');
                    name = name.Trim('"');
                    return name;
                }
            }

            return "?";
        }
    }
}
