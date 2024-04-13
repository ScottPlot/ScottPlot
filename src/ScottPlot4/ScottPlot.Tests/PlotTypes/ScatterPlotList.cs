using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    internal class ScatterPlotList
    {
        [Test]
        public void Test_ScatterPlotList_Smooth()
        {
            ScottPlot.Plot plt = new(500, 300);

            var spl = plt.AddScatterList(lineWidth: 2, markerSize: 7);
            spl.Add(18.5, 1.43);
            spl.Add(20.6, 1.48);
            spl.Add(22.3, 1.6);
            spl.Add(24.5, 1.59);
            spl.Add(26.6, 1.53);
            spl.Add(15, 1.52);
            spl.Add(15, 1.6);

            spl.Smooth = true;

            plt.SetAxisLimits(0, 30, 1.42, 1.62); // mimic excel
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Scatter_IgnoreNaN()
        {
            double[] xs = ScottPlot.DataGen.Consecutive(51);
            double[] ys = ScottPlot.DataGen.Sin(51);

            Random rand = new(0);
            double[] ys2 = ScottPlot.DataGen.InsertNanRanges(ys, rand, 5);

            ScottPlot.Plot plt = new(600, 400);
            var sp1 = plt.AddScatterList(color: Color.FromArgb(50, Color.Black), label: "original data");
            sp1.AddRange(xs, ys);
            var sp2 = plt.AddScatterList(color: Color.Black, label: "data with gaps");
            sp2.AddRange(xs, ys2);
            plt.Legend(location: ScottPlot.Alignment.LowerLeft);

            // default behavior throws with a NaN error
            Assert.Throws<InvalidOperationException>(() => { plt.Render(); });

            // ignoring NaN points prevents the error
            sp2.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Ignore;
            plt.Title($"OnNaN = {sp2.OnNaN}");
            Assert.DoesNotThrow(() => plt.Render());

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Scatter_GapNaN()
        {
            double[] xs = ScottPlot.DataGen.Consecutive(51);
            double[] ys = ScottPlot.DataGen.Sin(51);

            Random rand = new(0);
            double[] ys2 = ScottPlot.DataGen.InsertNanRanges(ys, rand, 5);

            ScottPlot.Plot plt = new(600, 400);
            var sp1 = plt.AddScatterList(color: Color.FromArgb(50, Color.Black), label: "original data");
            sp1.AddRange(xs, ys);
            var sp2 = plt.AddScatterList(color: Color.Black, label: "data with gaps");
            sp2.AddRange(xs, ys2);
            plt.Legend(location: ScottPlot.Alignment.LowerLeft);

            // default behavior throws with a NaN error
            Assert.Throws<InvalidOperationException>(() => { plt.Render(); });

            // gapping NaN points prevents the error
            sp2.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;
            plt.Title($"OnNaN = {sp2.OnNaN}");
            Assert.DoesNotThrow(() => plt.Render());

            TestTools.SaveFig(plt);
        }


        [Test]
        public void Test_Scatter_AllNan()
        {
            double[] xs = ScottPlot.DataGen.Full(51, double.NaN);
            double[] ys = ScottPlot.DataGen.Full(51, double.NaN);

            ScottPlot.Plot plt = new();
            var sp = plt.AddScatterList();
            sp.AddRange(xs, ys);

            sp.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Throw;
            Assert.Throws<InvalidOperationException>(() => { plt.Render(); });

            sp.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Ignore;
            Assert.DoesNotThrow(() => { plt.Render(); });

            sp.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;
            Assert.DoesNotThrow(() => { plt.Render(); });
        }


        [Test]
        public void Test_Scatter_AllYsNan()
        {
            double[] xs = ScottPlot.DataGen.Consecutive(51);
            double[] ys = ScottPlot.DataGen.Full(51, double.NaN);

            ScottPlot.Plot plt = new();
            var sp = plt.AddScatterList();
            sp.AddRange(xs, ys);

            sp.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Throw;
            Assert.Throws<InvalidOperationException>(() => { plt.Render(); });

            sp.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Ignore;
            Assert.DoesNotThrow(() => { plt.Render(); });

            sp.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;
            Assert.DoesNotThrow(() => { plt.Render(); });
        }


        [Test]
        public void Test_Scatter_AllNanButOne()
        {
            double[] xs = ScottPlot.DataGen.Full(51, double.NaN);
            double[] ys = ScottPlot.DataGen.Full(51, double.NaN);
            xs[42] = 420;
            ys[42] = 69;

            ScottPlot.Plot plt = new();
            var sp = plt.AddScatterList();
            sp.AddRange(xs, ys);

            sp.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Throw;
            Assert.Throws<InvalidOperationException>(() => { plt.Render(); });

            sp.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Ignore;
            Assert.DoesNotThrow(() => { plt.Render(); });

            sp.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;
            Assert.DoesNotThrow(() => { plt.Render(); });
        }

        [Test]
        public void Test_ScatterListWithNans_CanGetLimits()
        {
            var plt = new ScottPlot.Plot(400, 300);
            var scatter = plt.AddScatterList();
            scatter.Add(1, 4);
            scatter.Add(2, 5);
            scatter.Add(3, 6);

            // no NaN values
            Assert.DoesNotThrow(() => { scatter.GetAxisLimits(); });
            Assert.AreEqual(1, scatter.GetAxisLimits().XMin);
            Assert.AreEqual(3, scatter.GetAxisLimits().XMax);
            Assert.AreEqual(4, scatter.GetAxisLimits().YMin);
            Assert.AreEqual(6, scatter.GetAxisLimits().YMax);

            // some NaN values
            scatter.Clear();
            scatter.Add(1, 4);
            scatter.Add(double.NaN, double.NaN);
            scatter.Add(3, 6);
            Assert.Throws<InvalidOperationException>(() => { scatter.GetAxisLimits(); });
            scatter.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Ignore;
            Assert.DoesNotThrow(() => { scatter.GetAxisLimits(); });
            Assert.AreEqual(1, scatter.GetAxisLimits().XMin);
            Assert.AreEqual(3, scatter.GetAxisLimits().XMax);
            Assert.AreEqual(4, scatter.GetAxisLimits().YMin);
            Assert.AreEqual(6, scatter.GetAxisLimits().YMax);

            // all NaN values
            scatter.Clear();
            scatter.Add(double.NaN, double.NaN);
            scatter.Add(double.NaN, double.NaN);
            scatter.Add(double.NaN, double.NaN);
            Assert.DoesNotThrow(() => { scatter.GetAxisLimits(); });
            Assert.AreEqual(double.NaN, scatter.GetAxisLimits().XMin);
            Assert.AreEqual(double.NaN, scatter.GetAxisLimits().XMax);
            Assert.AreEqual(double.NaN, scatter.GetAxisLimits().YMin);
            Assert.AreEqual(double.NaN, scatter.GetAxisLimits().YMax);
        }
    }
}
