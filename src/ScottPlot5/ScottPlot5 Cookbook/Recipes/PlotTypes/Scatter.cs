namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Scatter : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Scatter Plot";
    public string CategoryDescription => "Scatter plots display points at X/Y locations in coordinate space.";

    public class ScatterQuickstart : RecipeBase
    {
        public override string Name => "Scatter Plot Quickstart";
        public override string Description => "Scatter plots can be created from " +
            "two arrays containing X and Y values.";

        [Test]
        public override void Execute()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 1, 4, 9, 16, 25 };

            myPlot.Add.Scatter(xs, ys);
        }
    }

    public class ScatterCoordinates : RecipeBase
    {
        public override string Name => "Scatter Plot Coordinates";
        public override string Description => "Scatter plots can be created from " +
            "a collection of Coordinates.";

        [Test]
        public override void Execute()
        {
            Coordinates[] coordinates =
            {
                new(1, 1),
                new(2, 4),
                new(3, 9),
                new(4, 16),
                new(5, 25),
            };

            myPlot.Add.Scatter(coordinates);
        }
    }

    public class ScatterDataType : RecipeBase
    {
        public override string Name => "Scatter Plot Data Type";
        public override string Description => "Scatter plots can be created " +
            "from any numeric data type, not just double.";

        [Test]
        public override void Execute()
        {
            float[] xs = { 1, 2, 3, 4, 5 };
            int[] ys = { 1, 4, 9, 16, 25 };

            myPlot.Add.Scatter(xs, ys);
        }
    }

    public class ScatterList : RecipeBase
    {
        public override string Name => "Scatter Plot of List Data";
        public override string Description => "Scatter plots can be created " +
            "from Lists, but be very cafeful not to add or remove items while a " +
            "render is occurring or you may throw an index exception. " +
            "See documentation about the Render Lock system for details.";

        [Test]
        public override void Execute()
        {
            List<double> xs = new() { 1, 2, 3, 4, 5 };
            List<double> ys = new() { 1, 4, 9, 16, 25 };

            myPlot.Add.Scatter(xs, ys);
        }
    }

    public class ScatterStyling : RecipeBase
    {
        public override string Name => "Scatter Plot Styling";
        public override string Description => "Scatter plots can be extensively styled by interacting " +
            "with the object that is returned after a scatter plot is added. " +
            "Assign text to a scatter plot's Label property to allow it to appear in the legend.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys1 = Generate.Sin(51);
            double[] ys2 = Generate.Cos(51);

            var sp1 = myPlot.Add.Scatter(xs, ys1);
            sp1.Label = "Sine";
            sp1.LineWidth = 3;
            sp1.Color = Colors.Magenta;
            sp1.MarkerSize = 15;

            var sp2 = myPlot.Add.Scatter(xs, ys2);
            sp2.Label = "Cosine";
            sp2.LineWidth = 2;
            sp2.Color = Colors.Green;
            sp2.MarkerSize = 10;

            myPlot.ShowLegend();
        }
    }

    public class ScatterLinePatterns : RecipeBase
    {
        public override string Name => "Scatter Line Patterns";
        public override string Description => "Several line patterns are available";

        [Test]
        public override void Execute()
        {
            LinePattern[] patterns = Enum.GetValues<LinePattern>();
            ScottPlot.Palettes.ColorblindFriendly palette = new();

            for (int i = 0; i < patterns.Length; i++)
            {
                double yOffset = patterns.Length - i;
                double[] xs = Generate.Consecutive(51);
                double[] ys = Generate.Sin(51, offset: yOffset);

                var sp = myPlot.Add.Scatter(xs, ys);
                sp.LineWidth = 2;
                sp.MarkerSize = 0;
                sp.LineStyle.Pattern = patterns[i];
                sp.Color = palette.GetColor(i);

                var txt = myPlot.Add.Text(patterns[i].ToString(), 51, yOffset);
                txt.Label.ForeColor = sp.Color;
                txt.Label.FontSize = 22;
                txt.Label.Bold = true;
                txt.Label.Alignment = Alignment.MiddleLeft;
            }

            myPlot.Axes.Margins(.05, .5, .05, .05);
        }
    }
}