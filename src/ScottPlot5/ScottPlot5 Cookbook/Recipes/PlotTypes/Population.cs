namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Population : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Population Plot";
    public string CategoryDescription => "Population plots display collections of individual values.";

    public class PopulationQuickstart : RecipeBase
    {
        public override string Name => "Population Quickstart";
        public override string Description => "A Population can be created from a collection " +
            "of values, styled as desired, and placed anywhere on the plot.";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 5; i++)
            {
                double[] values = Generate.RandomNormal(10, mean: 3 + i);
                myPlot.Add.Population(values, x: i);
            }

            // make the bottom of the plot snap to zero by default
            myPlot.Axes.Margins(bottom: 0);

            // replace the default numeric ticks with custom ones
            double[] tickPositions = Generate.Consecutive(5);
            string[] tickLabels = Enumerable.Range(1, 5).Select(x => $"Group {x}").ToArray();
            myPlot.Axes.Bottom.SetTicks(tickPositions, tickLabels);

            // refine appearance of the plot
            myPlot.Axes.Bottom.MajorTickStyle.Length = 0;
            myPlot.HideGrid();
        }
    }
}
