using System.Drawing;

namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class FillY : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.PlotTypes,
        PageName = "FillY plot",
        PageDescription = "FillY plots display the vertical range between two Y values at defined X positions",
    };

    internal class FillYFromArrays : RecipeTestBase
    {
        public override string Name => "FillY From Array Data";
        public override string Description => "FillY plots can be created from X, Y1, and Y2 arrays.";

        [Test]
        public override void Recipe()
        {
            RandomDataGenerator dataGen = new(0);

            int count = 20;
            double[] xs = Generate.Consecutive(count);
            double[] ys1 = dataGen.RandomWalk(count, offset: -5);
            double[] ys2 = dataGen.RandomWalk(count, offset: 5);

            var xyy = myPlot.Add.FillY(xs, ys1, ys2);
            xyy.FillStyle.Color = Colors.Magenta.WithAlpha(100);
        }
    }

    internal class FillYFromScatters : RecipeTestBase
    {
        public override string Name => "FillY From Scatter Plots";
        public override string Description => "FillY plots can be created from two scatter plots that share the same X values.";

        [Test]
        public override void Recipe()
        {
            RandomDataGenerator dataGen = new(0);

            int count = 20;
            double[] xs = Generate.Consecutive(count);
            double[] ys1 = dataGen.RandomWalk(count, offset: -5);
            double[] ys2 = dataGen.RandomWalk(count, offset: 5);

            var scatter1 = myPlot.Add.Scatter(xs, ys1);
            var scatter2 = myPlot.Add.Scatter(xs, ys2);

            var xyy = myPlot.Add.FillY(scatter1, scatter2);
            xyy.FillStyle.Color = Colors.Blue.WithAlpha(100);
        }
    }

    internal class Function : RecipeTestBase
    {
        public override string Name => "FillY with Custom Type";
        public override string Description => "FillY plots can be created from data of any type if a conversion function is supplied.";

        [Test]
        public override void Recipe()
        {
            // create source data in a nonstandard data type
            List<(int, int, int)> data = new();
            Random rand = new(0);
            for (int i = 0; i < 10; i++)
            {
                int x = i;
                int y1 = rand.Next(0, 10);
                int y2 = rand.Next(20, 30);
                data.Add((x, y1, y2));
            }

            // create a custom converter for the source data type
            static (double, double, double) MyConverter((int, int, int) s) => (s.Item1, s.Item2, s.Item3);

            // create a filled plot from source data using the custom converter
            myPlot.Add.FillY(data, MyConverter);
        }
    }

    internal class Styling : RecipeTestBase
    {
        public override string Name => "FillY Plot Styling";
        public override string Description => "FillY plots can be customized using public properties.";

        [Test]
        public override void Recipe()
        {
            RandomDataGenerator dataGen = new(0);

            int count = 20;
            double[] xs = Generate.Consecutive(count);
            double[] ys1 = dataGen.RandomWalk(count, offset: -5);
            double[] ys2 = dataGen.RandomWalk(count, offset: 5);

            var xyy = myPlot.Add.FillY(xs, ys1, ys2);
            xyy.FillStyle.Color = Colors.OrangeRed.WithAlpha(100);

            xyy.MarkerStyle.IsVisible = true;
            xyy.MarkerStyle.Shape = MarkerShape.OpenSquare;
            xyy.MarkerStyle.Size = 8;

            xyy.LineStyle.AntiAlias = true;
            xyy.LineStyle.Color = Colors.DarkBlue;
            xyy.LineStyle.Pattern = LinePattern.Dot;
            xyy.LineStyle.Width = 2;
        }
    }
}

