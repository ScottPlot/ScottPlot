namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Pie : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Pie";
    public string CategoryDescription => "Pie charts illustrate numerical proportions as slices of a circle.";

    public class PieQuickstart : RecipeBase
    {
        public override string Name => "Pie Chart from Values";
        public override string Description => "A pie chart can be created from a few values.";

        [Test]
        public override void Execute()
        {
            double[] values = { 5, 2, 8, 4, 8 };
            var pie = myPlot.Add.Pie(values);
            pie.ExplodeFraction = .1;
        }
    }

    public class PieSlices : RecipeBase
    {
        public override string Name => "Pie Chart from Slices";
        public override string Description => "A pie chart can be created from a collection of slices.";

        [Test]
        public override void Execute()
        {
            List<PieSlice> slices = new()
            {
                new PieSlice() { Value = 5, FillColor = Colors.Red, Label = "Red" },
                new PieSlice() { Value = 2, FillColor = Colors.Orange, Label = "Orange" },
                new PieSlice() { Value = 8, FillColor = Colors.Gold, Label = "Yellow" },
                new PieSlice() { Value = 4, FillColor = Colors.Green, Label = "Green" },
                new PieSlice() { Value = 8, FillColor = Colors.Blue, Label = "Blue" },
            };

            var pie = myPlot.Add.Pie(slices);
            pie.ExplodeFraction = .1;

            myPlot.ShowLegend();
        }
    }
}
