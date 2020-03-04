using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo
{
    public abstract class PlotDemo
    {
        public string classPath { get { return GetType().ToString(); } }
        public string sourceCode { get; private set; } = null;
        public string id { get { return MakeID(classPath); } }

        private string MakeID(string name)
        {
            foreach (string charToStrip in new string[] { "(", ")", "!", "," })
                name = name.Replace(charToStrip, "");

            foreach (string charToReplace in new string[] { " ", ".", "+" })
                name = name.Replace(charToReplace, "_");

            return name;
        }
    }
}
