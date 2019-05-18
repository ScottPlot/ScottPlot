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
        int width = 640;
        int height = 480;
        int figuresPlotted = 0;
        string outputFolderName = "images";

        public CookBookManager()
        {
            var plt = new ScottPlot.Plot(); // do this to pre-load modules
        }

        public void GenerateAllFigures(int width, int height)
        {
            this.width = width;
            this.height = height;

            Debug.WriteLine("generating data...");
            Debug.Indent();
            GenerateData_SinAndCos();
            Debug.IndentLevel = 0;

            Debug.WriteLine("preparing output folder...");
            Debug.Indent();
            ClearOutputFolder();
            Debug.IndentLevel = 0;

            Debug.WriteLine("generating cookbook figures...");
            Debug.Indent();
            Figure_Generic();
            Debug.IndentLevel = 0;

            Debug.WriteLine("generating reports...");
            GenerateReport();
            Debug.IndentLevel = 0;

            Debug.WriteLine("COMPLETE");
        }

        public void ClearOutputFolder()
        {
            string outputFolder = System.IO.Path.GetFullPath($"./{outputFolderName}/");
            Debug.WriteLine($"Checking output folder: {outputFolder}");
            if (System.IO.Directory.Exists(outputFolder))
            {
                Debug.WriteLine($"confirmed output folder exists");
            }
            else
            {
                System.IO.Directory.CreateDirectory(outputFolder);
                Debug.WriteLine($"output folder created");
            }

            foreach (string fileName in System.IO.Directory.GetFiles(outputFolder, "*.*"))
            {
                Debug.WriteLine($"deleting [{fileName}] ...");
                System.IO.File.Delete(System.IO.Path.Combine(outputFolder, fileName));
            }
        }

        double[] dataXs;
        double[] dataSin;
        double[] dataCos;
        public void GenerateData_SinAndCos(int pointCount = 100)
        {
            Debug.WriteLine($"simple Sin and Cos for scatter plots ({pointCount} points)");
            dataXs = new double[pointCount];
            dataSin = new double[pointCount];
            dataCos = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                dataXs[i] = i;
                dataSin[i] = Math.Sin(i * 2 * Math.PI / pointCount);
                dataCos[i] = Math.Cos(i * 2 * Math.PI / pointCount);
            }
        }

        public void Figure_Generic()
        {
            string name = string.Format("{0:000}-{1}", figuresPlotted++, System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "").ToLower());
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.SaveFig(fileName);
            Debug.WriteLine($"Saved: {fileName}");
        }

        public void GenerateReport()
        {
            string html = "";
            string md = "";

            string[] images = System.IO.Directory.GetFiles($"./{outputFolderName}", "*.png");
            Array.Sort(images);

            foreach (string image in images)
            {
                string url = image.Replace("\\", "/");
                string name = System.IO.Path.GetFileNameWithoutExtension(url);
                html += $"<h1>{name}</h1><a href='{url}'><img src='{url}' /></a>";
                md += $"## {name}\n\n![]({url})\n\n";
            }

            html = $"<html><body>{html}</body></html>";

            string pathHtml = System.IO.Path.GetFullPath("cookbook.html");
            System.IO.File.WriteAllText(pathHtml, html);
            Debug.WriteLine($"Saved HTML: {html}");

            string pathMd = System.IO.Path.GetFullPath("cookbook.md");
            System.IO.File.WriteAllText(pathMd, md);
            Debug.WriteLine($"Saved markdown: {pathMd}");
        }
    }
}
