using NUnit.Framework;
using System;

namespace ScottPlotTests.PlotTypes
{
    internal class Bracket
    {
        [Test]
        public void Test_Bracket_Rotation()
        {
            ScottPlot.Plot plt = new();

            for (int i = 0; i < 10; i++)
            {
                double t = i / 10.0 * 2 * Math.PI;
                var b = plt.AddBracket(0, 0, Math.Sin(t), Math.Cos(t), i.ToString());
                b.EdgeLength = 20;
                b.LineWidth = 2;
            }

            plt.Margins(.2, .2);
            TestTools.SaveFig(plt);
        }
    }
}
