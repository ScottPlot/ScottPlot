using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Plot
{
    class Move
    {
        [Test]
        public void Test_Move_Works()
        {
            Random rand = new(0);

            var plt = new ScottPlot.Plot();
            var sig1 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig2 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig3 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig4 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig5 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));

            Assert.AreEqual(sig1, plt.GetPlottables()[0]);
            Assert.AreEqual(sig2, plt.GetPlottables()[1]);
            Assert.AreEqual(sig3, plt.GetPlottables()[2]);
            Assert.AreEqual(sig4, plt.GetPlottables()[3]);
            Assert.AreEqual(sig5, plt.GetPlottables()[4]);

            plt.Move(2, 1);
            Assert.AreEqual(sig1, plt.GetPlottables()[0]);
            Assert.AreEqual(sig3, plt.GetPlottables()[1]);
            Assert.AreEqual(sig2, plt.GetPlottables()[2]);
            Assert.AreEqual(sig4, plt.GetPlottables()[3]);
            Assert.AreEqual(sig5, plt.GetPlottables()[4]);

            plt.Move(2, 4);
            Assert.AreEqual(sig1, plt.GetPlottables()[0]);
            Assert.AreEqual(sig3, plt.GetPlottables()[1]);
            Assert.AreEqual(sig4, plt.GetPlottables()[2]);
            Assert.AreEqual(sig5, plt.GetPlottables()[3]);
            Assert.AreEqual(sig2, plt.GetPlottables()[4]);
        }

        [Test]
        public void Test_Move_First()
        {
            Random rand = new(0);

            var plt = new ScottPlot.Plot();
            var sig1 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig2 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig3 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig4 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig5 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));

            plt.MoveFirst(sig3);
            Assert.AreEqual(sig3, plt.GetPlottables()[0]);
            Assert.AreEqual(sig1, plt.GetPlottables()[1]);
            Assert.AreEqual(sig2, plt.GetPlottables()[2]);
            Assert.AreEqual(sig4, plt.GetPlottables()[3]);
            Assert.AreEqual(sig5, plt.GetPlottables()[4]);
        }

        [Test]
        public void Test_Move_Last()
        {
            Random rand = new(0);

            var plt = new ScottPlot.Plot();
            var sig1 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig2 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig3 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig4 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));
            var sig5 = plt.AddSignal(ScottPlot.DataGen.Random(rand, 100));

            plt.MoveLast(sig3);
            Assert.AreEqual(sig1, plt.GetPlottables()[0]);
            Assert.AreEqual(sig2, plt.GetPlottables()[1]);
            Assert.AreEqual(sig4, plt.GetPlottables()[2]);
            Assert.AreEqual(sig5, plt.GetPlottables()[3]);
            Assert.AreEqual(sig3, plt.GetPlottables()[4]);
        }
    }
}
