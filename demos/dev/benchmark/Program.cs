using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Benchmark_Signal_1M_Points();
        }

        public static void Benchmark_Signal_1M_Points(int redrawCount = 1000)
        {

            // create the plot
            int pointCount = 1_000_000;
            Random rand = new Random(0);
            double[] data = ScottPlot.DataGen.RandomWalk(rand, pointCount);
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var pltTest = new ScottPlot.Plot();
            pltTest.Title(methodName.Replace("_", " "));
            pltTest.PlotSignal(data);
            pltTest.AxisAuto();
            pltTest.SaveFig(methodName + ".png");

            // time how long it takes to redraw it
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            double[] redrawTimes = new double[redrawCount];
            for (int i = 0; i < redrawCount; i++)
            {
                Console.WriteLine($"{methodName}() rendering {i + 1} of {redrawCount} ...");
                stopwatch.Restart();
                pltTest.GetBitmap(true);
                double timeMS = stopwatch.ElapsedTicks * 1000.0 / System.Diagnostics.Stopwatch.Frequency;
                redrawTimes[i] = timeMS;
            }
        }

        public static void CreateHistogram(string methodName, double[] redrawTimes)
        {
            var hist = new ScottPlot.Histogram(redrawTimes);
            var pltHist = new ScottPlot.Plot();
            pltHist.Title(methodName.Replace("_", " ") + $" ({redrawTimes.Length} runs)");
            pltHist.YLabel("count");
            pltHist.XLabel("render time (ms)");
            pltHist.PlotBar(hist.bins, hist.counts);
            pltHist.SaveFig(methodName + "_benchmark.png");
        }
    }
}
