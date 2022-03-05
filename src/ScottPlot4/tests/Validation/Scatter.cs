using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Validation
{
    class Scatter
    {
        [Test]
        public void Test_Render_AllValid()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot();
            plt.AddScatter(xs, ys);

            TestTools.SaveFig(plt);

            Assert.Pass();
        }

        [Test]
        public void Test_Validate_XContainsNan()
        {
            double[] xs = { 1, 2, double.NaN, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot();
            plt.AddScatter(xs, ys);

            Assert.DoesNotThrow(() => { plt.Validate(deep: false); });
            Assert.Throws<InvalidOperationException>(() => { plt.Validate(deep: true); });
        }

        [Test]
        public void Test_Render_XContainsNan()
        {
            double[] xs = { 1, 2, double.NaN, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot();
            plt.AddScatter(xs, ys);

            Assert.Throws<InvalidOperationException>(() => { plt.Render(); });
        }

        [Test]
        public void Test_Validate_YContainsNan()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, double.NaN, 16, 25 };

            var plt = new ScottPlot.Plot();
            plt.AddScatter(xs, ys);

            Assert.DoesNotThrow(() => { plt.Validate(deep: false); });
            Assert.Throws<InvalidOperationException>(() => { plt.Validate(deep: true); });
        }

        [Test]
        public void Test_Validate_XContainsInf()
        {
            double[] xs = { 1, 2, double.PositiveInfinity, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            var plt = new ScottPlot.Plot();
            plt.AddScatter(xs, ys);

            Assert.DoesNotThrow(() => { plt.Validate(deep: false); });
            Assert.Throws<InvalidOperationException>(() => { plt.Validate(deep: true); });
        }

        [Test]
        public void Test_Validate_YContainsInf()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, double.PositiveInfinity, 16, 25 };

            var plt = new ScottPlot.Plot();
            plt.AddScatter(xs, ys);

            Assert.DoesNotThrow(() => { plt.Validate(deep: false); });
            Assert.Throws<InvalidOperationException>(() => { plt.Validate(deep: true); });
        }

        [Test]
        public void Test_Validate_XYLengthMismatch()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16 };

            var plt = new ScottPlot.Plot();
            plt.AddScatter(xs, ys);

            Assert.Throws<InvalidOperationException>(() => { plt.Validate(deep: false); });
        }

        [Test]
        public void Test_AxisAuto_XYLengthMismatch()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16 };

            var plt = new ScottPlot.Plot();
            plt.AddScatter(xs, ys);

            Assert.Throws<InvalidOperationException>(() => { plt.AxisAuto(); });
        }
    }
}
