using NUnit.Framework;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Tests.PlottableUnitTests
{
    class FinancePlotTests
    {
        [Test]
        public void Test_GetSMA_HandlesSortedOHLCs()
        {
            OHLC[] OHLCs =
            {
                new OHLC(0,0,0,1,new DateTime(2000,1,1), TimeSpan.Zero),
                new OHLC(0,0,0,2,new DateTime(2000,1,2), TimeSpan.Zero),
                new OHLC(0,0,0,3,new DateTime(2000,1,3), TimeSpan.Zero),
                new OHLC(0,0,0,4,new DateTime(2000,1,4), TimeSpan.Zero),
                new OHLC(0,0,0,5,new DateTime(2000,1,5), TimeSpan.Zero),
                new OHLC(0,0,0,6,new DateTime(2000,1,6), TimeSpan.Zero)
            };

            var plot = new FinancePlot(OHLCs);
            var (xs, ys) = plot.GetSMA(2);

            double[] expectedXs =
            {
                OHLCs[2].DateTime.ToOADate(),
                OHLCs[3].DateTime.ToOADate(),
                OHLCs[4].DateTime.ToOADate(),
                OHLCs[5].DateTime.ToOADate(),
            };
            Assert.AreEqual(expectedXs, xs);

            double[] expectedYs =
            {
                2.5,
                3.5,
                4.5,
                5.5
            };
            Assert.AreEqual(expectedYs, ys);
        }

        [Test]
        public void Test_GetSMA_HandlesUnsortedOHLCs()
        {
            OHLC[] OHLCs =
            {
                new OHLC(0,0,0,1,new DateTime(2000,1,1), TimeSpan.Zero),
                new OHLC(0,0,0,2,new DateTime(2000,1,2), TimeSpan.Zero),
                new OHLC(0,0,0,4,new DateTime(2000,1,4), TimeSpan.Zero),
                new OHLC(0,0,0,5,new DateTime(2000,1,5), TimeSpan.Zero),
                new OHLC(0,0,0,6,new DateTime(2000,1,6), TimeSpan.Zero),
                new OHLC(0,0,0,3,new DateTime(2000,1,3), TimeSpan.Zero)
            };

            var plot = new FinancePlot(OHLCs);
            var (xs, ys) = plot.GetSMA(2);

            double[] expectedXs =
            {
                OHLCs[5].DateTime.ToOADate(),
                OHLCs[2].DateTime.ToOADate(),
                OHLCs[3].DateTime.ToOADate(),
                OHLCs[4].DateTime.ToOADate(),
            };
            Assert.AreEqual(expectedXs, xs);

            double[] expectedYs =
            {
                2.5,
                3.5,
                4.5,
                5.5
            };
            Assert.AreEqual(expectedYs, ys);
        }
        [Test]
        public void Test_GetBollingerBands_HandlesSortedOHLCs()
        {
            OHLC[] OHLCs =
            {
                new OHLC(0,0,0,1,new DateTime(2000,1,1), TimeSpan.Zero),
                new OHLC(0,0,0,2,new DateTime(2000,1,2), TimeSpan.Zero),
                new OHLC(0,0,0,3,new DateTime(2000,1,3), TimeSpan.Zero),
                new OHLC(0,0,0,4,new DateTime(2000,1,4), TimeSpan.Zero),
                new OHLC(0,0,0,5,new DateTime(2000,1,5), TimeSpan.Zero),
                new OHLC(0,0,0,6,new DateTime(2000,1,6), TimeSpan.Zero)
            };

            var plot = new FinancePlot(OHLCs);
            var (xs, ys, lowers, uppers) = plot.GetBollingerBands(2);

            double[] expectedXs =
            {
                OHLCs[2].DateTime.ToOADate(),
                OHLCs[3].DateTime.ToOADate(),
                OHLCs[4].DateTime.ToOADate(),
                OHLCs[5].DateTime.ToOADate(),
            };
            Assert.AreEqual(expectedXs, xs);

            double[] expectedYs =
            {
                2.5,
                3.5,
                4.5,
                5.5
            };
            Assert.AreEqual(expectedYs, ys);

            double[] expectedLowers =
            {
                1.5,
                2.5,
                3.5,
                4.5
            };
            Assert.AreEqual(expectedLowers, lowers);

            double[] expectedUppers =
            {
                3.5,
                4.5,
                5.5,
                6.5
            };
            Assert.AreEqual(expectedUppers, uppers);
        }

        [Test]
        public void Test_GetBollingerBands_HandlesUnsortedOHLCs()
        {
            OHLC[] OHLCs =
            {
                new OHLC(0,0,0,1,new DateTime(2000,1,1), TimeSpan.Zero),
                new OHLC(0,0,0,2,new DateTime(2000,1,2), TimeSpan.Zero),
                new OHLC(0,0,0,4,new DateTime(2000,1,4), TimeSpan.Zero),
                new OHLC(0,0,0,5,new DateTime(2000,1,5), TimeSpan.Zero),
                new OHLC(0,0,0,6,new DateTime(2000,1,6), TimeSpan.Zero),
                new OHLC(0,0,0,3,new DateTime(2000,1,3), TimeSpan.Zero)
            };

            var plot = new FinancePlot(OHLCs);
            var (xs, ys, lowers, uppers) = plot.GetBollingerBands(2);

            double[] expectedXs =
            {
                OHLCs[5].DateTime.ToOADate(),
                OHLCs[2].DateTime.ToOADate(),
                OHLCs[3].DateTime.ToOADate(),
                OHLCs[4].DateTime.ToOADate(),
            };
            Assert.AreEqual(expectedXs, xs);

            double[] expectedYs =
            {
                2.5,
                3.5,
                4.5,
                5.5
            };
            Assert.AreEqual(expectedYs, ys);

            double[] expectedLowers =
            {
                1.5,
                2.5,
                3.5,
                4.5
            };
            Assert.AreEqual(expectedLowers, lowers);

            double[] expectedUppers =
            {
                3.5,
                4.5,
                5.5,
                6.5
            };
            Assert.AreEqual(expectedUppers, uppers);
        }
    }
}
