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

    public class ScatterLine: RecipeBase
    {
        public override string Name => "Scatter Plot Line Only";
        public override string Description => "Scatter plots can be created " +
            "without markers using the ScatterLine convenience methods.";

        [Test]
        public override void Execute()
        {
            double[] xs1 = { 1, 2, 3, 4, 5 };
            double[] ys1 = { 1, 4, 9, 16, 25 };

            var sp1 = myPlot.Add.ScatterLine(xs1, ys1);
            sp1.Label = "Two arrays of doubles";

            List<double> xs2 = new() { 1, 2, 3, 4, 5 };
            List<double> ys2 = new() { 3, 6, 11, 18, 27 };

            var sp2 = myPlot.Add.ScatterLine(xs2, ys2);
            sp2.Label = "Two Lists of doubles";
            sp2.LineWidth = 2;

            float[] xs3 = { 1, 2, 3, 4, 5 };
            int[] ys3 = { 5, 8, 13, 20, 29 };

            var sp3 = myPlot.Add.ScatterLine(xs3, ys3);
            sp3.Label = "Array of floats and Array of ints";
            sp3.LineWidth = 3;

            Coordinates[] coordinates = {
                new(1, 7),
                new(2, 10),
                new(3, 15),
                new(4, 22),
                new(5, 31),
            };

            var sp4 = myPlot.Add.ScatterLine(coordinates);
            sp4.Label = "Array of Coordinates";
            sp4.LineWidth = 4;

            List<float> xs5 = new() { 1, 2, 3, 4, 5 };
            List<int> ys5 = new() { 9, 12, 17, 24, 33 };

            var sp5 = myPlot.Add.ScatterLine(xs5, ys5);
            sp5.Label = "List of floats and List of ints";
            sp5.LineWidth = 5;

            double[] xs6 = { 1, 2, 3, 4, 5 };
            double[] ys6 = { 11, 14, 19, 26, 35 };
            ScottPlot.DataSources.ScatterSourceDoubleArray source = new(xs6, ys6);

            var sp6 = myPlot.Add.ScatterLine(source);
            sp6.Label = "ScatterSourceDoubleArray, providing IScatterSource";
            sp6.LineWidth = 6;

            myPlot.ShowLegend(Alignment.UpperLeft);

            myPlot.ScaleFactor = 2; // FIXME: remove
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
                sp.LinePattern = patterns[i];
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

    public class ScatterGeneric : RecipeBase
    {
        public override string Name => "Scatter Generic";
        public override string Description => "Scatter plots support generic data types, " +
            "although double is typically the most performant.";

        [Test]
        public override void Execute()
        {
            int[] xs = { 1, 2, 3, 4, 5 };
            float[] ys = { 1, 4, 9, 16, 25 };

            myPlot.Add.Scatter(xs, ys);
        }
    }

    public class ScatterDateTime : RecipeBase
    {
        public override string Name => "Scatter DateTime";
        public override string Description => "A scatter plot may use DateTime units but " +
            "be sure to setup the respective axis to display using DateTime format.";

        [Test]
        public override void Execute()
        {
            DateTime[] xs = Generate.DateTime.Days(100);
            double[] ys = Generate.RandomWalk(xs.Length);

            myPlot.Add.Scatter(xs, ys);
            myPlot.Axes.DateTimeTicksBottom();
        }
    }

    public class ScatterStep : RecipeBase
    {
        public override string Name => "Step Plot";
        public override string Description => "Scatter plots can be created " +
            "using a step plot display where points are connected with right angles " +
            "instead of diagnal lines. The direction of the steps can be customized.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(20);
            double[] ys1 = Generate.Consecutive(20, first: 10);
            double[] ys2 = Generate.Consecutive(20, first: 5);
            double[] ys3 = Generate.Consecutive(20, first: 0);

            var sp1 = myPlot.Add.Scatter(xs, ys1);
            sp1.ConnectStyle = ConnectStyle.Straight;
            sp1.Label = "Straight";

            var sp2 = myPlot.Add.Scatter(xs, ys2);
            sp2.ConnectStyle = ConnectStyle.StepHorizontal;
            sp2.Label = "StepHorizontal";

            var sp3 = myPlot.Add.Scatter(xs, ys3);
            sp3.ConnectStyle = ConnectStyle.StepVertical;
            sp3.Label = "StepVertical";

            myPlot.ShowLegend();
        }
    }
}
