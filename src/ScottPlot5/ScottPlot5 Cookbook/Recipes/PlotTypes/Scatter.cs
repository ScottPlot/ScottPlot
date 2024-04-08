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

    public class ScatterLine : RecipeBase
    {
        public override string Name => "Scatter Plot with Lines Only";
        public override string Description => "The `ScatterLine()` method can be used " +
            "to create a scatter plot with a line only (marker size is set to 0).";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] sin = Generate.Sin(51);
            double[] cos = Generate.Cos(51);

            myPlot.Add.ScatterLine(xs, sin);
            myPlot.Add.ScatterLine(xs, cos);
        }
    }

    public class ScatterPoints : RecipeBase
    {
        public override string Name => "Scatter Plot with Points Only";
        public override string Description => "The `ScatterPoints()` method can be used " +
            "to create a scatter plot with markers only (line width is set to 0).";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] sin = Generate.Sin(51);
            double[] cos = Generate.Cos(51);

            myPlot.Add.ScatterPoints(xs, sin);
            myPlot.Add.ScatterPoints(xs, cos);
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

    public class ScatterWithGaps : RecipeBase
    {
        public override string Name => "Scatter with Gaps";
        public override string Description => "NaN values in a scatter plot's data " +
            "will appear as gaps in the line.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            // long stretch of empty data
            for (int i = 10; i < 20; i++)
                ys[i] = double.NaN;

            // single missing data point
            ys[30] = double.NaN;

            // single floating data point
            for (int i = 35; i < 40; i++)
                ys[i] = double.NaN;
            for (int i = 40; i < 45; i++)
                ys[i] = double.NaN;

            myPlot.Add.Scatter(xs, ys);
        }
    }

    public class ScatterSmooth : RecipeBase
    {
        public override string Name => "Scatter Plot with Smooth Lines";
        public override string Description => "Scatter plots draw straight lines " +
            "between points by default, but setting the Smooth property allows the " +
            "scatter plot to connect points with smooth lines.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(10);
            double[] ys = Generate.RandomSample(10, 5, 15);

            var sp = myPlot.Add.Scatter(xs, ys);
            sp.Smooth = true;
            sp.Label = "Smooth";
            sp.LineWidth = 2;
            sp.MarkerSize = 7;
        }
    }

    public class ScatterLimitIndex : RecipeBase
    {
        public override string Name => "Limiting Display with Render Indexes";
        public override string Description => "Although a scatter plot may contain " +
            "a very large amount of data, much of it may be unpopulated. The user can " +
            "define min and max render indexes, and only values within that range will " +
            "be displayed when the scatter plot is rendered.";

        [Test]
        public override void Execute()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            var sp = myPlot.Add.Scatter(xs, ys);
            sp.MinRenderIndex = 10;
            sp.MaxRenderIndex = 40;
        }
    }
}
