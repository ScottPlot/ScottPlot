

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using ScottPlot;

namespace ConsoleApp1
{
    class Program
    {
        public static DataGen gen = new ScottPlot.DataGen();

        static void Main(string[] args)
        {
            bool testAll = true;
            //testAll = false;

            if (testAll==true)
            {
                Console.WriteLine("RUNNING ALL TESTS:");
                test00_directDrawLine();
                test01_doubleArray();
                HTMLgen();
                Console.WriteLine("\nTests completed successfully.");
            } else
            {
                Console.WriteLine("### Experimental Code ###");
                /* EXPERIMENTAL CODE */

            }

            //System.Diagnostics.Process.Start("test01.png");
            System.Threading.Thread.Sleep(1000);

            //Console.Write("\npress ENTER to exit ... ");
            //System.Console.ReadKey();

        }

        /// <summary>
        /// Create a HTML report of all test figures
        /// </summary>
        public static void HTMLgen(bool launch=true)
        {
            string[] filePaths = System.IO.Directory.GetFiles("./", "*.png", System.IO.SearchOption.TopDirectoryOnly);
            string html = "<html><body><div align='center'>";
            string md = "# ScottPlot - Automated Test Output\n";
            md += "_These graphs are automatically generated from the latest ScottPlot using the code in [Program.cs](/)_\n\n";
            html += "<h1><u>ScottPlot Test Sequence</u></h1>";
            foreach (string filePath in filePaths)
            {
                html += $"<img style='padding-top: 50px;' src='{filePath}'><br>";
                md += $"![]({filePath})\n";
            }
            html += "</div></body></html>";
            System.IO.File.WriteAllText("output.html", html);
            System.IO.File.WriteAllText("readme.md", md);

            if (launch) System.Diagnostics.Process.Start("explorer", "output.html");
        }

        /// <summary>
        /// minimal-case example drawing a line and saving a file
        /// </summary>
        public static void test00_directDrawLine()
        {
            // create a new ScottPlot figure
            Figure fig = new Figure(640, 480);
            fig.title = "Direct Graphics Drawing";
            fig.yLabel = "Pure Awesomeness";
            fig.xLabel = "Relative Time (years)";
            fig.Axis(-15, 35, -10, 110); // x1, x2, y1, y2
            fig.BenchmarkThis();

            // draw a line directly on the Graphics object in AXIS units
            Point pt1 = new Point(fig.xAxis.UnitToPx(0), fig.yAxis.UnitToPx(13));
            Point pt2 = new Point(fig.xAxis.UnitToPx(32), fig.yAxis.UnitToPx(98));
            fig.gfxGraph.DrawLine(new Pen(new SolidBrush(Color.Blue), 5), pt1, pt2);

            // save the file
            fig.Save("test00.png");
        }

        /// <summary>
        /// plot data from an Xs and Ys array (of doubles)
        /// </summary>
        public static void test01_doubleArray()
        {

            // create a new ScottPlot figure
            Figure fig = new Figure(640, 480);
            fig.title = "Plotting Point Arrays";
            fig.yLabel = "Random Walk";
            fig.xLabel = "Sample Number";

            // generate data
            int pointCount = 123; // number of points which will be in this graph
            double[] Xs = gen.Sequence(pointCount); // create a series of Xs
            double[] Ys = gen.RandomWalk(pointCount);
            fig.ResizeToData(Xs, Ys, .9, .9);

            // make the plot
            fig.BenchmarkThis();
            fig.PlotLines(Xs, Ys, 1, Color.Red);
            fig.PlotScatter(Xs, Ys, 5, Color.Blue);
            
            // save the file
            fig.Save("test01.png");
        }
    }


}
