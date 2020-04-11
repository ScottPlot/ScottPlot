using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;

namespace ScottPlotTests2
{
    [TestClass]
    public class UnitTest1
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void Test_SimplePlot_OnEveryOS()
        {
            string osName = "unknown OS";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                osName = $"Linux ({System.Environment.OSVersion})";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                osName = $"MacOS ({System.Environment.OSVersion})";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                osName = $"Windows ({System.Environment.OSVersion})";

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotSignal(ScottPlot.DataGen.Sin(100), label: "sin");
            plt.PlotSignal(ScottPlot.DataGen.Cos(100), label: "cos");
            plt.YLabel("vertical units");
            plt.XLabel("horizontal units");
            plt.Title(osName);
            plt.Legend();

            string textFilePath = System.IO.Path.GetFullPath("outputTest.txt");
            System.Console.WriteLine(textFilePath);
            System.IO.File.WriteAllText(textFilePath, "demo text file content");
            TestContext.AddResultFile(textFilePath);

            string imageFilePath = System.IO.Path.GetFullPath("outputTest.png");
            System.Console.WriteLine(imageFilePath);
            plt.SaveFig(imageFilePath);
            TestContext.AddResultFile(imageFilePath);
        }
    }
}
