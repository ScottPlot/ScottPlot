using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo
{
    public abstract class RecipeBase
    {
        public string classPath => GetType().ToString();
        public string id => $"{categoryMajor}_{categoryMinor}_{categoryClass}";
        public string sourceFile => $"/src/ScottPlot.Demo/{categoryMajor}/{categoryMinor}.cs";

        public string categoryMajor
        {
            get
            {
                string category = classPath.Substring(15);
                string[] pathAndName = category.Split('+');
                string[] folderAndFile = pathAndName[0].Split('.');
                return folderAndFile[0];
            }
        }

        public string categoryMinor
        {
            get
            {
                string category = classPath.Substring(15);
                string[] pathAndName = category.Split('+');
                string[] folderAndFile = pathAndName[0].Split('.');
                return folderAndFile[1];
            }
        }

        public string categoryClass
        {
            get
            {
                string category = classPath.Substring(15);
                string[] pathAndName = category.Split('+');
                return pathAndName[1];
            }
        }

        public string GetSourceCode(string pathDemoFolder) => "TODO: new sourcecode lookup system";
    }
}
