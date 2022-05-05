using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Validation
{
    internal class SignalXY
    {
        private static ScottPlot.Plottable.SignalPlotXY CreateSignalPlot(double[] values) => new()
        {
            Xs = values,
            Ys = ScottPlot.DataGen.Sin(values.Length),
            MaxRenderIndex = values.Length - 1,
        };

        [Test]
        public void Test_SignalXY_Xs_AscendingOK()
        {
            double[] values = { 1, 2, 3, 4, 5 };

            ScottPlot.Plottable.SignalPlotXY sig = CreateSignalPlot(values);

            Assert.DoesNotThrow(() => sig.ValidateData(deep: true));
        }

        [Test]
        public void Test_SignalXY_Xs_MayContainDuplicates()
        {
            double[] values = { 1, 2, 3, 3, 3, 3, 3, 4, 5 };

            ScottPlot.Plottable.SignalPlotXY sig = CreateSignalPlot(values);

            Assert.DoesNotThrow(() => sig.ValidateData(deep: true));
        }

        [Test]
        public void Test_SignalXY_Xs_MayContainMaxValue()
        {
            double[] values = { 1, 2, 3, double.MaxValue, double.MaxValue };

            ScottPlot.Plottable.SignalPlotXY sig = CreateSignalPlot(values);

            Assert.DoesNotThrow(() => sig.ValidateData(deep: true));
        }

        [Test]
        public void Test_SignalXY_Xs_MustNotBeEmpty()
        {
            double[] values = { };

            Assert.Throws<ArgumentException>(() => CreateSignalPlot(values));
        }

        [Test]
        public void Test_SignalXY_Xs_MustNotDescend()
        {
            double[] values = { 1, 2, -42, 4, 5 };

            ScottPlot.Plottable.SignalPlotXY sig = CreateSignalPlot(values);

            Assert.Throws<InvalidOperationException>(() => sig.ValidateData(deep: true));
        }

        [Test]
        public void Test_SignalXY_Xs_MustNotContainNan()
        {
            double[] values = { 1, 2, double.NaN, 4, 5 };

            ScottPlot.Plottable.SignalPlotXY sig = CreateSignalPlot(values);

            Assert.Throws<InvalidOperationException>(() => sig.ValidateData(deep: true));
        }

        [Test]
        public void Test_SignalXY_Xs_MustNotContainInfinity()
        {
            double[] values = { 1, 2, 3, double.PositiveInfinity, double.PositiveInfinity };

            ScottPlot.Plottable.SignalPlotXY sig = CreateSignalPlot(values);

            Assert.Throws<InvalidOperationException>(() => sig.ValidateData(deep: true));
        }
    }
}
