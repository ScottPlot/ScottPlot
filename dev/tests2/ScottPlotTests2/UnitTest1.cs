using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void TestMethod1()
        {
            var plt = new ScottPlot.Plot();
            plt.PlotSignal(ScottPlot.DataGen.Sin(100));

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
