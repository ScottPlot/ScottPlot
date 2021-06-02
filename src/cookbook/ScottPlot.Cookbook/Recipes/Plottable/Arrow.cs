namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class Arrow : IRecipe
    {
        public string Category => "Plottable: Arrow";
        public string ID => "plottable_arrow_quickstart";
        public string Title => "Arrows";
        public string Description =>
            "Arrows point to specific locations on the plot. " +
            "Arrows are actually just scatter plots with two points and an arrowhead.";

        public void ExecuteRecipe(Plot plt)
        {
            // plot some sample data
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            // add arrows using coordinates
            plt.AddArrow(25, 0, 27, .2);
            plt.AddArrow(27, -.25, 23, -.5, lineWidth: 10);

            // the shape of the arrowhead can be adjusted
            var skinny = plt.AddArrow(12, 1, 12, .5);
            skinny.ArrowheadLength *= 2;

            var fat = plt.AddArrow(20, .6, 20, 1);
            fat.ArrowheadWidth *= 2; ;
        }
    }
}
