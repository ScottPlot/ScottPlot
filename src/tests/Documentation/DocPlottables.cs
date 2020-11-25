using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Documentation
{
    class DocPlottables
    {
        private string createHash = null;

        [Test]
        public void Test_DocPlottables_CreateAndAdd()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot(400, 300);
            var scatter = new ScottPlot.Plottable.ScatterPlot(xs, ys)
            {
                color = Color.Green,
                lineWidth = 2
            };
            plt.Add(scatter);

            TestTools.SaveFig(plt);

            string thisHash = ScottPlot.Tools.BitmapHash(plt.Render());
            createHash ??= thisHash;
            Assert.AreEqual(createHash, thisHash);
        }

        [Test]
        public void Test_DocPlottables_CreateWithHelperMethod()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotScatter(xs, ys, color: Color.Red, lineWidth: 2);

            TestTools.SaveFig(plt);

            string thisHash = ScottPlot.Tools.BitmapHash(plt.Render());
            createHash ??= thisHash;
            Assert.AreEqual(createHash, thisHash);
        }

        [Test]
        public void Test_DocPlottables_CreateWithHelperMethodAndModify()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot(400, 300);
            var scatter = plt.PlotScatter(xs, ys);
            scatter.color = Color.Red;
            scatter.lineWidth = 2;

            TestTools.SaveFig(plt);

            string thisHash = ScottPlot.Tools.BitmapHash(plt.Render());
            createHash ??= thisHash;
            Assert.AreEqual(createHash, thisHash);
        }

        [Test]
        public void Test_DocPlottables_ModifyValues()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot(400, 300);
            var scatter = plt.PlotScatter(xs, ys);

            TestTools.SaveFig(plt, "before");
            ys[2] = 23;
            TestTools.SaveFig(plt, "after");
        }
    }
}
