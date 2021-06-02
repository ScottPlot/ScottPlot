using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Validation
{
    class Signal
    {
        [Test]
        public void Test_Render_AllValid()
        {
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot();
            plt.AddSignal(ys);

            TestTools.SaveFig(plt);

            Assert.Pass();
        }

        [Test]
        public void Test_Validate_YContainsNan()
        {
            double[] ys = { 1, 4, double.NaN, 16, 25 };

            var plt = new ScottPlot.Plot();
            plt.AddSignal(ys);

            Assert.DoesNotThrow(() => { plt.Validate(deep: false); });
            Assert.Throws<InvalidOperationException>(() => { plt.Validate(deep: true); });
        }

        [Test]
        public void Test_Validate_YContainsInf()
        {
            double[] ys = { 1, 4, double.PositiveInfinity, 16, 25 };

            var plt = new ScottPlot.Plot();
            plt.AddSignal(ys);

            Assert.DoesNotThrow(() => { plt.Validate(deep: false); });
            Assert.Throws<InvalidOperationException>(() => { plt.Validate(deep: true); });
        }

        [Test]
        public void Test_Render_YIsAllNan()
        {
            double[] ys = { double.NaN, double.NaN, double.NaN, double.NaN, double.NaN };

            var plt = new ScottPlot.Plot();
            plt.AddSignal(ys);

            Assert.Throws<InvalidOperationException>(() => { plt.AxisAuto(); });
        }

        [Test]
        public void Test_Render_YHasNanPixel()
        {
            double[] ys = { 1, 4, double.NaN, 16, 25 };

            var plt = new ScottPlot.Plot();
            plt.AddSignal(ys);

            Assert.DoesNotThrow(() => { plt.AxisAuto(); });
            Assert.Throws<InvalidOperationException>(() => { plt.Render(); });
        }
    }
}
