namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class Box : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.PlotTypes,
        PageName = "Box Plot",
        PageDescription = "Box plots show a distribution at a glance",
    };

    internal class Quickstart : RecipeTestBase
    {
        public override string Name => "Box Plot Quickstart";
        public override string Description => "Box plots can be added from a series of values.";

        [Test]
        public override void Recipe()
        {
            int N = 50;
            double[] values = Generate.RandomNormal(N);
            Array.Sort(values);
            double min = values[0];
            double q1 = values[N / 4];
            double median = values[N / 2];
            double q3 = values[3 * N / 4];
            double max = values[N - 1];

            var box = new ScottPlot.Plottables.Box
            {
                BoxMin = q1,
                BoxMiddle = median,
                BoxMax = q3,
            };
            
            myPlot.Add.Box(box);
            myPlot.AutoScale();
            myPlot.SetAxisLimits(bottom: 0);
        }
    }
}
