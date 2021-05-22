using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Control
{
    class DrawingTest
    {
        [Test]
        public void Test_DrawingTest_CanDrawGraphics()
        {
            Assert.DoesNotThrow(ScottPlot.Drawing.GDI.DrawingTest);
        }
    }
}
