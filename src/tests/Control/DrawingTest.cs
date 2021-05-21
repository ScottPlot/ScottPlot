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
            var errorMessage = ScottPlot.Drawing.GDI.DrawingTest();
            Assert.IsNull(errorMessage);
        }
    }
}
