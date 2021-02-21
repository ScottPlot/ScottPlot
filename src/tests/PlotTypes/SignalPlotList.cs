using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    class SignalPlotList
    {
        [Test]
        public void Test_SignalPlotList_InitRespectsCapacity()
        {
            var plt = new ScottPlot.Plot();
            var spl = plt.AddSignalList(capacity: 3);

            Assert.AreEqual(3, spl.Capacity);
            Assert.AreEqual(0, spl.Count);
            Assert.AreEqual(0, spl.FillPercentage);
        }

        [Test]
        public void Test_SignalPlotList_AddFillsYs()
        {
            var plt = new ScottPlot.Plot();
            var spl = plt.AddSignalList(capacity: 3);

            spl.Add(111);
            spl.Add(222);
            spl.Add(333);
            Assert.AreEqual(new double[] { 111, 222, 333 }, spl.Ys);
            Assert.AreEqual(3, spl.Count);
            Assert.AreEqual(100, spl.FillPercentage);
        }

        [Test]
        public void Test_SignalPlotList_YsGrowsAutomatically()
        {
            var plt = new ScottPlot.Plot();
            var spl = plt.AddSignalList(capacity: 3);

            spl.Add(111);
            spl.Add(222);
            spl.Add(333);
            spl.Add(444);
            spl.Add(555);
            spl.Add(666);
            Assert.AreEqual(new double[] { 111, 222, 333, 444, 555, 666 }, spl.Ys);
        }

        [Test]
        public void Test_SignalPlotList_AddMovesMaxRenderIndex()
        {
            var plt = new ScottPlot.Plot();
            var spl = plt.AddSignalList(capacity: 3);

            spl.Add(111);
            Assert.AreEqual(0, spl.MaxRenderIndex);

            spl.Add(222);
            Assert.AreEqual(1, spl.MaxRenderIndex);

            spl.Add(333);
            Assert.AreEqual(2, spl.MaxRenderIndex);

            spl.Add(444);
            Assert.AreEqual(3, spl.MaxRenderIndex);

            spl.Add(555);
            Assert.AreEqual(4, spl.MaxRenderIndex);
        }

        [Test]
        public void Test_SignalPlotList_Clear()
        {
            var plt = new ScottPlot.Plot();
            var spl = plt.AddSignalList(capacity: 3);

            spl.Add(111);
            spl.Add(222);
            Assert.AreEqual(1, spl.MaxRenderIndex);
            Assert.AreEqual(2, spl.Count);
            Assert.AreEqual(100.0 * 2 / 3, spl.FillPercentage);

            spl.Clear();
            Assert.AreEqual(new double[] { 111, 222, 0 }, spl.Ys);
            Assert.AreEqual(0, spl.MaxRenderIndex);
            Assert.AreEqual(0, spl.Count);
            Assert.AreEqual(0, spl.FillPercentage);

            spl.Add(333);
            spl.Add(444);
            spl.Add(555);
            spl.Add(666);
            Assert.AreEqual(new double[] { 333, 444, 555, 666, 0, 0 }, spl.Ys);
            Assert.AreEqual(4, spl.Count);
            Assert.AreEqual(100.0 * 4 / 6, spl.FillPercentage);
        }

        [Test]
        public void Test_SignalPlotList_AddRange()
        {
            var plt = new ScottPlot.Plot();
            var spl = plt.AddSignalList(capacity: 3);

            spl.AddRange(new double[] { 111, 222, 333 });
            Assert.AreEqual(new double[] { 111, 222, 333 }, spl.Ys);
        }

        [Test]
        public void Test_SignalPlotList_AddRangeGrowsAutomatically()
        {
            var plt = new ScottPlot.Plot();
            var spl = plt.AddSignalList(capacity: 3);

            spl.AddRange(new double[] { 111, 222, 333, 444 });
            Assert.AreEqual(new double[] { 111, 222, 333, 444, 0, 0 }, spl.Ys);
        }

        [Test]
        public void Test_SignalPlotList_AddRangeListWorksToo()
        {
            var plt = new ScottPlot.Plot();
            var spl = plt.AddSignalList(capacity: 3);

            spl.AddRange(new List<double>() { 111, 222, 333, 444 });
            Assert.AreEqual(new double[] { 111, 222, 333, 444, 0, 0 }, spl.Ys);
        }
    }
}
