using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Cookbook
{
    public static class Tools
    {
        public static string PathToID(string filePath, string className)
        {
            filePath = System.IO.Path.GetFullPath(filePath);

            string fileFolder = System.IO.Path.GetDirectoryName(filePath);
            string fileFolderName = System.IO.Path.GetFileName(fileFolder);
            string fileName = System.IO.Path.GetFileName(filePath);

            string id = fileFolderName + "_" + fileName.Replace(".cs", "") + "_" + className;

            return id;
        }
    }
}
