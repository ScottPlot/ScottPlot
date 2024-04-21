namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Box : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Box Plot";
    public string CategoryDescription => "Box plots show a distribution at a glance";

    public class BoxPlotQuickstart : RecipeBase
    {
        public override string Name => "Box Plot Quickstart";
        public override string Description => "Box plots can be created " +
            "individually and added to the plot.";

        [Test]
        public override void Execute()
        {
            ScottPlot.Box box = new()
            {
                Position = 5,
                BoxMin = 81,
                BoxMax = 93,
                WhiskerMin = 76,
                WhiskerMax = 107,
                BoxMiddle = 84,
            };

            myPlot.Add.Box(box);

            myPlot.Axes.SetLimits(0, 10, 70, 110);
        }
    }

    public class BoxPlotGroups : RecipeBase
    {
        public override string Name => "Box Plot Groups";
        public override string Description => "Each collection of boxes added to the plot " +
            "gets styled the same and appears as a single item in the legend. " +
            "Add multiple bar series plots with defined X positions to give the " +
            "appearance of grouped data.";

        [Test]
        public override void Execute()
        {
            List<ScottPlot.Box> boxes1 = new() {
                Generate.RandomBox(1),
                Generate.RandomBox(2),
                Generate.RandomBox(3),
            };

            List<ScottPlot.Box> boxes2 = new() {
                Generate.RandomBox(5),
                Generate.RandomBox(6),
                Generate.RandomBox(7),
            };

            var bp1 = myPlot.Add.Boxes(boxes1);
            bp1.LegendText = "Group 1";

            var bp2 = myPlot.Add.Boxes(boxes2);
            bp2.LegendText = "Group 2";

            myPlot.ShowLegend(Alignment.UpperRight);
        }
    }
}
