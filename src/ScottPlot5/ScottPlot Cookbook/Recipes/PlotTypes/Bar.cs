namespace ScottPlotCookbook.Recipes.Introduction;

internal class Bar : RecipePage
{
    public override Chapter Chapter => Chapter.PlotTypes;
    public override string PageName => "Bar Plot";
    public override string PageDescription => "Bar plots represent values as horizontal or vertical rectangles";

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
                Fill = new(MyPlot.Palette.GetColor(i))
                {
                    HatchColor = MyPlot.Palette.GetColor(i + 1),
                    Hatch = new ScottPlot.Style.Hatches.Striped(ScottPlot.Style.Hatches.StripeDirection.DiagonalDown)
                },
                Label = $"Series {i + 1}"
            }).ToArray();

            foreach (var s in series)
                for (int i = 1; i < s.Bars.Count; i++)
                    s.Bars[i].ValueBase = s.Bars[i - 1].Value;

            MyPlot.Add.Bar(series);

            MyPlot.Add.Legend();
        }
    }
}
