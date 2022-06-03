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
                double x = Math.Sin(t);
                double y = Math.Cos(t);
                var b = plt.AddBracket(0, 0, x, y, i.ToString());
                b.EdgeLength = 20;
                b.LineWidth = 2;
            }

            plt.Margins(.2, .2);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bracket_Rotation_When_Stretched()
        {
            ScottPlot.Plot plt = new();

            for (int i = 0; i < 10; i++)
            {
                double t = i / 10.0 * 2 * Math.PI;
                double x = Math.Sin(t) * 10; // stretched
                double y = Math.Cos(t);
                var b = plt.AddBracket(0, 0, x, y, i.ToString());
                b.EdgeLength = 20;
                b.LineWidth = 2;
            }

            plt.Margins(.2, .2);
            TestTools.SaveFig(plt);
        }
    }
}
