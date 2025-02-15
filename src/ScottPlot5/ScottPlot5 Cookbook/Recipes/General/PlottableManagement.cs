using ScottPlot.Plottables;

namespace ScottPlotCookbook.Recipes.General;

public class PlottableManagement : ICategory
{
    public Chapter Chapter => Chapter.General;
    public string CategoryName => "Plottable Management";
    public string CategoryDescription => "How to add, remove, and reorder items in plots";

    public class AddPlottablesManually : RecipeBase
    {
        public override string Name => "Add Plottables Manually";
        public override string Description => "Although the Plot.Add class has many helpful methods " +
            "for creating plottable objects and adding them to the plot, users can instantiate plottable " +
            "objects themselves and use Add.Plottable() to place it on the plot. This strategy allows " +
            "users to create their own plottables (implementing IPlottable) with custom appearance or behavior.";

        [Test]
        public override void Execute()
        {
            // create a plottable and modify it as desired
            ScottPlot.Plottables.LinePlot line = new()
            {
                Start = new Coordinates(1, 2),
                End = new Coordinates(3, 4),
            };

            // add the custom plottable to the plot
            myPlot.Add.Plottable(line);
        }
    }

    public class Clear : RecipeBase
    {
        public override string Name => "Clearing Plots";
        public override string Description => "Use Clear() to remove all plottables.";

        [Test]
        public override void Execute()
        {
            // add plottables
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());

            // remove all plottables
            myPlot.Clear();
        }
    }

    public class Remove : RecipeBase
    {
        public override string Name => "Removing a Plottable";
        public override string Description => "Individual items may be removed from the plot.";

        [Test]
        public override void Execute()
        {
            // add plottables
            var sig1 = myPlot.Add.Signal(Generate.Sin());
            var sig2 = myPlot.Add.Signal(Generate.Cos());

            // remove a specific plottable
            myPlot.Remove(sig1);
        }
    }

    public class RemoveAll : RecipeBase
    {
        public override string Name => "Removing all Plottables of a Type";
        public override string Description => "All plottables of a given type " +
            "may be removed from a plot with a single command.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Scatter(Generate.Consecutive(51), Generate.Sin(51));
            myPlot.Add.Signal(Generate.Cos());
            myPlot.Add.HorizontalLine(0.75);

            // remove every instance of a specific plottable type
            myPlot.Remove<ScottPlot.Plottables.Signal>();
        }
    }

    public class ChangeOrder : RecipeBase
    {
        public override string Name => "Moving plottables";
        public override string Description => "The plottable list contains all " +
            "plottables which will be rendered in order. " +
            "Helper methods are available to move plottables to the front.";

        [Test]
        public override void Execute()
        {
            CoordinateRect wideRect = new(-2, 2, -1, 1);
            CoordinateRect tallRect = new(-1, 1, -2, 2);

            // rect1 is added first, so plottables added later will appear on top
            var rect1 = myPlot.Add.Rectangle(wideRect);
            var rect2 = myPlot.Add.Rectangle(tallRect);

            // plottables may be moved to the front so they always appear on top
            myPlot.MoveToTop(rect1);
        }
    }
}
