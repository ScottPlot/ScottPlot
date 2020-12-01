namespace ScottPlotTests.Cookbook.PlotTypes.Scatter
{
    class LineStyle : RecipeTester, IRecipe
    {
        public override RecipeCategory Category => RecipeCategory.PlotTypes;
        public override string Section => "Scatter Plot";
        public override string Title => "Customize Line Style";
        public override string Description => "The lines connecting scatter plot points can be customized.";

        public override void ExecuteRecipe(ScottPlot.Plot plt)
        {
            plt.Title("Scatter Plot with Custom Line Style");

            // generate some sample data
            double[] xs = ScottPlot.DataGen.Consecutive(51);
            double[] sin = ScottPlot.DataGen.Sin(51);
            double[] cos = ScottPlot.DataGen.Cos(51);

            // some customizations are available as optional arguments
            var sp1 = plt.PlotScatter(xs, sin);
            sp1.markerSize = 0;
            sp1.lineWidth = 5;
            sp1.lineStyle = ScottPlot.LineStyle.Dash;
            sp1.color = System.Drawing.Color.Magenta;

            // other customizations are available as public fields
            var sp2 = plt.PlotScatter(xs, cos);
            sp2.markerSize = 0;
            sp2.lineWidth = 2;
            sp2.lineStyle = ScottPlot.LineStyle.Dot;
        }
    }
}
