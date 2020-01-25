using NUnit.Framework;
using ScottPlot.Config;

namespace ScottPlotTests
{
    [TestFixture]
    public class AxesTests
    {
        [Test]
        public void Expand_NoExpandParams_BothAxesChanged()
        {
            Axes axes = new Axes();
            axes.Set(1, 2, 3, 4);
            axes.Expand(new double[] { -11, 12, -13, 14 });

            Assert.AreEqual(-11, axes.limits[0]);
            Assert.AreEqual(12, axes.limits[1]);

            Assert.AreEqual(-13, axes.limits[2]);
            Assert.AreEqual(14, axes.limits[3]);
        }
        [Test]
        public void Expand_xExpandOnly_ylimitsUnchanged()
        {
            Axes axes = new Axes();
            axes.Set(1, 2, 3, 4);
            axes.Expand(new double[] { -11, 12, -13, 14 }, xExpandOnly: true);
            Assert.AreNotEqual(-13, axes.limits[2]);
            Assert.AreNotEqual(14, axes.limits[3]);

            Assert.AreNotEqual(1, axes.limits[0]);
            Assert.AreNotEqual(2, axes.limits[1]);
        }

        [Test]
        public void Expand_yExpandOnly_xlimitsUnchanged()
        {
            Axes axes = new Axes();
            axes.Set(1, 2, 3, 4);
            axes.Expand(new double[] { -11, 12, -13, 14 }, yExpandOnly: true);

            Assert.AreEqual(1, axes.limits[0]);
            Assert.AreEqual(2, axes.limits[1]);

            Assert.AreEqual(-13, axes.limits[2]);
            Assert.AreEqual(14, axes.limits[3]);
        }
    }
}
