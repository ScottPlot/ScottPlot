using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotCookbook.Recipes.PlotTypes
{
    internal class ErrorBar : RecipePageBase
    {
        public override RecipePageDetails PageDetails => new()
        {
            Chapter = Chapter.PlotTypes,
            PageName = "Error Bars",
            PageDescription = "Error Bars communicate the range of possible values for a measurement.",
        };

        internal class Quickstart : RecipeTestBase
        {
            public override string Name => "Error Bar Quickstart";
            public override string Description => "Error Bars go well with scatter plots.";

            [Test]
            public override void Recipe()
            {
                int N = 50;

                double[] xs = Generate.Consecutive(N);
                double[] ys = Generate.RandomWalk(N);

                double[] xErrPositive = Generate.Random(N, 0.1, 0.25);
                double[] xErrNegative = Generate.Random(N, 0.1, 0.25);
                double[] yErrPositive = Generate.Random(N, 0.1, 0.25);
                double[] yErrNegative = Generate.Random(N, 0.1, 0.25);

                var scatter = myPlot.Add.Scatter(xs, ys);
                var errorBars = myPlot.Add.ErrorBar(xs, ys, xErrPositive, xErrNegative, yErrPositive, yErrNegative, scatter.LineStyle.Color);
            }
        }

    }
}
