using NUnit.Framework;
using ScottPlot.Control;
using System;

namespace ScottPlotTests.Control
{

    [TestFixture]
    public class ConfigurationTests
    {
        [TestCase(0.01)]
        [TestCase(0.15)]
        [TestCase(0.7)]
        [TestCase(0.5)]
        public void ScrollWheelZoomIncrement_WithinValidRange_NotThrows(double value)
        {
            Configuration config = new Configuration();
            config.ScrollWheelZoomFraction = value;
        }

        [TestCase(-5)]
        [TestCase(2)]
        [TestCase(7)]
        [TestCase(0)]
        [TestCase(1)]
        public void ScrollWheelZoomIncrement_WithoutValidRange_Throws(double value)
        {
            Configuration config = new Configuration();
            Assert.Throws<ArgumentOutOfRangeException>(() => config.ScrollWheelZoomFraction = value);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(7)]
        public void Test_RenderCount_Increments(int renderCount)
        {
            ControlBackEnd backend = new(400, 300);
            for (int i = 0; i < renderCount; i++)
                backend.Render();
            Assert.AreEqual(renderCount, backend.RenderCount);
        }

        public void Test_AutoRender_CanBeDisabled()
        {
            ControlBackEnd backend = new(400, 300);
            backend.Plot.AddVerticalLine(123);
            backend.Render();
            int originalCount = backend.RenderCount;

            backend.Configuration.RenderIfPlottableListChanges = false;
            backend.Plot.AddVerticalLine(123);
            backend.RenderIfPlottableListChanged();

            Assert.AreEqual(originalCount, backend.RenderCount);
        }

        [Test]
        public void Test_AutoRender_AfterPlottablesAdded()
        {
            ControlBackEnd backend = new(600, 400);

            backend.Plot.AddVerticalLine(123);
            Assert.AreEqual(0, backend.RenderCount);

            backend.RenderIfPlottableListChanged();
            Assert.AreEqual(1, backend.RenderCount);

            backend.RenderIfPlottableListChanged();
            Assert.AreEqual(1, backend.RenderCount);

            backend.Plot.AddVerticalLine(123);
            backend.RenderIfPlottableListChanged();
            Assert.AreEqual(2, backend.RenderCount);
        }

        [Test]
        public void Test_AutoRender_AfterPlottablesRemoved()
        {
            ControlBackEnd backend = new(600, 400);

            var p1 = backend.Plot.AddVerticalLine(123);
            var p2 = backend.Plot.AddVerticalLine(123);
            var p3 = backend.Plot.AddVerticalLine(123);
            backend.RenderIfPlottableListChanged();
            Assert.AreEqual(1, backend.RenderCount);

            // remove a plottable
            backend.Plot.Remove(p3);
            backend.RenderIfPlottableListChanged();
            Assert.AreEqual(2, backend.RenderCount);
        }

        [Test]
        public void Test_AutoRender_AfterPlottablesChanged()
        {
            ControlBackEnd backend = new(600, 400);

            var p1 = backend.Plot.AddVerticalLine(123);
            var p2 = backend.Plot.AddVerticalLine(123);
            var p3 = backend.Plot.AddVerticalLine(123);
            backend.RenderIfPlottableListChanged();
            Assert.AreEqual(1, backend.RenderCount);

            // remove and replace a plottable
            backend.Plot.Remove(p3);
            var p4 = backend.Plot.AddVerticalLine(123);
            backend.RenderIfPlottableListChanged();
            Assert.AreEqual(2, backend.RenderCount);
        }
    }
}
