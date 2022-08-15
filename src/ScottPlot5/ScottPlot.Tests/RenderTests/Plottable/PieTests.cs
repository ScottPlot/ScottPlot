using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Tests.RenderTests.Plottable
{
    internal class PieTests
    {
        [Test]
        public void Test_Pie_Render()
        {
            Plot plt = new();

            Plottables.PieSlice[] slices =
            {
                new(6, Colors.Red),
                new(4, Colors.Blue),
                new(3, Colors.Green),
                new(1, Colors.DarkCyan),
            };

            plt.Add.Pie(slices);

            TestTools.SaveImage(plt);
        }
    }
}
