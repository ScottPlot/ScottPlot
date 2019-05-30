using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Tests tests = new Tests();
            tests.TestGroup_Instantiation();
            tests.TestGroup_PlotTypes(clearAfter: false);
            tests.TestGroup_Resize();
            tests.TestGroup_Axes();

            Console.WriteLine("\nAll tests complete.");
            Console.WriteLine($"Passed: {tests.passed}");
            Console.WriteLine($"Failed: {tests.failed}");

            Console.WriteLine("\npress ENTER to exit...");
            Console.ReadLine();
        }

    }

    public class Tests
    {
        public int passed = 0;
        public int failed = 0;

        ScottPlot.Plot plt;
        Random rand = new Random();

        public double[] xs;
        public double[] ys;
        public double[] signal;

        public Tests()
        {
            int pointsScatter = 1_000;
            int pointsSignal = 1_000_000;

            xs = new double[pointsScatter];
            ys = new double[pointsScatter];
            for (int i = 0; i < xs.Length; i++)
            {
                xs[i] = (rand.NextDouble() - .5) * 100;
                ys[i] = (rand.NextDouble() - .5) * 100;
            }

            signal = new double[pointsSignal];
            for (int i = 0; i < pointsSignal; i++)
                signal[i] = (rand.NextDouble() - .5) * 100;
        }

        private void Pass()
        {
            Console.WriteLine("PASS");
            passed += 1;
        }

        public void TestGroup_Instantiation()
        {
            Test_Instantiation(600, 400);
            Test_Instantiation(20, 400);
            Test_Instantiation(600, 20);
            Test_Instantiation(1, 400);
            Test_Instantiation(600, 1);
            Test_Instantiation(1, 1);
        }

        public void TestGroup_PlotTypes(int width = 600, int height = 400, bool clearAfter = true)
        {
            plt = new ScottPlot.Plot(width, height);
            Test_AddPlot_Text();
            Test_AddPlot_Point();
            Test_AddPlot_Scatter();
            Test_AddPlot_Signal();
            Test_AddPlot_Vline();
            Test_AddPlot_Hline();
            plt.Title("title");
            plt.XLabel("horizontal");
            plt.YLabel("vertical");
            plt.Legend();

            Console.Write($"Rendering {plt.GetPlottables().Count} plots with {plt.GetTotalPoints()} points ... ");
            plt.GetBitmap();
            Pass();

            if (clearAfter)
                ClearPlot();

        }

        public void TestGroup_Resize()
        {
            Test_Resize(600, 400);
            Test_Resize(100, 50);
            Test_Resize(1, 400);
            Test_Resize(600, 1);
            Test_Resize(0, 400);
            Test_Resize(600, 0);
            Test_Resize(-10, 400);
            Test_Resize(600, -10);
            Test_Resize(-123, -123);
            Test_Resize(800, 600);
        }

        private void Test_Resize(int width = 600, int height = 400)
        {
            Console.Write($"Resizing to [{width}, {height}] ... ");
            plt.Resize(width, height);
            plt.GetBitmap();
            Pass();
        }

        public void ClearPlot()
        {
            Console.Write($"Clearing plot of all data elements ... ");
            Debug.Assert(plt.GetPlottables().Count > 0);
            plt.Clear();
            Debug.Assert(plt.GetPlottables().Count == 0);
            Pass();
        }

        private bool axesAreDifferent(double[] axes1, double[] axes2)
        {
            if (axes1.Length != axes2.Length)
                return true;
            for (int i = 0; i < axes1.Length; i++)
                if (axes1[i] != axes2[i])
                    return true;
            return false;
        }

        public void TestGroup_Axes()
        {
            Console.Write($"Axis adjustments (auto, pan, zoom) ... ");
            plt.Clear();
            plt.PlotScatter(xs, ys);
            plt.AxisAuto();
            double[] axesBefore = new double[4];
            Array.Copy(plt.Axis(), axesBefore, 4);
            plt.Axis(-1, 1, -1, 1);
            Debug.Assert(axesAreDifferent(axesBefore, plt.Axis()));
            plt.AxisAuto();
            Debug.Assert(!axesAreDifferent(axesBefore, plt.Axis()));
            plt.AxisPan(1, 2);
            Debug.Assert(axesAreDifferent(axesBefore, plt.Axis()));
            plt.AxisAuto();
            Debug.Assert(!axesAreDifferent(axesBefore, plt.Axis()));
            plt.AxisZoom(2, 3);
            Debug.Assert(axesAreDifferent(axesBefore, plt.Axis()));
            plt.AxisAuto();
            Debug.Assert(!axesAreDifferent(axesBefore, plt.Axis()));
            Pass();
        }

        private void TestGroup_Display()
        {
            // resize too!
        }

        private void Test_Instantiation(int width, int height)
        {
            Console.Write($"Instantiating new ScottPlot [{width}, {height}] ... ");
            plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(xs, ys);
            plt.AxisAuto();
            plt.GetBitmap();
            Pass();
        }

        private void Test_AddPlot_Text()
        {
            double x = Math.Round(rand.NextDouble() * 100, 2);
            double y = Math.Round(rand.NextDouble() * 100, 2);
            Console.Write($"Adding text to ({x}, {y}) ... ");
            plt.PlotText("test", x, y, label: "text");
            Pass();
        }

        private void Test_AddPlot_Point()
        {
            double x = Math.Round(rand.NextDouble() * 100, 2);
            double y = Math.Round(rand.NextDouble() * 100, 2);
            Console.Write($"Adding point to ({x}, {y}) ... ");
            plt.PlotPoint(x, y, label: "point");
            Pass();
        }

        private void Test_AddPlot_Scatter()
        {
            Console.Write($"Adding scatter plot with {xs.Length} points... ");
            plt.PlotScatter(xs, ys, label: "scatter");
            Pass();
        }

        private void Test_AddPlot_Signal()
        {
            Console.Write($"Adding signal with {signal.Length} points... ");
            plt.PlotSignal(signal, label: "signal");
            Pass();
        }

        private void Test_AddPlot_Vline()
        {
            double x = Math.Round(rand.NextDouble() * 100, 2);
            Console.Write($"Adding vertical line at {x} ... ");
            plt.PlotVLine(x, label: "vline");
            Pass();
        }

        private void Test_AddPlot_Hline()
        {
            double y = Math.Round(rand.NextDouble() * 100, 2);
            Console.Write($"Adding horizontal line at {y} ... ");
            plt.PlotHLine(y, label: "hline");
            Pass();
        }

    }
}
