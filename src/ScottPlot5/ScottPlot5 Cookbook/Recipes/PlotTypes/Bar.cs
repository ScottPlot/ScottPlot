namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class Bar : RecipePageBase
{
    public override RecipePageDetails PageDetails => new()
    {
        Chapter = Chapter.PlotTypes,
        PageName = "Bar Plot",
        PageDescription = "Bar plots represent values as horizontal or vertical rectangles",
    };

    internal class Quickstart : RecipeTestBase
    {

        public override string Name => "Bar Plot Quickstart";
        public override string Description => "Bar plots can be added from a series of values.";

        [Test]
        public override void Recipe()
        {
            Random rand = new(0);

            var series = Enumerable.Range(0, 4).Select(i => new ScottPlot.Plottables.BarSeries
            {
                Bars = Enumerable.Range(0, rand.Next(3, 10)).Select(j => new ScottPlot.Plottables.Bar
                {
                    Position = j,
                    Value = rand.NextDouble() * 10
                }).ToArray(),
                Fill = new(myPlot.Palette.GetColor(i))
                {
                    HatchColor = myPlot.Palette.GetColor(i + 1),
                    Hatch = new ScottPlot.Style.Hatches.Striped(ScottPlot.Style.Hatches.StripeDirection.DiagonalDown)
                },
                Label = $"Series {i + 1}"
            }).ToArray();

            foreach (var s in series)
                for (int i = 1; i < s.Bars.Count; i++)
                    s.Bars[i].ValueBase = s.Bars[i - 1].Value;

            myPlot.Add.Bar(series);

            myPlot.Add.Legend();
        }
    }
}
